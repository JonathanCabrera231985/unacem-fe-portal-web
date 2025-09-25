using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Text;
using Control;
using System.IO;
using CriptoSimetrica;
using clibLogger;

namespace DataExpressWeb.adminstracion.empresas
{
    public partial class agregarEmpresa : System.Web.UI.Page
    {
        String IdEmpresa = "";
        ValidaRUC rucValida = new ValidaRUC();
        AES cs = new AES();
        String clavep12 = "";
        String claveprecp = "";
        String clavepsmtp = "";
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
                if (Request.QueryString["IdEmpresa"] != null)
                {
                    try
                    {
                        IdEmpresa = Request.QueryString["IdEmpresa"].ToString();
                        if (!IdEmpresa.Equals(""))
                        {
                            DB.Conectar();
                            StringBuilder xmlDocumento = new StringBuilder("");
                            xmlDocumento.Append("<INSTRUCCION>");
                            xmlDocumento.Append("<Filtro>");
                            xmlDocumento.Append("<Opcion>" + 3 + "</Opcion>");
                            xmlDocumento.Append("<IDEEMI>" + IdEmpresa + "</IDEEMI>");
                            xmlDocumento.Append("<RFCEMI></RFCEMI>");
                            xmlDocumento.Append("<NOMEMI></NOMEMI>");
                            xmlDocumento.Append("<dirMatriz></dirMatriz>");
                            xmlDocumento.Append("</Filtro>");
                            xmlDocumento.Append("</INSTRUCCION>");
                            DataSet ds = DB.TraerDataset("PA_Empresa_AMC", new Object[] { xmlDocumento.ToString() });
                            if (!IsPostBack)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    tbRUC.Text = ds.Tables[0].Rows[0]["RFCEMI"].ToString();
                                    tbRUC.ReadOnly = true;
                                    tbRazonSocial.Text = ds.Tables[0].Rows[0]["NOMEMI"].ToString();
                                    tbNombreComercial.Text = ds.Tables[0].Rows[0]["nombreComercial"].ToString();
                                    tbMatriz.Text = ds.Tables[0].Rows[0]["dirMatriz"].ToString();
                                    txtTelefonos.Text = ds.Tables[0].Rows[0]["telefonos"].ToString();
                                    txtConEspecial.Text = ds.Tables[0].Rows[0]["contribuyenteEspecial"].ToString();
                                    if (ds.Tables[0].Rows[0]["obligadollevarcontabilidad"].ToString() == "NO")
                                    {
                                        chkContabilidad.Checked = false;
                                    }
                                    else
                                    {
                                        chkContabilidad.Checked = true;
                                    }
                                    tbServidor.Text = ds.Tables[0].Rows[0]["servidorSMTP"].ToString();
                                    tbPuerto.Text = ds.Tables[0].Rows[0]["puertoSMTP"].ToString();
                                    cbSSL.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["sslSMTP"].ToString());
                                    tbusuario.Text = ds.Tables[0].Rows[0]["userSMTP"].ToString();
                                    clavepsmtp = cs.desencriptar(ds.Tables[0].Rows[0]["passSMTP"].ToString(), "CIMAIT");
                                    tbcontrasena.Attributes.Add("Value", clavepsmtp);
                                    tbemailenvio.Text = ds.Tables[0].Rows[0]["emailEnvio"].ToString();
                                    tbcorreoRecepcion.Text = ds.Tables[0].Rows[0]["correoRecepcion"].ToString();
                                    claveprecp= cs.desencriptar(ds.Tables[0].Rows[0]["passRecepcion"].ToString(), "CIMAIT");
                                    tbcontrasenaRecepcion.Attributes.Add("Value", claveprecp);
                                    tbmensajecorreo.Text = ds.Tables[0].Rows[0]["configCorreo"].ToString().Replace("__", Environment.NewLine);
                                    tbcadenaConexion.Text = ds.Tables[0].Rows[0]["ParametroConexion"].ToString();
                                    cboTipoconexion.SelectedValue = ds.Tables[0].Rows[0]["tipoConexion"].ToString();
                                    tbFirmaElectronica.Text = ds.Tables[0].Rows[0]["dirP12"].ToString();
                                    clavep12 = cs.desencriptar(ds.Tables[0].Rows[0]["passP12"].ToString(), "CIMAIT");
                                    tbClave.Attributes.Add("Value", clavep12);
                                    ImageLogo.ImageUrl = ds.Tables[0].Rows[0]["logoName"].ToString();
                                    radioConfig.SelectedValue = ds.Tables[0].Rows[0]["ConfigRecepcion"].ToString();
                                    txtURL.Text = ds.Tables[0].Rows[0]["urlexchange"].ToString();
                                    RadioExchange.SelectedValue = ds.Tables[0].Rows[0]["configExchange"].ToString();
                                    checkConfig_SelectedIndexChanged(radioConfig, null);
                                }
                            }
                            clavep12 = cs.desencriptar(ds.Tables[0].Rows[0]["passP12"].ToString(), "CIMAIT");
                            DB.Desconectar();
                        }
                        else
                            tbRUC.ReadOnly = false;
                    }
                    catch (Exception ex)
                    {
                        DB.Desconectar();
                        clsLogger.Graba_Log_Error(ex.Message);
                        Label2.Text = "Error : " + ex.Message;
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

        public string ImageToBase64_(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        protected void bGuardar_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                tbRUC_TextChanged(tbRUC, null);
                if (Label1.Text != "")
                    return;
                if (cboTipoconexion.SelectedValue == "0")
                {
                    Label2.Text = "Seleccionar la base de datos";
                    return;
                }
                StringBuilder xmlDocumento = new StringBuilder("");
                xmlDocumento.Append("<INSTRUCCION>");
                xmlDocumento.Append("<Filtro>");
                xmlDocumento.Append("<Opcion>" + (IdEmpresa.Equals("") ? "1" : "2") + "</Opcion>");
                xmlDocumento.Append("</Filtro>");
                xmlDocumento.Append("<Empresa>");
                xmlDocumento.Append("<RFCEMI>" + tbRUC.Text + "</RFCEMI>");
                xmlDocumento.Append("<NOMEMI>" + tbRazonSocial.Text + "</NOMEMI>");
                xmlDocumento.Append("<nombreComercial>" + tbNombreComercial.Text + "</nombreComercial>");
                xmlDocumento.Append("<dirMatriz>" + tbMatriz.Text + "</dirMatriz>");
                xmlDocumento.Append("<contribuyenteespecialE>" + txtConEspecial.Text + "</contribuyenteespecialE>");
                if (chkContabilidad.Checked)
                {
                    xmlDocumento.Append("<obligadoContabilidad>" + "SI" + "</obligadoContabilidad>");
                }
                else
                {
                    xmlDocumento.Append("<obligadoContabilidad>" + "NO" + "</obligadoContabilidad>");
                }
                xmlDocumento.Append("<telefonoE>" + txtTelefonos.Text + "</telefonoE>");
                xmlDocumento.Append("<dirdocs/>");
                xmlDocumento.Append("<dirXMLbase/>");
                xmlDocumento.Append("<servidorSMTP>" + tbServidor.Text + "</servidorSMTP>");
                xmlDocumento.Append("<puertoSMTP>" + tbPuerto.Text + "</puertoSMTP>");
                xmlDocumento.Append("<sslSMTP>" + cbSSL.Checked + "</sslSMTP>");
                xmlDocumento.Append("<userSMTP>" + tbusuario.Text + "</userSMTP>");
                if(!tbcontrasena.Text.Equals(""))
                {
                    xmlDocumento.Append("<passSMTP>" + cs.encriptar(tbcontrasena.Text, "CIMAIT") + "</passSMTP>");
                }
                else
                    xmlDocumento.Append("<passSMTP>" + cs.encriptar(clavepsmtp, "CIMAIT") + "</passSMTP>");
                xmlDocumento.Append("<emailEnvio>" + tbemailenvio.Text + "</emailEnvio>");
                xmlDocumento.Append("<servidorRecepcion/>");
                xmlDocumento.Append("<correoRecepcion>" + tbcorreoRecepcion.Text + "</correoRecepcion>");
                if(!tbcontrasenaRecepcion.Text.Equals(""))
                {
                    xmlDocumento.Append("<passRecepcion>" + cs.encriptar(tbcontrasenaRecepcion.Text, "CIMAIT") + "</passRecepcion>");
                }
                else
                    xmlDocumento.Append("<passRecepcion>" + cs.encriptar(claveprecp, "CIMAIT") + "</passRecepcion>");
                xmlDocumento.Append("<configCorreo>" + tbmensajecorreo.Text.Replace(Environment.NewLine, "__") + "</configCorreo>");
                xmlDocumento.Append("<ParametroConexion>" + tbcadenaConexion.Text + "</ParametroConexion>");
                xmlDocumento.Append("<tipoConexion>" + cboTipoconexion.SelectedValue + "</tipoConexion>");
                xmlDocumento.Append("<dirP12>" + tbFirmaElectronica.Text + "</dirP12>");
                if (!tbClave.Text.Equals(""))
                {
                    xmlDocumento.Append("<passP12>" + cs.encriptar(tbClave.Text, "CIMAIT") + "</passP12>");
                    clavep12 = tbClave.Text;
                }
                else
                    xmlDocumento.Append("<passP12>" + cs.encriptar(clavep12, "CIMAIT") + "</passP12>");
                xmlDocumento.Append("<ConfigRecepcion>" + radioConfig.SelectedValue + "</ConfigRecepcion>");
                xmlDocumento.Append("<urlexchange>" + txtURL.Text + "</urlexchange>");
                xmlDocumento.Append("<configExchange>" + RadioExchange.SelectedValue + "</configExchange>");
                xmlDocumento.Append("</Empresa>");
                xmlDocumento.Append("</INSTRUCCION>");
                DB.Conectar();
                DB.TraerDataset("PA_Empresa_AMC", new Object[] { xmlDocumento.ToString() });
                DB.Desconectar();
                DB.Conectar();
                DB.CrearComandoProcedimiento("PA_LogoEmpresa");
                DB.AsignarParametroProcedimiento("@RFCEMI", System.Data.DbType.String, tbRUC.Text);
                System.Drawing.Image imagenguardar = new System.Drawing.Bitmap(AppDomain.CurrentDomain.BaseDirectory.Trim('\\') + ImageLogo.ImageUrl.Replace("/", @"\"));
                byte[] myByteImage = ConvertImageToByteArray(imagenguardar, imagenguardar.RawFormat);
                DB.AsignarParametroProcedimiento("@LogoImagen", System.Data.DbType.Binary, myByteImage);
                DB.EjecutarConsulta1();
                DB.Desconectar();
                Response.Redirect("../empresas/Empresas.aspx");
            }
            catch (Exception ex)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ex.Message);
                Label2.Text = "Error : " + ex.Message;
            }
        }
        protected void tbRUC_TextChanged(object sender, EventArgs e)
        {
            if (tbRUC.Text.Length > 9)
            {
                if (!rucValida.ValidarNumeroIdentificacion(tbRUC.Text))
                    Label1.Text = "Numero de Identificación Incorrecto";
                else
                    Label1.Text = "";
            }
        }
        protected void btoImangen_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                if (FileUploadLogo.FileName.Equals(""))
                {
                    Label2.Text = "Error : No se puede visualizar la imagen porque no se a cargado ningun archivo";
                    return;
                }
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"imagenes\" + FileUploadLogo.FileName))
                {
                    FileUploadLogo.SaveAs(MapPath("/imagenes/" + FileUploadLogo.FileName));
                }
                ImageLogo.ImageUrl = "/imagenes/" + FileUploadLogo.FileName;
                DB.Conectar();
                DB.TraerDataSetConsulta(@"update EMISOR set logoName = @p1 where RFCEMI = @p2", new Object[] { "/imagenes/" + FileUploadLogo.FileName, tbRUC.Text });
                DB.Desconectar();
            }
            catch (Exception ex)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ex.Message);
                Label2.Text = "Error : " + ex.Message;
            }
        }
        public static byte[] ConvertImageToByteArray(System.Drawing.Image _image, System.Drawing.Imaging.ImageFormat _formatImage)
        {
            byte[] ImageByte;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    _image.Save(ms, _formatImage);
                    ImageByte = ms.ToArray();
                }
            }
            catch (Exception) { throw; }
            return ImageByte;
        }

        protected void checkConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
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
    }
}