using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.Common;
using Control;
using System.IO;
using clibLogger;
using CryptoNETStandar;

namespace DataExpressWeb
{
	public partial class ConsultaRecepcion : Generic
    {

        private String consulta;
        private String aux;
        private String separador = "|";
        private DataTable DT = new DataTable();
        private Log logg1 = new Log();
        private EnviarMail EM;
        static public string historicad = "";
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
                Boolean a = true;
                if (IsPostBack)
                {
                    if (Session["sucursalUser"] != null)
                    {
                    }
                    a = false;
                }
                historicad = "1";
                DataFilter11.DataSource = SqlDataSource1;
                DataFilter11.DataColumns = gvFacturas.Columns;
                DataFilter11.FilterSessionID = "ConsultaRecepcion.aspx";
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

        protected void CambiarTipo(object sender, EventArgs e)
        {
            PanelRetenciones.Visible = ddlTipoDocumento.SelectedIndex != 0 && ddlTipoDocumento.SelectedValue == "072";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            buscar();
        }

        private void buscar()
        {
            try
            {
                int CONTEO = 0;
                int CONTEOvacio = 0;
                string msjbuscar = "-";
                consulta = "";
                string FechaInicial = "";
                string FechaFinal = "";
                string tipo = "";
                historicad = "";
                DT.Clear();
                if (tbFolioAnterior.Text.Length != 0)
                {
                    consulta = tbFolioAnterior.Text;
                    CONTEO++;
                }
                else { consulta = "null"; CONTEOvacio++; }

                if (ddlTipoDocumento.SelectedIndex != 0)
                {
                    tipo = ddlTipoDocumento.SelectedValue;
                    CONTEO++;
                }
                else
                {
                    CONTEOvacio++;
                    tipo = "null";
                }
                if (tbFechaInicial.Text.Length > 5 && tbFechaFinal.Text.Length > 5)
                {
                    if (tbFechaInicial.Text.Length > 5)
                    {
                        FechaInicial = Convert.ToDateTime(tbFechaInicial.Text).ToString("yyyy-MM-dd") + " 00:00:00.000";
                        CONTEO++;
                        //fecha = 1;
                    }
                    if (tbFechaFinal.Text.Length > 5)
                    {
                        FechaFinal = Convert.ToDateTime(tbFechaFinal.Text).ToString("yyyy-MM-dd") + " 23:59:59.000";
                        CONTEO++;
                    }
                    if (CONTEO == 4) { msjbuscar = "----"; }
                }
                else
                {
                    FechaInicial = "null";
                    CONTEOvacio = CONTEOvacio + 2;
                    FechaFinal = "null";
                }
                SqlDataSource1.SelectParameters["consulta"].DefaultValue = consulta;
                SqlDataSource1.SelectParameters["fechaconsultaInicio"].DefaultValue = FechaInicial;
                SqlDataSource1.SelectParameters["fechaconsultaFinal"].DefaultValue = FechaFinal;
                SqlDataSource1.SelectParameters["tipodocumento"].DefaultValue = tipo;
                SqlDataSource1.SelectParameters["MYWHERE"].DefaultValue = msjbuscar;
                SqlDataSource1.DataBind();
                gvFacturas.DataBind();
            }
            catch (Exception e)
            {
                clsLogger.Graba_Log_Error(e.Message + "-" + consulta + "-" + Session["sucursalUser"].ToString() + "-" + aux + "-" + Session["rfcCliente"].ToString());

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
		            case "03":
			            rpt = "LIQUIDACIÓN DE COMPRA";
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
                        rpt = "GUÍA DE REMISIÓN";
                        break;
                }
            }
            return rpt;
        }
        void DataFilter1_OnFilterAdded()
        {
            try
            {
                DataFilter11.FilterSessionID = "ConsultaRecepcion.aspx";
                DataFilter11.FilterDataSource();
                gvFacturas.DataBind();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                DB.Desconectar2();
                clsLogger.Graba_Log_Error(ex.Message);
            }
            finally
            {
                DB.Desconectar2();
            }
            return rpt;
        }

        private MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(value ?? ""));
        }
    }
}