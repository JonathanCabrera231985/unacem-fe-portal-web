using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using clibLogger;
using DataExpressWeb;

namespace Administracion
{
    public partial class crear_sesion : System.Web.UI.Page
    {
        string user="";
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

        protected void bCrear_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Conectar();
                DB.CrearComandoProcedimiento("PA_inserta_sesion");
                DB.AsignarParametroProcedimiento("@descripcion", System.Data.DbType.String, tbDescripcion.Text);
                DB.AsignarParametroProcedimiento("@conexiones_simultaneas", System.Data.DbType.Int16, ddlConexiones.SelectedValue);
                DB.AsignarParametroProcedimiento("@duracion_sesion", System.Data.DbType.String, ddlDuracion.SelectedValue);
                DB.AsignarParametroProcedimiento("@intentos", System.Data.DbType.Int16, ddlIntentos.SelectedValue);
                DB.EjecutarConsulta1();
                DB.Desconectar();
                Response.Redirect("sesiones.aspx");
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
    }
}