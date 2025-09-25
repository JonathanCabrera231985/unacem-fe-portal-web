using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.Common;
using Control;
using Ionic.Zip;
using System.IO;
using System.Configuration;
using clibLogger;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace DataExpressWeb
{
    public partial class Documentos : Generic
    {
        private String consulta;
        private String aux;
        private String separador = "|";
        private DataTable DT = new DataTable();
        private BasesDatos DB = new BasesDatos();
        private Log logg1 = new Log();
        private EnviarMail EM;
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
        private string compania = "UNACEM ECUADOR S.A.";
        static public string historicad = "";
        private CrearPDF cPDF;
        private CrearPDFold cPDFold;
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
                Boolean a = true;
                if (IsPostBack)
                {
                    if (Session["sucursalUser"] != null)
                    {
                    }
                    a = false;
                    if (String.IsNullOrEmpty((String)Session["rfcCliente"]))
                    {
                        ddlSucursal.Visible = false;
                        lSucursal.Visible = false;
                        bMail.Visible = false;
                        consulta = "";
                    }
                }
                historicad = "1";
                DataFilter11.DataSource = SqlDataSource1;
                DataFilter11.DataColumns = gvFacturas.Columns;
                DataFilter11.FilterSessionID = "Documentos.aspx";
                DataFilter11.OnFilterAdded += new DataFilter1.RefreshDataGridView(DataFilter1_OnFilterAdded);
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
            try
            {
                int fecha = 0;
                string msjbuscar = "";
                consulta = "";
                historicad = "";
                DT.Clear();
                if (this.ddlAut.SelectedValue.Equals("1"))
                {
                    if (consulta.Length != 0) { consulta = consulta + "NNIS NOT NULL" + separador; }
                    else { consulta = "NNIS NOT NULL" + separador; }
                }
                if (this.ddlAut.SelectedValue.Equals("2"))
                {
                    if (consulta.Length != 0) { consulta = consulta + "NAIS NULL" + separador; }
                    else { consulta = "NAIS NULL" + separador; }
                }
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
                if (Convert.ToBoolean(Session["coFactTodas"])) { aux = "1"; } else { aux = "0"; }
                SqlDataSource1.SelectParameters["QUERY"].DefaultValue = consulta;
                SqlDataSource1.SelectParameters["SUCURSAL"].DefaultValue = Session["sucursalUser"].ToString();
                SqlDataSource1.SelectParameters["RFC"].DefaultValue = Session["rfcCliente"].ToString();
                SqlDataSource1.SelectParameters["ROL"].DefaultValue = aux;
                SqlDataSource1.SelectParameters["IDEMISOR"].DefaultValue = Session["RucEmpresa"].ToString();
                SqlDataSource1.SelectParameters["USUARIO"].DefaultValue = Session["idUser"].ToString();
                SqlDataSource1.DataBind();
                gvFacturas.DataBind();
                if (gvFacturas.PageCount >= 1)
                {
                    historicad = "1";
                    consulta = "";
                    lMensaje.Text = msjbuscar;

                }
                else
                {
                    SqlDataSource1.ConnectionString = ConfigurationManager.AppSettings.Get("CADENA_CONEXION_HISTORICA");
                    if (Convert.ToBoolean(Session["coFactTodas"])) { aux = "1"; } else { aux = "0"; }
                    SqlDataSource1.SelectParameters["QUERY"].DefaultValue = consulta;
                    SqlDataSource1.SelectParameters["SUCURSAL"].DefaultValue = Session["sucursalUser"].ToString();
                    SqlDataSource1.SelectParameters["RFC"].DefaultValue = Session["rfcCliente"].ToString();
                    SqlDataSource1.SelectParameters["ROL"].DefaultValue = aux;
                    SqlDataSource1.DataBind();
                    gvFacturas.DataBind();
                    consulta = "";
                    lMensaje.Text = msjbuscar;
                    historicad = "2";
                }
            }
            catch (Exception e)
            {
                clsLogger.Graba_Log_Error(e.Message + "-" + consulta + "-" + Session["sucursalUser"].ToString() + "-" + aux + "-" + Session["rfcCliente"].ToString());
            }
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            Response.Redirect("Documentos.aspx", false);
        }

        protected void Button1_Click2(object sender, EventArgs e)
        {
            envia_mail();
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

        protected void btnZip_Click(object sender, EventArgs e)
        {
            lbMsgZip.Text = "";
            cantidad = gvFacturas.Rows.Count;
            seleccionados = new String[cantidad];
            Boolean bCHK = false;
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
                    if (checkPDF.Checked)
                    {
                        zip.AddItem(System.AppDomain.CurrentDomain.BaseDirectory + hd_SeleccionaPDF.Value, "Archivos");
                    }
                    if (checkXML.Checked)
                    {
                        zip.AddItem(System.AppDomain.CurrentDomain.BaseDirectory + hd_SeleccionarXML.Value, "Archivos");
                    }
                }
            }
            if (bCHK)
            {
                zip.Save(Response.OutputStream);
                Response.AddHeader("Content-Disposition", "attachment; filename=Facturas.zip");
                Response.ContentType = "application/octet-stream";
                Response.End();
            }
            else
            {
                lbMsgZip.Text = "Debes seleccionar una factura";
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
            }
        }

        void DataFilter1_OnFilterAdded()
        {
            try
            {
                DataFilter11.FilterSessionID = "Documentos.aspx";
                DataFilter11.FilterDataSource();
                gvFacturas.DataBind();
            }
            catch (Exception e)
            {
            }
        }

        protected void gvFacturas_PageIndexChanged(object sender, EventArgs e)
        {
            DataFilter1_OnFilterAdded();
        }

        protected void gvFacturas_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataFilter1_OnFilterAdded();
        }

        protected void check_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void envia_mail()
        {
            var DB = new BasesDatos();
            try
            {
                var aes = new CriptoSimetrica.AES();
                cPDF = new CrearPDF();
                cPDFold = new CrearPDFold();
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
                        passCredencial = aes.desencriptar(DR1[4].ToString(), "CIMAIT");
                        RutaDOC = DR1[5].ToString();
                        emailEnviar = DR1[6].ToString();
                    }
                }               
                DB.Desconectar();
                string emails = "";
                string asunto = "";
                string mensaje = "";
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
                    Label l_Label5 = (Label)row.FindControl("Label5");
                    Label l_Label1 = (Label)row.FindControl("Label1");
                    Label l_Label10 = (Label)row.FindControl("Label10");
                    Label l_Label3 = (Label)row.FindControl("Label3");
                    EM = new EnviarMail();
                    EM.servidorSTMP(servidor, puerto, ssl, emailCredencial, passCredencial);
                    if (chk_Seleccionar.Checked)
                    {
                        bCHK = true;
                        emails = "";
                        if (chkReglas.Checked)
                        {
                            if (!l_Label2.Text.Trim().Equals("9999999999999"))
                            {
                                DB.Conectar();
                                DB.CrearComando("select emailsRegla from EmailsReglas WITH (NOLOCK)   where Receptor=@rfcrec and estadoRegla=1");
                                DB.AsignarParametroCadena("@rfcrec", l_Label2.Text.Trim());
                                using (DbDataReader DR3 = DB.EjecutarConsulta())
                                {
                                    if (DR3.Read())
                                    {
                                        emails = DR3[0].ToString();
                                    }
                                }
                                DB.Desconectar();
                            }
                        }
                        emails = tbEmail.Text + "," + emails;
                        emails = emails.Trim();
                        emails = emails.Trim(',');
                        if (emails.Length > 15)
                        {
                            if (checkPDF.Checked)
                            {
                                if (Documentos.historicad.Equals("2"))
                                {
                                    EM.adjuntar_xml(cPDFold.msPDF(hd_SeleccionaPDF.Value.ToString().Replace("docus/", "").Replace(".pdf", "")), hd_SeleccionaPDF.Value.ToString().Replace("docus/", ""));
                                }
                                else
                                {
                                    EM.adjuntar_xml(cPDF.msPDF(hd_SeleccionaPDF.Value.ToString().Replace("docus/", "").Replace(".pdf", "")), hd_SeleccionaPDF.Value.ToString().Replace("docus/", ""));
                                }
                            }
                            if (checkXML.Checked)
                            {
                                if (Documentos.historicad.Equals("2"))
                                {
                                    //EM.adjuntar(RutaDOC + hd_SeleccionarXML.Value.ToString().Replace("docus/", ""));
                                    //m.Attachments.Add(new System.Net.Mail.Attachment(RutaDOC + hd_SeleccionarXML.Value.ToString().Replace("docus/", "")));
                                    EM.adjuntar_xml(consulta_archivo_xml(hd_SeleccionarXML.Value.ToString().Replace("docus/", "").Replace(".xml", ""), 7), hd_SeleccionarXML.Value.ToString().Replace("docus/", ""));
                                }
                                else
                                {
                                    EM.adjuntar_xml(consulta_archivo_xml(hd_SeleccionarXML.Value.ToString().Replace("docus/", "").Replace(".xml", ""), 7), hd_SeleccionarXML.Value.ToString().Replace("docus/", ""));
                                }
                            }
                            asunto = leeCodDoc(l_Label10.Text) + " ELECTRONICA No: " + l_Label5.Text + " de " + compania;
                            System.Net.Mail.LinkedResource image001 = new System.Net.Mail.LinkedResource(Server.MapPath("~\\imagenes\\logo_cima.PNG"), "image/png");
                            image001.ContentId = "image001";
                            image001.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                            System.Text.StringBuilder htmlBody = new System.Text.StringBuilder();
                            htmlBody.Append("<html>");
                            htmlBody.Append("<body>");
                            htmlBody.Append("<table style=\"width:100%;\">");
                            htmlBody.Append("<tr>");
                            htmlBody.Append("<td colspan=\"3\"></td>");
                            htmlBody.Append("</tr>");
                            htmlBody.Append("<tr>");
                            htmlBody.Append("<tr>");
                            htmlBody.Append("<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td><br/>Estimado(a)  " + l_Label3.Text + "<br/><br/>Adjunto s&iacute;rvase encontrar su " + leeCodDoc(l_Label10.Text) + " ELECTRONICA No: " + l_Label5.Text + @"&sup1; y el archivo PDF&sup2; de dicho
							comprobante que hemos emitido en nuestra empresa.<br/> Gracias por preferirnos.<br/><br/> Atentamente, " + " <br/> <img height=\"50\" width=\"130\" align=\"middle\" src=\"cid:image001\" /><br/>" + compania +
@"<br/>--------------------------------------------------------------------------------" +
@"<br/>&sup1; El comprobante electr&oacute;nico es el archivo XML adjunto, le socilitamos que lo almacene de manera segura puesto que tiene validez tributaria." +
@"<br/>&sup2; La representaci&oacute;n impresa del comprobante electr&oacute;nico es el archivo PDF adjunto, y no es necesario que la imprima." +
@"</td><td> </td>");
                            htmlBody.Append("</tr>");
                            htmlBody.Append("</tr>");
                            htmlBody.Append("</table>");
                            htmlBody.Append("</body>");
                            htmlBody.Append("</html>");
                            System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(htmlBody.ToString(), null, "text/html");
                            htmlView.LinkedResources.Add(image001);
                            EM.llenarEmailHTML(emailEnviar, emails.Trim(','), "", "", asunto, htmlView, compania);
                            try
                            {
                                EM.enviarEmail();
                                lbMsgZip.Text = "Email enviado";
                            }
                            catch (System.Net.Mail.SmtpException ex)
                            {
                                DB.Desconectar();
                                DB.Conectar();
                                DB.CrearComando(@"insert into LogErrorFacturas
								(detalle,fecha,archivo,linea,numeroDocumento,tipo) 
								values 
								(@detalle,@fecha,@archivo,@linea,@numeroDocumento,@tipo)");
                                DB.AsignarParametroCadena("@detalle", ex.Message.Replace("'", "''"));
                                DB.AsignarParametroFecha("@fecha", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                DB.AsignarParametroCadena("@archivo", "");
                                DB.AsignarParametroCadena("@linea", "");
                                DB.AsignarParametroCadena("@tipo", "E-mail");
                                DB.AsignarParametroCadena("@numeroDocumento", l_Label5.Text + " ");
                                DB.EjecutarConsulta1();
                                DB.Desconectar();
                                lbMsgZip.Text = ex.Message;
                            }
                        }
                        else
                        {
                            lbMsgZip.Text = "Tienes que seleccionar algún E-mail";
                        }
                    }
                }
                if (!bCHK)
                {
                    lbMsgZip.Text = "Debes seleccionar una factura";
                }
            }
            catch (Exception ex)
            {
                DB.Desconectar();
                lbMsgZip.Text = ex.Message;
                clsLogger.Graba_Log_Error(ex.Message);
            }
            finally
            {
                DB.Desconectar();
            }
        }
        public MemoryStream consulta_archivo_xml(string p_codigoControl, int p_opcion)
        {
            var DB = new BasesDatos();
            MemoryStream rpt = new MemoryStream();
            string doc = "";
            string repl1 = "";
            string repl2 = "";
            string repl3 = "";
            try
            {
                DB.Conectar();
                DB.CrearComandoProcedimiento("PA_ARCHIVO_XML");
                DB.AsignarParametroProcedimiento("@documentoXML", System.Data.DbType.Xml, "0");
                DB.AsignarParametroProcedimiento("@codigoControl", System.Data.DbType.String, p_codigoControl);
                DB.AsignarParametroProcedimiento("@idComprobante", System.Data.DbType.Int32, "0");
                DB.AsignarParametroProcedimiento("@opcion", System.Data.DbType.Int32, p_opcion);
                using (DbDataReader dr = DB.EjecutarConsulta())
                {
                    if (dr.Read())
                    {
                        repl1 = dr[0].ToString().Replace("&lt;", "<");
                        repl2 = repl1.ToString().Replace("&gt;", ">");
                        repl3 = repl2.ToString().Replace(@"<?xml version=""1.0"" encoding=""UTF-8""?>", "");
                        doc = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + repl3.ToString();
                        rpt = GenerateStreamFromString(doc);
                    }
                }

                DB.Desconectar();
            }
            catch (Exception ex)
            {
                DB.Desconectar();
                lbMsgZip.Text = "Error en proceso consulta_archivo_xml. " + ex.Message;
                clsLogger.Graba_Log_Error(ex.Message);
            }
            finally
            {
                DB.Desconectar();
            }
            return rpt;
        }

        public MemoryStream consulta_archivo_xml2(string p_codigoControl, int p_opcion)
        {
            var DB = new BasesDatos();
            MemoryStream rpt = new MemoryStream();
            string doc = "";
            try
            {
                DB.Conectar2();
                DB.CrearComandoProcedimiento("PA_ARCHIVO_XML");
                DB.AsignarParametroProcedimiento("@documentoXML", System.Data.DbType.Xml, "0");
                DB.AsignarParametroProcedimiento("@codigoControl", System.Data.DbType.String, p_codigoControl);
                DB.AsignarParametroProcedimiento("@idComprobante", System.Data.DbType.Int32, "0");
                DB.AsignarParametroProcedimiento("@opcion", System.Data.DbType.Int32, p_opcion);
                using (DbDataReader dr = DB.EjecutarConsulta())
                {
                    if (dr.Read())
                    {
                        doc = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + dr[0].ToString();
                        rpt = GenerateStreamFromString(doc);
                    }
                }
                DB.Desconectar2();
            }
            catch (Exception ex)
            {
                DB.Desconectar();
                lbMsgZip.Text = "Error en proceso consulta_archivo_xml. " + ex.Message;
                clsLogger.Graba_Log_Error(ex.Message);
            }
            finally
            {
                DB.Desconectar();
            }
            return rpt;
        }

        private MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(value ?? ""));
        }

        protected void SendSap(object sender, EventArgs e)
        {
            LblSendSapInfo.Text = "";
            LblSendSapInfo.Text = "Documento enviado a SAP.";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "hideMessage", "hideMessageAfterTimeout();", true);
            foreach (GridViewRow row in gvFacturas.Rows)
            {
                var check = (CheckBox)row.FindControl("check");

                if (check.Checked)
                {
                    var idComprobante = (HiddenField)row.FindControl("IdComprobante");
                    UpdateForSendSap(idComprobante.Value);
                }
            }
        }

        private void UpdateForSendSap(string idComprobante)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["dataexpressConnectionString"].ToString();
            var queryString =
                "UPDATE GENERAL SET estadoWLF = 'S', archivoWLF = 'RC' "
                    + "WHERE IdComprobante = @idComprobante";
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@idComprobante", idComprobante);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    clsLogger.Graba_Log_Error($"{Session["sucursalUser"]}-{Session["rfcCliente"]}-{ex.Message}///{ex.InnerException}///{ex.StackTrace}");
                    LblErrors.Text = "Hubo al menos un error en el envío a SAP. Contectese con el administrador del sistema.";
                }
            }
        }
    }
}