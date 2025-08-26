namespace Agenda.Servicios
{
    public class RecordatorioBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public RecordatorioBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var horaActual = DateTime.Now;

                // Ejecutar a las 09:00 AM
                if (horaActual.Hour == 9 && horaActual.Minute == 0)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var recordatorioService = scope.ServiceProvider.GetRequiredService<RecordatorioTurnoService>();
                        await recordatorioService.EnviarRecordatoriosAsync();
                    }

                    // Espera 60 segundos para evitar ejecución múltiple en el mismo minuto
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); // Chequea cada 30 seg
            }
        }
    }
}
