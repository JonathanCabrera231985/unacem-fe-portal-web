using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Control;
using System.Data.Common;
using System.Text;
using System.Data;
using clibLogger;

namespace DataExpressWeb
{
    public partial class RepGeneral : System.Web.UI.Page
    {
        String fecha, fechanom, fechacreacion, fecharini, fecharfin;
        String dir;
        private String consulta;
        private String separador = "|";
        int count;
        string user = "";
        String rucReceptor = "";
        int secuenciamax = 0, secuenciamin = 0;
        string RFCREC = "", NOMREC = "", TIPODOC = "", ID_EMISOR = "", SECUENCIA = "", FACTURA = "", DOC_SAP = "", numeroAutorizacion = "", FECHA = "", SUBTOTAL = "0", DESCT = "0", JUBILACION_IESS = "0", IVA12 = "0", RET_IVA = "0", RET_FUENTE = "0", ICE = "0", TOTAL = "0", fechaAutorizacion = "";
        string RFCEMI = "", EmailCli = "";
        String mensajerespuesta = "";
        String location = "";
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
                if (Session["rfcCliente"] != null)
                    rucReceptor = Session["rfcCliente"].ToString();
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

        public void setear_variables_temp()
        {
            RFCREC = ""; NOMREC = ""; TIPODOC = ""; SECUENCIA = ""; FACTURA = ""; DOC_SAP = ""; numeroAutorizacion = ""; SUBTOTAL = "0"; DESCT = "0"; JUBILACION_IESS = "0"; IVA12 = "0"; RET_IVA = "0"; RET_FUENTE = "0"; ICE = "0"; TOTAL = "0"; fechaAutorizacion = "";
        }

        protected void bGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                location = "";
                consulta = "";
                count = 0;
                if (!calentario.SelectedDate.ToShortDateString().Equals("01/01/0001") &&
                !calendario2.SelectedDate.ToShortDateString().Equals("01/01/0001")
                )
                {
                    if (calentario.SelectedDate <= calendario2.SelectedDate)
                    {
                        Label2.Text = "";
                        fechacreacion = System.DateTime.Now.ToString("ddMMyyyyHHmm");
                        dir = System.AppDomain.CurrentDomain.BaseDirectory + @"reportes\docs\UNACEM_" + fechacreacion;
                        System.Data.DataSet dsDatods = new System.Data.DataSet();
                        System.Data.DataSet dsDatodsOLD = new System.Data.DataSet();
                        dsDatods = ConsultaTipoComprobante();
                        if (dsDatods.Tables["Table"].Rows.Count > 0)
                        {
                            Elimina_tabla_Temporal();
                            Label2.Text = "";
                            RepSucursal reporteSuc = new RepSucursal(dir, dsDatods, ddlDocumento.SelectedValue);
                            if (reporteSuc.getError() != null)
                            {
                                mensajerespuesta = "Reporte Generado Con Exitó!";
                                try
                                {
                                    location = "UNACEM_" + fechacreacion + ".xls";
                                }
                                catch (Exception ec)
                                {

                                }
                            }
                            else
                            {
                                mensajerespuesta = reporteSuc.getError();
                            }
                        }
                        else
                        {
                            dsDatodsOLD = ConsultaTipoComprobanteOLD();
                            if (dsDatodsOLD.Tables["Table"].Rows.Count > 0)
                            {
                                Elimina_tabla_TemporalOLD();
                                RepSucursal reporteSuc = new RepSucursal(dir, dsDatodsOLD, ddlDocumento.SelectedValue);
                                if (reporteSuc.getError() != null)
                                {
                                    mensajerespuesta = "Reporte Generado Con Exitó!";
                                    try
                                    {
                                        location = "UNACEM_" + fechacreacion + ".xls";
                                    }
                                    catch (Exception ec)
                                    {
                                    }
                                }
                                else
                                {
                                    mensajerespuesta = reporteSuc.getError();
                                }
                            }
                            else
                            {
                                mensajerespuesta = "No se han elaborado Documentos durante estas fechas";
                            }
                        }
                    }
                    else
                    {
                        mensajerespuesta = "Selecciona fecha inical y fecha final";
                    }
                }
                else
                {
                    mensajerespuesta = "Tienes que seleccionar ambas fechas.";
                }
            }
            catch (Exception ea)
            {
                mensajerespuesta = ea.Message;
            }
            Label2.Text = mensajerespuesta;
            String Script2 = "";
            if (!String.IsNullOrEmpty(location))
            {
                Script2 = "<script language='javascript'>" +
            "b(false);" +
            "window.open('Docs/" + location + "');" +
            "</script>";
            }
            else
            {
                Script2 = "<script language='javascript'>" +
        "b(false);" +
            "</script>";
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ajax", Script2, false);
        }

