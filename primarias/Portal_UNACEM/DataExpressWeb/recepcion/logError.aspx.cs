using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Text;
using clibLogger;

namespace DataExpressWeb.recepcion
{
    public partial class logError : System.Web.UI.Page
    {
        string consulta;
        string separador;
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
            if (this.tbNoDoc.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "FA" + tbNoDoc.Text + separador; }
                else { consulta = "FA" + tbNoDoc.Text + separador; }
            }
            if (this.tbRucProv.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "RF" + tbRucProv.Text + separador; }
                else { consulta = "RF" + tbRucProv.Text + separador; }
            }
            if (ddlTipoDocumento.SelectedIndex != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "TD" + ddlTipoDocumento.SelectedValue + separador; }
                else { consulta = "TD" + ddlTipoDocumento.SelectedValue + separador; }
            }
            if (this.tbClaveAcceso.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "CA" + tbClaveAcceso.Text + separador; }
                else { consulta = "CA" + tbClaveAcceso.Text + separador; }
            }
            if (consulta.Length != 0)
            {
                consulta = consulta.Substring(0, consulta.Length - 1);
            }
            else
            {
                consulta = "-";
            }
            StringBuilder filtro = new StringBuilder("");
            filtro.Append("<INSTRUCCION>");
            filtro.Append("<FILTRO>");
            filtro.Append("<opcion>4</opcion>");
            filtro.Append("<query>" + consulta + "</query>");
            filtro.Append("</FILTRO>");
            filtro.Append("</INSTRUCCION>");
            SqlDataSource1.SelectParameters["documentoXML"].DefaultValue = filtro.ToString();
            SqlDataSource1.DataBind();
            gvLog.DataBind();
            consulta = "";
        }

        protected void bActualizar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/recepcion/logError.aspx");
        }
    }
}