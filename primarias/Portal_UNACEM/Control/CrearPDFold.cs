using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
//using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Datos;
using System.Configuration;
using ReportesDEI;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data.Common;
using clibLogger;

namespace Control
{
    public class CrearPDFold
    {
        //private BasesDatos DB;
        string msj = "";
        string msjT = "";
        Log log;
        string compen = "";
        DateTime fecha;

        public void PoblarReporte(out MemoryStream ruta, string nombre, string idComprobante, string comprobante)
        {
            ruta = null;
            var DB = new BasesDatos();
            log = new Log();
            SqlConnection sqlConn;
            //SqlDataAdapter sqlDaComprobante, sqlDaDetalles, sqlDaInfoAdicional, sqlDaImpuestosRetenciones, sqlDaInfoAdicional1;
            //SqlDataAdapter sqlDaDetalleDest, sqlDaDestinatarios, sqlDaDetallesAdicionales, sqlDaDetAdicionales2, sqlDaIva, sqlDaPagos, sqlDACompensacion, sqlrubros, sqlnotaAclaratoria;

            DB.Conectar2();
            DB.CrearComando2(@"select tarifa from Compensacion WITH (NOLOCK) inner join GENERAL WITH (NOLOCK) on  idComprobante= id_Comprobante where id_Comprobante = @idComprobante");
            DB.AsignarParametroCadena("@idComprobante", idComprobante);
            using (DbDataReader DR3 = DB.EjecutarConsulta())
            {
                if (DR3.Read())
                {
                    compen = DR3["tarifa"].ToString();
                }
            }

            DB.Desconectar2();

            DatosComprobantes dsPc = new DatosComprobantes();
            String strConn2;
            strConn2 = ConfigurationManager.ConnectionStrings["dataexpressConnectionString2"].ConnectionString;

            String StrComprobante = @"SELECT        GENERAL.idComprobante, GENERAL.id, GENERAL.version, GENERAL.serie, GENERAL.folio, GENERAL.fecha, GENERAL.sello, GENERAL.noCertificado, 
                         GENERAL.subTotal, GENERAL.total, GENERAL.tipoDeComprobante, GENERAL.firmaSRI, GENERAL.id_Config, GENERAL.id_Empleado, GENERAL.id_Receptor, 
                         GENERAL.id_Emisor, GENERAL.id_EmisorExp, GENERAL.id_ReceptorCon,
                             (SELECT        descripcion
                               FROM            Catalogo1_C WITH (NOLOCK) 
                               WHERE        (codigo = GENERAL.ambiente) AND (tipo = 'Ambiente')) AS ambienteDesc,
                             (SELECT        descripcion
                               FROM            Catalogo1_C AS Catalogo1_C_2 WITH (NOLOCK) 
                               WHERE        (codigo = GENERAL.tipoEmision) AND (tipo = 'Emision')) AS tipoEmision, GENERAL.claveAcceso,
                             (SELECT        descripcion
                               FROM            Catalogo_Comprobante AS Catalogo1_C_1 WITH (NOLOCK) 
                               WHERE        (codigo = GENERAL.codDoc) AND (tipo = 'Comprobante')) AS codDocDesc, GENERAL.estab, GENERAL.ptoEmi, GENERAL.secuencial, 
                         GENERAL.totalSinImpuestos, GENERAL.totalDescuento, GENERAL.periodoFiscal, GENERAL.fechaIniTransporte, GENERAL.fechaFinTransporte, GENERAL.placa, 
                         ISNULL
                             ((SELECT        descripcion AS Expr1
                                 FROM            Catalogo_Comprobante AS Catalogo1_C_1 WITH (NOLOCK) 
                                 WHERE        (codigo = GENERAL.codDocModificado) AND (tipo = 'Comprobante')), '') AS codDocModificadoDesc, GENERAL.codDocModificado, 
                         GENERAL.numDocModificado, GENERAL.fechaEmisionDocSustento, GENERAL.valorModificacion, GENERAL.moneda, GENERAL.propina, GENERAL.importeTotal, 
                         GENERAL.motivo, GENERAL.subtotal12, GENERAL.subtotal0, GENERAL.subtotalNoSujeto, GENERAL.ICE, GENERAL.IVA12, GENERAL.importeAPagar, 
                         EMISOR.IDEEMI, EMISOR.RFCEMI, EMISOR.NOMEMI, EMISOR.nombreComercial, EMISOR.dirMatriz, RECEPTOR.IDEREC, RECEPTOR.RFCREC,RECEPTOR.direccionComprador,
                         RECEPTOR.NOMREC, RECEPTOR.contribuyenteEspecial, RECEPTOR.obligadoContabilidad, RECEPTOR.tipoIdentificacionComprador, DOMEMIEXP.IDEDOMEMIEXP, 
                         DOMEMIEXP.dirEstablecimientos, GENERAL.codigoBarras, GENERAL.numeroAutorizacion, GENERAL.estado, GENERAL.fechaAutorizacion, GENERAL.rise, 
                         GENERAL.dirPartida, GENERAL.termino, GENERAL.proforma, GENERAL.pedido, RECEPTOR.domicilio, RECEPTOR.telefono, GENERAL.cantletras
FROM            GENERAL WITH (NOLOCK)  INNER JOIN
                         EMISOR WITH (NOLOCK) ON GENERAL.id_Emisor = EMISOR.IDEEMI INNER JOIN
                         RECEPTOR WITH (NOLOCK) ON GENERAL.id_Receptor = RECEPTOR.IDEREC INNER JOIN
                         DOMEMIEXP WITH (NOLOCK) ON GENERAL.id_EmisorExp = DOMEMIEXP.IDEDOMEMIEXP
                                  WHERE 
                         General.idComprobante='" + idComprobante + "'";
            String StrInfoAdicional = @"SELECT nombre, valor, id_Comprobante FROM InfoAdicional WITH (NOLOCK)  WHERE id_Comprobante='" + idComprobante + "'";

            String StrDetalles = @"SELECT        idDetalles, codigoPrincipal, codigoAuxiliar, descripcion AS Descripcion, convert(decimal(18,4),isnull(cantidad,0)) as cantidad, convert(decimal(18,4),isnull(precioUnitario,0)) as precioUnitario, descuento, precioTotalSinImpuestos, id_Comprobante
FROM            Detalles WITH (NOLOCK)  WHERE id_Comprobante='" + idComprobante + "'";

            String StrImpuestosIva = @"select top 1 TI.tarifa,TI.codigoPorcentaje
                                      from TotalConImpuestos TI WITH (NOLOCK) , CatImpuestos_C CA WITH (NOLOCK) where  TI.id_comprobante = '" + idComprobante + "' and TI.codigo = 2 and CA.tipo = 'IVA' and TI.codigoPorcentaje = CA.codigo  and TI.codigoPorcentaje in('2', '3','0')";


            String StrImpuestosRetenciones = @"SELECT tci.codigo,ISNULL((SELECT descripcion FROM CatImpuestos_C where tipo = 'Retencion' AND codigo =tci.codigo ),'') as descripcionImpuesto,
                                                tci.codigoPorcentaje, tci.baseImponible, tci.tarifa, tci.valor,tci.porcentajeRetener, 
                                                ISNULL((SELECT descripcion FROM Catalogo_Comprobante WITH (NOLOCK)  where tipo = 'Comprobante' AND codigo =tci.codDocSustento ),'') as descripcionComprobante,
                                                tci.numDocSustento,tci.fechaEmisionDocSustento, tci.id_Comprobante
                                                FROM TotalConImpuestos AS tci WITH (NOLOCK)  where tci.numDocSustento !='' and tci.id_Comprobante ='" + idComprobante + "'";

            String StrDetallesDest = @"SELECT idDetalles, codigoPrincipal, codigoAuxiliar, descripcion, cantidad,
                                              precioUnitario, descuento, precioTotalSinImpuestos, id_Comprobante, id_Destinatario
FROM            Detalles WITH (NOLOCK) WHERE id_Comprobante='" + idComprobante + "'";
            String StrDestinatarios = @"DECLARE @FECHA VARCHAR(MAX) ;

                  SELECT        idDestinatario, identificacionDestinatario, razonSocialDestinatario, dirDestinatario, motivoTraslado, docAduaneroUnico, codEstabDestino, ruta, ISNULL
                             ((SELECT        descripcion
                                 FROM            Catalogo_Comprobante AS Catalogo1_C_1 WITH (NOLOCK) 
                                 WHERE        (codigo = Destinatarios.codDocSustento) AND (tipo = 'Comprobante')), '') AS codDocDesc, numDocSustento, numAutDocSustento, 
                          case when  fechaEmisionDocSustento = '' then @FECHA
						 when  fechaEmisionDocSustento  IS NOT null then fechaEmisionDocSustento end as fechaEmisionDocSustento , id_Comprobante
FROM            Destinatarios WITH (NOLOCK)  WHERE id_Comprobante='" + idComprobante + "' ORDER BY razonSocialDestinatario DESC";

            String StrDetallesAdicionales = @"
DECLARE @RC int
DECLARE @id_comprobante bigint

SET @id_comprobante = " + idComprobante + @"

EXECUTE @RC = [PA_consulta_detalles_adicionales] 
   @id_comprobante";



            //Info Adicional1
            String StrInfoAdicional1 = @"DECLARE @cols AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX)

select @cols = STUFF((SELECT ',' + QUOTENAME(nombre) 
                    from InfoAdicional WITH (NOLOCK) 
                    where id_Comprobante = " + idComprobante + @"
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')

set @query = 'SELECT ' + @cols + '," + idComprobante + @" idComprobante 
             from 
             (
                select ia.valor, ia.nombre
                from InfoAdicional ia WITH (NOLOCK) 
                where id_Comprobante = " + idComprobante + @"               
            ) x
            pivot 
            (
                max(valor)
                for nombre in (' + @cols + ')
            ) p '

execute sp_executesql @query";

            //Detalles Adicionales2
            String StrDetAdicional2 = @"SELECT Unidad,id_Detalles as idDetalles
             from 
             (
                select da.valor, da.nombre,da.id_Detalles
                from DetallesAdicionales da WITH (NOLOCK) 
                inner join Detalles d WITH (NOLOCK) on da.id_Detalles = d.idDetalles where d.id_Comprobante = " + idComprobante + @"               
            ) x
            pivot 
            (
                max(valor)
                for nombre in (Unidad,idDetalles)
            ) p";

            //Detalles AdicionalesNC
            String StrDetAdicionalNC = @"SELECT ConceptoInterno,numeroFactura,UnidadDescuento,id_Detalles as idDetalles
             from 
             (
                select da.valor, da.nombre,da.id_Detalles
                from DetallesAdicionales da WITH (NOLOCK) 
                inner join Detalles d WITH (NOLOCK)  on da.id_Detalles = d.idDetalles where d.id_Comprobante = " + idComprobante + @"               
            ) x
            pivot 
            (
                max(valor)
                for nombre in (ConceptoInterno,numeroFactura,UnidadDescuento,idDetalles)
            ) p";

            //Detalles AdicionalesGR
            String StrDetAdicionalGR1 = @"SELECT Unidad,Saldo_Anterior,Saldo_Actual,id_Detalles as idDetalles
             from 
             (
                select da.valor, da.nombre,da.id_Detalles
                from DetallesAdicionales da WITH (NOLOCK) 
                inner join Detalles d WITH (NOLOCK) on da.id_Detalles = d.idDetalles where d.id_Comprobante = " + idComprobante + @"               
            ) x
            pivot 
            (
                max(valor)
                for nombre in (Unidad,Saldo_Anterior,Saldo_Actual,idDetalles)
            ) p";

            String pagos = @"select distinct b.descripcion, a.id_Comprobante,a.plazo,a.total,a.unidadTiempo,a.formaPago from Pago A WITH (NOLOCK)  inner join Catalogo1_C B WITH (NOLOCK) on a.formaPago = b.codigo
            and b.tipo = 'pagos' where a.id_Comprobante = " + idComprobante;

            String desc_solidario = @"SELECT codigo,tarifa,CAST(valor AS DECIMAL(14,2)) as valor  FROM Compensacion WITH (NOLOCK)   WHERE id_Comprobante='" + idComprobante + "'";

            String rubros = @"select concepto ,CAST(total AS DECIMAL(18,2)) as total  from otrosRubrosTerceros WITH (NOLOCK)  where id_Comprobante ='" + idComprobante + "'";

            String notaAclaratoria = @"select valor from InfoAdicional WITH (NOLOCK)  where id_Comprobante ='" + idComprobante + "' and nombre ='Nota_Aclaratoria'";

            sqlConn = new SqlConnection(strConn2);

            try
            {

                switch (comprobante)
                {
                    case "01":
                        //Comprobante
                        using (SqlDataAdapter sqlDaComprobante = new SqlDataAdapter(StrComprobante, sqlConn))
                        {
                            dsPc.EnforceConstraints = false;
                            sqlDaComprobante.Fill(dsPc, "Comprobante");
                        }
                        //Detalles
                        using (SqlDataAdapter sqlDaDetalles = new SqlDataAdapter(StrDetalles, sqlConn))
                        {
                            sqlDaDetalles.Fill(dsPc, "DetallesConDetalleAdicionales");
                        }
                        //InfoAdicional1
                        using (SqlDataAdapter sqlDaInfoAdicional1 = new SqlDataAdapter(StrInfoAdicional1, sqlConn))
                        {
                            sqlDaInfoAdicional1.Fill(dsPc, "InfoAdicionalF");
                        }
                        //Detalles Adicionales2
                        using (SqlDataAdapter sqlDaDetAdicionales2 = new SqlDataAdapter(StrDetAdicional2, sqlConn))
                        {
                            sqlDaDetAdicionales2.Fill(dsPc, "detAdicionalF");
                        }

                        using (SqlDataAdapter sqlDaIva = new SqlDataAdapter(StrImpuestosIva, sqlConn))
                        {
                            sqlDaIva.Fill(dsPc, "IVA");
                        }
                        using (SqlDataAdapter sqlDaPagos = new SqlDataAdapter(pagos, sqlConn))
                        {
                            sqlDaPagos.Fill(dsPc, "Pagos");
                        }
                        using (SqlDataAdapter sqlDACompensacion = new SqlDataAdapter(desc_solidario, sqlConn))
                        {
                            sqlDACompensacion.Fill(dsPc, "Compensacion");
                        }
                        using (Facturaold rptold = new Facturaold())
                        {
                            rptold.SetDataSource(dsPc);
                            ruta = (MemoryStream)rptold.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                            rptold.Dispose();
                            rptold.Close();
                        }

                        break;
                    //}
                    case "04":
                        //Comprobante
                        using (SqlDataAdapter sqlDaComprobante = new SqlDataAdapter(StrComprobante, sqlConn))
                        {
                            dsPc.EnforceConstraints = false;
                            sqlDaComprobante.Fill(dsPc, "Comprobante");
                        }
                        //Detalles
                        using (SqlDataAdapter sqlDaDetalles = new SqlDataAdapter(StrDetalles, sqlConn))
                        {
                            sqlDaDetalles.Fill(dsPc, "DetallesConDetalleAdicionales");
                        }
                        //InfoAdicional1
                        using (SqlDataAdapter sqlDaInfoAdicional1 = new SqlDataAdapter(StrInfoAdicional1, sqlConn))
                        {
                            sqlDaInfoAdicional1.Fill(dsPc, "InfoAdicionalNC");
                        }

                        //Detalles Adicionales2
                        using (SqlDataAdapter sqlDaDetAdicionales2 = new SqlDataAdapter(StrDetAdicional2, sqlConn))
                        {
                            sqlDaDetAdicionales2.Fill(dsPc, "detAdicionalF");
                        }

                        using (SqlDataAdapter sqlDaIva = new SqlDataAdapter(StrImpuestosIva, sqlConn))
                        {
                            sqlDaIva.Fill(dsPc, "IVA");
                        }
                        using (SqlDataAdapter sqlDACompensacion = new SqlDataAdapter(desc_solidario, sqlConn))
                        {
                            sqlDACompensacion.Fill(dsPc, "Compensacion");
                        }

                        if (!String.IsNullOrEmpty(compen))
                        {
                            using (NotaCredito_Compensacionold rptNCold = new NotaCredito_Compensacionold())
                            {
                                rptNCold.SetDataSource(dsPc);
                                ruta = (MemoryStream)rptNCold.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                rptNCold.Dispose();
                                rptNCold.Close();
                            }
                        }
                        else
                        {
                            using (NotaCreditoold rptNCold = new NotaCreditoold())
                            {
                                rptNCold.SetDataSource(dsPc);
                                ruta = (MemoryStream)rptNCold.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                rptNCold.Dispose();
                                rptNCold.Close();
                            }
                        }

                        break;
                    case "05":

                        //Comprobante
                        using (SqlDataAdapter sqlDaComprobante = new SqlDataAdapter(StrComprobante, sqlConn))
                        {
                            //InfoAdicional1
                            using (SqlDataAdapter sqlDaInfoAdicional1 = new SqlDataAdapter(StrInfoAdicional1, sqlConn))
                            {
                                using (SqlDataAdapter sqlDaIva = new SqlDataAdapter(StrImpuestosIva, sqlConn))
                                {
                                    using (SqlDataAdapter sqlDaPagos = new SqlDataAdapter(pagos, sqlConn))
                                    {
                                        using (SqlDataAdapter sqlDACompensacion = new SqlDataAdapter(desc_solidario, sqlConn))
                                        {
                                            if (consulta_tipo_ND(idComprobante).Equals("05A"))
                                            {
                                                dsPc.EnforceConstraints = false;
                                                sqlDaComprobante.Fill(dsPc, "Comprobante");
                                                sqlDaInfoAdicional1.Fill(dsPc, "InfoAdicionalNC");
                                                sqlDaIva.Fill(dsPc, "IVA");
                                                sqlDaPagos.Fill(dsPc, "Pagos");
                                                sqlDACompensacion.Fill(dsPc, "Compensacion");

                                                using (NotaDebitoAR1 rptNDAR = new NotaDebitoAR1())
                                                {
                                                    rptNDAR.SetDataSource(dsPc);
                                                    ruta = (MemoryStream)rptNDAR.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                                    rptNDAR.Dispose();
                                                    rptNDAR.Close();
                                                }
                                            }
                                            else
                                            {
                                                using (SqlDataAdapter sqlDaDetalles = new SqlDataAdapter(StrDetalles, sqlConn))
                                                {
                                                    using (SqlDataAdapter sqlDaDetAdicionales2 = new SqlDataAdapter(StrDetAdicional2, sqlConn))
                                                    {
                                                        dsPc.EnforceConstraints = false;
                                                        sqlDaComprobante.Fill(dsPc, "Comprobante");
                                                        sqlDaDetalles.Fill(dsPc, "DetallesConDetalleAdicionales");
                                                        sqlDaInfoAdicional1.Fill(dsPc, "InfoAdicionalNC");
                                                        sqlDaDetAdicionales2.Fill(dsPc, "detAdicionalF");
                                                    }
                                                }

                                                sqlDaIva.Fill(dsPc, "IVA");
                                                sqlDaPagos.Fill(dsPc, "Pagos");
                                                sqlDACompensacion.Fill(dsPc, "Compensacion");

                                                using (NotaDebito rptND = new NotaDebito())
                                                {
                                                    rptND.SetDataSource(dsPc);
                                                    ruta = (MemoryStream)rptND.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                                    rptND.Dispose();
                                                    rptND.Close();
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }

                        break;
                    case "06":

                        using (SqlDataAdapter sqlDaComprobante = new SqlDataAdapter(StrComprobante, sqlConn))
                        {
                            //Destinatarios
                            using (SqlDataAdapter sqlDaDestinatarios = new SqlDataAdapter(StrDestinatarios, sqlConn))
                            {
                                //DetallesDestinatarios
                                using (SqlDataAdapter sqlDaDetalleDest = new SqlDataAdapter(StrDetallesDest, sqlConn))
                                {
                                    //info Adicional                        
                                    using (SqlDataAdapter sqlDaInfoAdicional1 = new SqlDataAdapter(StrInfoAdicional1, sqlConn))
                                    {
                                        //Detalles Adicionales2
                                        using (SqlDataAdapter sqlDaDetAdicionales2 = new SqlDataAdapter(StrDetAdicionalGR1, sqlConn))
                                        {
                                            dsPc.EnforceConstraints = false;
                                            sqlDaComprobante.Fill(dsPc, "Comprobante");
                                            sqlDaDestinatarios.Fill(dsPc, "Destinatarios");
                                            sqlDaDetalleDest.Fill(dsPc, "Detalles");
                                            sqlDaDetAdicionales2.Fill(dsPc, "detAdicionalGR");
                                        }

                                        sqlDaInfoAdicional1.Fill(dsPc, "InfoAdicionalGR");
                                    }
                                }
                            }
                        }

                        using (GuiaRemisionold rptGRold = new GuiaRemisionold())
                        {
                            rptGRold.SetDataSource(dsPc);
                            ruta = (MemoryStream)rptGRold.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                            rptGRold.Dispose();
                            rptGRold.Close();
                        }

                        break;
                    case "07":

                        using (SqlDataAdapter sqlDaComprobante = new SqlDataAdapter(StrComprobante, sqlConn))
                        {
                            using (SqlDataAdapter sqlDaImpuestosRetenciones = new SqlDataAdapter(StrImpuestosRetenciones, sqlConn))
                            {
                                using (SqlDataAdapter sqlDaInfoAdicional1 = new SqlDataAdapter(StrInfoAdicional1, sqlConn))
                                {
                                    dsPc.EnforceConstraints = false;
                                    sqlDaComprobante.Fill(dsPc, "Comprobante");
                                    sqlDaImpuestosRetenciones.Fill(dsPc, "TotalConImpuestos");
                                    sqlDaInfoAdicional1.Fill(dsPc, "InfoAdicionalRetFG");
                                }
                            }
                        }

                        using (CompRetencionold rptCRold = new CompRetencionold())
                        {
                            rptCRold.SetDataSource(dsPc);
                            ruta = (MemoryStream)rptCRold.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                            rptCRold.Dispose();
                            rptCRold.Close();
                        }

                        break;
                }

            }
            catch (Exception ex)
            {
                msj = "";
                msjT = ex.Message;
                clsLogger.Graba_Log_Error(ex.Message);
                log.mensajesLog("EM009", msjT, ex.Source, nombre, "");
                msjT = "";
                DB.Desconectar2();
            }
            finally
            {
                if (sqlConn != null)
                {
                    sqlConn.Close();
                    dsPc.Dispose();
                  

                }
                DB.Desconectar2();
            }
        }


        public void modificaPDF(string oldFile, string newFile, string valor, int val_x, int val_y)
        {
            //String oldFile = "C:\\DataExpress\\Invoicec\\WebSite\\docus\\0100100420130726101446.pdf";
            //String newFile = "C:\\DataExpress\\Invoicec\\certificado\\0100100420130726101446.pdf";
            //String newFile = "prueba.pdf";

            // open the reader
            PdfReader reader = new PdfReader(oldFile);
            Rectangle size = reader.GetPageSizeWithRotation(1);
            Document document = new Document(size);

            // open the writer
            FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            // the pdf content
            PdfContentByte cb = writer.DirectContent;

            // select the font properties
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.RED);
            cb.SetFontAndSize(bf, 8);

            // write the text in the pdf content
            cb.BeginText();
            string text = valor;// "NOTA DE CRÉDITO";
                                // put the alignment and coordinates here
            cb.ShowTextAligned(2, text, val_x, val_y, 0);
            cb.EndText();
            //cb.BeginText();
            //text = "Other random blabla...";
            //// put the alignment and coordinates here
            //cb.ShowTextAligned(2, text, 80, 200, 0);
            //cb.EndText();

            // create the new page and add it to the pdf
            PdfImportedPage page = writer.GetImportedPage(reader, 1);
            cb.AddTemplate(page, 0, 0);

            // close the streams and voilá the file should be changed :)
            document.Close();
            fs.Close();
            writer.Close();
            reader.Close();

            ReplaceFile(newFile, oldFile, oldFile + ".bk");


        }

        // Move a file into another file, delete the original, and create a backup of the replaced file.
        public static void ReplaceFile(string FileToMoveAndDelete, string FileToReplace, string BackupOfFileToReplace)
        {
            File.Replace(FileToMoveAndDelete, FileToReplace, BackupOfFileToReplace, false);

        }

        private String consulta_tipo_ND(string pr_tipo)
        {

            var DB = new BasesDatos();
            string rpt = "N";
            try
            {

               
                DB.Conectar2();
                DB.CrearComando2("select top 1 termino from general WITH (NOLOCK)  where idComprobante =@id_Comprobante");
                DB.AsignarParametroCadena("@id_Comprobante", pr_tipo);
                using (DbDataReader DR3 = DB.EjecutarConsulta())
                {
                    if (DR3.Read())
                    {
                        rpt = DR3[0].ToString();

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

        private string obtener_codigo(string a_parametro)
        {
            string retorna = ConfigurationManager.AppSettings.Get(a_parametro);

            return retorna;
        }
        public MemoryStream msPDF(string p_codigoControl)
        {
            var DB = new BasesDatos();
            MemoryStream mrpt = new MemoryStream();
            
                DB = new BasesDatos();
                try
                {
                    string idComprobante = "", codDoc = "";
                    DB.Conectar2();
                    DB.CrearComando2("select top 1 idComprobante, codDoc from GENERAL WITH (NOLOCK)  where codigoControl = @codigoControl");
                    DB.AsignarParametroCadena("@codigoControl", p_codigoControl);
                    using (DbDataReader DR3 = DB.EjecutarConsulta())
                    {
                        if (DR3.Read())
                        {
                            idComprobante = DR3[0].ToString();
                            codDoc = DR3[1].ToString();
                        }
                    }

                    DB.Desconectar2();
                    if (!String.IsNullOrEmpty(idComprobante) && !String.IsNullOrEmpty(codDoc))
                        PoblarReporte(out mrpt, p_codigoControl, idComprobante, codDoc);

                }
                catch (Exception ex)
                {
                    DB.Desconectar2();
                    clsLogger.Graba_Log_Error(ex.Message);
                    log.mensajesLog("EM009", " Error en proceso msPDF ", ex.Message, p_codigoControl, "");
                }
                finally
                {
                    DB.Desconectar2();
                }

                return mrpt;
            
        }

    }
}


