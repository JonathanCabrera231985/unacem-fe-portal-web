using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using clibLogger;

namespace DataExpressWeb.configuracion.log
{
    public partial class logError : System.Web.UI.Page
    {
        string consulta;
        string separador;
        string user;
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

        protected void bBuscarReg_Click(object sender, EventArgs e)
        {
            separador = "|";
            consulta = "";
            if (tbNoOrden.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "NO" + tbNoOrden.Text + separador; }
                else { consulta = "NO" + tbNoOrden.Text + separador; }
            }
            if (tbArchivo.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "AR" + tbArchivo.Text + separador; }
                else { consulta = "AR" + tbArchivo.Text + separador; }
            }
            if (Txt_Cod_documento.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "CO" + Txt_Cod_documento.Text + separador; }
                else { consulta = "CO" + Txt_Cod_documento.Text + separador; }
            }
            if (consulta.Length != 0)
            {
                consulta = consulta.Substring(0, consulta.Length - 1);
                SqlDataSource1.SelectParameters["QUERY"].DefaultValue = consulta;
                SqlDataSource1.DataBind();
                gvLog.DataBind();
                consulta = "";
            }
        }

        protected void bActualizar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/configuracion/log/logError.aspx");
        }
    }
}