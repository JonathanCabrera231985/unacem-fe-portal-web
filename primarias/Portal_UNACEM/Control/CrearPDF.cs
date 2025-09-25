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
    public class CrearPDF
    {
        //private BasesDatos DB;
        string msj = "";
        string msjT = "";
        Log log;
        string estab2 = "";
        string ptoemi2 = "";
        string compen = "";
        DateTime fecha;

        public void PoblarReporte(out MemoryStream ruta, string nombre, string idComprobante, string comprobante, string categoriaNegocio = null)
        {
            ruta = null;
            var DB = new BasesDatos();
            log = new Log();
            SqlConnection sqlConn;
            //SqlDataAdapter sqlDaComprobante, sqlDaDetalles, sqlDaInfoAdicional, sqlDaImpuestosRetenciones, sqlDaInfoAdicional1;
            //SqlDataAdapter sqlDaDetalleDest, sqlDaDestinatarios, sqlDaDetallesAdicionales, sqlDaDetAdicionales2, sqlDaIva, sqlDaPagos, sqlDACompensacion, sqlrubros, sqlnotaAclaratoria;
            //SqlDataAdapter sqlCabeceraExportacion, sqlInfoExportacion, sqlDaDetAdicionalGuia;
            //SqlDataAdapter adapteCatalogoTotales;
            var auxAmbiente = "";
            sucursal(idComprobante);


            DB.Conectar();
            DB.CrearComando(@"select tarifa from Compensacion WITH (NOLOCK) inner join GENERAL WITH (NOLOCK) on  idComprobante= id_Comprobante where id_Comprobante = @idComprobante");
            DB.AsignarParametroCadena("@idComprobante", idComprobante);
            using (DbDataReader DR3 = DB.EjecutarConsulta())
            {
                if (DR3.Read())
                {
                    compen = DR3["tarifa"].ToString();
                }
            }

            DB.Desconectar();

            DatosComprobantes dsPc = new DatosComprobantes();
            String strConn;
            strConn = ConfigurationManager.ConnectionStrings["dataexpressConnectionString"].ConnectionString;

            String StrComprobante = @"
                        DECLARE @strXML XML = '', @IdDoc INT  = 0, @strXML2 XML = '', @COD_DOC VARCHAR(2) = '', @nameOpenXML VARCHAR(50) = '',@INFODOCUMENTO VARCHAR(50) = '', 
					    @NOMBRECOMPROBANTE VARCHAR(50) = '', @nameOpenXML2 VARCHAR(50) = '', @strXMLNormal XML = '', @strXMLAUT XML = '', @TIPO VARCHAR(1) = '', @ESTADO VARCHAR(1) = ''

                        SELECT @strXMLNormal = AX.xmlEnviado, @strXMLAUT = AX.xmlSRI, @COD_DOC = G.codDoc, @TIPO = G.tipo, @ESTADO = G.estado FROM dbo.GENERAL G WITH (NOLOCK)
					    inner join dbo.ArchivoXml AX WITH (NOLOCK)  ON AX.codigoControl = G.codigoControl
					    WHERE G.idComprobante = '" + idComprobante + @"'

                        IF @strXMLNormal IS NULL
					    BEGIN
						    SET @strXML = @strXMLAUT
					    END
					    ELSE
					    BEGIN
						    SET @strXML = @strXMLNormal
					    END

                        EXEC sp_xml_preparedocument @IdDoc OUTPUT, @strXML
					
					IF @strXMLNormal IS NULL AND @TIPO = 'E' AND @ESTADO = '1'
					BEGIN
						SELECT @strXML2 = replace(comprobante , '<?xml version=""1.0"" encoding=""UTF-8""?>' ,'')
											FROM OPENXML (@IdDoc, '/autorizacion' ,2)  WITH (comprobante varchar(max))
					END
					ELSE
					BEGIN
						SET @strXML2 = replace(CONVERT(VARCHAR(MAX),@strXML) , '<?xml version=""1.0"" encoding=""UTF-8""?>' ,'')
					END

					IF ISNULL(CAST(@strXML2 AS VARCHAR(max)), '') = '' SET @strXML2 = @strXML

					EXEC sp_xml_removedocument @IdDoc
					
					EXEC sp_xml_preparedocument @IdDoc OUTPUT, @strXML2	

					SELECT @INFODOCUMENTO = (SELECT CASE @COD_DOC WHEN '01' THEN 'infoFactura'
								                                            WHEN '04' THEN 'infoNotaCredito'
                                                                            WHEN '03' THEN 'infoLiquidacionCompra'
								                                            WHEN '05' THEN 'infoNotaDebito'
								                                            WHEN '06' THEN 'infoGuiaRemision'
								                                            WHEN '07' THEN 'infoCompRetencion'
								                                            ELSE '' END),
	                @NOMBRECOMPROBANTE = (SELECT CASE @COD_DOC WHEN '01' THEN 'factura'
                                                                    WHEN '03' THEN 'liquidacionCompra'
						                                            WHEN '04' THEN 'notaCredito'
						                                            WHEN '05' THEN 'notaDebito'
						                                            WHEN '06' THEN 'guiaRemision'
						                                            WHEN '07' THEN 'comprobanteRetencion'
						                                            END )

					 set @nameOpenXML = '/'+ @NOMBRECOMPROBANTE + '/' + @INFODOCUMENTO
					 SET @nameOpenXML2 = '/'+ @NOMBRECOMPROBANTE  + '/infoTributaria'

					 select razonSocial,nombreComercial,dirMatriz,agenteRetencion
					 into #tmpXmlaRet
					 from OPENXML (@IdDoc, @nameOpenXML2 ,2)  WITH (dirMatriz VARCHAR(300), razonSocial VARCHAR(300), nombreComercial VARCHAR(300), agenteRetencion VARCHAR(300)) 

					 select direccionComprador,contribuyenteEspecial,obligadoContabilidad,dirEstablecimiento,dirPartida,razonSocialComprador,razonSocialSujetoRetenido,
					 identificacionComprador, identificacionSujetoRetenido
					 into #tmpXmla2Ret
					 from OPENXML (@IdDoc, @nameOpenXML ,2)  WITH (dirEstablecimiento VARCHAR(300), razonSocialComprador VARCHAR(300), direccionComprador varchar(300),
					 razonSocialSujetoRetenido VARCHAR(300), identificacionComprador VARCHAR(300), identificacionSujetoRetenido VARCHAR(300),
					 dirPartida VARCHAR(300), contribuyenteEspecial varchar(30), obligadoContabilidad varchar(30) )
                        
                        SELECT        GENERAL.idComprobante, GENERAL.id, GENERAL.version, GENERAL.serie, GENERAL.folio, GENERAL.fecha, GENERAL.sello, GENERAL.noCertificado, 
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
                         EMISOR.IDEEMI, EMISOR.RFCEMI, EMISOR.NOMEMI, EMISOR.nombreComercial, isnull(XMLA.dirMatriz,EMISOR.dirMatriz) dirMatriz,EMISOR.telefonoE, RECEPTOR.IDEREC, RECEPTOR.RFCREC,RECEPTOR.direccionComprador,
                         RECEPTOR.NOMREC, RECEPTOR.contribuyenteEspecial, RECEPTOR.obligadoContabilidad, RECEPTOR.tipoIdentificacionComprador, DOMEMIEXP.IDEDOMEMIEXP, 
                         isnull(XMLA2.dirEstablecimiento,DOMEMIEXP.dirEstablecimientos) dirEstablecimientos, GENERAL.codigoBarras, GENERAL.numeroAutorizacion, GENERAL.estado, GENERAL.fechaAutorizacion, GENERAL.rise, 
                         GENERAL.dirPartida, GENERAL.termino, GENERAL.proforma, GENERAL.pedido, RECEPTOR.domicilio, RECEPTOR.telefono, GENERAL.cantletras
FROM            GENERAL  WITH (NOLOCK) INNER JOIN
                         EMISOR WITH (NOLOCK) ON GENERAL.id_Emisor = EMISOR.IDEEMI INNER JOIN
                         RECEPTOR WITH (NOLOCK) ON GENERAL.id_Receptor = RECEPTOR.IDEREC INNER JOIN
                         DOMEMIEXP WITH (NOLOCK)  ON GENERAL.id_EmisorExp = DOMEMIEXP.IDEDOMEMIEXP
                            left JOIN #tmpXmlaRet XMLA on 1 = 1
								 left JOIN #tmpXmla2Ret XMLA2 on 1 = 1
                                  WHERE 
                         General.idComprobante='" + idComprobante + @"'
                            drop table #tmpXmlaRet
							drop table #tmpXmla2Ret";
            String StrInfoAdicional = @"SELECT nombre, valor, id_Comprobante FROM InfoAdicional WITH (NOLOCK)  WHERE id_Comprobante='" + idComprobante + "'";

            String StrDetalles = @"SELECT        idDetalles, codigoPrincipal, codigoAuxiliar, descripcion AS Descripcion, 
                                    convert(decimal(18,4),isnull(cantidad,0)) as cantidad, 
                                    convert(decimal(18,4),isnull(precioUnitario,0)) as precioUnitario, 
                                    descuento, precioTotalSinImpuestos, 
                                    replace (B.tarifa,'.00','%') as tarifa, 
                                    id_Comprobante
                                    FROM   Detalles A with(nolock) 
                                    inner join ImpuestosDetalles B with(nolock) on A.idDetalles = B.id_Detalles
                                     WHERE id_Comprobante='" + idComprobante + "'";

            String StrImpuestosIva = @"select top 1 TI.tarifa,TI.codigoPorcentaje
                                      from TotalConImpuestos TI WITH (NOLOCK) , CatImpuestos_C CA WITH (NOLOCK)  where  TI.id_comprobante = '" + idComprobante + "' and TI.codigo = 2 and CA.tipo = 'IVA' and TI.codigoPorcentaje = CA.codigo  and TI.codigoPorcentaje in('2', '3','0')";


            String StrImpuestosRetenciones = @"SELECT tci.codigo,ISNULL((SELECT descripcion FROM CatImpuestos_C WITH (NOLOCK) where tipo = 'Retencion' AND codigo =tci.codigo ),'') as descripcionImpuesto,
                                                tci.codigoPorcentaje, tci.baseImponible, tci.tarifa, tci.valor,tci.porcentajeRetener, 
                                                ISNULL((SELECT descripcion FROM Catalogo_Comprobante WITH (NOLOCK) where tipo = 'Comprobante' AND codigo =tci.codDocSustento ),'') as descripcionComprobante,
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
FROM            Destinatarios  WITH (NOLOCK) WHERE id_Comprobante='" + idComprobante + "' ORDER BY razonSocialDestinatario DESC";

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
                inner join Detalles d WITH (NOLOCK) on da.id_Detalles = d.idDetalles where d.id_Comprobante = " + idComprobante + @"               
            ) x
            pivot 
            (
                max(valor)
                for nombre in (ConceptoInterno,numeroFactura,UnidadDescuento,idDetalles)
            ) p";

            //Detalles AdicionalesGR
            String StrDetAdicionalGR1 = @"SELECT Unidad,Saldo_Anterior,Saldo_Actual,id_Detalles as idDetalles, Despachado, Programado, Viaje
             from 
             (
                select da.valor, da.nombre,da.id_Detalles, iaD.valor as Despachado, iaP.valor as Programado, iaV.valor as Viaje
                from DetallesAdicionales da WITH (NOLOCK) 
                inner join Detalles d WITH (NOLOCK) on da.id_Detalles = d.idDetalles
                left join InfoAdicional iaD WITH (NOLOCK)  on iaD.id_Comprobante = d.id_Comprobante and iaD.nombre = 'despachadoM3'
				left join InfoAdicional iaP WITH (NOLOCK) on iaP.id_Comprobante = d.id_Comprobante and iaP.nombre = 'programadoM3'
				left join InfoAdicional iaV WITH (NOLOCK) on iaV.id_Comprobante = d.id_Comprobante and iaV.nombre = 'viaje'
                where d.id_Comprobante = " + idComprobante + @"               
            ) x
            pivot 
            (
                max(valor)
                for nombre in (Unidad,Saldo_Anterior,Saldo_Actual,idDetalles)
            ) p";

            String pagos = @"select distinct b.descripcion, a.id_Comprobante,a.plazo,a.total,a.unidadTiempo,a.formaPago from Pago A WITH (NOLOCK) inner join Catalogo1_C B WITH (NOLOCK) on a.formaPago = b.codigo
            and b.tipo = 'pagos' where a.id_Comprobante = " + idComprobante;

            String desc_solidario = @"SELECT codigo,tarifa,CAST(valor AS DECIMAL(14,2)) as valor  FROM Compensacion WITH (NOLOCK)  WHERE id_Comprobante='" + idComprobante + "'";

            String rubros = @"select concepto ,CAST(total AS DECIMAL(18,2)) as total  from otrosRubrosTerceros WITH (NOLOCK)  where id_Comprobante ='" + idComprobante + "'";

            String notaAclaratoria = @"select valor from InfoAdicional WITH (NOLOCK)  where id_Comprobante ='" + idComprobante + "' and nombre ='Nota_Aclaratoria'";

            String cabeceraExportacion = @"select g.incoTermFactura, c.descripcion as paisOrigen, cd.descripcion as paisDestino, g.puertoEmbarque, g.puertoDestino, 
                                                g.fleteInternacional, g.seguroInternacional, g.gastosAduaneros, g.gastosTransporteOtros
                                            FROM GENERALEXPORTACION g WITH (NOLOCK) 
	                                            LEFT JOIN Catalogo1_C c WITH (NOLOCK) 
	                                            ON c.codigo = g.paisOrigen AND c.tipo = 'Pais'
                                                LEFT JOIN Catalogo1_C cd WITH (NOLOCK) 
	                                            ON cd.codigo = g.paisDestino AND c.tipo = 'Pais'
                                            WHERE g.idComprobante ='" + idComprobante + "'";

            String infoAdicionalExportacion = @"SELECT [VIA_EMBARQUE],[FECHA_EMBARQUE],[PESO_NETO],[PESO_BRUTO],[PARTIDA_ARANCELARIA]
                                                from 
                                                (
	                                                SELECT ia.valor, ia.nombre FROM InfoAdicional ia WITH (NOLOCK)  WHERE id_Comprobante = '" + idComprobante + @"' 
                                                ) x
                                                pivot 
                                                (
                                                max(valor)
                                                for nombre in ([VIA_EMBARQUE],[FECHA_EMBARQUE],[PESO_NETO],[PESO_BRUTO],[PARTIDA_ARANCELARIA])
                                                ) p ";

            String infoAdicionalGuiaPesoNeto = "select top 1 valor as Peso_Neto from InfoAdicional WITH (NOLOCK)  where nombre ='Peso_Neto' and id_Comprobante = " + idComprobante;

            sqlConn = new SqlConnection(strConn);

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
                        using (SqlDataAdapter sqlrubros = new SqlDataAdapter(rubros, sqlConn))
                        {
                            sqlrubros.Fill(dsPc, "rubros");
                        }

                        using (SqlDataAdapter sqlnotaAclaratoria = new SqlDataAdapter(notaAclaratoria, sqlConn))
                        {
                            sqlnotaAclaratoria.Fill(dsPc, "notaAclaratoria");
                        }
                        using (SqlDataAdapter sqlCabeceraExportacion = new SqlDataAdapter(cabeceraExportacion, sqlConn))
                        {
                            sqlCabeceraExportacion.Fill(dsPc, "cabeceraExportacion");
                        }
                        using (SqlDataAdapter sqlInfoExportacion = new SqlDataAdapter(infoAdicionalExportacion, sqlConn))
                        {
                            sqlInfoExportacion.Fill(dsPc, "infoExportacion");
                        }


                        //IVA 15

                        using (SqlDataAdapter adapteCatalogoTotales = new SqlDataAdapter("SP_ConsultaTotalesDocumento", sqlConn))
                        {
                            adapteCatalogoTotales.SelectCommand.CommandType = CommandType.StoredProcedure;
                            adapteCatalogoTotales.SelectCommand.Parameters.Add("@idComprobante", SqlDbType.BigInt).Value = idComprobante;

                            adapteCatalogoTotales.Fill(dsPc, "CatalogoTotales");
                        }

                        if (dsPc.Tables["cabeceraExportacion"].Rows.Count > 0)
                        {
                            using (FacturaExportacion rptEx = new FacturaExportacion())
                            {
                                rptEx.SetDataSource(dsPc);
                                auxAmbiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
                                if (auxAmbiente.Equals("1"))//QA
                                {
                                    rptEx.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat).CopyTo(ruta = new MemoryStream());

                                }
                                else//PROD
                                {
                                    ruta = (MemoryStream)rptEx.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                }
                                rptEx.Dispose();
                                rptEx.Close();
                            }
                        }
                        else
                        {
                            //ReportDocument rpt = null;
                            string configCN = ConfigurationManager.AppSettings.Get("categoria_negocio");

                            if (configCN.Contains(categoriaNegocio) && !String.IsNullOrEmpty(categoriaNegocio))
                            {
                                using (ReportDocument rpt = new FacturaConcreto())
                                {
                                    rpt.SetDataSource(dsPc);
                                    auxAmbiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
                                    if (auxAmbiente.Equals("1"))//QA
                                    {
                                        rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat).CopyTo(ruta = new MemoryStream());

                                    }
                                    else//PROD
                                    {
                                        ruta = (MemoryStream)rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                    }
                                    rpt.Dispose();
                                    rpt.Close();
                                }
                            }

                            else
                            {
                                using (ReportDocument rpt = new Factura())
                                {
                                    rpt.SetDataSource(dsPc);
                                    auxAmbiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
                                    if (auxAmbiente.Equals("1"))//QA
                                    {
                                        rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat).CopyTo(ruta = new MemoryStream());

                                    }
                                    else//PROD
                                    {
                                        ruta = (MemoryStream)rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                    }
                                    rpt.Dispose();
                                    rpt.Close();
                                }
                            }
                               
                          
                        }


                        break;
                    case "03":

                        using (SqlDataAdapter sqlDaComprobante = new SqlDataAdapter(StrComprobante, sqlConn))
                        {
                            dsPc.EnforceConstraints = false;
                            sqlDaComprobante.Fill(dsPc, "Comprobante");
                        }

                        using (SqlDataAdapter sqlDaDetalles = new SqlDataAdapter(StrDetalles, sqlConn))
                        {
                            sqlDaDetalles.Fill(dsPc, "DetallesConDetalleAdicionales");
                        }
                        using (SqlDataAdapter sqlDaInfoAdicional1 = new SqlDataAdapter(StrInfoAdicional1, sqlConn))
                        {
                            sqlDaInfoAdicional1.Fill(dsPc, "InfoAdicionalF");
                        }
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
                        using (SqlDataAdapter sqlrubros = new SqlDataAdapter(rubros, sqlConn))
                        {
                            sqlrubros.Fill(dsPc, "rubros");
                        }
                        using (SqlDataAdapter sqlnotaAclaratoria = new SqlDataAdapter(notaAclaratoria, sqlConn))
                        {
                            sqlnotaAclaratoria.Fill(dsPc, "notaAclaratoria");
                        }

                        //IVA 15

                        using (SqlDataAdapter adapteCatalogoTotales = new SqlDataAdapter("SP_ConsultaTotalesDocumento", sqlConn))
                        {
                            adapteCatalogoTotales.SelectCommand.CommandType = CommandType.StoredProcedure;
                            adapteCatalogoTotales.SelectCommand.Parameters.Add("@idComprobante", SqlDbType.BigInt).Value = idComprobante;

                            adapteCatalogoTotales.Fill(dsPc, "CatalogoTotales");
                        }

                        using (LiquidacionCompra lq = new LiquidacionCompra())
                        {
                            lq.SetDataSource(dsPc);
                            auxAmbiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
                            if (auxAmbiente.Equals("1"))//QA
                            {
                                lq.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat).CopyTo(ruta = new MemoryStream());

                            }
                            else//PROD
                            {
                                ruta = (MemoryStream)lq.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                            }
                            lq.Dispose();
                            lq.Close();
                        }
                        break;
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




                        //IVA 15

                        using (SqlDataAdapter adapteCatalogoTotales = new SqlDataAdapter("SP_ConsultaTotalesDocumento", sqlConn))
                        {
                            adapteCatalogoTotales.SelectCommand.CommandType = CommandType.StoredProcedure;
                            adapteCatalogoTotales.SelectCommand.Parameters.Add("@idComprobante", SqlDbType.BigInt).Value = idComprobante;

                            adapteCatalogoTotales.Fill(dsPc, "CatalogoTotales");
                        }

                        if (!String.IsNullOrEmpty(compen))
                        {
                            NotaCredito_Compensacion rptNC = new NotaCredito_Compensacion();
                            rptNC.SetDataSource(dsPc);
                            auxAmbiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
                            if (auxAmbiente.Equals("1"))//QA
                            {
                                rptNC.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat).CopyTo(ruta = new MemoryStream());

                            }
                            else//PROD
                            {
                                ruta = (MemoryStream)rptNC.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                            }
                            rptNC.Dispose();
                            rptNC.Close();
                        }
                        else
                        {
                            using (NotaCredito rptNC = new NotaCredito())
                            {
                                rptNC.SetDataSource(dsPc);
                                auxAmbiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
                                if (auxAmbiente.Equals("1"))//QA
                                {
                                    rptNC.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat).CopyTo(ruta = new MemoryStream());

                                }
                                else//PROD
                                {
                                    ruta = (MemoryStream)rptNC.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                }
                                rptNC.Dispose();
                                rptNC.Close();
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
                                                    auxAmbiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
                                                    if (auxAmbiente.Equals("1"))//QA
                                                    {
                                                        rptNDAR.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat).CopyTo(ruta = new MemoryStream());

                                                    }
                                                    else//PROD
                                                    {
                                                        ruta = (MemoryStream)rptNDAR.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                                    }
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
                                                    auxAmbiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
                                                    if (auxAmbiente.Equals("1"))//QA
                                                    {
                                                        rptND.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat).CopyTo(ruta = new MemoryStream());
                                                    }
                                                    else//PROD
                                                    {
                                                        ruta = (MemoryStream)rptND.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                                    }
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
                            dsPc.EnforceConstraints = false;
                            sqlDaComprobante.Fill(dsPc, "Comprobante");
                        }
                        //Destinatarios
                        using (SqlDataAdapter sqlDaDestinatarios = new SqlDataAdapter(StrDestinatarios, sqlConn))
                        {
                            sqlDaDestinatarios.Fill(dsPc, "Destinatarios");
                        }
                        //DetallesDestinatarios
                        using (SqlDataAdapter sqlDaDetalleDest = new SqlDataAdapter(StrDetallesDest, sqlConn))
                        {
                            sqlDaDetalleDest.Fill(dsPc, "Detalles");
                        }
                        //info Adicional                        
                        using (SqlDataAdapter sqlDaInfoAdicional1 = new SqlDataAdapter(StrInfoAdicional1, sqlConn))
                        {
                            sqlDaInfoAdicional1.Fill(dsPc, "InfoAdicionalGR");
                        }
                        //Detalles Adicionales2
                        using (SqlDataAdapter sqlDaDetAdicionales2 = new SqlDataAdapter(StrDetAdicionalGR1, sqlConn))
                        {
                            sqlDaDetAdicionales2.Fill(dsPc, "detAdicionalGR");
                        }
                        using (SqlDataAdapter sqlDaDetAdicionalGuia = new SqlDataAdapter(infoAdicionalGuiaPesoNeto, sqlConn))
                        {
                            sqlDaDetAdicionalGuia.Fill(dsPc, "InfoAdicionalGRDetalle");
                        }

                        if (estab2.Equals(obtener_codigo("estabEliminarCampos")) && ptoemi2.Equals(obtener_codigo("ptoemiEliminarCampos")))
                        {
                            using (GuiaRemision014024 rptGR = new GuiaRemision014024())
                            {
                                rptGR.SetDataSource(dsPc);
                                auxAmbiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
                                if (auxAmbiente.Equals("1"))//QA
                                {
                                    rptGR.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat).CopyTo(ruta = new MemoryStream());
                                }
                                else//PROD
                                {
                                    ruta = (MemoryStream)rptGR.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

                                }
                                rptGR.Dispose();
                                rptGR.Close();
                            }
                        }
                        else
                        {
                            //ReportDocument rptGR = null;
                            string configCN = ConfigurationManager.AppSettings.Get("categoria_negocio");
                            string configCNT = ConfigurationManager.AppSettings.Get("categoria_negocioT");
                            if (configCN.Contains(categoriaNegocio) && !String.IsNullOrEmpty(categoriaNegocio))
                            {
                                using (ReportDocument rptGR = new GuiaRemisionConcreto())
                                {
                                    rptGR.SetDataSource(dsPc);
                                    auxAmbiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
                                    if (auxAmbiente.Equals("1"))//QA
                                    {
                                        rptGR.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat).CopyTo(ruta = new MemoryStream());
                                    }
                                    else//PROD
                                    {
                                        ruta = (MemoryStream)rptGR.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                    }
                                    rptGR.Dispose();
                                    rptGR.Close();
                                }
                            }
                            
                            else if (configCNT.Contains(categoriaNegocio) && !String.IsNullOrEmpty(configCNT))
                            {

                                using (ReportDocument rptGR = new GuiaRemisionTraslado())
                                {
                                    rptGR.SetDataSource(dsPc);
                                    auxAmbiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
                                    if (auxAmbiente.Equals("1"))//QA
                                    {
                                        rptGR.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat).CopyTo(ruta = new MemoryStream());
                                    }
                                    else//PROD
                                    {
                                        ruta = (MemoryStream)rptGR.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                    }
                                    rptGR.Dispose();
                                    rptGR.Close();
                                }

                            }

                            else
                            {
                                using (ReportDocument rptGR = new GuiaRemision())
                                {
                                    rptGR.SetDataSource(dsPc);
                                    auxAmbiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
                                    if (auxAmbiente.Equals("1"))//QA
                                    {
                                        rptGR.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat).CopyTo(ruta = new MemoryStream());
                                    }
                                    else//PROD
                                    {
                                        ruta = (MemoryStream)rptGR.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                                    }
                                    rptGR.Dispose();
                                    rptGR.Close();
                                }
                            }
                                
                            
                        }
                        break;
                    case "07":

                        using (SqlDataAdapter sqlDaComprobante = new SqlDataAdapter(StrComprobante, sqlConn))
                        {
                            dsPc.EnforceConstraints = false;
                            sqlDaComprobante.Fill(dsPc, "Comprobante");
                        }
                        using (SqlDataAdapter sqlDaImpuestosRetenciones = new SqlDataAdapter(StrImpuestosRetenciones, sqlConn))
                        {
                            sqlDaImpuestosRetenciones.Fill(dsPc, "TotalConImpuestos");
                        }
                        using (SqlDataAdapter sqlDaInfoAdicional1 = new SqlDataAdapter(StrInfoAdicional1, sqlConn))
                        {

                            sqlDaInfoAdicional1.Fill(dsPc, "InfoAdicionalRetFG");
                        }
                        using (CompRetencion rptCR = new CompRetencion())
                        {
                            rptCR.SetDataSource(dsPc);
                            auxAmbiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
                            if (auxAmbiente.Equals("1"))//QA
                            {
                                rptCR.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat).CopyTo(ruta = new MemoryStream());
                            }
                            else//PROD
                            {
                                ruta = (MemoryStream)rptCR.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                            }
                            rptCR.Dispose();
                            rptCR.Close();
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
                DB.Desconectar();
            }
            finally
            {
                if (sqlConn != null)
                {
                    sqlConn.Close();
                    dsPc.Dispose();
                  


                }
                DB.Desconectar();
            }
        }


        private void sucursal(string idComprobante)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Conectar();
                DB.CrearComando(@"select estab,ptoEmi from general WITH (NOLOCK) where idComprobante = @idComprobante");
                DB.AsignarParametroCadena("@idComprobante", idComprobante);
                using (DbDataReader DR6 = DB.EjecutarConsulta())
                {
                    if (DR6.Read())
                    {
                        estab2 = DR6["estab"].ToString();
                        ptoemi2 = DR6["ptoEmi"].ToString();

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
               
                DB.Conectar();
                DB.CrearComando("select top 1 termino from general WITH (NOLOCK)  where idComprobante =@id_Comprobante");
                DB.AsignarParametroCadena("@id_Comprobante", pr_tipo);
                using (DbDataReader DR3 = DB.EjecutarConsulta())
                {
                    if (DR3.Read())
                    {
                        rpt = DR3[0].ToString();

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

        private string obtener_codigo(string a_parametro)
        {
            string retorna = ConfigurationManager.AppSettings.Get(a_parametro);

            return retorna;
        }
        public MemoryStream msPDF(string p_codigoControl)
        {
            var DB = new BasesDatos();
            MemoryStream mrpt = new MemoryStream();
            //DB = new BasesDatos();
            try
            {
                string idComprobante = "", codDoc = "", categoriaNegocio = "";
                DB.Conectar();
                DB.CrearComando("select top 1 g.idComprobante, g.codDoc, ia.valor as categoria_negocio from GENERAL g WITH (NOLOCK)  left join InfoAdicional ia WITH (NOLOCK)  on ia.id_Comprobante = g.idComprobante and ia.nombre = 'categoria_negocio' where g.codigoControl = @codigoControl");
                DB.AsignarParametroCadena("@codigoControl", p_codigoControl);
                using (DbDataReader DR3 = DB.EjecutarConsulta())
                {
                    if (DR3.Read())
                    {
                        idComprobante = DR3[0].ToString();
                        codDoc = DR3[1].ToString();
                        categoriaNegocio = DR3[2].ToString();
                    }
                }

                DB.Desconectar();
                if (!String.IsNullOrEmpty(idComprobante) && !String.IsNullOrEmpty(codDoc))
                    PoblarReporte(out mrpt, p_codigoControl, idComprobante, codDoc, categoriaNegocio);

            }
            catch (Exception ex)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ex.Message);
                log.mensajesLog("EM009", " Error en proceso msPDF ", ex.Message, p_codigoControl, "");
            }
            finally
            {
                DB.Desconectar();
            }

            return mrpt;
        }

    }
}


