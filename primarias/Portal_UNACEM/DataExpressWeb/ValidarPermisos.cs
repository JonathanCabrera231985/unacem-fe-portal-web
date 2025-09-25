using clibLogger;
using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace DataExpressWeb
{
    public class ValidarPermisos : System.Web.UI.Page
    {
        public BasesDatos DB = new BasesDatos();

        public bool PermisoAccederFormulario(string iduser)
        {
            var DB = new BasesDatos();
            string user = "";
            user = Session["rfcUser"].ToString();
            int IdRol = ObtenerIdRol(user, iduser);
            bool Permiso = false;
            try
            {

                DB.Conectar();
                if (IdRol > 0)
                {
                    DB.Conectar();
                    DataSet ds = DB.TraerDataset("PA_ConsultaRol", new Object[] { IdRol.ToString() });
                    DataTable dt = ds.Tables[0];
                    DB.Desconectar();
                    string path = "~" + HttpContext.Current.Request.Url.AbsolutePath;
                    String pathMen;
                    pathMen = System.Configuration.ConfigurationManager.AppSettings["pathMenu"];
                    foreach (DataRow drDataRow in dt.Rows)
                    {
                        string pathMenu = drDataRow[5].ToString().Replace("~", pathMen);
                        if (pathMenu.Contains(path))
                        {
                            Permiso = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ex.Message);

            }
            finally
            {
                DB.Desconectar();
            }

            return Permiso;
        }

        public int ObtenerIdRol(string user, string idUser)
        {
            var DB = new BasesDatos();
            int? idRol = null;
            try
            {

                if (idRol == null)
                {
                    DB.Conectar();
                    DB.CrearComandoProcedimiento("PA_ValidarRol");
                    DB.AsignarParametroProcedimiento("@USER", System.Data.DbType.String, user);
                    DB.AsignarParametroProcedimiento("@idUSER", System.Data.DbType.String, idUser);
                    using (DbDataReader DRR = DB.EjecutarConsulta())
                    {
                        DRR.Read();
                        if (DRR[0] != null)
                            idRol = Convert.ToInt32(DRR[0].ToString());
                    }

                    DB.Desconectar();
                }
            }
            catch (Exception ex)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ex.Message);

            }
            finally
            {
                DB.Desconectar();
            }

            return Convert.ToInt32(idRol);
        }

        public string AgregarAlertaRedireccionar()
        {
            String pathDefault;
            pathDefault = System.Configuration.ConfigurationManager.AppSettings["pathRespDefault"];
            string script = @"function alertaNoPermisos() {
                window.location.href = """+ pathDefault + @""";
                alert(""No posee permisos para acceder a este formulario."");                
                }";
            return script;
        }
    }
}