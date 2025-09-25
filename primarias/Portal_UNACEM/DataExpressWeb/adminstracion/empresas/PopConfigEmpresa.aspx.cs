using System;
using System.Web.UI;
using System.Data;
using System.Text;
using Datos;
using clibLogger;
using CriptoSimetrica;

namespace DataExpressWeb.adminstracion.empresas
{
    public partial class PopConfigEmpresa : System.Web.UI.Page
    {
        string user = "";
        String claveprecp = "";
        String clavepsmtp = "";
        private AES cs = new AES();
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
                        string script = vP.AgregarAlertaRedireccionar();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "confirm", script.ToString(), true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertaNoPermisos()", true);
                    }
                }

                if (!IsPostBack)
                {
                    if (Request.QueryString["IdEmpresa"] != null)
                    {
                        DB.Conectar();
                        StringBuilder xmlDocumento = new StringBuilder("");
                        xmlDocumento.Append("<INSTRUCCION>");
                        xmlDocumento.Append("<Filtro>");
                        xmlDocumento.Append("<Opcion>" + 3 + "</Opcion>");
                        xmlDocumento.Append("<IDEEMI>" + Request.QueryString["IdEmpresa"].ToString() + "</IDEEMI>");
                        xmlDocumento.Append("<RFCEMI></RFCEMI>");
                        xmlDocumento.Append("<NOMEMI></NOMEMI>");
                        xmlDocumento.Append("<dirMatriz></dirMatriz>");
                        xmlDocumento.Append("</Filtro>");
                        xmlDocumento.Append("</INSTRUCCION>");
                        DataSet ds = DB.TraerDataset("PA_Empresa_AMC", new Object[] { xmlDocumento.ToString() });

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Page.Title = "Empresa " + ds.Tables[0].Rows[0]["NOMEMI"].ToString();
                            tbcadenaConexion.Text = ds.Tables[0].Rows[0]["ParametroConexion"].ToString();
                            clavepsmtp = cs.desencriptar(ds.Tables[0].Rows[0]["passSMTP"].ToString(), "CIMAIT");
                            tbcontrasena.Attributes.Add("Value", clavepsmtp);
                            claveprecp = cs.desencriptar(ds.Tables[0].Rows[0]["passRecepcion"].ToString(), "CIMAIT");
                            tbcontrasenaRecepcion.Attributes.Add("Value", claveprecp);
                            tbcorreoRecepcion.Text = ds.Tables[0].Rows[0]["correoRecepcion"].ToString();
                            tbemailenvio.Text = ds.Tables[0].Rows[0]["emailEnvio"].ToString();
                            tbmensajecorreo.Text = ds.Tables[0].Rows[0]["configCorreo"].ToString().Replace("__", Environment.NewLine);
                            tbPuerto.Text = ds.Tables[0].Rows[0]["puertoSMTP"].ToString();
                            tbServidor.Text = ds.Tables[0].Rows[0]["servidorSMTP"].ToString();
                            tbusuario.Text = ds.Tables[0].Rows[0]["userSMTP"].ToString();
                            cboTipoconexion.SelectedValue = ds.Tables[0].Rows[0]["tipoConexion"].ToString();
                            cbSSL.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["sslSMTP"].ToString());
                            ImageLogo.ImageUrl = ds.Tables[0].Rows[0]["logoName"].ToString();
                            radioConfig.SelectedValue = ds.Tables[0].Rows[0]["ConfigRecepcion"].ToString();
                            txtURL.Text = ds.Tables[0].Rows[0]["urlexchange"].ToString();
                            RadioExchange.SelectedValue = ds.Tables[0].Rows[0]["configExchange"].ToString();

                            Boolean VisibleControl = true;
                            if (radioConfig.SelectedValue.Trim().Equals("SMTP"))
                                VisibleControl = true;
                            else
                            {
                                VisibleControl = false;
                                txtURL.Text = "";
                            }

                            lbConfigXMTP.Visible = VisibleControl;
                            lbURL.Visible = VisibleControl;
                            txtURL.Visible = VisibleControl;
                            RadioExchange.Visible = VisibleControl;

                        }
                        DB.Desconectar();
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
    }
}