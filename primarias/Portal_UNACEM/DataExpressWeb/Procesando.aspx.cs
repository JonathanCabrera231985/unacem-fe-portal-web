using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Control;
using System.Data.Common;
using clibLogger;

namespace DataExpressWeb
{
    public partial class Procesando : System.Web.UI.Page
    {
        Log log = new Log();
        String msj = "";
        string idUser;
        string codigoControl = "";
        int countTimer = 0;
        Boolean banEliminar = false;
        string user = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                user = Session["idUser"].ToString();
                if (!string.IsNullOrEmpty(user))
                {
                    ValidarPermisos vP = new ValidarPermisos();
                    vP.DB = DB;
                    bool Permiso = vP.PermisoAccederFormulario(user);
                    if (!Permiso)
                    {
                        ContentPlaceHolder mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("MainContent");
                        mpContentPlaceHolder.Visible = false;
                        string script = vP.AgregarAlertaRedireccionar();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "confirm", script.ToString(), true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertaNoPermisos()", true);
                    }
                }
                if (Session["idUser"] != null)
                {
                    idUser = Session["idUser"].ToString();
                    codigoControl = Session["codigoControl"].ToString();
                    if (!Page.IsPostBack)
                    {
                        hdCount.Value = countTimer.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogger.Graba_Log_Error(ex.Message);
                DB.Desconectar();
                throw;
            }
            finally
            {
                DB.Desconectar();
            }
        }

        protected void timer1_tick(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                banEliminar = false;
                Timer1.Enabled = false;
                hdCount.Value = (Convert.ToInt32(hdCount.Value) + 1).ToString();
                this.countTimer = this.countTimer + 1;
                DB.Conectar();
                DB.CrearComando(@"SELECT codigoControl FROM General WITH (NOLOCK)  WHERE codigoControl=@codigoControl and creado='1'");
                DB.AsignarParametroCadena("@codigoControl", codigoControl);
                using (DbDataReader DRDT = DB.EjecutarConsulta())
                {
                    if (DRDT.Read())
                    {
                        Timer1.Enabled = false;
                        banEliminar = true;
                    }
                }
                DB.Desconectar();
                if (banEliminar) { eliminarRegistros(); Response.Redirect("~/Documentos.aspx"); }
                if (hdCount.Value.Equals("5"))
                {
                    Timer1.Enabled = false;
                    eliminarRegistros();
                    Response.Redirect("~/Documentos.aspx");
                }
                Timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ex.Message);
                throw;
            }
            finally
            {
                DB.Desconectar();
            }
        }

        private void eliminarRegistros()
        {
            var DB = new BasesDatos();
            try
            {
                DB.Conectar();
                DB.CrearComando(@"DELETE FROM DestinatariosTemp WHERE id_Empleado=@id_Empleado");
                DB.AsignarParametroCadena("@id_Empleado", idUser);
                DB.EjecutarConsulta1();
                DB.Desconectar();
                DB.Conectar();
                DB.CrearComando(@"DELETE FROM DetallesTemp WHERE id_Empleado=@id_Empleado");
                DB.AsignarParametroCadena("@id_Empleado", idUser);
                DB.EjecutarConsulta1();
                DB.Desconectar();
                DB.Conectar();
                DB.CrearComando(@"DELETE FROM InfoAdicionalTemp WHERE id_Empleado=@id_Empleado");
                DB.AsignarParametroCadena("@id_Empleado", idUser);
                DB.EjecutarConsulta1();
                DB.Desconectar();
                DB.Conectar();
                DB.CrearComando(@"DELETE FROM DetallesAdicionalesTemp WHERE id_Empleado=@id_Empleado");
                DB.AsignarParametroCadena("@id_Empleado", idUser);
                DB.EjecutarConsulta1();
                DB.Desconectar();
                DB.Conectar();
                DB.CrearComando(@"DELETE FROM TotalConImpuestosTemp WHERE id_Empleado=@id_Empleado");
                DB.AsignarParametroCadena("@id_Empleado", idUser);
                DB.EjecutarConsulta1();
                DB.Desconectar();
                DB.Conectar();
                DB.CrearComando(@"DELETE FROM ImpuestosDetallesTemp WHERE id_Empleado=@id_Empleado");
                DB.AsignarParametroCadena("@id_Empleado", idUser);
                DB.EjecutarConsulta1();
                DB.Desconectar();
                DB.Conectar();
                DB.CrearComando(@"DELETE FROM MotivosDebitoTemp WHERE id_Empleado=@id_Empleado");
                DB.AsignarParametroCadena("@id_Empleado", idUser);
                DB.EjecutarConsulta1();
                DB.Desconectar();
                DB.Conectar();
                DB.CrearComando(@"DELETE FROM pagoTemp WHERE id_Empleado=@id_Empleado");
                DB.AsignarParametroCadena("@id_Empleado", idUser);
                DB.EjecutarConsulta1();
                DB.Desconectar();
            }
            catch (Exception exdelete)
            {
                msj = log.PA_mensajes("EM011")[0];
                clsLogger.Graba_Log_Error(exdelete.Message);
                log.mensajesLog("EM011", "", exdelete.Message, "Problema al Eliminar Temporales", "");
                DB.Desconectar();
            }
            finally
            {
                DB.Desconectar();
            }
        }
        private void HistoryBack()
        {
            string key = "historyback";
            string javascript = @"<SCRIPT Language=JavaScript>
                            window.onload = function() {
                              history.back()
                            };
                        </SCRIPT>";
            if (!Page.ClientScript.IsStartupScriptRegistered(key))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), key, javascript, true);
            }
        }
    }
}
