using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.Common;
using Control;
using Ionic.Zip;
namespace DataExpressWeb
{
    public partial class aceptarDocs : System.Web.UI.Page
    {
        private String consulta;
        private String aux;
        private String separador = "|";
        private DataTable DT = new DataTable();
        private BasesDatos DB = new BasesDatos();
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
        protected void Page_Load(object sender, EventArgs e)
        {
            tbRFC.Attributes.Add("onkeyup", "changeToUpperCase(this.id)");
            tbRFC.Attributes.Add("onchange", "changeToUpperCase(this.id)");


            if (IsPostBack)
            {
                if (Session["sucursalUser"] != null)
                {
                    //buscar();
                    if (!Convert.ToBoolean(Session["validarFactura"]))
                    {
                        Response.Redirect("~/cuenta/login.aspx");
                        // lSeleccionDocus.Visible = false;
                        consulta = "";
                    }
                }
                if (String.IsNullOrEmpty((String)Session["rfcCliente"]))
                {
                    // lSeleccionDocus.Visible = false;
                    consulta = "";
                }
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
            /* if (tbSerie.Text.Length != 0)
             {
                 if (consulta.Length != 0) { consulta = consulta + "SE" + tbSerie.Text + separador; }
                 else { consulta = "SE" + tbSerie.Text + separador; }
             }*/
            if (ddlTipoDocumento.SelectedIndex != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "TD" + ddlTipoDocumento.SelectedValue + separador; }
                else { consulta = "TD" + ddlTipoDocumento.SelectedValue + separador; }
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
            //if (((String)Session["coFactTodas"])=="") { miSucursal = "S---"; } else { miSucursal = (String)Session["sucursalUser"]; }
            // miSucursal = "S---";
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
            SqlDataSource1.DataBind();
            gvFacturas.DataBind();
            consulta = "";
            lMensaje.Text = msjbuscar;
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            Response.Redirect("aceptarDocs.aspx");
        }

        protected void Button1_Click2(object sender, EventArgs e)
        {
            DB.Conectar();
            DB.CrearComando("select servidorSMTP,puertoSMTP,sslSMTP,userSMTP,passSMTP,dirdocs,emailEnvio from ParametrosSistema WITH (NOLOCK) ");
            DbDataReader DR1 = DB.EjecutarConsulta();

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
            DB.Desconectar();

            string emails = "";
            string asunto = "";
            string mensaje = "";
            string temp = "";
            Boolean bCHK = false;
            DB.Desconectar();
            consulta = "";
            cantidad = gvFacturas.Rows.Count;
            DB.Conectar();
            DB.CrearComando("select emailsRegla from EmailsReglas WITH (NOLOCK)   where Receptor=@rfcrec and estadoRegla=1");
            DB.AsignarParametroCadena("@rfcrec", "");
            DbDataReader DR3 = DB.EjecutarConsulta();
            if (DR3.Read())
            {
                emails = DR3[0].ToString();
            }

            //7   emails = tbEmail.Text + ","+emails;
            emails = emails.Trim();

            //  EM.adjuntar(RutaDOC + hd_SeleccionarXML.Value.ToString().Replace("docus/",""));

            /*    if (emails.Length > 15)
                {
                    asunto = "Factura con folio" + l_Label5.Text + " de Cima IT";
                    mensaje = @"Buenas! <br>
                        Acabas de Recibir tu factura generada el " + l_Label1.Text + @"<br>
                        con folio " + l_Label5.Text+ ".";
                    mensaje += "<br>Saludos cordiales ";
                    mensaje += "<br>Cima IT, ";
                    mensaje += "<br><br>Servicio proporcionado por DataExpress Internacional";
                    mensaje += "<br><br>Tel. 01 800 8 41 33 29";

                    EM.llenarEmail(emailEnviar, emails.Trim(','), "", "", asunto, mensaje);
                    try
                    {
                        EM.enviarEmail();
                        lbMsgZip.Text = "Email enviado";

                    }
                    catch (System.Net.Mail.SmtpException ex)
                    {

                    }
                }
                else {
                    lbMsgZip.Text = "Tienes seleccionar algún E-mail";
                }
             * */
        }

        protected void gvFacturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string secuencial = "";
            string razonSocial = "";
            string xml = "";
            string pdf = "";
            string emails = "";
            GridViewRow gvRow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
            String strKey = gvFacturas.DataKeys[gvRow.RowIndex][0].ToString();
            secuencial = gvFacturas.Rows[gvRow.RowIndex].Cells[3].Text;
            razonSocial = gvFacturas.Rows[gvRow.RowIndex].Cells[1].Text;
            xml = gvFacturas.Rows[gvRow.RowIndex].Cells[7].Text;
            pdf = gvFacturas.Rows[gvRow.RowIndex].Cells[8].Text;
            if (e.CommandName == "Aceptar")
            {
                SqlDataSource1.UpdateParameters["idComprobante"].DefaultValue = strKey;
                SqlDataSource1.Update();
            }
            if (e.CommandName == "Rechazar")
            {
                SqlDataSource1.DeleteParameters["idComprobante"].DefaultValue = strKey;
                SqlDataSource1.Delete();
            }
            /*
            DB.Conectar();
            DB.CrearComando("select emailsRegla from EmailsReglas  where Receptor=@rfcrec and estadoRegla=1");
            DB.AsignarParametroCadena("@rfcrec", "");
            DbDataReader DR3 = DB.EjecutarConsulta();
            if (DR3.Read())
            {
                emails = DR3[0].ToString();
            }

            //7   emails = tbEmail.Text + ","+emails;
            emails = emails.Trim();
            emails = "eddie.lozano@servientrega.com.ec," + emails;
            emails = emails.Trim(',');
              EM.adjuntar(RutaDOC + xml.Replace("docus/",""));
             EM.adjuntar(RutaDOC + pdf.Replace("docus/",""));
             if (emails.Length > 15)
             {
                 string asunto = "Factura con folio" + secuencial + " de " + razonSocial;
                 string mensaje = @"Estimados! <br>";
                 mensaje += "La factura con Folio " + secuencial + @"<br>";
                 mensaje += "No fue aceptada posiblemente no cumple con lo solicidado por Servientrega<br> ";
                 mensaje += "<br>Saludos cordiales ";
                 mensaje += "<br><br>Servicio proporcionado por DataExpress Internacional";
                 mensaje += "<br><br>helpdesk@dataexpressint.com";

                 EM.llenarEmail(emailEnviar, emails.Trim(','), "", "", asunto, mensaje);
                 try
                 {
                     EM.enviarEmail();
                     lMensaje.Text = "Email enviado";

                 }
                 catch (System.Net.Mail.SmtpException ex)
                 {
                     log.mensajesLog("EM001", "", ex.Message, "", secuencial);
                 }
             }
             * */
        }
    }
}