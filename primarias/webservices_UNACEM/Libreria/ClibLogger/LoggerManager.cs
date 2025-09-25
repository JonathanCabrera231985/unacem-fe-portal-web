using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ClibLogger
{
    public class LoggerManager : ILoggerManager
    {
        private readonly ILogger<LoggerManager>? _logger;
        public LoggerManager(IConfiguration configuration, ILogger<LoggerManager>? logger = null)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        public void LogAdvertencia(string message)
        {

            if (Configuration["MySettings:LogInfo"]!.Equals("S")) { }
            Graba_Log(message, "WARN");

        }

        public void LogAdvertencia(string message, Exception ex)
        {
            if (Configuration["MySettings:LogInfo"]!.Equals("S"))
                Graba_Log(message, "WARN");
        }

        public void LogError(string message)
        {
            if (Configuration["MySettings:LogError"]!.Equals("S"))
                Graba_Log(message, "ERROR");
        }

        public void LogError(string message, Exception ex)
        {
            if (Configuration["MySettings:LogError"]!.Equals("S"))
                Graba_Log(message, "ERROR");
        }

        public void LogInformation(string message)
        {
            if (Configuration["MySettings:LogInfo"]!.Equals("S"))
                Graba_Log(message, "INFO");
        }

        public void LogInformation(string message, Exception ex)
        {
            if (Configuration["MySettings:LogInfo"]!.Equals("S"))
                Graba_Log(message, "INFO");
        }



        public void Graba_Log(string Datos, string Tipo)
        {

            if (_logger != null)
            {
                if (Tipo == "INFO")
                {
                    _logger.LogInformation(Datos);
                }

                if (Tipo == "ERROR")
                {
                    _logger.LogError(Datos);
                }

                if (Tipo == "WARN")
                {
                    _logger.LogWarning(Datos);
                }
            }



            DirectoryInfo dir;
            try
            {

                if (Configuration["MySettings:Auditar"]!.Equals("S"))
                {
                    string NombreArchivo = Configuration["MySettings:ArchivoLog"]!;
                    NombreArchivo = NombreArchivo.Replace("|dd", DateTime.Now.ToString("dd"));
                    NombreArchivo = NombreArchivo.Replace("|MM", DateTime.Now.ToString("MM"));
                    NombreArchivo = NombreArchivo.Replace("|yyyy", DateTime.Now.ToString("yyyy"));
                    NombreArchivo = NombreArchivo.Replace("|HH", DateTime.Now.ToString("HH"));

                    dir = new DirectoryInfo(Path.GetDirectoryName(NombreArchivo)!);
                    string Archivo = Path.Combine(dir.FullName, NombreArchivo);

                    if (!(dir.Exists))
                    {
                        dir.Create();
                    }
                    FileStream objStream = new(Archivo, FileMode.Append, FileAccess.Write);
                    TextWriterTraceListener objTraceListener = new(objStream);
                    Trace.Listeners.Add(objTraceListener);
                    Trace.WriteLine(DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss:fff") + " " + Tipo + " " + Datos.ToString());

                    Trace.Flush();
                    Trace.Close();

                    objStream.Close();

                }
            }
            catch
            {
                Console.WriteLine("");
            }
        }
    }
}