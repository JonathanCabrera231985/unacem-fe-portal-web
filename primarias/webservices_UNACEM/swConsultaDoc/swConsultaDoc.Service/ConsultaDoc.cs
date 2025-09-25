using ClibLogger;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using swConsultaDoc.Data.Interfaces;
using swConsultaDoc.Domain;
using System.Data;
using System.Runtime.CompilerServices;

namespace swConsultaDoc.Data
{
    public class ConsultaDoc : BaseDatos, IConsultaDoc
    {
        #region Atributos
        static string GetActualAsyncMethodName([CallerMemberName] string name = null) => name;
        string className = "";
        #endregion

        private readonly string P_Documentos = "PA_CONSULTA_FE";
        private readonly string P_DimeUsuario = "PA_DimeUsuario";
        private string RucUnacem ;
        private string RucCantivol;

        public ConsultaDoc(IConfiguration _config, ILoggerManager loggerManager) : base(_config, loggerManager)
        {
            className = GetType().Name;
            RucUnacem = _config["RucUnacem"];
            RucCantivol = _config["RucCantivol"];
        }

        public List<T> GetConsulta<T>(string RucEmisor, string xmlContent, ref string mensajeerror)
        {
            #region LOG
            string _metodo = GetActualAsyncMethodName();
            _logger.LogInformation(String.Format("ClassName: {0} - Metodo: {1} --> INICIA", className, _metodo));
            #endregion

            List<T> resultado =  new();

            try
            { 

                using IDbConnection db = new SqlConnection(ObtieneConexion(RucEmisor, ref mensajeerror));
                if (!string.IsNullOrEmpty(mensajeerror))
                {
                    return resultado;
                }
                db.Open();

                var parameters = new DynamicParameters();
                parameters.Add("@PI_ParamXML", xmlContent, DbType.String, ParameterDirection.Input);

                resultado = db.Query<T>(P_Documentos, commandType: CommandType.StoredProcedure, param: parameters).ToList();

                db.Close();

            }
            catch (Exception ex)
            {
                _logger.LogError(String.Format("ClassName: {0} -- Metodo: {1} -- Error Consultando base de datos: {2} -- Criterio1: {3}", className, _metodo, ex.Message, xmlContent));
                mensajeerror = "Ocurrio un Error Interno";
                resultado = new List<T>();
            }
            return resultado;

        }

        public UsuarioModel? ValidaUsuario(UsuarioRequestModel Datos, ref string mensajeerror)
        {
            #region LOG
            string _metodo = GetActualAsyncMethodName();
            _logger.LogInformation(String.Format("ClassName: {0} - Metodo: {1} --> INICIA", className, _metodo));
            #endregion

            UsuarioModel? resultado = new();
            try
            {

                using IDbConnection db = new SqlConnection(connectionUNACEM);

                db.Open();
                var parameters = new DynamicParameters();

                parameters.Add("Usuario", Datos.Username, DbType.String, ParameterDirection.Input);
                resultado = db.Query<UsuarioModel>(P_DimeUsuario, commandType: CommandType.StoredProcedure, param: parameters).FirstOrDefault();

                db.Close();

            }
            catch (Exception ex)
            {
                _logger.LogError(String.Format("ClassName: {0} -- Metodo: {1} -- Error Consultando base de datos: {2}", className, _metodo, ex.Message));
                mensajeerror = "Ocurrio un Error Interno";
            }
            return resultado;

        }

        public string ObtieneConexion(String Ruc, ref string mensajeerror)
        {
            if (RucUnacem.Equals(Ruc))
            {
                return connectionUNACEM;
            }
            else if (RucCantivol.Equals(Ruc))
            {
                return connectionCANTIVOL;
            }
            else
            {
                mensajeerror = "RucEmisor no Válido";
                return "";
            }

       }
    }
}
