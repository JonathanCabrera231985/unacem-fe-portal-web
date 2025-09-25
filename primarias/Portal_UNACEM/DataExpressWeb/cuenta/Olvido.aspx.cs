using System;
using Datos;
using System.Data.Common;
using Control;
using CriptoSimetrica;
using System.Data.OleDb;
using System.Configuration;
using clibLogger;

namespace DataExpressWeb
{
    public partial class Olvido : System.Web.UI.Page
    {
        private EnviarMail EM;
        private AES Cs = new AES();
        string servidor = "";
        int puerto = 25;
        Boolean ssl = false;
        string emailCredencial = "";
        string passCredencial = "";
        string emailEnviar = "";
        string RutaDOC = "";
        string compania = "UNACEM ECUADOR S.A.";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void bEnviar_Click(object sender, EventArgs e)
        {
            envia_mail();
        }

        protected void envia_mail()
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
                string clave = "";
                string asunto = "";
                string mensaje = "";
                EM = new EnviarMail();
                EM.servidorSTMP(servidor, puerto, ssl, emailCredencial, passCredencial);
                emails = "";
                if (!string.IsNullOrEmpty(this.user.Text.Trim()))
                {
                    String query = "select top 1 r.email,c.claveCliente from RECEPTOR r  inner join Clientes c on r.IDEREC = c.id_Receptor where r.RFCREC = ?";
                    try
                    {
                        using (var conecction = new OleDbConnection(ConfigurationManager.AppSettings.Get("PROVEEDOR_ADONET2")))
                        {
                            conecction.Open();
                            using (var command = new OleDbCommand(query, conecction))
                            {
                                command.Parameters.Add(new OleDbParameter("customerName", this.user.Text));
                                var reader = command.ExecuteReader();
                                if (reader.Read())
                                {
                                    emails = reader[0].ToString();
                                    clave = reader[1].ToString();
                                }
                            }
                        }
                    }
                    catch (OleDbException se)
                    {
                        this.lblMensaje.Text = se.Message;
                    }
                }
                if (emails.Length > 10)
                {
                    string[] aEmails = emails.Split(',');
                    emails = aEmails[0].Trim();
                    if (!clave.Equals(this.user.Text.Trim()))
                        clave = Cs.desencriptar(clave, "CIMAIT");
                    asunto = "RECUPERACIÓN DE CONTRASEÑA PARA CONSULTA DE FACTURACIÓN ELECTRONICA DE " + compania;
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
                    htmlBody.Append("<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td><br/>Estimado(a) Usuario:<br/><br/>Su contrase&#241;a para consulta de documentos electr&oacute;nicos en el portal " + compania + " es: <br/>" + clave + @" <br>
							" + "<br><br> Saludos, <br> <img height=\"50\" width=\"90\" align=\"middle\" src=\"cid:image001\" /><br>Web Site: <a href=\"www.unacem.com.ec\">www.unacem.com.ec</a><br>" +
                        @"</td><td> </td>");
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
                        this.lblMensaje.Text = "Email enviado";
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
                        DB.AsignarParametroCadena("@numeroDocumento", user.Text + " ");
                        DB.EjecutarConsulta1();
                        DB.Desconectar();
                        this.lblMensaje.Text = ex.Message;
                    }
                }
                else
                {
                    this.lblMensaje.Text = "El Usuario no cuenta con algún E-mail";
                }
            }
            catch (Exception ex)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ex.Message);
                this.lblMensaje.Text = "Ocurrió el siguiente error: " + ex.Message;
            }
            finally
            {
                DB.Desconectar();
            }
        }
    }
}