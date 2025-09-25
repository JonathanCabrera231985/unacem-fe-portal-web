using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.Common;
using Control;
using System.Text;
using clibLogger;

namespace DataExpressWeb
{
    public partial class verificarDocs : System.Web.UI.Page
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
                        if (!Convert.ToBoolean(Session["validarFactura"]))
                        {
                            Response.Redirect(Server.HtmlEncode("~/cuenta/login.aspx"));
                            consulta = "";
                        }
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
                consulta = consulta + "ETR" + separador;
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
                consulta = "ETR";
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
            Response.Redirect(Server.HtmlEncode("verificarDocs.aspx"));
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
                DB.Conectar();
                DB.CrearComando("select emailsRegla from EmailsReglas WITH (NOLOCK) where Receptor=@rfcrec and estadoRegla=1");
                DB.AsignarParametroCadena("@rfcrec", "");
                using (DbDataReader DR3 = DB.EjecutarConsulta())
                {
                    if (DR3.Read())
                    {
                        emails = DR3[0].ToString();
                    }
                }
                DB.Desconectar();
                emails = emails.Trim();
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

        private void notificacion(string email, string mensaje, string asuinto)
        {
            var DB = new BasesDatos();
            try
            {
                string emails = "";
                string asunto = "";
                string temp = "";
                Boolean bCHK = false;
                DB.Desconectar();
                consulta = "";
                cantidad = gvFacturas.Rows.Count;
                DB.Conectar();
                DB.CrearComando("select emailsRegla from EmailsReglas WITH (NOLOCK) where Receptor=@rfcrec and estadoRegla=1");
                DB.AsignarParametroCadena("@rfcrec", "");
                using (DbDataReader DR3 = DB.EjecutarConsulta())
                {
                    if (DR3.Read())
                    {
                        emails = DR3[0].ToString();
                    }
                }
                DB.Desconectar();
                emails = emails.Trim();
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

        protected void gvFacturas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string P_RUCRECEPTOR = "";
            string P_RUCPROVEEDOR = "";
            string P_CLAVEACCESO = "";
            string P_NUMAUT = "";
            string P_USUARIO = "";
            string P_MENSAJE = "";
            if (e.CommandName.Equals("Aceptar") || e.CommandName.Equals("Rechazar"))
            {
                P_USUARIO = Session["rfcUser"].ToString();
                GridViewRow gvRow2 = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                P_RUCRECEPTOR = gvFacturas.DataKeys[gvRow2.RowIndex]["rucReceptor"].ToString();
                P_RUCPROVEEDOR = gvFacturas.DataKeys[gvRow2.RowIndex]["rucProveedor"].ToString();
                P_CLAVEACCESO = gvFacturas.DataKeys[gvRow2.RowIndex]["claveAcceso"].ToString();
                P_NUMAUT = gvFacturas.DataKeys[gvRow2.RowIndex]["numeroAutorizacion"].ToString();
                if (e.CommandName == "Aceptar")
                {
                    SqlDataSource1.UpdateParameters["documentoXML"].DefaultValue = "<INSTRUCCION><FILTRO><opcion>2</opcion><query>ET2|CA" + P_CLAVEACCESO + "|RC" + P_RUCRECEPTOR + "|RF" + P_RUCPROVEEDOR + "|NA" + P_NUMAUT + "|US" + P_USUARIO + "</query></FILTRO></INSTRUCCION>";
                    SqlDataSource1.Update();
                }
                if (e.CommandName == "Rechazar")
                {
                    SqlDataSource1.DeleteParameters["documentoXML"].DefaultValue = "<INSTRUCCION><FILTRO><opcion>2</opcion><query>ET0|CA" + P_CLAVEACCESO + "|RC" + P_RUCRECEPTOR + "|RF" + P_RUCPROVEEDOR + "|NA" + P_NUMAUT + "|US" + P_USUARIO + "</query></FILTRO></INSTRUCCION>";
                    SqlDataSource1.Delete();
                }
                gvFacturas.DataBind();
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
            ClientScript.RegisterStartupScript(this.GetType(), "newWindow", String.Format(Server.HtmlEncode("<script>window.open('{0}');</script>"), url));
        }
    }
}