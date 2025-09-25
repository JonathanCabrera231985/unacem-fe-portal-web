using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using clibLogger;
using DataExpressWeb;

namespace Administracion
{
    public partial class eliminarRol : System.Web.UI.Page
    {

        string idRol;
        string user = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
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
            try
            {
                idRol = Request.QueryString.Get("id");
                if (!String.IsNullOrEmpty(idRol))
                {
                    DB.Conectar();
                    DB.CrearComandoProcedimiento("PA_eliminar_rol");
                    DB.AsignarParametroProcedimiento("@idRol", System.Data.DbType.String, idRol);
                    DB.EjecutarConsulta1();
                    DB.Desconectar();
                    Response.Redirect("roles.aspx");

                }
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