using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Reportes;
using System.Text;
using Datos;
using System.IO;
using System.Xml;
using clibLogger;

namespace DataExpressWeb.recepcion
{
    public partial class PopUpFE : System.Web.UI.Page
    {
        String ds_xml = "", version = "", claveAcceso = "", numAutorizacion = "", fechaAut = "", codDoc = "", tipo = "";
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
                ds_xml = ""; version = ""; claveAcceso = ""; numAutorizacion = ""; fechaAut = ""; codDoc = ""; tipo = "";
                claveAcceso = Request.QueryString.Get("CA");
                tipo = Request.QueryString.Get("tipo");
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
                        numAutorizacion = ds.Tables["Table"].Rows[0][14].ToString();
                        fechaAut = ds.Tables["Table"].Rows[0][15].ToString();
                        ds_xml = ds.Tables["Table"].Rows[0][16].ToString();
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(ds_xml);
                        XmlElement fe = doc.DocumentElement;
                        XmlNodeList xnodeList = fe.GetElementsByTagName("ds:Signature");
                        if (xnodeList.Count > 0)
                        {
                            XmlNode xnode = xnodeList.Item(0);
                            fe.RemoveChild(xnode);
                        }
                        DataSet dsdoc = new DataSet("rpt");
                        dsdoc.ReadXml(new XmlTextReader(new StringReader(doc.InnerXml)), XmlReadMode.Auto);
                        byte[] imgBar = ImageToByte2(GenCode128.Code128Rendering.MakeBarcodeImage(claveAcceso, 3, true));
                        switch (tipo)
                        {
                            case "1":
                                ds_xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + ds_xml;
                                XmlDocument vdoc = new XmlDocument();
                                vdoc.LoadXml(ds_xml);
                                this.txtXml.Text = vdoc.OuterXml;
                                this.txtXml.Visible = true;
                                break;
                            case "2":
                                String rutaDocumento = System.AppDomain.CurrentDomain.BaseDirectory + @"recepcion\docus\";
                                String nombre = DateTime.Now.ToString("MM_dd_yyyyhh_mm_ss") + ".pdf";
                                String tablaInformacion = "";
                                tablaInformacion = (dsdoc.Tables.Contains("infoFactura") ? "infoFactura" :
                                    dsdoc.Tables.Contains("infoNotaCredito") ? "infoNotaCredito" :
                                    dsdoc.Tables.Contains("infoCompRetencion") ? "infoCompRetencion" :
                                    dsdoc.Tables.Contains("infoNotaDebito") ? "infoNotaDebito" : "infoGuiaRemision");
                                dsdoc.Tables[tablaInformacion].Columns.Add("codBarra", typeof(Byte[]));
                                dsdoc.Tables[tablaInformacion].Columns.Add("p_numAut", typeof(String));
                                dsdoc.Tables[tablaInformacion].Columns.Add("p_fechaAut", typeof(String));
                                dsdoc.Tables[tablaInformacion].Rows[0]["codBarra"] = imgBar;
                                dsdoc.Tables[tablaInformacion].Rows[0]["p_numAut"] = numAutorizacion;
                                dsdoc.Tables[tablaInformacion].Rows[0]["p_fechaAut"] = fechaAut;
                                DirectoryInfo DIR = new DirectoryInfo(rutaDocumento);
                                if (!DIR.Exists)
                                    DIR.Create();
                                switch (version)
                                {
                                    case "1.0.0":
                                    case "1.1.0":
                                        switch (codDoc)
                                        {
                                            case "01":
                                                System.Data.DataSet dsNuevo = LLenarContenido1_0_0(dsdoc, codDoc);
                                                comprobanteFactura1_0_0 report = new comprobanteFactura1_0_0();
                                                report.SetDataSource(dsNuevo);
                                                report.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaDocumento + nombre);
                                                report.Close();
                                                break;
                                            case "04":
                                                System.Data.DataSet dsNuevoNC = LLenarContenido1_0_0(dsdoc, codDoc);
                                                comprobanteNC1_0_0 reportnc = new comprobanteNC1_0_0();
                                                reportnc.SetDataSource(dsNuevoNC);
                                                reportnc.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaDocumento + nombre);
                                                reportnc.Close();
                                                //NC1_0_0 reportnc = new NC1_0_0();
                                                //reportnc.Database.Tables[0].SetDataSource(dsdoc);
                                                //CRV_FE.ReportSource = reportnc;
                                                //reportnc.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaDocumento + nombre);
                                                //reportnc.Close();
                                                break;
                                            case "07":
                                                System.Data.DataSet dsNuevoCR = LLenarContenido1_0_0(dsdoc, codDoc);
                                                comprobanteCR1_0_0 reportcr = new comprobanteCR1_0_0();
                                                reportcr.SetDataSource(dsNuevoCR);
                                                reportcr.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaDocumento + nombre);
                                                reportcr.Close();
                                                break;
                                        }
                                        break;
                                    case "2_00":
                                        factura2_00 report2 = new factura2_00();
                                        report2.Database.Tables[0].SetDataSource(dsdoc);
                                        report2.SetParameterValue("p_numAut", numAutorizacion);
                                        report2.SetParameterValue("p_fechaAut", fechaAut);
                                        report2.SetParameterValue("p_codBarra", claveAcceso);
                                        CRV_FE.ReportSource = report2;
                                        report2.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, rutaDocumento + nombre);
                                        report2.Close();
                                        break;
                                    default:
                                        lblMensaje.Text = "No se puede mostrar el documento";
                                        lblMensaje.Visible = true;
                                        break;
                                }
                                String scrip = "<script language='javascript'>" +
                                                                 " window.close(); window.opener.HideModalDiv();" +
                                                                 "window.open('../recepcion/docus/" + nombre + "', '', 'width=800, height=600 , left=340,  top=70, status=0toolbar=no,scrollbars=no,menubar=no');" +
                                                                 "</script>";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "ajax", scrip, false);
                                String[] arregloArchivo = Directory.GetFiles(rutaDocumento);
                                for (int i = 0; i < arregloArchivo.Length; i++)
                                {
                                    FileInfo archivos = new FileInfo(arregloArchivo[i]);
                                    if (Convert.ToDateTime(archivos.CreationTime.ToString()) < DateTime.Now.AddDays(-1))
                                        File.Delete(arregloArchivo[i]);
                                }
                                descargarArchivo(@"docus/" + nombre);
                                break;
                        }
                        dsdoc.Dispose();
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

        private void descargarArchivo(String filename)
        {
            if (!String.IsNullOrEmpty(filename))
            {
                String dlDir = "";//@"docus/";
                String path = Server.MapPath(dlDir + filename);
                System.IO.FileInfo toDownload =
                             new System.IO.FileInfo(path);
                if (toDownload.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition",
                               "attachment; filename=" + toDownload.Name);
                    Response.AddHeader("Content-Length",
                               toDownload.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.WriteFile(dlDir + filename);
                    Response.End();
                }
            }
        }

        private System.Data.DataSet LLenarContenido1_0_0(System.Data.DataSet ds, String codDoc)
        {
            System.Data.DataSet ds_tablas = new System.Data.DataSet("ComprobantesRPT");
            String etiquetaInicial = "", etiquetaInfo = "";
            DataTable dtComprobante = new DataTable(), infoTributaria = new DataTable(), infoComprobante = new DataTable(), totalConImpuestos = new DataTable(),
                detalles = new DataTable(), detalle = new DataTable(), impuestos = new DataTable(), impuesto = new DataTable(), detalleAdicionales = new DataTable(),
                detAdicional = new DataTable(), infoAdicional = new DataTable(), campoAdicional = new DataTable(), motivos = new DataTable(), motivo = new DataTable(),
                destinatarios = new DataTable(), destinatario = new DataTable();
            #region "Inicializacion de variables dinamicos"
            if (codDoc.Equals("01"))
            {
                etiquetaInicial = "factura";
                dtComprobante = new Reportes.xds._1_0_0.comproxsdFactura1_0_0.facturaDataTable();
                infoTributaria = new Reportes.xds._1_0_0.comproxsdFactura1_0_0.infoTributariaDataTable();
                etiquetaInfo = "infoFactura";
                infoComprobante = new Reportes.xds._1_0_0.comproxsdFactura1_0_0.infoFacturaDataTable();
                totalConImpuestos = new Reportes.xds._1_0_0.comproxsdFactura1_0_0.totalConImpuestosDataTable();
                ds_tablas.Tables.Add(totalConImpuestos);
                detalles = new Reportes.xds._1_0_0.comproxsdFactura1_0_0.detallesDataTable();
                ds_tablas.Tables.Add(detalles);
                detalle = new Reportes.xds._1_0_0.comproxsdFactura1_0_0.detalleDataTable();
                impuestos = new Reportes.xds._1_0_0.comproxsdFactura1_0_0.impuestosDataTable();
                ds_tablas.Tables.Add(impuestos);
                impuesto = new Reportes.xds._1_0_0.comproxsdFactura1_0_0.impuestoDataTable();
                detalleAdicionales = new Reportes.xds._1_0_0.comproxsdFactura1_0_0.detallesAdicionalesDataTable();
                ds_tablas.Tables.Add(detalleAdicionales);
                detAdicional = new Reportes.xds._1_0_0.comproxsdFactura1_0_0.detAdicionalDataTable();
                ds_tablas.Tables.Add(detAdicional);
                infoAdicional = new Reportes.xds._1_0_0.comproxsdFactura1_0_0.infoAdicionalDataTable();
                campoAdicional = new Reportes.xds._1_0_0.comproxsdFactura1_0_0.campoAdicionalDataTable();
            }
            else if (codDoc.Equals("04"))
            {
                etiquetaInicial = "notaCredito";
                dtComprobante = new Reportes.xds._1_0_0.comproxsdNC1_0_0.notaCreditoDataTable();
                infoTributaria = new Reportes.xds._1_0_0.comproxsdNC1_0_0.infoTributariaDataTable();
                etiquetaInfo = "infoNotaCredito";
                infoComprobante = new Reportes.xds._1_0_0.comproxsdNC1_0_0.infoNotaCreditoDataTable();
                totalConImpuestos = new Reportes.xds._1_0_0.comproxsdNC1_0_0.totalConImpuestosDataTable();
                ds_tablas.Tables.Add(totalConImpuestos);
                detalles = new Reportes.xds._1_0_0.comproxsdNC1_0_0.detallesDataTable();
                ds_tablas.Tables.Add(detalles);
                detalle = new Reportes.xds._1_0_0.comproxsdNC1_0_0.detalleDataTable();
                impuestos = new Reportes.xds._1_0_0.comproxsdNC1_0_0.impuestosDataTable();
                ds_tablas.Tables.Add(impuestos);
                impuesto = new Reportes.xds._1_0_0.comproxsdNC1_0_0.impuestoDataTable();
                detalleAdicionales = new Reportes.xds._1_0_0.comproxsdNC1_0_0.detallesAdicionalesDataTable();
                ds_tablas.Tables.Add(detalleAdicionales);
                detAdicional = new Reportes.xds._1_0_0.comproxsdNC1_0_0.detAdicionalDataTable();
                ds_tablas.Tables.Add(detAdicional);
                infoAdicional = new Reportes.xds._1_0_0.comproxsdNC1_0_0.infoAdicionalDataTable();
                campoAdicional = new Reportes.xds._1_0_0.comproxsdNC1_0_0.campoAdicionalDataTable();
            }
            else if (codDoc.Equals("05"))
            {
                etiquetaInicial = "notaDebito";
                dtComprobante = new Reportes.xds._1_0_0.comproxsdND1_0_0.notaDebitoDataTable();
                infoTributaria = new Reportes.xds._1_0_0.comproxsdND1_0_0.infoTributariaDataTable();
                etiquetaInfo = "infoNotaDebito";
                infoComprobante = new Reportes.xds._1_0_0.comproxsdND1_0_0.notaDebitoDataTable();
                infoAdicional = new Reportes.xds._1_0_0.comproxsdND1_0_0.infoAdicionalDataTable();
                campoAdicional = new Reportes.xds._1_0_0.comproxsdND1_0_0.campoAdicionalDataTable();
                motivos = new Reportes.xds._1_0_0.comproxsdND1_0_0.motivosDataTable();
                ds_tablas.Tables.Add(motivos);
                motivo = new Reportes.xds._1_0_0.comproxsdND1_0_0.motivoDataTable();
            }
            else if (codDoc.Equals("07"))
            {
                etiquetaInicial = "comprobanteRetencion";
                dtComprobante = new Reportes.xds._1_0_0.comproxsdCR1_0_0.comprobanteRetencionDataTable();
                infoTributaria = new Reportes.xds._1_0_0.comproxsdCR1_0_0.infoTributariaDataTable();
                etiquetaInfo = "infoCompRetencion";
                infoComprobante = new Reportes.xds._1_0_0.comproxsdCR1_0_0.infoCompRetencionDataTable();
                infoAdicional = new Reportes.xds._1_0_0.comproxsdCR1_0_0.infoAdicionalDataTable();
                campoAdicional = new Reportes.xds._1_0_0.comproxsdCR1_0_0.campoAdicionalDataTable();
                impuestos = new Reportes.xds._1_0_0.comproxsdCR1_0_0.impuestosDataTable();
                ds_tablas.Tables.Add(impuestos);
                impuesto = new Reportes.xds._1_0_0.comproxsdCR1_0_0.impuestoDataTable();
            }
            else
            {
                etiquetaInicial = "guiaRemision";
                dtComprobante = new Reportes.xds._1_0_0.comproxsdGuiaRemision1_0_0.guiaRemisionDataTable();
                infoTributaria = new Reportes.xds._1_0_0.comproxsdGuiaRemision1_0_0.infoTributariaDataTable();
                etiquetaInfo = "infoGuiaRemision";
                infoComprobante = new Reportes.xds._1_0_0.comproxsdGuiaRemision1_0_0.infoGuiaRemisionDataTable();
                infoAdicional = new Reportes.xds._1_0_0.comproxsdGuiaRemision1_0_0.infoAdicionalDataTable();
                campoAdicional = new Reportes.xds._1_0_0.comproxsdGuiaRemision1_0_0.campoAdicionalDataTable();
                destinatarios = new Reportes.xds._1_0_0.comproxsdGuiaRemision1_0_0.destinatariosDataTable();
                ds_tablas.Tables.Add(destinatarios);
                destinatario = new Reportes.xds._1_0_0.comproxsdGuiaRemision1_0_0.destinatarioDataTable();
                detalles = new Reportes.xds._1_0_0.comproxsdGuiaRemision1_0_0.detallesDataTable();
                ds_tablas.Tables.Add(detalles);
                detalle = new Reportes.xds._1_0_0.comproxsdGuiaRemision1_0_0.detalleDataTable();
            }
            ds_tablas.Tables.Add(infoAdicional);
            ds_tablas.Tables.Add(campoAdicional);
            #endregion

            if (ds.Tables.Count > 0)
            {
                #region "Ingreso de tabla facturas"
                if (ds.Tables.Contains(etiquetaInicial))
                    if (ds.Tables[etiquetaInicial].Rows.Count > 0)
                    {
                        String[] columnas = new String[ds.Tables[etiquetaInicial].Columns.Count];
                        for (int i = 0; i < ds.Tables[etiquetaInicial].Columns.Count; i++)
                            columnas[i] = ds.Tables[etiquetaInicial].Columns[i].ColumnName;
                        foreach (DataRow dr in ds.Tables[etiquetaInicial].Rows)
                        {
                            DataRow dr_factura = dtComprobante.NewRow();
                            dr_factura["id"] = (columnas.Contains("id") ? dr["id"].ToString() : "");
                            dr_factura["version"] = (columnas.Contains("version") ? dr["version"].ToString() : "");

                            if (codDoc.Equals("01"))
                                dr_factura["factura_Id"] = (columnas.Contains("factura_Id") ? dr["factura_Id"].ToString() : "");
                            else if (codDoc.Equals("04"))
                                dr_factura["notaCredito_Id"] = (columnas.Contains("notaCredito_Id") ? dr["notaCredito_Id"].ToString() : "");
                            else if (codDoc.Equals("07"))
                                dr_factura["comprobanteRetencion_Id"] = (columnas.Contains("comprobanteRetencion_Id") ? dr["comprobanteRetencion_Id"].ToString() : "");
                            else if (codDoc.Equals("05"))
                                dr_factura["notaDebito_Id"] = (columnas.Contains("notaDebito_Id") ? dr["notaDebito_Id"].ToString() : "");
                            else
                                dr_factura["guiaRemision_Id"] = (columnas.Contains("guiaRemision_Id") ? dr["guiaRemision_Id"].ToString() : "");
                            dtComprobante.Rows.Add(dr_factura);
                        }
                        ds_tablas.Tables.Add(dtComprobante);
                    }
                #endregion
                #region "Ingreso de Informacion Tributaria"
                if (ds.Tables.Contains("infoTributaria"))
                    if (ds.Tables["infoTributaria"].Rows.Count > 0)
                    {
                        String[] columnas = new String[ds.Tables["infoTributaria"].Columns.Count];
                        for (int i = 0; i < ds.Tables["infoTributaria"].Columns.Count; i++)
                            columnas[i] = ds.Tables["infoTributaria"].Columns[i].ColumnName;
                        foreach (DataRow dr in ds.Tables["infoTributaria"].Rows)
                        {
                            DataRow dr_infoTributaria = infoTributaria.NewRow();
                            dr_infoTributaria["ambiente"] = (columnas.Contains("ambiente") ? dr["ambiente"] : "");
                            dr_infoTributaria["tipoEmision"] = (columnas.Contains("tipoEmision") ? dr["tipoEmision"] : "");
                            dr_infoTributaria["razonSocial"] = (columnas.Contains("razonSocial") ? dr["razonSocial"] : "");
                            dr_infoTributaria["nombreComercial"] = (columnas.Contains("nombreComercial") ? dr["nombreComercial"] : "");
                            dr_infoTributaria["ruc"] = (columnas.Contains("ruc") ? dr["ruc"] : "");
                            dr_infoTributaria["claveAcceso"] = (columnas.Contains("claveAcceso") ? dr["claveAcceso"] : "");
                            dr_infoTributaria["codDoc"] = (columnas.Contains("codDoc") ? dr["codDoc"] : "");
                            dr_infoTributaria["estab"] = (columnas.Contains("estab") ? dr["estab"] : "");
                            dr_infoTributaria["ptoEmi"] = (columnas.Contains("ptoEmi") ? dr["ptoEmi"] : "");
                            dr_infoTributaria["secuencial"] = (columnas.Contains("secuencial") ? dr["secuencial"] : "");
                            dr_infoTributaria["dirMatriz"] = (columnas.Contains("dirMatriz") ? dr["dirMatriz"] : "");
                            if (codDoc.Equals("01"))
                                dr_infoTributaria["factura_Id"] = (columnas.Contains("factura_Id") ? dr["factura_Id"] : 0);
                            else if (codDoc.Equals("04"))
                                dr_infoTributaria["notaCredito_Id"] = (columnas.Contains("notaCredito_Id") ? dr["notaCredito_Id"] : 0);
                            else if (codDoc.Equals("07"))
                                dr_infoTributaria["comprobanteRetencion_Id"] = (columnas.Contains("comprobanteRetencion_Id") ? dr["comprobanteRetencion_Id"].ToString() : "");
                            else if (codDoc.Equals("05"))
                                dr_infoTributaria["notaDebito_Id"] = (columnas.Contains("notaDebito_Id") ? dr["notaDebito_Id"].ToString() : "");
                            else
                                dr_infoTributaria["guiaRemision_Id"] = (columnas.Contains("guiaRemision_Id") ? dr["guiaRemision_Id"].ToString() : "");
                            infoTributaria.Rows.Add(dr_infoTributaria);
                        }
                        ds_tablas.Tables.Add(infoTributaria);
                    }
                #endregion
                #region "Ingreso de tabla Informacion factura infoFactura"
                if (ds.Tables.Contains(etiquetaInfo))
                    if (ds.Tables[etiquetaInfo].Rows.Count > 0)
                    {
                        String[] columnas = new String[ds.Tables[etiquetaInfo].Columns.Count];
                        for (int i = 0; i < ds.Tables[etiquetaInfo].Columns.Count; i++)
                            columnas[i] = ds.Tables[etiquetaInfo].Columns[i].ColumnName;
                        foreach (DataRow dr in ds.Tables[etiquetaInfo].Rows)
                        {
                            DataRow dr_infoFactura = infoComprobante.NewRow();
                            dr_infoFactura["contribuyenteEspecial"] = (columnas.Contains("contribuyenteEspecial") ? dr["contribuyenteEspecial"] : "");
                            dr_infoFactura["obligadoContabilidad"] = (columnas.Contains("obligadoContabilidad") ? dr["obligadoContabilidad"] : "");
                            dr_infoFactura["dirEstablecimiento"] = (columnas.Contains("dirEstablecimiento") ? dr["dirEstablecimiento"] : "");

                            if (!codDoc.Equals("06"))
                            {
                                dr_infoFactura["fechaEmision"] = (columnas.Contains("fechaEmision") ? dr["fechaEmision"] : DateTime.Now.Date);
                            }
                            if (codDoc.Equals("01") | codDoc.Equals("04") | codDoc.Equals("05"))
                            {
                                dr_infoFactura["tipoIdentificacionComprador"] = (columnas.Contains("tipoIdentificacionComprador") ? dr["tipoIdentificacionComprador"] : "");
                                dr_infoFactura["razonSocialComprador"] = (columnas.Contains("razonSocialComprador") ? dr["razonSocialComprador"] : "");
                                dr_infoFactura["identificacionComprador"] = (columnas.Contains("identificacionComprador") ? dr["identificacionComprador"] : "");
                                dr_infoFactura["totalSinImpuestos"] = (columnas.Contains("totalSinImpuestos") ? dr["totalSinImpuestos"] : 0);
                            }
                            if (codDoc.Equals("01") | codDoc.Equals("04"))
                                dr_infoFactura["moneda"] = (columnas.Contains("moneda") ? dr["moneda"] : "");

                            if (codDoc.Equals("04") | codDoc.Equals("05") | codDoc.Equals("06"))
                                dr_infoFactura["rise"] = (columnas.Contains("rise") ? dr["rise"] : "");

                            if (codDoc.Equals("04") | codDoc.Equals("05"))
                            {
                                dr_infoFactura["codDocModificado"] = (columnas.Contains("codDocModificado") ? dr["codDocModificado"] : "");
                                dr_infoFactura["numDocModificado"] = (columnas.Contains("numDocModificado") ? dr["numDocModificado"] : "");
                                dr_infoFactura["fechaEmisionDocSustento"] = (columnas.Contains("fechaEmisionDocSustento") ? dr["fechaEmisionDocSustento"] : "");
                            }

                            if (codDoc.Equals("01"))
                            {
                                dr_infoFactura["factura_Id"] = (columnas.Contains("factura_Id") ? dr["factura_Id"] : 0);
                                dr_infoFactura["infoFactura_Id"] = (columnas.Contains("infoFactura_Id") ? dr["infoFactura_Id"] : 0);
                                dr_infoFactura["guiaRemision"] = (columnas.Contains("guiaRemision") ? dr["guiaRemision"] : "");
                                dr_infoFactura["totalDescuento"] = (columnas.Contains("totalDescuento") ? dr["totalDescuento"] : 0);
                                dr_infoFactura["propina"] = (columnas.Contains("propina") ? dr["propina"] : 0);
                                dr_infoFactura["importeTotal"] = (columnas.Contains("importeTotal") ? dr["importeTotal"] : 0);
                            }
                            else if (codDoc.Equals("04"))
                            {
                                dr_infoFactura["notaCredito_Id"] = (columnas.Contains("notaCredito_Id") ? dr["notaCredito_Id"] : 0);
                                dr_infoFactura["infoNotaCredito_Id"] = (columnas.Contains("infoNotaCredito_Id") ? dr["infoNotaCredito_Id"] : 0);
                                dr_infoFactura["valorModificacion"] = (columnas.Contains("valorModificacion") ? dr["valorModificacion"] : 0);
                                dr_infoFactura["motivo"] = (columnas.Contains("motivo") ? dr["motivo"] : "");
                            }
                            else if (codDoc.Equals("05"))
                            {
                                dr_infoFactura["notaDebito_Id"] = (columnas.Contains("notaDebito_Id") ? dr["notaDebito_Id"] : 0);
                                dr_infoFactura["infoNotaDebito_Id"] = (columnas.Contains("infoNotaDebito_Id") ? dr["infoNotaDebito_Id"] : 0);
                                dr_infoFactura["valorTotal"] = (columnas.Contains("valorTotal") ? dr["valorTotal"] : 0);
                            }
                            else if (codDoc.Equals("06"))
                            {
                                dr_infoFactura["infoGuiaRemision_Id"] = (columnas.Contains("infoGuiaRemision_Id") ? dr["infoGuiaRemision_Id"] : 0);
                                dr_infoFactura["dirPartida"] = (columnas.Contains("dirPartida") ? dr["dirPartida"] : "");
                                dr_infoFactura["razonSocialTransportista"] = (columnas.Contains("razonSocialTransportista") ? dr["razonSocialTransportista"] : "");
                                dr_infoFactura["tipoIdentificacionTransportista"] = (columnas.Contains("tipoIdentificacionTransportista") ? dr["tipoIdentificacionTransportista"] : "");
                                dr_infoFactura["rucTransportista"] = (columnas.Contains("rucTransportista") ? dr["rucTransportista"] : "");
                                dr_infoFactura["fechaIniTransporte"] = (columnas.Contains("fechaIniTransporte") ? dr["fechaIniTransporte"] : DateTime.Now.Date);
                                dr_infoFactura["fechaFinTransporte"] = (columnas.Contains("fechaFinTransporte") ? dr["fechaFinTransporte"] : DateTime.Now.Date);
                                dr_infoFactura["placa"] = (columnas.Contains("placa") ? dr["placa"] : "");
                            }
                            else // 07
                            {
                                dr_infoFactura["comprobanteRetencion_Id"] = (columnas.Contains("comprobanteRetencion_Id") ? dr["comprobanteRetencion_Id"] : 0);
                                dr_infoFactura["tipoIdentificacionSujetoRetenido"] = (columnas.Contains("tipoIdentificacionSujetoRetenido") ? dr["tipoIdentificacionSujetoRetenido"] : "");
                                dr_infoFactura["razonSocialSujetoRetenido"] = (columnas.Contains("razonSocialSujetoRetenido") ? dr["razonSocialSujetoRetenido"] : "");
                                dr_infoFactura["identificacionSujetoRetenido"] = (columnas.Contains("identificacionSujetoRetenido") ? dr["identificacionSujetoRetenido"] : "");
                                dr_infoFactura["periodoFiscal"] = (columnas.Contains("periodoFiscal") ? dr["periodoFiscal"] : "");
                            }
                            dr_infoFactura["codBarra"] = ImageToByte2(GenCode128.Code128Rendering.MakeBarcodeImage(claveAcceso, 3, true));
                            dr_infoFactura["p_numAut"] = (columnas.Contains("p_numAut") ? dr["p_numAut"] : "");
                            dr_infoFactura["p_fechaAut"] = (columnas.Contains("p_fechaAut") ? dr["p_fechaAut"] : "");

                            infoComprobante.Rows.Add(dr_infoFactura);
                        }
                        ds_tablas.Tables.Add(infoComprobante);
                    }
                #endregion
                #region "Ingreso de total Con Impuestos"
                if (ds_tablas.Tables.Contains("totalConImpuestos"))
                {
                    if (ds.Tables.Contains("totalConImpuestos"))
                    {
                        if (ds.Tables["totalConImpuestos"].Rows.Count > 0)
                        {
                            String[] columnas = new String[ds.Tables["totalConImpuestos"].Columns.Count];
                            for (int i = 0; i < ds.Tables["totalConImpuestos"].Columns.Count; i++)
                                columnas[i] = ds.Tables["totalConImpuestos"].Columns[i].ColumnName;
                            foreach (DataRow dr in ds.Tables["totalConImpuestos"].Rows)
                            {
                                DataRow dr_totalConImpuestos = totalConImpuestos.NewRow();
                                dr_totalConImpuestos["totalConImpuestos_Id"] = (columnas.Contains("totalConImpuestos_Id") ? dr["totalConImpuestos_Id"] : 0);
                                if (codDoc.Equals("01"))
                                    dr_totalConImpuestos["infoFactura_Id"] = (columnas.Contains("infoFactura_Id") ? dr["infoFactura_Id"] : 0);
                                else
                                    dr_totalConImpuestos["infoNotaCredito_Id"] = (columnas.Contains("infoNotaCredito_Id") ? dr["infoNotaCredito_Id"] : 0);
                                totalConImpuestos.Rows.Add(dr_totalConImpuestos);
                            }
                        }
                    }
                    else
                    {
                        DataRow dr_totalConImpuestos = totalConImpuestos.NewRow();
                        dr_totalConImpuestos["totalConImpuestos_Id"] = 0;
                        if (codDoc.Equals("01"))
                            dr_totalConImpuestos["infoFactura_Id"] = 0;
                        else
                            dr_totalConImpuestos["infoNotaCredito_Id"] = 0;
                        totalConImpuestos.Rows.Add(dr_totalConImpuestos);
                    }
                }
                #endregion
                #region "Ingreso de tabla total impuestos"
                if (ds.Tables.Contains("totalImpuesto"))
                    if (ds.Tables["totalImpuesto"].Rows.Count > 0)
                    {
                        Reportes.xds._1_0_0.comproxsdFactura1_0_0.totalImpuestoDataTable totalImpuesto = new Reportes.xds._1_0_0.comproxsdFactura1_0_0.totalImpuestoDataTable();
                        String[] columnas = new String[ds.Tables["totalImpuesto"].Columns.Count];
                        for (int i = 0; i < ds.Tables["totalImpuesto"].Columns.Count; i++)
                            columnas[i] = ds.Tables["totalImpuesto"].Columns[i].ColumnName;
                        foreach (DataRow dr in ds.Tables["totalImpuesto"].Rows)
                        {
                            DataRow dr_totalImpuesto = totalImpuesto.NewRow();
                            dr_totalImpuesto["totalConImpuestos_Id"] = (columnas.Contains("totalConImpuestos_Id") ? dr["totalConImpuestos_Id"] : 0);
                            dr_totalImpuesto["codigo"] = (columnas.Contains("codigo") ? dr["codigo"] : 0);
                            dr_totalImpuesto["codigoPorcentaje"] = (columnas.Contains("codigoPorcentaje") ? dr["codigoPorcentaje"] : 0);
                            dr_totalImpuesto["baseImponible"] = (columnas.Contains("baseImponible") ? dr["baseImponible"] : 0);
                            if (codDoc.Equals("01"))
                                dr_totalImpuesto["tarifa"] = (columnas.Contains("tarifa") ? dr["tarifa"] : 0);
                            dr_totalImpuesto["valor"] = (columnas.Contains("valor") ? dr["valor"] : 0);
                            totalImpuesto.Rows.Add(dr_totalImpuesto);
                        }
                        ds_tablas.Tables.Add(totalImpuesto);
                    }
                #endregion
                #region "Ingreso de tabla detalles"
                if (ds.Tables.Contains("detalles"))
                    if (ds.Tables["detalles"].Rows.Count > 0)
                    {
                        String[] columnas = new String[ds.Tables["detalles"].Columns.Count];
                        for (int i = 0; i < ds.Tables["detalles"].Columns.Count; i++)
                            columnas[i] = ds.Tables["detalles"].Columns[i].ColumnName;
                        foreach (DataRow dr in ds.Tables["detalles"].Rows)
                        {
                            DataRow dr_detalles = detalles.NewRow();
                            dr_detalles["detalles_Id"] = (columnas.Contains("detalles_Id") ? dr["detalles_Id"] : 0);
                            if (codDoc.Equals("06"))
                                dr_detalles["destinatario_Id"] = (columnas.Contains("destinatario_Id") ? dr["destinatario_Id"] : 0);
                            else if (codDoc.Equals("04"))
                                dr_detalles["notaCredito_Id"] = (columnas.Contains("notaCredito_Id") ? dr["notaCredito_Id"] : 0);
                            else if (codDoc.Equals("01"))
                                dr_detalles["factura_Id"] = (columnas.Contains("factura_Id") ? dr["factura_Id"] : 0);

                            detalles.Rows.Add(dr_detalles);
                        }
                    }
                #endregion
                #region "Ingreso de tabla detalle"
                if (ds.Tables.Contains("detalle"))
                    if (ds.Tables["detalle"].Rows.Count > 0)
                    {
                        String[] columnas = new String[ds.Tables["detalle"].Columns.Count];
                        for (int i = 0; i < ds.Tables["detalle"].Columns.Count; i++)
                            columnas[i] = ds.Tables["detalle"].Columns[i].ColumnName;
                        foreach (DataRow dr in ds.Tables["detalle"].Rows)
                        {
                            DataRow dr_detalle = detalle.NewRow();
                            if (codDoc.Equals("01"))
                            {
                                dr_detalle["codigoPrincipal"] = (columnas.Contains("codigoPrincipal") ? dr["codigoPrincipal"] : "");
                                dr_detalle["codigoAuxiliar"] = (columnas.Contains("codigoAuxiliar") ? dr["codigoAuxiliar"] : "");
                            }
                            else
                            {
                                dr_detalle["codigoInterno"] = (columnas.Contains("codigoInterno") ? dr["codigoInterno"] : "");
                                dr_detalle["codigoAdicional"] = (columnas.Contains("codigoAdicional") ? dr["codigoAdicional"] : "");
                            }
                            if (!codDoc.Equals("06"))
                            {
                                dr_detalle["precioUnitario"] = (columnas.Contains("precioUnitario") ? dr["precioUnitario"] : 0);
                                dr_detalle["descuento"] = (columnas.Contains("descuento") ? dr["descuento"] : 0);
                                dr_detalle["precioTotalSinImpuesto"] = (columnas.Contains("precioTotalSinImpuesto") ? dr["precioTotalSinImpuesto"] : 0);
                            }

                            dr_detalle["descripcion"] = (columnas.Contains("descripcion") ? dr["descripcion"] : "");
                            dr_detalle["cantidad"] = (columnas.Contains("cantidad") ? dr["cantidad"] : 0);
                            dr_detalle["detalle_Id"] = (columnas.Contains("detalle_Id") ? dr["detalle_Id"] : 0);
                            dr_detalle["detalles_Id"] = (columnas.Contains("detalles_Id") ? dr["detalles_Id"] : 0);
                            detalle.Rows.Add(dr_detalle);
                        }
                        ds_tablas.Tables.Add(detalle);
                    }
                #endregion
                #region "Ingreso de tabla impuestos"
                if (ds_tablas.Tables.Contains("impuestos"))
                {
                    if (ds.Tables.Contains("impuestos"))
                    {
                        if (ds.Tables["impuestos"].Rows.Count > 0)
                        {
                            String[] columnas = new String[ds.Tables["impuestos"].Columns.Count];
                            for (int i = 0; i < ds.Tables["impuestos"].Columns.Count; i++)
                                columnas[i] = ds.Tables["impuestos"].Columns[i].ColumnName;
                            foreach (DataRow dr in ds.Tables["impuestos"].Rows)
                            {
                                DataRow dr_impuestos = impuestos.NewRow();
                                if (codDoc.Equals("07"))
                                    dr_impuestos["comprobanteRetencion_Id"] = (columnas.Contains("comprobanteRetencion_Id") ? dr["comprobanteRetencion_Id"] : 0);
                                else
                                    dr_impuestos["detalle_Id"] = (columnas.Contains("detalle_Id") ? dr["detalle_Id"] : 0);
                                dr_impuestos["impuestos_Id"] = (columnas.Contains("impuestos_Id") ? dr["impuestos_Id"] : 0);
                                impuestos.Rows.Add(dr_impuestos);
                            }
                        }
                    }
                    else
                    {
                        DataRow dr_impuestos = impuestos.NewRow();
                        if (codDoc.Equals("07"))
                            dr_impuestos["comprobanteRetencion_Id"] = 0;
                        else
                            dr_impuestos["detalle_Id"] = 0;
                        dr_impuestos["impuestos_Id"] = 0;
                        impuestos.Rows.Add(dr_impuestos);
                    }
                }
                #endregion
                #region "Ingreso de tabla total impuesto"
                if (ds.Tables.Contains("impuesto"))
                    if (ds.Tables["impuesto"].Rows.Count > 0)
                    {
                        String[] columnas = new String[ds.Tables["impuesto"].Columns.Count];
                        for (int i = 0; i < ds.Tables["impuesto"].Columns.Count; i++)
                            columnas[i] = ds.Tables["impuesto"].Columns[i].ColumnName;
                        foreach (DataRow dr in ds.Tables["impuesto"].Rows)
                        {
                            DataRow dr_impuesto = impuesto.NewRow();
                            dr_impuesto["impuestos_Id"] = (columnas.Contains("impuestos_Id") ? dr["impuestos_Id"] : 0);
                            dr_impuesto["codigo"] = (columnas.Contains("codigo") ? dr["codigo"] : 0);
                            dr_impuesto["baseImponible"] = (columnas.Contains("baseImponible") ? dr["baseImponible"] : 0);

                            if (codDoc.Equals("07"))
                            {
                                dr_impuesto["codigoRetencion"] = (columnas.Contains("codigoRetencion") ? dr["codigoRetencion"] : 0);
                                dr_impuesto["porcentajeRetener"] = (columnas.Contains("porcentajeRetener") ? dr["porcentajeRetener"] : 0);
                                dr_impuesto["valorRetenido"] = (columnas.Contains("valorRetenido") ? dr["valorRetenido"] : 0);
                                dr_impuesto["codDocSustento"] = (columnas.Contains("codDocSustento") ? dr["codDocSustento"] : "");
                                dr_impuesto["numDocSustento"] = (columnas.Contains("numDocSustento") ? dr["numDocSustento"] : "");
                                dr_impuesto["fechaEmisionDocSustento"] = (columnas.Contains("fechaEmisionDocSustento") ? dr["fechaEmisionDocSustento"] : DateTime.Now.Date);
                            }
                            else
                            {
                                dr_impuesto["codigoPorcentaje"] = (columnas.Contains("codigoPorcentaje") ? dr["codigoPorcentaje"] : 0);
                                dr_impuesto["tarifa"] = (columnas.Contains("tarifa") ? dr["tarifa"] : 0);
                                dr_impuesto["valor"] = (columnas.Contains("valor") ? dr["valor"] : 0);
                            }
                            impuesto.Rows.Add(dr_impuesto);
                        }
                        ds_tablas.Tables.Add(impuesto);
                    }
                #endregion
                #region "Ingreso de tabla motivos"
                if (ds_tablas.Tables.Contains("motivos"))
                {
                    if (ds.Tables.Contains("motivos"))
                    {
                        if (ds.Tables["motivos"].Rows.Count > 0)
                        {
                            String[] columnas = new String[ds.Tables["motivos"].Columns.Count];
                            for (int i = 0; i < ds.Tables["motivos"].Columns.Count; i++)
                                columnas[i] = ds.Tables["motivos"].Columns[i].ColumnName;
                            foreach (DataRow dr in ds.Tables["motivos"].Rows)
                            {
                                DataRow dr_motivos = motivos.NewRow();
                                dr_motivos["motivos_Id"] = (columnas.Contains("motivos_Id") ? dr["motivos_Id"] : 0);
                                dr_motivos["notaDebito_Id"] = (columnas.Contains("notaDebito_Id") ? dr["notaDebito_Id"] : 0);
                                motivos.Rows.Add(dr_motivos);
                            }
                        }
                    }
                    else
                    {
                        DataRow dr_motivos = motivos.NewRow();
                        dr_motivos["motivos_Id"] = 0;
                        dr_motivos["notaDebito_Id"] = 0;
                        motivos.Rows.Add(dr_motivos);
                    }
                }
                #endregion
                #region "Ingreso de tabla motivo"
                if (ds.Tables.Contains("motivo"))
                    if (ds.Tables["motivo"].Rows.Count > 0)
                    {
                        String[] columnas = new String[ds.Tables["motivo"].Columns.Count];
                        for (int i = 0; i < ds.Tables["motivo"].Columns.Count; i++)
                            columnas[i] = ds.Tables["motivo"].Columns[i].ColumnName;
                        foreach (DataRow dr in ds.Tables["motivo"].Rows)
                        {
                            DataRow dr_motivo = motivo.NewRow();
                            dr_motivo["motivos_Id"] = (columnas.Contains("motivos_Id") ? dr["motivos_Id"] : 0);
                            dr_motivo["razon"] = (columnas.Contains("razon") ? dr["razon"] : 0);
                            dr_motivo["valor"] = (columnas.Contains("valor") ? dr["valor"] : 0);
                            motivo.Rows.Add(dr_motivo);
                        }
                        ds_tablas.Tables.Add(motivo);
                    }
                #endregion
                #region "Ingreso de tabla destinatarios"
                if (ds_tablas.Tables.Contains("destinatarios"))
                {
                    if (ds.Tables.Contains("destinatarios"))
                    {
                        if (ds.Tables["destinatarios"].Rows.Count > 0)
                        {
                            String[] columnas = new String[ds.Tables["destinatarios"].Columns.Count];
                            for (int i = 0; i < ds.Tables["destinatarios"].Columns.Count; i++)
                                columnas[i] = ds.Tables["destinatarios"].Columns[i].ColumnName;
                            foreach (DataRow dr in ds.Tables["destinatarios"].Rows)
                            {
                                DataRow dr_destinatarios = destinatarios.NewRow();
                                dr_destinatarios["destinatarios_Id"] = (columnas.Contains("destinatarios_Id") ? dr["destinatarios_Id"] : 0);
                                dr_destinatarios["guiaRemision_Id"] = (columnas.Contains("guiaRemision_Id") ? dr["guiaRemision_Id"] : 0);
                                destinatarios.Rows.Add(dr_destinatarios);
                            }
                        }
                    }
                    else
                    {
                        DataRow dr_destinatarios = destinatarios.NewRow();
                        dr_destinatarios["destinatarios_Id"] = 0;
                        dr_destinatarios["guiaRemision_Id"] = 0;
                        destinatarios.Rows.Add(dr_destinatarios);
                    }
                }
                #endregion
                #region "Ingreso de tabla destinatario"
                if (ds.Tables.Contains("destinatario"))
                    if (ds.Tables["destinatario"].Rows.Count > 0)
                    {
                        String[] columnas = new String[ds.Tables["destinatario"].Columns.Count];
                        for (int i = 0; i < ds.Tables["destinatario"].Columns.Count; i++)
                            columnas[i] = ds.Tables["destinatario"].Columns[i].ColumnName;
                        foreach (DataRow dr in ds.Tables["destinatario"].Rows)
                        {
                            DataRow dr_destinatario = destinatario.NewRow();
                            dr_destinatario["destinatarios_Id"] = (columnas.Contains("destinatarios_Id") ? dr["destinatarios_Id"] : 0);
                            dr_destinatario["identificacionDestinatario"] = (columnas.Contains("identificacionDestinatario") ? dr["identificacionDestinatario"] : "");
                            dr_destinatario["razonSocialDestinatario"] = (columnas.Contains("razonSocialDestinatario") ? dr["razonSocialDestinatario"] : "");
                            dr_destinatario["dirDestinatario"] = (columnas.Contains("dirDestinatario") ? dr["dirDestinatario"] : "");
                            dr_destinatario["motivoTraslado"] = (columnas.Contains("motivoTraslado") ? dr["motivoTraslado"] : "");
                            dr_destinatario["docAduaneroUnico"] = (columnas.Contains("docAduaneroUnico") ? dr["docAduaneroUnico"] : "");
                            dr_destinatario["codEstabDestino"] = (columnas.Contains("codEstabDestino") ? dr["codEstabDestino"] : "");
                            dr_destinatario["ruta"] = (columnas.Contains("ruta") ? dr["ruta"] : "");
                            dr_destinatario["codDocSustento"] = (columnas.Contains("codDocSustento") ? dr["codDocSustento"] : "");
                            dr_destinatario["numDocSustento"] = (columnas.Contains("numDocSustento") ? dr["numDocSustento"] : "");
                            dr_destinatario["numAutDocSustento"] = (columnas.Contains("numAutDocSustento") ? dr["numAutDocSustento"] : "");
                            dr_destinatario["fechaEmisionDocSustento"] = (columnas.Contains("fechaEmisionDocSustento") ? dr["fechaEmisionDocSustento"] : "");
                            dr_destinatario["destinatario_Id"] = (columnas.Contains("destinatario_Id") ? dr["destinatario_Id"] : 0);
                            destinatario.Rows.Add(dr_destinatario);
                        }
                        ds_tablas.Tables.Add(destinatario);
                    }
                #endregion
                #region "Ingreso de tabla detalle Adicionales"
                if (ds_tablas.Tables.Contains("detallesAdicionales"))
                {
                    if (ds.Tables.Contains("detallesAdicionales"))
                    {
                        if (ds.Tables["detallesAdicionales"].Rows.Count > 0)
                        {
                            String[] columnas = new String[ds.Tables["detallesAdicionales"].Columns.Count];
                            for (int i = 0; i < ds.Tables["detallesAdicionales"].Columns.Count; i++)
                                columnas[i] = ds.Tables["detallesAdicionales"].Columns[i].ColumnName;
                            foreach (DataRow dr in ds.Tables["detallesAdicionales"].Rows)
                            {
                                DataRow dr_detalleAdicionales = detalleAdicionales.NewRow();
                                dr_detalleAdicionales["detallesAdicionales_Id"] = (columnas.Contains("detallesAdicionales_Id") ? dr["detallesAdicionales_Id"] : 0);
                                dr_detalleAdicionales["detalle_Id"] = (columnas.Contains("detalle_Id") ? dr["detalle_Id"] : 0);
                                detalleAdicionales.Rows.Add(dr_detalleAdicionales);
                            }
                        }
                    }
                    else
                    {
                        DataRow dr_detalleAdicionales = detalleAdicionales.NewRow();
                        dr_detalleAdicionales["detallesAdicionales_Id"] = 0;
                        dr_detalleAdicionales["detalle_Id"] = 0;
                        detalleAdicionales.Rows.Add(dr_detalleAdicionales);
                    }
                }
                #endregion
                #region "Ingreso de tabla detalle Adicional"
                if (ds.Tables.Contains("detAdicional"))
                    if (ds.Tables["detAdicional"].Rows.Count > 0)
                    {
                        String[] columnas = new String[ds.Tables["detAdicional"].Columns.Count];
                        for (int i = 0; i < ds.Tables["detAdicional"].Columns.Count; i++)
                            columnas[i] = ds.Tables["detAdicional"].Columns[i].ColumnName;
                        foreach (DataRow dr in ds.Tables["detAdicional"].Rows)
                        {
                            DataRow dr_detAdicional = detAdicional.NewRow();
                            dr_detAdicional["detallesAdicionales_Id"] = (columnas.Contains("detallesAdicionales_Id") ? dr["detallesAdicionales_Id"] : 0);
                            dr_detAdicional["nombre"] = (columnas.Contains("nombre") ? dr["nombre"] : "");
                            dr_detAdicional["valor"] = (columnas.Contains("valor") ? dr["valor"] : "");
                            detAdicional.Rows.Add(dr_detAdicional);
                        }
                    }
                #endregion
                #region "Ingreso de tabla informacion Adicional"
                if (ds_tablas.Tables.Contains("infoAdicional"))
                {
                    String comprobanteId = "";
                    if (codDoc.Equals("01"))
                        comprobanteId = "factura_Id";
                    else if (codDoc.Equals("04"))
                        comprobanteId = "notaCredito_Id";
                    else if (codDoc.Equals("07"))
                        comprobanteId = "comprobanteRetencion_Id";
                    else if (codDoc.Equals("05"))
                        comprobanteId = "notaDebito_Id";
                    else
                        comprobanteId = "guiaRemision_Id";

                    if (ds.Tables.Contains("infoAdicional"))
                    {
                        if (ds.Tables["infoAdicional"].Rows.Count > 0)
                        {
                            String[] columnas = new String[ds.Tables["infoAdicional"].Columns.Count];
                            for (int i = 0; i < ds.Tables["infoAdicional"].Columns.Count; i++)
                                columnas[i] = ds.Tables["infoAdicional"].Columns[i].ColumnName;
                            foreach (DataRow dr in ds.Tables["infoAdicional"].Rows)
                            {
                                DataRow dr_infoAdicional = infoAdicional.NewRow();
                                dr_infoAdicional["infoAdicional_Id"] = (columnas.Contains("infoAdicional_Id") ? dr["infoAdicional_Id"] : 0);

                                dr_infoAdicional[comprobanteId] = (columnas.Contains(comprobanteId) ? dr[comprobanteId] : 0);
                                infoAdicional.Rows.Add(dr_infoAdicional);
                            }
                        }
                    }
                    else
                    {
                        DataRow dr_infoAdicional = infoAdicional.NewRow();
                        dr_infoAdicional["infoAdicional_Id"] = 0;
                        dr_infoAdicional[comprobanteId] = 0;
                        infoAdicional.Rows.Add(dr_infoAdicional);
                    }
                }
                #endregion
                #region "Ingreso de tabla campo adicional"
                if (ds.Tables.Contains("campoAdicional"))
                    if (ds.Tables["campoAdicional"].Rows.Count > 0)
                    {
                        String[] columnas = new String[ds.Tables["campoAdicional"].Columns.Count];
                        for (int i = 0; i < ds.Tables["campoAdicional"].Columns.Count; i++)
                            columnas[i] = ds.Tables["campoAdicional"].Columns[i].ColumnName;
                        foreach (DataRow dr in ds.Tables["campoAdicional"].Rows)
                        {
                            DataRow dr_campoAdicional = campoAdicional.NewRow();
                            dr_campoAdicional["infoAdicional_Id"] = (columnas.Contains("infoAdicional_Id") ? dr["infoAdicional_Id"] : 0);
                            dr_campoAdicional["nombre"] = (columnas.Contains("nombre") ? dr["nombre"] : "");
                            dr_campoAdicional["campoAdicional_Text"] = (columnas.Contains("campoAdicional_Text") ? dr["campoAdicional_Text"] : "");
                            campoAdicional.Rows.Add(dr_campoAdicional);
                        }
                    }
                #endregion
            }
            return ds_tablas;
        }

        public static byte[] ImageToByte2(System.Drawing.Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }
    }
}