using ClibLogger;
using Microsoft.Extensions.Configuration;

namespace swConsultaDoc.Data
{
    public class BaseDatos
    {
        protected readonly string? connectionUNACEM;
        protected readonly string? connectionCANTIVOL;

        protected readonly ILoggerManager _logger;
        public BaseDatos(IConfiguration _config, ILoggerManager loggerManager)
        {
            connectionUNACEM = _config.GetConnectionString("UNACEM");
            if (string.IsNullOrEmpty(connectionUNACEM))
            {
                throw new Exception("Cadena de conexión no está definida.");
            }
            connectionCANTIVOL = _config.GetConnectionString("CANTIVOL");
            if (string.IsNullOrEmpty(connectionCANTIVOL))
            {
                throw new Exception("Cadena de conexión no está definida.");
            }
            _logger = loggerManager;
        }
    }
}
