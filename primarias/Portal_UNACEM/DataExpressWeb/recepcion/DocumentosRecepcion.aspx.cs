using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.Common;
using Control;
using Ionic.Zip;
using System.Text;
using System.IO;
using clibLogger;

namespace DataExpressWeb
{
    public partial class DocumentosRecepcion : System.Web.UI.Page
    {
        private String consulta;
        private String aux;
        private String separador = "|";
        private DataTable DT = new DataTable();
        private EnviarMail EM;
        private Log log = new Log();
        String[] seleccionados;
        int cantidad;
        string servidor = "";
        int puerto = 25;
        Boolean ssl = false;
        string emailCredencial = "";
        string passCredencial = "";
        string emailEnviar = "";
        string RutaDOC = "";
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
                tbRFC.Attributes.Add("onkeyup", "changeToUpperCase(this.id)");
                tbRFC.Attributes.Add("onchange", "changeToUpperCase(this.id)");
                if (IsPostBack)
                {
                    if (Session["sucursalUser"] != null)
                    {
                    }
                    if (String.IsNullOrEmpty((String)Session["rfcCliente"]))
                    {
                        ddlSucursal.Visible = false;
                        lSucursal.Visible = false;
                        bMail.Visible = false;
                        consulta = "";
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            buscar();
        }

        private void buscar()
        {
            int fecha = 0;
            string msjbuscar = "";
            consulta = "";
            DT.Clear();
            if (tbFolioAnterior.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "FA" + tbFolioAnterior.Text + separador; }
                else { consulta = "FA" + tbFolioAnterior.Text + separador; }
            }
            if (tbNombre.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "RS" + tbNombre.Text + separador; }
                else { consulta = "RS" + tbNombre.Text + separador; }
            }
            if (tbRFC.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "RF" + tbRFC.Text + separador; }
                else { consulta = "RF" + tbRFC.Text + separador; }
            }
            if (ddlTipoDocumento.SelectedIndex != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "TD" + ddlTipoDocumento.SelectedValue + separador; }
                else { consulta = "TD" + ddlTipoDocumento.SelectedValue + separador; }
            }
            if (ddlSucursal.SelectedIndex != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "SU" + ddlSucursal.SelectedValue + separador; }
                else { consulta = "SU" + ddlSucursal.SelectedValue + separador; }
            }
            if (tbFechaInicial.Text.Length > 5 && tbFechaFinal.Text.Length > 5)
            {
                if (tbFechaInicial.Text.Length > 5)
                {
                    if (consulta.Length != 0) { consulta = consulta + "DA" + Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyyMMdd") + separador; }
                    else { consulta = "DA" + Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyyMMdd") + separador; }
                    fecha = 1;
                }
                if (tbFechaFinal.Text.Length > 5)
                {
                    if (consulta.Length != 0) { consulta = consulta + "DF" + Convert.ToDateTime(tbFechaFinal.Text).ToString("yyyyMMdd") + separador; }
                    else { consulta = "DF" + Convert.ToDateTime(tbFechaFinal.Text).ToString("yyyyMMdd") + separador; }
                    fecha = fecha + 1;
                }
            }
            if (consulta.Length > 0)
            {
                if (fecha > 1)
                {
                    consulta = consulta.Substring(0, consulta.Length - 1);
                }
                if (fecha == 1)
                {
                    msjbuscar = "Es necesario seleccionar ambas fechas.";
                }
            }
            else
            {
                consulta = "-";
            }
            StringBuilder filtro = new StringBuilder("");
            filtro.Append("<INSTRUCCION>");
            filtro.Append("<FILTRO>");
            filtro.Append("<opcion>1</opcion>");
            filtro.Append("<query>" + consulta + "</query>");
            filtro.Append("</FILTRO>");
            filtro.Append("</INSTRUCCION>");
            SqlDataSource1.SelectParameters["documentoXML"].DefaultValue = filtro.ToString();
            SqlDataSource1.DataBind();
            gvFacturas.DataBind();
            consulta = "";
            lMensaje.Text = msjbuscar;
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            Response.Redirect("DocumentosRecepcion.aspx");
        }
        protected void Button1_Click2(object sender, EventArgs e)
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
                string temp = "";
                Boolean bCHK = false;
                DB.Desconectar();
                consulta = "";
                cantidad = gvFacturas.Rows.Count;
                foreach (GridViewRow row in gvFacturas.Rows)
                {
                    CheckBox chk_Seleccionar = (CheckBox)row.FindControl("check");
                    HiddenField hd_SeleccionaPDF = (HiddenField)row.FindControl("checkHdPDF");
                    HiddenField hd_SeleccionarXML = (HiddenField)row.FindControl("checkHdXML");
                    Label l_Label2 = (Label)row.FindControl("Label2");
                    //Label l_Label4 = (Label)row.FindControl("Label4");
                    Label l_Label5 = (Label)row.FindControl("Label5");
                    Label l_Label1 = (Label)row.FindControl("Label1");
                    Label l_Label6 = (Label)row.FindControl("Label10");
                    EM = new EnviarMail();
                    EM.servidorSTMP(servidor, puerto, ssl, emailCredencial, passCredencial);
                    if (chk_Seleccionar.Checked)
                    {
                        bCHK = true;
                        emails = "";
                        emails = tbEmail.Text + "," + emails;
                        emails = emails.Trim();

                        if (checkXML.Checked)
                        {
                            EM.adjuntar_xml(getXML(hd_SeleccionarXML.Value.ToString()), l_Label5.Text + ".xml");
                        }
                        if (emails.Length > 15)
                        {
                            asunto = leeCodDoc(l_Label6.Text) + " con N° " + l_Label5.Text + " de Transoceanica S.A.";
                            mensaje = @"<br>
                            Adjunto encontrarás el Recibir tu " + leeCodDoc(l_Label6.Text) + " generada el " + l_Label1.Text + @"<br>
                            con N° " + l_Label5.Text + ".";
                            mensaje += "<br>Saludos cordiales. ";
                            mensaje += "<br>Transoceanica S.A. ";
                            mensaje += "<br><br>Servicio proporcionado por DataExpress Internacional";
                            mensaje += "<br><br>PBX. +593 (4) 2280217";
                            EM.llenarEmail(emailEnviar, emails.Trim(','), "", "", asunto, mensaje);
                            try
                            {
                                EM.enviarEmail();
                                lbMsgZip.Text = "Email enviado";
                            }
                            catch (System.Net.Mail.SmtpException ex)
                            {
                                log.mensajesLogRecepcion("EM001", "", ex.Message, "", l_Label5.Text);
                                DB.Desconectar();
                            }
                        }
                        else
                        {
                            lbMsgZip.Text = "Tienes seleccionar algún E-mail";
                        }
                    }
                }
                if (!bCHK)
                {
                    lbMsgZip.Text = "Debes seleccionar una factura";
                }
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }
        }

        protected void btnZip_Click(object sender, EventArgs e)
        {
            lbMsgZip.Text = "";
            cantidad = gvFacturas.Rows.Count;
            seleccionados = new String[cantidad];
            String mensaje = "", val_rb = "";
            Boolean bRB = false, bCHK = false, bSelect = false;
            int a = 0;
            Response.Clear();
            ZipFile zip = new ZipFile();
            using (zip) ;
            foreach (GridViewRow row in gvFacturas.Rows)
            {
                CheckBox chk_Seleccionar = (CheckBox)row.FindControl("check");
                HiddenField hd_SeleccionaPDF = (HiddenField)row.FindControl("checkHdPDF");
                HiddenField hd_SeleccionarXML = (HiddenField)row.FindControl("checkHdXML");

                if (chk_Seleccionar.Checked)
                {
                    bCHK = true;
                    if (checkXML.Checked)
                    {
                    }
                }
            }
            if (bCHK)
            {
                zip.Save(Response.OutputStream);
                Response.AddHeader("Content-Disposition", "attachment; filename=Facuras.zip");
                Response.ContentType = "application/octet-stream";
                Response.End();
            }
            else
            {
                lbMsgZip.Text = "Debes seleccionar  una factura";
            }
        }

        public string leeCodDoc(object myValue)
        {
            string rpt = "";
            if (myValue != null)
            {
                string valor = Convert.ToString(myValue);
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

        public void OpenNewWindow(string url)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format("<script>window.open('{0}');</script>", url));
        }

        public MemoryStream getXML(string claveAcceso)
        {
            MemoryStream result = new MemoryStream();
            var DB = new BasesDatos();
            try
            {
                if (!String.IsNullOrEmpty(claveAcceso))
                {
                    string ds_xml = "";
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
                        ds_xml = ds.Tables["Table"].Rows[0][16].ToString();

                        ds_xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + ds_xml;

                        byte[] toBytes = Encoding.UTF8.GetBytes(ds_xml);

                        result.Write(toBytes, 0, toBytes.Length);
                        result.Position = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ex.Message);
            }
            finally
            {
                DB.Desconectar();
            }
            return result;
        }
    }
}