        protected void ddlPtoEmiR_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void ddlSucursalR_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDocumento.SelectedValue.Equals("0") || ddlDocumento.SelectedValue.Equals("-1"))
            {
                SqlDataPtoEmisionR.SelectParameters["docf"].DefaultValue = "01";//clave
                SqlDataPtoEmisionR.SelectParameters["docnc"].DefaultValue = "04";//clave
                SqlDataPtoEmisionR.SelectParameters["docnd"].DefaultValue = "05";//clave
                SqlDataPtoEmisionR.SelectParameters["docg"].DefaultValue = "06";//clave
                SqlDataPtoEmisionR.SelectParameters["docr"].DefaultValue = "07";//clave
            }
            else
            {
                SqlDataPtoEmisionR.SelectParameters["docf"].DefaultValue = ddlDocumento.SelectedValue;//clave
                SqlDataPtoEmisionR.SelectParameters["docnc"].DefaultValue = ddlDocumento.SelectedValue;//clave
                SqlDataPtoEmisionR.SelectParameters["docnd"].DefaultValue = ddlDocumento.SelectedValue;//clave
                SqlDataPtoEmisionR.SelectParameters["docg"].DefaultValue = ddlDocumento.SelectedValue;//clave
                SqlDataPtoEmisionR.SelectParameters["docr"].DefaultValue = ddlDocumento.SelectedValue;//clave
            }
            SqlDataPtoEmisionR.SelectParameters["idSucursal"].DefaultValue = ddlSucursalR.SelectedValue;
            SqlDataPtoEmisionR.DataBind();
            ddlPtoEmiR.DataBind();
        }

        protected void ddlDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDocumento.SelectedValue.Equals("0") || ddlDocumento.SelectedValue.Equals("-1"))
            {
                SqlDataPtoEmisionR.SelectParameters["docf"].DefaultValue = "01";//clave
                SqlDataPtoEmisionR.SelectParameters["docnc"].DefaultValue = "04";//clave
                SqlDataPtoEmisionR.SelectParameters["docnd"].DefaultValue = "05";//clave
                SqlDataPtoEmisionR.SelectParameters["docg"].DefaultValue = "06";//clave
                SqlDataPtoEmisionR.SelectParameters["docr"].DefaultValue = "07";//clave
            }
            else
            {
                SqlDataPtoEmisionR.SelectParameters["docf"].DefaultValue = ddlDocumento.SelectedValue;//clave
                SqlDataPtoEmisionR.SelectParameters["docnc"].DefaultValue = ddlDocumento.SelectedValue;//clave
                SqlDataPtoEmisionR.SelectParameters["docnd"].DefaultValue = ddlDocumento.SelectedValue;//clave
                SqlDataPtoEmisionR.SelectParameters["docg"].DefaultValue = ddlDocumento.SelectedValue;//clave
                SqlDataPtoEmisionR.SelectParameters["docr"].DefaultValue = ddlDocumento.SelectedValue;//clave
            }
            SqlDataPtoEmisionR.SelectParameters["idSucursal"].DefaultValue = ddlSucursalR.SelectedValue;//clave
            SqlDataPtoEmisionR.DataBind();
            ddlPtoEmiR.DataBind();
        }

        public DataSet ConsultarecepInfo(string fechar, string fechanomr, string opcion, string doc, string estab, string ptoemi, string ruc)
        {
            var DB = new BasesDatos();
            System.Data.DataSet dsDatodsR = new System.Data.DataSet();
            try
            {
                DB.Conectar();
                StringBuilder documentoXMLR = new StringBuilder("");
                documentoXMLR.Append("<INSTRUCCION>");
                documentoXMLR.Append("<FILTRO>");
                documentoXMLR.Append("<opcion>" + opcion + "</opcion>");
                documentoXMLR.Append("<FechaMinima>" + String.Format("{0:yyyyMMdd H:mm:ss}", Convert.ToDateTime(fechar + " 00:00:00")) + "</FechaMinima>");
                documentoXMLR.Append("<Fechamaxima>" + String.Format("{0:yyyyMMdd H:mm:ss}", Convert.ToDateTime(fechanomr + " 23:59:58")) + "</Fechamaxima>");
                documentoXMLR.Append("<documento>" + doc + "</documento>");
                documentoXMLR.Append("<estab>" + estab + "</estab>");
                documentoXMLR.Append("<ptoemi>" + ptoemi + "</ptoemi>");
                documentoXMLR.Append("<ruc>" + ruc + "</ruc>");
                documentoXMLR.Append("<idEmpresa>" + Session["rucEmpresa"].ToString() + "</idEmpresa>");
                documentoXMLR.Append("</FILTRO>");
                documentoXMLR.Append("</INSTRUCCION>");
                dsDatodsR = DB.TraerDataset("PA_SerReportes_Emi", documentoXMLR.ToString());
            }
            catch (Exception ea)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error("ConsultarecepInfo: " + ea.ToString());
            }
            finally
            {
                DB.Desconectar();
            }
            return dsDatodsR;
        }

        public DataSet ConsultaTipoComprobante()
        {
            string sucursal = "";
            string ptoEmision = "";
            string documento = "";
            string opc = "";
            fecharini = "";
            fecharfin = "";
            secuenciamin = 0;
            secuenciamax = 0;
            fecha = calentario.SelectedDate.ToShortDateString();
            fechanom = calendario2.SelectedDate.ToShortDateString();
            fecharini = fecha;
            fecharfin = fechanom;
            System.Data.DataSet dsDatods = new System.Data.DataSet();
            if (ddlDocumento.SelectedIndex != 0 && ddlSucursalR.SelectedIndex != 0 && ddlPtoEmiR.SelectedIndex != 0 && tb_ruc.Text == "")
            {
                opc = "1";
                documento = ddlDocumento.SelectedValue;
                sucursal = ddlSucursalR.SelectedValue;
                ptoEmision = ddlPtoEmiR.SelectedValue;
                Elimina_tabla_Temporal();
                Consulta_secuencia(sucursal, ptoEmision, documento);
                ProcesoInsertar_secuencia_tablaTEMP(sucursal, ptoEmision, documento);
            }
            else if (ddlDocumento.SelectedIndex == 0 && ddlSucursalR.SelectedIndex != 0 && ddlPtoEmiR.SelectedIndex != 0 && tb_ruc.Text == "")
            {
                opc = "1";
                documento = "";
                sucursal = ddlSucursalR.SelectedValue;
                ptoEmision = ddlPtoEmiR.SelectedValue;

            }
            else if (ddlDocumento.SelectedIndex == 0 && ddlSucursalR.SelectedIndex == 0 && (ddlPtoEmiR.SelectedIndex == 0 || ddlPtoEmiR.SelectedValue == "Selecciona...") && tb_ruc.Text == "")
            {
                opc = "2";
                documento = "";
                sucursal = "";
                ptoEmision = "";
            }
            else if (ddlDocumento.SelectedIndex != 0 && ddlSucursalR.SelectedIndex == 0 && (ddlPtoEmiR.SelectedIndex == 0 || ddlPtoEmiR.SelectedValue == "Selecciona...") && tb_ruc.Text == "")
            {
                opc = "2";
                documento = ddlDocumento.SelectedValue;
                sucursal = "";
                ptoEmision = "";
            }
            else if (ddlDocumento.SelectedIndex != 0 && ddlSucursalR.SelectedIndex != 0 && ddlPtoEmiR.SelectedValue == "Selecciona..." && tb_ruc.Text == "")
            {
                opc = "3";
                documento = ddlDocumento.SelectedValue;
                sucursal = ddlSucursalR.SelectedValue;
                ptoEmision = "";
            }
            else if (ddlDocumento.SelectedIndex == 0 && ddlSucursalR.SelectedIndex != 0 && (ddlPtoEmiR.SelectedIndex == 0 || ddlPtoEmiR.SelectedValue == "Selecciona...") && tb_ruc.Text == "")
            {
                opc = "3";
                documento = "";
                sucursal = ddlSucursalR.SelectedValue;
                ptoEmision = "";
            }
            else if (ddlDocumento.SelectedIndex != 0 && ddlSucursalR.SelectedIndex != 0 && ddlPtoEmiR.SelectedIndex != 0 && tb_ruc.Text != "")
            {
                opc = "4";
                documento = ddlDocumento.SelectedValue;
                sucursal = ddlSucursalR.SelectedValue;
                ptoEmision = ddlPtoEmiR.SelectedValue;
            }
            else if (ddlDocumento.SelectedIndex == 0 && ddlSucursalR.SelectedIndex != 0 && ddlPtoEmiR.SelectedIndex != 0 && tb_ruc.Text != "")
            {
                opc = "4";
                documento = "";
                sucursal = ddlSucursalR.SelectedValue;
                ptoEmision = ddlPtoEmiR.SelectedValue;
            }
            else if (ddlDocumento.SelectedIndex == 0 && ddlSucursalR.SelectedIndex == 0 && (ddlPtoEmiR.SelectedIndex == 0 || ddlPtoEmiR.SelectedValue == "Selecciona...") && tb_ruc.Text != "")
            {
                opc = "5";
                documento = "";
                sucursal = "";
                ptoEmision = "";
            }
            else if (ddlDocumento.SelectedIndex != 0 && ddlSucursalR.SelectedIndex == 0 && (ddlPtoEmiR.SelectedIndex == 0 || ddlPtoEmiR.SelectedValue == "Selecciona...") && tb_ruc.Text != "")
            {
                opc = "5";
                documento = ddlDocumento.SelectedValue;
                sucursal = "";
                ptoEmision = "";
            }
            else if (ddlDocumento.SelectedIndex != 0 && ddlSucursalR.SelectedIndex != 0 && ddlPtoEmiR.SelectedValue == "Selecciona..." && tb_ruc.Text != "")
            {
                opc = "6";
                documento = ddlDocumento.SelectedValue;
                sucursal = ddlSucursalR.SelectedValue;
                ptoEmision = "";

            }
            else if (ddlDocumento.SelectedIndex == 0 && ddlSucursalR.SelectedIndex != 0 && (ddlPtoEmiR.SelectedIndex == 0 || ddlPtoEmiR.SelectedValue == "Selecciona...") && tb_ruc.Text != "")
            {
                opc = "6";
                documento = "";
                sucursal = ddlSucursalR.SelectedValue;
                ptoEmision = "";
            }
            dsDatods = ConsultarecepInfo(fecha, fechanom, opc, documento, sucursal, ptoEmision, tb_ruc.Text);
            return dsDatods;
        }

        public void Consulta_secuencia(string estab, string ptoemi, string documento)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Conectar();
                DB.CrearComando(@"select  isnull(min(secuencial),0) as secuencialmin,  isnull(max(secuencial),0) as secuencialmax from GENERAL WITH (NOLOCK) where estab=@estab and ptoEmi=@ptoEmi and codDoc=@codDoc and fecha >= @fechaMinima and fecha <= @fechaMaxima and ambiente=2");
                DB.AsignarParametroCadena("@estab", estab);
                DB.AsignarParametroCadena("@ptoEmi", ptoemi);
                DB.AsignarParametroCadena("@codDoc", documento);
                DB.AsignarParametroCadena("@fechaMinima", String.Format("{0:yyyyMMdd H:mm:ss}", Convert.ToDateTime(fecharini + " 00:00:00")));
                DB.AsignarParametroCadena("@fechaMaxima", String.Format("{0:yyyyMMdd H:mm:ss}", Convert.ToDateTime(fecharfin + " 23:59:58")));
                using (DbDataReader DR8 = DB.EjecutarConsulta())
                {
                    if (DR8.Read())
                    {
                        secuenciamin = Convert.ToInt32(DR8["secuencialmin"].ToString());
                        secuenciamax = Convert.ToInt32(DR8["secuencialmax"].ToString());
                    }
                }
                DB.Desconectar();
            }
            catch (Exception ea)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error("Consulta_secuencia: " + ea.ToString());
            }
            finally
            {
                DB.Desconectar();
            }
        }

        private String asignandoleSecuencial(String codigoSecuencial)
        {
            String codigoSecuencialAu = "";
            switch (codigoSecuencial.ToString().Trim().Length)
            {
                case 1:
                    codigoSecuencialAu = "00000000" + codigoSecuencial.ToString().Trim();
                    break;
                case 2:
                    codigoSecuencialAu = "0000000" + codigoSecuencial.ToString().Trim();
                    break;
                case 3:
                    codigoSecuencialAu = "000000" + codigoSecuencial.ToString().Trim();
                    break;
                case 4:
                    codigoSecuencialAu = "00000" + codigoSecuencial.ToString().Trim();
                    break;
                case 5:
                    codigoSecuencialAu = "0000" + codigoSecuencial.ToString().Trim();
                    break;
                case 6:
                    codigoSecuencialAu = "000" + codigoSecuencial.ToString().Trim();
                    break;
                case 7:
                    codigoSecuencialAu = "00" + codigoSecuencial.ToString().Trim();
                    break;
                case 8:
                    codigoSecuencialAu = "0" + codigoSecuencial.ToString().Trim();
                    break;
                case 9:
                    codigoSecuencialAu = codigoSecuencial.ToString().Trim();
                    break;
            }
            return codigoSecuencialAu;
        }

        public void ProcesoInsertar_secuencia_tablaTEMP(string estab, string ptoemi, string documento)
        {
            var DB = new BasesDatos();
            try
            {
                string secu = "";
                if ((secuenciamax > 0 && secuenciamin > 0))
                {
                    while (secuenciamin <= secuenciamax)
                    {
                        secu = asignandoleSecuencial(secuenciamin.ToString());
                        setear_variables_temp();
                        DB.Conectar();
                        DB.CrearComandoProcedimiento("Consulta_documento_reporte");
                        DB.AsignarParametroProcedimiento("@estab", System.Data.DbType.String, estab);
                        DB.AsignarParametroProcedimiento("@ptoEmi", System.Data.DbType.String, ptoemi);
                        DB.AsignarParametroProcedimiento("@codDoc", System.Data.DbType.String, documento);
                        DB.AsignarParametroProcedimiento("@secuencia", System.Data.DbType.String, secu);
                        DB.AsignarParametroProcedimiento("@FechaMinima", System.Data.DbType.String, String.Format("{0:yyyyMMdd H:mm:ss}", Convert.ToDateTime(fecharini + " 00:00:00")));
                        DB.AsignarParametroProcedimiento("@Fechamaxima", System.Data.DbType.String, String.Format("{0:yyyyMMdd H:mm:ss}", Convert.ToDateTime(fecharfin + " 23:59:58")));
                        using (DbDataReader DR9 = DB.EjecutarConsulta())
                        {
                            if (DR9.Read())
                            {
                                RFCREC = DR9["RFCREC"].ToString();
                                NOMREC = DR9["NOMREC"].ToString();
                                TIPODOC = DR9["TIPODOC"].ToString();
                                ID_EMISOR = DR9["ID_EMISOR"].ToString();
                                SECUENCIA = DR9["SECUENCIA"].ToString();
                                FACTURA = DR9["FACTURA"].ToString();
                                DOC_SAP = DR9["DOC_SAP"].ToString();
                                numeroAutorizacion = DR9["numeroAutorizacion"].ToString();
                                FECHA = DR9["FECHA"].ToString();
                                clsLogger.Graba_Log_Info("ProcesoInsertar_secuencia_tablaTEMP: " + FECHA);
                                SUBTOTAL = DR9["SUBTOTAL"].ToString();
                                DESCT = DR9["DESCT"].ToString();
                                JUBILACION_IESS = DR9["JUBILACION_IESS"].ToString();
                                IVA12 = DR9["IVA12"].ToString();
                                RET_IVA = DR9["RET_IVA"].ToString();
                                RET_FUENTE = DR9["RET_FUENTE"].ToString();
                                ICE = DR9["ICE"].ToString();
                                TOTAL = DR9["TOTAL"].ToString();
                                fechaAutorizacion = DR9["fechaAutorizacion"].ToString();
                                RFCEMI = DR9["RFCEMI"].ToString();
                                EmailCli = DR9["EmailCli"].ToString();
                                Insertar_secuencia_tablaTEMP1(RFCREC, NOMREC, documento, TIPODOC, estab, ptoemi, secu, SECUENCIA, ID_EMISOR, FACTURA, DOC_SAP, numeroAutorizacion, FECHA, SUBTOTAL, DESCT, IVA12, JUBILACION_IESS, ICE, RET_IVA, RET_FUENTE, TOTAL, fechaAutorizacion, RFCEMI, EmailCli);
                            }
                            else
                            {
                                SECUENCIA = estab + ptoemi + secu;
                                Insertar_secuencia_tablaTEMP2(documento, estab, ptoemi, secu, SECUENCIA, ID_EMISOR, FECHA);
                            }
                        }
                        DB.Desconectar();
                        secuenciamin = secuenciamin + 1;
                    }
                    secuenciamax = 0;
                    secuenciamin = 0;
                }
            }
            catch (Exception ea)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error("ProcesoInsertar_secuencia_tablaTEMP: " + ea.ToString());
            }
            finally
            {
                DB.Desconectar();
            }
        }

        public void Insertar_secuencia_tablaTEMP1(string ruc, string nombrec, string documento, string tipoDoc, string estab, string ptoemi, string secuencial, string secuencia, string ideemisor, string factura, string doc_sap, string numeroAut, string fecha, string subtotal, string descuento, string iva, string jubilacion, string ice, string retiva, string retfuente, string total, string fechaAut, string RFCEMI_, string EmailCli_)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Desconectar();
                DB.Conectar();
                DB.CrearComandoProcedimiento("Insert_Temp_reporte");
                DB.AsignarParametroProcedimiento("@rfc", System.Data.DbType.String, ruc);
                DB.AsignarParametroProcedimiento("@nombrec", System.Data.DbType.String, nombrec);
                DB.AsignarParametroProcedimiento("@codDoc", System.Data.DbType.String, documento);
                DB.AsignarParametroProcedimiento("@tipoDoc", System.Data.DbType.String, tipoDoc);
                DB.AsignarParametroProcedimiento("@estab", System.Data.DbType.String, estab);
                DB.AsignarParametroProcedimiento("@ptoemi", System.Data.DbType.String, ptoemi);
                DB.AsignarParametroProcedimiento("@secuencial", System.Data.DbType.String, secuencial);
                DB.AsignarParametroProcedimiento("@secuencia", System.Data.DbType.String, secuencia);
                DB.AsignarParametroProcedimiento("@ideemisor", System.Data.DbType.Int32, 0);
                DB.AsignarParametroProcedimiento("@factura", System.Data.DbType.String, factura);
                DB.AsignarParametroProcedimiento("@doc_sap", System.Data.DbType.String, doc_sap);
                DB.AsignarParametroProcedimiento("@numeroAut", System.Data.DbType.String, numeroAut);
                DB.AsignarParametroProcedimiento("@fecha", System.Data.DbType.DateTime, Convert.ToDateTime(fecha));
                DB.AsignarParametroProcedimiento("@subtotal", System.Data.DbType.Decimal, valida_texto_a_numero(subtotal));
                DB.AsignarParametroProcedimiento("@descuento", System.Data.DbType.Decimal, valida_texto_a_numero(descuento));
                DB.AsignarParametroProcedimiento("@iva", System.Data.DbType.Decimal, valida_texto_a_numero(iva));
                DB.AsignarParametroProcedimiento("@jubilacion", System.Data.DbType.Decimal, valida_texto_a_numero(jubilacion));
                DB.AsignarParametroProcedimiento("@ice", System.Data.DbType.Decimal, valida_texto_a_numero(ice));
                DB.AsignarParametroProcedimiento("@retiva", System.Data.DbType.Decimal, valida_texto_a_numero(retiva));
                DB.AsignarParametroProcedimiento("@retfuente", System.Data.DbType.Decimal, valida_texto_a_numero(retfuente));
                DB.AsignarParametroProcedimiento("@total", System.Data.DbType.Decimal, valida_texto_a_numero(total));
                DB.AsignarParametroProcedimiento("@fechaAut", System.Data.DbType.DateTime, Convert.ToDateTime(fechaAut));
                DB.AsignarParametroProcedimiento("@RFCEMI", System.Data.DbType.String, RFCEMI_);
                DB.AsignarParametroProcedimiento("@EmailCli", System.Data.DbType.String, EmailCli_);
                using (var x = DB.EjecutarConsulta())
                {
                }
                DB.Desconectar();
            }
            catch (Exception ea)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error("Insertar_secuencia_tablaTEMP1: " + ea.ToString());
            }
            finally
            {
                DB.Desconectar();
            }
        }

        public void Insertar_secuencia_tablaTEMP2(string documento, string estab, string ptoemi, string secuencial, string secuencia, string ideemisors, string fechas)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Desconectar();
                DB.Conectar();
                DB.CrearComandoProcedimiento("Insert_Tempnull_reporte");
                DB.AsignarParametroProcedimiento("@codDoc", System.Data.DbType.String, documento);
                DB.AsignarParametroProcedimiento("@estab", System.Data.DbType.String, estab);
                DB.AsignarParametroProcedimiento("@ptoemi", System.Data.DbType.String, ptoemi);
                DB.AsignarParametroProcedimiento("@secuencial", System.Data.DbType.String, secuencial);
                DB.AsignarParametroProcedimiento("@secuencia", System.Data.DbType.String, secuencia);
                DB.AsignarParametroProcedimiento("@ideemisor", System.Data.DbType.Int32, 0);
                DB.AsignarParametroProcedimiento("@fecha", System.Data.DbType.DateTime, Convert.ToDateTime(fechas));
                using (var x = DB.EjecutarConsulta())
                {
                }
                DB.Desconectar();
            }
            catch (Exception ea)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error("Insertar_secuencia_tablaTEMP2: " + ea.ToString());
            }
            finally
            {
                DB.Desconectar();
            }
        }

        public void Elimina_tabla_Temporal()
        {
            var DB = new BasesDatos();
            try
            {
                DB.Conectar();
                DB.CrearComandoProcedimiento("delete_Tempnull_reporte");
                using (var x = DB.EjecutarConsulta())
                {
                }
                DB.Desconectar();
            }
            catch (Exception ea)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error("Elimina_tabla_Temporal: " + ea.ToString());
            }
            finally
            {
                DB.Desconectar();
            }
        }

        private string valida_texto_a_numero(string p_valor)
        {
            if (string.IsNullOrEmpty(p_valor))
            {
                p_valor = "0";
            }
            else
            {
                p_valor.Trim();
            }
            return p_valor;
        }


        #region OLDHISTORY
        public DataSet ConsultarecepInfoOLD(string fechar, string fechanomr, string opcion, string doc, string estab, string ptoemi, string ruc)
        {
            var DB = new BasesDatos();
            System.Data.DataSet dsDatodsR = new System.Data.DataSet();
            try
            {
                DB.Conectar2();
                StringBuilder documentoXMLR = new StringBuilder("");
                documentoXMLR.Append("<INSTRUCCION>");
                documentoXMLR.Append("<FILTRO>");
                documentoXMLR.Append("<opcion>" + opcion + "</opcion>");
                documentoXMLR.Append("<FechaMinima>" + String.Format("{0:yyyyMMdd H:mm:ss}", Convert.ToDateTime(fechar + " 00:00:00")) + "</FechaMinima>");
                documentoXMLR.Append("<Fechamaxima>" + String.Format("{0:yyyyMMdd H:mm:ss}", Convert.ToDateTime(fechanomr + " 23:59:58")) + "</Fechamaxima>");
                documentoXMLR.Append("<documento>" + doc + "</documento>");
                documentoXMLR.Append("<estab>" + estab + "</estab>");
                documentoXMLR.Append("<ptoemi>" + ptoemi + "</ptoemi>");
                documentoXMLR.Append("<ruc>" + ruc + "</ruc>");
                documentoXMLR.Append("</FILTRO>");
                documentoXMLR.Append("</INSTRUCCION>");
                dsDatodsR = DB.TraerDataset2("PA_SerReportes_Emi", documentoXMLR.ToString());
            }
            catch (Exception ea)
            {
                DB.Desconectar2();
                clsLogger.Graba_Log_Error("ConsultarecepInfoOLD: " + ea.ToString());
            }
            finally
            {
                DB.Desconectar();
            }
            return dsDatodsR;
        }

        public DataSet ConsultaTipoComprobanteOLD()
        {
            string sucursal = "";
            string ptoEmision = "";
            string documento = "";
            string opc = "";
            fecharini = "";
            fecharfin = "";
            secuenciamin = 0;
            secuenciamax = 0;
            fecha = calentario.SelectedDate.ToShortDateString();
            fechanom = calendario2.SelectedDate.ToShortDateString();
            fecharini = fecha;
            fecharfin = fechanom;
            System.Data.DataSet dsDatods = new System.Data.DataSet();
            if (ddlDocumento.SelectedIndex != 0 && ddlSucursalR.SelectedIndex != 0 && ddlPtoEmiR.SelectedIndex != 0 && tb_ruc.Text == "")
            {
                opc = "1";
                documento = ddlDocumento.SelectedValue;
                sucursal = ddlSucursalR.SelectedValue;
                ptoEmision = ddlPtoEmiR.SelectedValue;
                Elimina_tabla_TemporalOLD();
                Consulta_secuenciaOLD(sucursal, ptoEmision, documento);
                ProcesoInsertar_secuencia_tablaTEMPOLD(sucursal, ptoEmision, documento);
            }
            else if (ddlDocumento.SelectedIndex == 0 && ddlSucursalR.SelectedIndex != 0 && ddlPtoEmiR.SelectedIndex != 0 && tb_ruc.Text == "")
            {
                opc = "1";
                documento = "";
                sucursal = ddlSucursalR.SelectedValue;
                ptoEmision = ddlPtoEmiR.SelectedValue;

            }
            else if (ddlDocumento.SelectedIndex == 0 && ddlSucursalR.SelectedIndex == 0 && (ddlPtoEmiR.SelectedIndex == 0 || ddlPtoEmiR.SelectedValue == "Selecciona...") && tb_ruc.Text == "")
            {
                opc = "2";
                documento = "";
                sucursal = "";
                ptoEmision = "";
            }
            else if (ddlDocumento.SelectedIndex != 0 && ddlSucursalR.SelectedIndex == 0 && (ddlPtoEmiR.SelectedIndex == 0 || ddlPtoEmiR.SelectedValue == "Selecciona...") && tb_ruc.Text == "")
            {
                opc = "2";
                documento = ddlDocumento.SelectedValue;
                sucursal = "";
                ptoEmision = "";
            }
            else if (ddlDocumento.SelectedIndex != 0 && ddlSucursalR.SelectedIndex != 0 && ddlPtoEmiR.SelectedValue == "Selecciona..." && tb_ruc.Text == "")
            {
                opc = "3";
                documento = ddlDocumento.SelectedValue;
                sucursal = ddlSucursalR.SelectedValue;
                ptoEmision = "";
            }
            else if (ddlDocumento.SelectedIndex == 0 && ddlSucursalR.SelectedIndex != 0 && (ddlPtoEmiR.SelectedIndex == 0 || ddlPtoEmiR.SelectedValue == "Selecciona...") && tb_ruc.Text == "")
            {
                opc = "3";
                documento = "";
                sucursal = ddlSucursalR.SelectedValue;
                ptoEmision = "";
            }
            else if (ddlDocumento.SelectedIndex != 0 && ddlSucursalR.SelectedIndex != 0 && ddlPtoEmiR.SelectedIndex != 0 && tb_ruc.Text != "")
            {
                opc = "4";
                documento = ddlDocumento.SelectedValue;
                sucursal = ddlSucursalR.SelectedValue;
                ptoEmision = ddlPtoEmiR.SelectedValue;
            }
            else if (ddlDocumento.SelectedIndex == 0 && ddlSucursalR.SelectedIndex != 0 && ddlPtoEmiR.SelectedIndex != 0 && tb_ruc.Text != "")
            {
                opc = "4";
                documento = "";
                sucursal = ddlSucursalR.SelectedValue;
                ptoEmision = ddlPtoEmiR.SelectedValue;
            }
            else if (ddlDocumento.SelectedIndex == 0 && ddlSucursalR.SelectedIndex == 0 && (ddlPtoEmiR.SelectedIndex == 0 || ddlPtoEmiR.SelectedValue == "Selecciona...") && tb_ruc.Text != "")
            {
                opc = "5";
                documento = "";
                sucursal = "";
                ptoEmision = "";
            }
            else if (ddlDocumento.SelectedIndex != 0 && ddlSucursalR.SelectedIndex == 0 && (ddlPtoEmiR.SelectedIndex == 0 || ddlPtoEmiR.SelectedValue == "Selecciona...") && tb_ruc.Text != "")
            {
                opc = "5";
                documento = ddlDocumento.SelectedValue;
                sucursal = "";
                ptoEmision = "";
            }
            else if (ddlDocumento.SelectedIndex != 0 && ddlSucursalR.SelectedIndex != 0 && ddlPtoEmiR.SelectedValue == "Selecciona..." && tb_ruc.Text != "")
            {
                opc = "6";
                documento = ddlDocumento.SelectedValue;
                sucursal = ddlSucursalR.SelectedValue;
                ptoEmision = "";

            }
            else if (ddlDocumento.SelectedIndex == 0 && ddlSucursalR.SelectedIndex != 0 && (ddlPtoEmiR.SelectedIndex == 0 || ddlPtoEmiR.SelectedValue == "Selecciona...") && tb_ruc.Text != "")
            {
                opc = "6";
                documento = "";
                sucursal = ddlSucursalR.SelectedValue;
                ptoEmision = "";
            }

            dsDatods = ConsultarecepInfoOLD(fecha, fechanom, opc, documento, sucursal, ptoEmision, tb_ruc.Text);
            return dsDatods;
        }
        public void Consulta_secuenciaOLD(string estab, string ptoemi, string documento)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Conectar();
                DB.CrearComando(@"select  isnull(min(secuencial),0) as secuencialmin,  isnull(max(secuencial),0) as secuencialmax from GENERAL WITH (NOLOCK) where estab=@estab and ptoEmi=@ptoEmi and codDoc=@codDoc and fecha >= @fechaMinima and fecha <= @fechaMaxima and ambiente=2");
                DB.AsignarParametroCadena("@estab", estab);
                DB.AsignarParametroCadena("@ptoEmi", ptoemi);
                DB.AsignarParametroCadena("@codDoc", documento);
                DB.AsignarParametroCadena("@fechaMinima", String.Format("{0:yyyyMMdd H:mm:ss}", Convert.ToDateTime(fecharini + " 00:00:00")));
                DB.AsignarParametroCadena("@fechaMaxima", String.Format("{0:yyyyMMdd H:mm:ss}", Convert.ToDateTime(fecharfin + " 23:59:58")));
                using (DbDataReader DR8 = DB.EjecutarConsulta())
                {
                    if (DR8.Read())
                    {
                        secuenciamin = Convert.ToInt32(DR8["secuencialmin"].ToString());
                        secuenciamax = Convert.ToInt32(DR8["secuencialmax"].ToString());
                    }
                }

                DB.Desconectar();
            }
            catch (Exception ea)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error("Consulta_secuenciaOLD: " + ea.ToString());
            }
            finally
            {
                DB.Desconectar();
            }
        }

        public void ProcesoInsertar_secuencia_tablaTEMPOLD(string estab, string ptoemi, string documento)
        {
            var DB = new BasesDatos();
            try
            {
                string secuold = "";
                if ((secuenciamax > 0 && secuenciamin > 0))
                {
                    while (secuenciamin <= secuenciamax)
                    {
                        secuold = asignandoleSecuencial(secuenciamin.ToString());
                        setear_variables_temp();
                        DB.Conectar2();
                        DB.CrearComandoProcedimiento("Consulta_documento_reporte");
                        DB.AsignarParametroProcedimiento("@estab", System.Data.DbType.String, estab);
                        DB.AsignarParametroProcedimiento("@ptoEmi", System.Data.DbType.String, ptoemi);
                        DB.AsignarParametroProcedimiento("@codDoc", System.Data.DbType.String, documento);
                        DB.AsignarParametroProcedimiento("@secuencia", System.Data.DbType.String, secuold);
                        DB.AsignarParametroProcedimiento("@FechaMinima", System.Data.DbType.String, String.Format("{0:yyyyMMdd H:mm:ss}", Convert.ToDateTime(fecharini + " 00:00:00")));
                        DB.AsignarParametroProcedimiento("@Fechamaxima", System.Data.DbType.String, String.Format("{0:yyyyMMdd H:mm:ss}", Convert.ToDateTime(fecharfin + " 23:59:58")));
                        using (DbDataReader DR9 = DB.EjecutarConsulta())
                        {
                            if (DR9.Read())
                            {
                                RFCREC = DR9["RFCREC"].ToString();
                                NOMREC = DR9["NOMREC"].ToString();
                                TIPODOC = DR9["TIPODOC"].ToString();
                                ID_EMISOR = DR9["ID_EMISOR"].ToString();
                                SECUENCIA = DR9["SECUENCIA"].ToString();
                                FACTURA = DR9["FACTURA"].ToString();
                                DOC_SAP = DR9["DOC_SAP"].ToString();
                                numeroAutorizacion = DR9["numeroAutorizacion"].ToString();
                                FECHA = DR9["FECHA"].ToString();
                                SUBTOTAL = DR9["SUBTOTAL"].ToString();
                                DESCT = DR9["DESCT"].ToString();
                                JUBILACION_IESS = DR9["JUBILACION_IESS"].ToString();
                                IVA12 = DR9["IVA12"].ToString();
                                RET_IVA = DR9["RET_IVA"].ToString();
                                RET_FUENTE = DR9["RET_FUENTE"].ToString();
                                ICE = DR9["ICE"].ToString();
                                TOTAL = DR9["TOTAL"].ToString();
                                fechaAutorizacion = DR9["fechaAutorizacion"].ToString();
                                Insertar_secuencia_tablaTEMP1OLD(RFCREC, NOMREC, documento, TIPODOC, estab, ptoemi, secuold, SECUENCIA, Session["rucEmpresa"].ToString(), FACTURA, DOC_SAP, numeroAutorizacion, FECHA, SUBTOTAL, DESCT, IVA12, JUBILACION_IESS, ICE, RET_IVA, RET_FUENTE, TOTAL, fechaAutorizacion);
                            }
                            else
                            {
                                SECUENCIA = estab + ptoemi + secuold;
                                Insertar_secuencia_tablaTEMP2OLD(documento, estab, ptoemi, secuold, SECUENCIA, ID_EMISOR, FECHA);
                            }
                        }

                        DB.Desconectar();
                        secuenciamin = secuenciamin + 1;
                    }
                    secuenciamax = 0;
                    secuenciamin = 0;
                }
            }
            catch (Exception ea)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error("ProcesoInsertar_secuencia_tablaTEMPOLD: " + ea.ToString());
            }
            finally
            {
                DB.Desconectar();
            }


        }

        public void Insertar_secuencia_tablaTEMP1OLD(string ruc, string nombrec, string documento, string tipoDoc, string estab, string ptoemi, string secuencial, string secuencia, string ideemisor, string factura, string doc_sap, string numeroAut, string fecha, string subtotal, string descuento, string iva, string jubilacion, string ice, string retiva, string retfuente, string total, string fechaAut)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Desconectar2();
                DB.Conectar2();
                DB.CrearComandoProcedimiento("Insert_Temp_reporte");
                DB.AsignarParametroProcedimiento("@rfc", System.Data.DbType.String, ruc);
                DB.AsignarParametroProcedimiento("@nombrec", System.Data.DbType.String, nombrec);
                DB.AsignarParametroProcedimiento("@codDoc", System.Data.DbType.String, documento);
                DB.AsignarParametroProcedimiento("@tipoDoc", System.Data.DbType.String, tipoDoc);
                DB.AsignarParametroProcedimiento("@estab", System.Data.DbType.String, estab);
                DB.AsignarParametroProcedimiento("@ptoemi", System.Data.DbType.String, ptoemi);
                DB.AsignarParametroProcedimiento("@secuencial", System.Data.DbType.String, secuencial);
                DB.AsignarParametroProcedimiento("@secuencia", System.Data.DbType.String, secuencia);
                DB.AsignarParametroProcedimiento("@ideemisor", System.Data.DbType.Int32, 0);
                DB.AsignarParametroProcedimiento("@factura", System.Data.DbType.String, factura);
                DB.AsignarParametroProcedimiento("@doc_sap", System.Data.DbType.String, doc_sap);
                DB.AsignarParametroProcedimiento("@numeroAut", System.Data.DbType.String, numeroAut);
                DB.AsignarParametroProcedimiento("@fecha", System.Data.DbType.DateTime, Convert.ToDateTime(fecha));
                DB.AsignarParametroProcedimiento("@subtotal", System.Data.DbType.Decimal, valida_texto_a_numero(subtotal));
                DB.AsignarParametroProcedimiento("@descuento", System.Data.DbType.Decimal, valida_texto_a_numero(descuento));
                DB.AsignarParametroProcedimiento("@iva", System.Data.DbType.Decimal, valida_texto_a_numero(iva));
                DB.AsignarParametroProcedimiento("@jubilacion", System.Data.DbType.Decimal, valida_texto_a_numero(jubilacion));
                DB.AsignarParametroProcedimiento("@ice", System.Data.DbType.Decimal, valida_texto_a_numero(ice));
                DB.AsignarParametroProcedimiento("@retiva", System.Data.DbType.Decimal, valida_texto_a_numero(retiva));
                DB.AsignarParametroProcedimiento("@retfuente", System.Data.DbType.Decimal, valida_texto_a_numero(retfuente));
                DB.AsignarParametroProcedimiento("@total", System.Data.DbType.Decimal, valida_texto_a_numero(total));
                DB.AsignarParametroProcedimiento("@fechaAut", System.Data.DbType.DateTime, Convert.ToDateTime(fechaAut));
                using (var x = DB.EjecutarConsulta())
                {
                }
                DB.Desconectar2();
            }
            catch (Exception ea)
            {
                DB.Desconectar2();
                clsLogger.Graba_Log_Error("Insertar_secuencia_tablaTEMP1OLD: " + ea.ToString());
            }
            finally
            {
                DB.Desconectar2();
            }
        }

        public void Insertar_secuencia_tablaTEMP2OLD(string documento, string estab, string ptoemi, string secuencial, string secuencia, string ideemisors, string fechas)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Desconectar2();
                DB.Conectar2();
                DB.CrearComandoProcedimiento("Insert_Tempnull_reporte");
                DB.AsignarParametroProcedimiento("@codDoc", System.Data.DbType.String, documento);
                DB.AsignarParametroProcedimiento("@estab", System.Data.DbType.String, estab);
                DB.AsignarParametroProcedimiento("@ptoemi", System.Data.DbType.String, ptoemi);
                DB.AsignarParametroProcedimiento("@secuencial", System.Data.DbType.String, secuencial);
                DB.AsignarParametroProcedimiento("@secuencia", System.Data.DbType.String, secuencia);
                DB.AsignarParametroProcedimiento("@ideemisor", System.Data.DbType.Int32, 0);
                DB.AsignarParametroProcedimiento("@fecha", System.Data.DbType.DateTime, Convert.ToDateTime(fechas));
                using (var x = DB.EjecutarConsulta())
                {
                }
                DB.Desconectar2();
            }
            catch (Exception ea)
            {
                DB.Desconectar2();
                clsLogger.Graba_Log_Error("Insertar_secuencia_tablaTEMP2OLD: " + ea.ToString());
            }
            finally
            {
                DB.Desconectar2();
            }
        }

        public void Elimina_tabla_TemporalOLD()
        {
            var DB = new BasesDatos();
            try
            {
                DB.Conectar2();
                DB.CrearComandoProcedimiento("delete_Tempnull_reporte");
                using (var x = DB.EjecutarConsulta())
                {
                }
                DB.Desconectar2();
            }
            catch (Exception ea)
            {
                DB.Desconectar2();
                clsLogger.Graba_Log_Error("Elimina_tabla_TemporalOLD: " + ea.ToString());
            }
            finally
            {
                DB.Desconectar2();
            }
        }
        #endregion
    }
}