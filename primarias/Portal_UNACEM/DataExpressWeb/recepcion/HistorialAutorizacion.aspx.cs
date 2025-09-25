using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Text;
using clibLogger;

namespace DataExpressWeb.recepcion
{
    public partial class HistorialAutorizacion : System.Web.UI.Page
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
            if (tbNumDoc.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "FA" + tbNumDoc.Text + separador; }
                else { consulta = "FA" + tbNumDoc.Text + separador; }
            }
            if (this.tbUsuario.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "US" + tbUsuario.Text + separador; }
                else { consulta = "US" + tbUsuario.Text + separador; }
            }
            if (this.tbRucProv.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "RF" + tbRucProv.Text + separador; }
                else { consulta = "RF" + tbRucProv.Text + separador; }
            }
            if (this.tbProveedor.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "RS" + tbProveedor.Text + separador; }
                else { consulta = "RS" + tbProveedor.Text + separador; }
            }
            if (this.tbCA.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "CA" + tbCA.Text + separador; }
                else { consulta = "CA" + tbCA.Text + separador; }
            }
            if (consulta.Length != 0)
            {
                StringBuilder filtro = new StringBuilder("");
                filtro.Append("<INSTRUCCION>");
                filtro.Append("<FILTRO>");
                filtro.Append("<opcion>3</opcion>");
                filtro.Append("<query>" + consulta + "</query>");
                filtro.Append("</FILTRO>");
                filtro.Append("</INSTRUCCION>");

                SqlDataSource1.SelectParameters["documentoXML"].DefaultValue = filtro.ToString();
                SqlDataSource1.DataBind();
                gvLog.DataBind();
                consulta = "";
            }
            else
            {
                consulta = "-";
            }
        }

        protected void bActualizar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/recepcion/HistorialAutorizacion.aspx");
        }
    }
}