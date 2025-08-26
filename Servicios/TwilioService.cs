using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Agenda.Servicios
{
    public class TwilioService
    {
        private readonly IConfiguration _config;

        public TwilioService(IConfiguration config)
        {
            _config = config;
            TwilioClient.Init(_config["Twilio:SID"], _config["Twilio:Token"]);
        }

        public async Task EnviarMensajeWhatsAppAsync(string telefonoDestino, string mensaje)
        {
            var to = new PhoneNumber("whatsapp:" + telefonoDestino);
            var from = new PhoneNumber("whatsapp:" + _config["Twilio:From"]);

            await MessageResource.CreateAsync(
                to: to,
                from: from,
                body: mensaje
            );
        }
    }
}
