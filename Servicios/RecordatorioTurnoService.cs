using Agenda.Base;
using Microsoft.EntityFrameworkCore;

namespace Agenda.Servicios
{
    public class RecordatorioTurnoService
    {
        private readonly AppDbContext _context;
        private readonly TwilioService _twilio;

        public RecordatorioTurnoService(AppDbContext context, TwilioService twilio)
        {
            _context = context;
            _twilio = twilio;
        }

        public async Task EnviarRecordatoriosAsync()
        {
            var mañana = DateTime.Today;
            Console.WriteLine($"Buscando turnos para el día: {mañana:dd/MM/yyyy}");

            var turnos = await _context.Turnos
                .Include(t => t.Paciente)
                .Include(t => t.Odontologo)
                .Where(t =>
                    t.Fecha == mañana &&
                    !t.Disponible &&
                    !t.MensajeConfirmacionEnviado &&
                    t.IdPaciente != null)
                .ToListAsync();

            Console.WriteLine($"Turnos encontrados: {turnos.Count}");

            foreach (var turno in turnos)
            {
                var paciente = turno.Paciente;
                if (paciente == null || string.IsNullOrWhiteSpace(paciente.Telefono))
                {
                    Console.WriteLine("Turno con paciente inválido o sin teléfono.");
                    continue;
                }

                var telefono = paciente.Telefono.StartsWith("+") ? paciente.Telefono : "+549" + paciente.Telefono;
                Console.WriteLine($"Enviando mensaje a: {telefono}");

                string mensaje = $"Hola {paciente.Nombre}, le recordamos su turno con el Dr. {turno.Odontologo?.Nombre} " +
                                 $"el {turno.Fecha:dd/MM} a las {turno.Horario:hh\\:mm}. Si no puede asistir, por favor avísenos. ¡Gracias!";

                Console.WriteLine("Mensaje:");
                Console.WriteLine(mensaje);

                try
                {
                    await _twilio.EnviarMensajeWhatsAppAsync(telefono, mensaje);
                    Console.WriteLine("Mensaje enviado correctamente.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Error al enviar mensaje:");
                    Console.WriteLine(ex.Message);
                }

                turno.MensajeConfirmacionEnviado = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}
