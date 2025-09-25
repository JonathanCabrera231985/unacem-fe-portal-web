using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using clibLogger;
using DataExpressWeb;

namespace Administracion
{
    public partial class eliminarSucursal : System.Web.UI.Page
    {
        string idSucursal;
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
                idSucursal = Request.QueryString.Get("id");
                if (!String.IsNullOrEmpty(idSucursal))
                {
                    DB.Conectar();
                    DB.CrearComandoProcedimiento("PA_eliminar_sucursal");
                    DB.AsignarParametroProcedimiento("@idSucursal", System.Data.DbType.String, idSucursal);
                    DB.EjecutarConsulta1();
                    DB.Desconectar();
                    Response.Redirect("sucursales.aspx");
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