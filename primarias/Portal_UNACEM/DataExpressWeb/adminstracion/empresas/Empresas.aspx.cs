using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Text;
using clibLogger;

namespace DataExpressWeb.adminstracion.empresas
{
    public partial class Empresas : System.Web.UI.Page
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

        protected void bBuscarReg_Click(object sender, EventArgs e)
        {
            StringBuilder xmlDocumento = new StringBuilder("");
            xmlDocumento.Append("<INSTRUCCION>");
            xmlDocumento.Append("<Filtro>");
            xmlDocumento.Append("<Opcion>" + 3 + "</Opcion>");
            xmlDocumento.Append("<IDEEMI></IDEEMI>");
            xmlDocumento.Append("<RFCEMI>" + tbRuc.Text + "</RFCEMI>");
            xmlDocumento.Append("<NOMEMI>" + tbrazonSocial.Text + "</NOMEMI>");
            xmlDocumento.Append("<dirMatriz>" + tbMatriz.Text + "</dirMatriz>");
            xmlDocumento.Append("</Filtro>");
            xmlDocumento.Append("</INSTRUCCION>");

            SqlDataSource1.SelectParameters["documentoXML"].DefaultValue = xmlDocumento.ToString();
            SqlDataSource1.DataBind();
            gvDetalleEmpresa.DataBind();
        }

        protected void bActualizar_Click(object sender, EventArgs e)
        {
            StringBuilder xmlDocumento = new StringBuilder("");
            xmlDocumento.Append("<INSTRUCCION>");
            xmlDocumento.Append("<Filtro>");
            xmlDocumento.Append("<Opcion>" + 3 + "</Opcion>");
            xmlDocumento.Append("<IDEEMI></IDEEMI>");
            xmlDocumento.Append("<RFCEMI></RFCEMI>");
            xmlDocumento.Append("<NOMEMI></NOMEMI>");
            xmlDocumento.Append("<dirMatriz></dirMatriz>");
            xmlDocumento.Append("</Filtro>");
            xmlDocumento.Append("</INSTRUCCION>");

            SqlDataSource1.SelectParameters["documentoXML"].DefaultValue = xmlDocumento.ToString();
            SqlDataSource1.DataBind();
            gvDetalleEmpresa.DataBind();
        }
    }
}