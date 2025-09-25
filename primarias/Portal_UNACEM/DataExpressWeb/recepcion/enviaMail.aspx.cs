using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Datos;
using System.IO;
using System.Data.Common;
using Control;
using clibLogger;

namespace DataExpressWeb.recepcion
{
    public partial class enviaMail : System.Web.UI.Page
    {
        string servidor = "";
        int puerto = 25;
        Boolean ssl = false;
        string emailCredencial = "";
        string passCredencial = "";
        string emailEnviar = "";
        string RutaDOC = "";
        string mailProv = "", estab = "", ptoEmi = "", secuencial = "";
        private byte[] docByte;
        String ds_xml = "", version = "", claveAcceso = "", numAutorizacion = "", fechaAut = "", codDoc = "", tipo = "";
        private EnviarMail EM;
        string user = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            ds_xml = ""; version = ""; claveAcceso = ""; numAutorizacion = ""; fechaAut = ""; codDoc = ""; tipo = "";
            claveAcceso = Request.QueryString.Get("CA");
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
                if (!String.IsNullOrEmpty(claveAcceso))
                {
                    StringBuilder filtro = new StringBuilder("");
                    filtro.Append("<INSTRUCCION>");
                    filtro.Append("<FILTRO>");
                    filtro.Append("<opcion>1</opcion>");
                    filtro.Append("<query>CA" + claveAcceso + "</query>");
                    filtro.Append("</FILTRO>");
                    filtro.Append("</INSTRUCCION>");
                    DB.Conectar();
                    DataSet ds = DB.TraerDataset("SP_Recepcion", filtro.ToString());
                    DB.Desconectar();
                    if (ds.Tables["Table"].Rows.Count > 0)
                    {
                        version = ds.Tables["Table"].Rows[0][4].ToString();
                        codDoc = ds.Tables["Table"].Rows[0][10].ToString();
                        estab = ds.Tables["Table"].Rows[0][11].ToString();
                        ptoEmi = ds.Tables["Table"].Rows[0][12].ToString();
                        secuencial = ds.Tables["Table"].Rows[0][13].ToString();
                        numAutorizacion = ds.Tables["Table"].Rows[0][14].ToString();
                        fechaAut = ds.Tables["Table"].Rows[0][15].ToString();
                        ds_xml = ds.Tables["Table"].Rows[0][16].ToString();
                        mailProv = ds.Tables["Table"].Rows[0][17].ToString();
                        ds_xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + ds_xml;
                        docByte = Encoding.UTF8.GetBytes(ds_xml);
                        if (!Page.IsPostBack)
                        {
                            this.txtMailProv.Text = mailProv;
                            this.txtAsunto.Text = "Observaciones a la " + leeCodDoc(codDoc) + " No: " + estab + ptoEmi + secuencial + " de UNACEM ECUADOR S.A.";
                        }
                    }
                    ds.Dispose();
                }
                this.txtMensaje.Focus();
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }
        }


        protected void btnMail_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Conectar();
                DB.CrearComando("select servidorSMTP,puertoSMTP,sslSMTP,userSMTP,passSMTP,dirdocs,emailEnvio from ParametrosSistema WITH (NOLOCK) ");
                using (DbDataReader DR1 = DB.EjecutarConsulta())
                {
                    while (DR1.Read())
                    {
                        servidor = DR1[0].ToString();
                        puerto = Convert.ToInt32(DR1[1].ToString());
                        ssl = Convert.ToBoolean(DR1[2].ToString());
                        emailCredencial = DR1[3].ToString();
                        passCredencial = DR1[4].ToString();
                        RutaDOC = DR1[5].ToString();
                        emailEnviar = DR1[6].ToString();
                    }
                }
                DB.Desconectar();
                string emails = "";
                string asunto = "";
                string mensaje = "";
                DB.Desconectar();
                EM = new EnviarMail();
                EM.servidorSTMP(servidor, puerto, ssl, emailCredencial, passCredencial);
                MemoryStream result = new MemoryStream();
                result.Write(docByte, 0, docByte.Length);
                result.Position = 0;
                EM.adjuntar_xml(result, codDoc + estab + ptoEmi + secuencial + ".xml");
                emails = this.txtMailProv.Text;
                if (emails.Length > 15)
                {
                    asunto = txtAsunto.Text;
                    mensaje = txtMensaje.Text;
                    EM.llenarEmail(emailEnviar, emails.Trim(','), "", "", asunto, mensaje);
                    try
                    {
                        EM.enviarEmail();
                        lblMensaje.Text = "Email enviado";
                        lblMensaje.Visible = true;
                    }
                    catch (System.Net.Mail.SmtpException ex)
                    {
                        lblMensaje.Text = "Error en enviar mail: " + ex.Message;
                        lblMensaje.Visible = true;
                    }
                }
                else
                {
                    this.lblMensaje.Text = "Tienes ingresar algún E-mail";
                    lblMensaje.Visible = true;
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

        public string leeCodDoc(string valor)
        {
            string rpt = "";
            if (!string.IsNullOrEmpty(valor))
            {
                switch (valor)
                {
                    case "01":
                        rpt = "FACTURA";
                        break;
                    case "04":
                        rpt = "NOTA DE CRÉDITO";
                        break;
                    case "05":
                        rpt = "NOTA DE DÉBITO";
                        break;
                    case "07":
                        rpt = "COMPROBANTE DE RETENCIÓN";
                        break;
                    case "06":
                        rpt = "GUIA DE REMISIÓN";
                        break;
                }
            }
            return rpt;
        }
    }
}