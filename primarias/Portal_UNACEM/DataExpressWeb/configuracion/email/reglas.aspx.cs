using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using clibLogger;

namespace DataExpressWeb
{
    public partial class distribucion : System.Web.UI.Page
    {
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
                String pathRequLogin, pathRespLogin;
                pathRequLogin = System.Configuration.ConfigurationManager.AppSettings["pathRequLogin"];
                pathRespLogin = System.Configuration.ConfigurationManager.AppSettings["pathRespLogin"];
                if (Session["rfcCliente"] == null)
                {
                    if (Request.Path != pathRequLogin)
                    {
                        Response.Redirect(pathRespLogin);
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
        protected void btoBuscar_Click(object sender, EventArgs e)
        {
            reglasDataSource.SelectParameters["nombre"].DefaultValue = (txt_nombre.Text.Equals("") ? "-" : txt_nombre.Text);
            reglasDataSource.SelectParameters["ruc"].DefaultValue = (txt_identificacion.Text.Equals("") ? "-" : txt_identificacion.Text);
            reglasDataSource.DataBind();
            GridView1.DataBind();
        }
        protected void btoActualizar_Click(object sender, EventArgs e)
        {
            Response.Redirect("reglas.aspx", false);
        }
    }
}