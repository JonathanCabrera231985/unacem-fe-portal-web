using ClibLogger;
using swConsultaDoc.Api.Services.Contracts;
using swConsultaDoc.Api.Util;
using swConsultaDoc.Data.Interfaces;
using swConsultaDoc.Domain;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Xml;

namespace swConsultaDoc.Api.Services
{
    public class DocumentoService : ServiBase, IDocumentoService
    {
        #region Atributos
        static string GetActualAsyncMethodName([CallerMemberName] string name = null) => name;
        string className = "";
        private readonly IConsultaDoc con;
        private readonly ILoggerManager _logger;
        private readonly IConfiguration config;
        #endregion

        #region Constructor
        public DocumentoService(IConfiguration config, IConsultaDoc con, ILoggerManager loggerManager)
        {
            _logger = loggerManager;
            this.con = con;
            this.config = config;
        }

        #endregion

        public Response<JsonElement?> ObtieneDocumentoxIdSistema(ConsultarxIdSistemaRequest datos)
        {
            #region LOG
            string _metodo = GetActualAsyncMethodName();
            _logger.LogInformation(String.Format("ClassName: {0} - Metodo: {1} --> INICIA", className, _metodo));
            #endregion
            
            Response<JsonElement?> respuesta = new();
            string mensajeerror = "";
            List<dynamic> RespDynamic;

            try
            {
                #region Validacion Parametro
                if (string.IsNullOrEmpty(datos.RucEmisor))
                {
                    mensajeerror = "Debe Ingresar el RucEmisor.";
                }
                else if (string.IsNullOrEmpty(datos.IdSistema))
                {
                    mensajeerror = "Debe Ingresar el Id del sistema.";
                }

                if (!string.IsNullOrEmpty(mensajeerror))
                {
                    respuesta.Mensaje = mensajeerror;

                    return respuesta;
                }
                #endregion

                _logger.LogInformation(String.Format("ClassName: {0} - Metodo: {1} --> Leyendo campos desde appsettings", className, _metodo));
                IConfigurationSection campos = config.GetSection("Campos");
                Dictionary<string, string> camposConsultaDocumento = campos.GetSection("ConsultaDocumento").Get<Dictionary<string, string>>();


                XmlDocument xmlParamObtienexIdSistema = new XmlDocument();
                xmlParamObtienexIdSistema.LoadXml("<Root />");
                xmlParamObtienexIdSistema.DocumentElement.SetAttribute("Trx", "1");
                xmlParamObtienexIdSistema.DocumentElement.SetAttribute("IdSistema", datos.IdSistema);

                RespDynamic = con.GetConsulta<dynamic>(datos.RucEmisor, xmlParamObtienexIdSistema.OuterXml, ref mensajeerror).ToList();

                if (!String.IsNullOrEmpty(mensajeerror))
                {
                    respuesta.Estado = "ERROR";
                    respuesta.Mensaje = mensajeerror;
                    respuesta.Data = null;
                }
                else
                {
                    if (RespDynamic != null && RespDynamic.Count == 0)
                    {
                        respuesta.Estado = "OK";
                        respuesta.Mensaje = "No se encontraron Datos.";
                    }
                    else if (RespDynamic != null && RespDynamic.Count > 0)
                    {
                        respuesta.Estado = "OK";
                        var data = MapeoDynamico(camposConsultaDocumento, RespDynamic);
                        respuesta.Data = data;
                    }
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                _logger.LogError(String.Format("ClassName: {0} - Metodo: {1} -- Error: {2}", className, _metodo, ex.Message));
                respuesta.Estado = "ERROR";
                respuesta.Mensaje = "Ocurrio un Error Interno";
            }
            finally
            {
                _logger.LogInformation(String.Format("ClassName: {0} - Metodo: {1} --> FIN", className, _metodo));
            }
            return respuesta;
        }

        public Response<JsonElement?> ObtieneDocumentoxNumDoc(ConsultarxNumDocRequest datos)
        {
            #region LOG
            string _metodo = GetActualAsyncMethodName();
            _logger.LogInformation(String.Format("ClassName: {0} - Metodo: {1} --> INICIA", className, _metodo));
            #endregion

            Response<JsonElement?> respuesta = new();
            string mensajeerror = "";
            List<dynamic> RespDynamic;

            try
            {

                #region Validacion Parametro

                if (string.IsNullOrEmpty(datos.RucEmisor))
                {
                    mensajeerror = "Debe Ingresar el RucEmisor.";
                }
                else if (string.IsNullOrEmpty(datos.NumDocumento))
                {
                    mensajeerror = "Debe Ingresar el Número del documento.";
                }
                else if (string.IsNullOrEmpty(datos.TipoDocumento))
                {
                    mensajeerror = "Debe Ingresar el Tipo del Documento.";
                }

                if (!string.IsNullOrEmpty(mensajeerror))
                {
                    respuesta.Mensaje = mensajeerror;

                    return respuesta;
                }
                #endregion Validacion Parametro

                _logger.LogInformation(String.Format("ClassName: {0} - Metodo: {1} --> Leyendo campos desde appsettings", className, _metodo));
                IConfigurationSection campos = config.GetSection("Campos");
                Dictionary<string, string> camposConsultaDocumento = campos.GetSection("ConsultaDocumento").Get<Dictionary<string, string>>();


                XmlDocument xmlParamObtienexIdSistema = new XmlDocument();
                xmlParamObtienexIdSistema.LoadXml("<Root />");
                xmlParamObtienexIdSistema.DocumentElement.SetAttribute("Trx", "2");
                xmlParamObtienexIdSistema.DocumentElement.SetAttribute("codDoc", datos.TipoDocumento);
                xmlParamObtienexIdSistema.DocumentElement.SetAttribute("numDoc", datos.NumDocumento);

                RespDynamic = con.GetConsulta<dynamic>(datos.RucEmisor, xmlParamObtienexIdSistema.OuterXml, ref mensajeerror).ToList();

                if (!String.IsNullOrEmpty(mensajeerror))
                {
                    respuesta.Estado = "ERROR";
                    respuesta.Mensaje = mensajeerror;
                    respuesta.Data = null;
                }
                else
                {
                    if (RespDynamic != null && RespDynamic.Count == 0)
                    {
                        respuesta.Estado = "OK";
                        respuesta.Mensaje = "No se encontraron Datos.";
                    }
                    else if (RespDynamic != null && RespDynamic.Count > 0)
                    {
                        respuesta.Estado = "OK";
                        var data = MapeoDynamico(camposConsultaDocumento, RespDynamic);
                        respuesta.Data = data;
                    }
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                _logger.LogError(String.Format("ClassName: {0} - Metodo: {1} -- Error: {2}", className, _metodo, ex.Message));
                respuesta.Estado = "ERROR";
                respuesta.Mensaje = "Ocurrio un Error Interno";
            }
            finally
            {
                _logger.LogInformation(String.Format("ClassName: {0} - Metodo: {1} --> FIN", className, _metodo));
            }
            return respuesta;

        }

        public Response<JsonElement?> ObtieneDocumentoxRangoFecha(ConsultarxRangoFechaRequest datos)
        {
            #region LOG
            string _metodo = GetActualAsyncMethodName();
            _logger.LogInformation(String.Format("ClassName: {0} - Metodo: {1} --> INICIA", className, _metodo));
            #endregion

            Response<JsonElement?> respuesta = new();
            string mensajeerror = "";
            List<dynamic> RespDynamic;

            try
            {
                _logger.LogInformation(String.Format("ClassName: {0} - Metodo: {1} --> Leyendo campos desde appsettings", className, _metodo));
                IConfigurationSection campos = config.GetSection("Campos");
                Dictionary<string, string> camposConsultaDocumento = campos.GetSection("ConsultaDocumento").Get<Dictionary<string, string>>();

                int numDias = Convert.ToInt32(config["RangoDias"]);

                #region Validacion Parametros

                if (string.IsNullOrEmpty(datos.RucEmisor))
                {
                    mensajeerror = "Debe Ingresar el RucEmisor.";
                }
                else if (string.IsNullOrEmpty(datos.TipoDocumento))
                {
                    mensajeerror = "Debe Ingresar el Tipo del documento.";
                }
                else if (!ValidaFormatFecha(datos.FechaInicio))
                {
                    mensajeerror = "Debe Ingresar la Fecha de Inicio con el formato ddMMyyyy.";
                }
                else if (!ValidaFormatFecha(datos.FechaFin))
                {
                    mensajeerror = "Debe Ingresar la Fecha de Fin con el formato ddMMyyyy.";
                }
                if (!string.IsNullOrEmpty(mensajeerror))
                {
                    respuesta.Mensaje = mensajeerror;

                    return respuesta;
                }
                string validacionFecha = ValidarFechas(datos.FechaInicio, datos.FechaFin, numDias);
                if (!string.IsNullOrEmpty(validacionFecha))
                {
                    respuesta.Mensaje = validacionFecha;

                    return respuesta;
                }

                #endregion

              

                XmlDocument xmlParamObtienexIdSistema = new XmlDocument();
                xmlParamObtienexIdSistema.LoadXml("<Root />");
                xmlParamObtienexIdSistema.DocumentElement.SetAttribute("Trx", "3");
                xmlParamObtienexIdSistema.DocumentElement.SetAttribute("codDoc", datos.TipoDocumento);
                xmlParamObtienexIdSistema.DocumentElement.SetAttribute("FechaInicio", ConvertirFechaASql(datos.FechaInicio));
                xmlParamObtienexIdSistema.DocumentElement.SetAttribute("FechaFin", ConvertirFechaASql(datos.FechaFin));
                xmlParamObtienexIdSistema.DocumentElement.SetAttribute("estado", datos.Estado);

                RespDynamic = con.GetConsulta<dynamic>(datos.RucEmisor, xmlParamObtienexIdSistema.OuterXml, ref mensajeerror).ToList();

                if (!String.IsNullOrEmpty(mensajeerror))
                {
                    respuesta.Estado = "ERROR";
                    respuesta.Mensaje = mensajeerror;
                    respuesta.Data = null;
                }
                else
                {
                    if (RespDynamic != null && RespDynamic.Count == 0)
                    {
                        respuesta.Estado = "OK";
                        respuesta.Mensaje = "No se encontraron Datos.";
                    }
                    else if (RespDynamic != null && RespDynamic.Count > 0)
                    {
                        respuesta.Estado = "OK";
                        var data = MapeoDynamico(camposConsultaDocumento, RespDynamic);
                        respuesta.Data = data;
                    }
                }

                return respuesta;
            }
            catch (Exception ex)
            {
                _logger.LogError(String.Format("ClassName: {0} - Metodo: {1} -- Error: {2}", className, _metodo, ex.Message));
                respuesta.Estado = "ERROR";
                respuesta.Mensaje = "Ocurrio un Error Interno";
            }
            finally
            {
                _logger.LogInformation(String.Format("ClassName: {0} - Metodo: {1} --> FIN", className, _metodo));
            }
            return respuesta;
        }



    }
}
