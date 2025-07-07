namespace Agenda.Exceptions
{
    public class ResultadoOperacion
    {
        public ResultadoOperacion() { }

        public ResultadoOperacion(bool success, string? errorMessage = null)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }

        public static ResultadoOperacion Exito()
        {
            return new ResultadoOperacion(true);
        }

        public static ResultadoOperacion Fallo(string mensaje)
        {
            return new ResultadoOperacion(false, mensaje);
        }
    }
}
