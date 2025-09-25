using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
using System.Data.Common;
using System.Collections;
using System.IO;
using clibLogger;

namespace Control
{
    public class Spool
    {
        //private BasesDatos DB;
       // private DbDataReader DR;
        private EnviarMail em;
        private NumerosALetras numA;
        private Log log;
        private string msj;
        private string msjT;
        private string RutaTXT = "";
        private string RutaBCK = "";
        private string RutaDOC = "";
        private string RutaERR = "";
        private string RutaCER = "";
        private string RutaKEY = "";

        #region Parametros
        private string servidor = "";
        private int puerto = 587;
        private Boolean ssl =false;
        private string emailCredencial = "";
        private string passCredencial = "";
        private string emailEnviar = "";
        private string emails = "";
        #endregion
        #region General

        string tipoComprobante, idComprobante, version;
        //Informacion Tributaria
        string ambiente, tipoEmision, razonSocial, nombreComercial, ruc, claveAcceso, codDoc, estab, ptoEmi, secuencial, dirMatriz;
        //Informacion del Documento(Factura,guia,notas,retenciones)
        string fechaEmision, dirEstablecimiento, contribuyenteEspecial, obligadoContabilidad, tipoIdentificacionComprador;
								string guiaRemision, razonSocialComprador, identificacionComprador, direccionComprador, totalSinImpuestos, totalDescuento, propina, importeTotal, moneda;
        string dirPartida, razonSocialTransportista, tipoIdentificacionTransportista, rucTransportista, rise, fechaIniTransporte, fechaFinTransporte, placa;//Guia de Remision
        string codDocModificado, numDocModificado, fechaEmisionDocSustentoNota, valorModificacion, motivo;//Nota de Credito
        string valorTotal;
        //Nota de Debito
        string tipoIdentificacionSujetoRetenido, razonSocialSujetoRetenido, identificacionSujetoRetenido, periodoFiscal;
        //Destinatario Para Guia de Remision
        string identificacionDestinatario, razonSocialDestinatario, dirDestinatario, motivoTraslado, docAduaneroUnico, codEstabDestino, ruta, codDocSustentoDestinatario, numDocSustentoDestinatario, numAutDocSustento, fechaEmisionDocSustentoDestinatario;
        //Total de Impuestos
        string codigo, codigoPorcentaje, baseImponible, tarifa, valor;
        string codigoRetencion, porcentajeRetener, valorRetenido, codDocSustento, numDocSustento, fechaEmisionDocSustento; //Retenciones
        //detalles
        string codigoPrincipal, codigoAuxiliar, descripcion, cantidad, precioUnitario, descuento, precioTotalSinImpuesto;
        string codigoInterno, codigoAdicional;
        //detalles Adicionales
        string detAdicionalNombre, detAdicionalValor;
        //Impuestos Detalles
        string impuestoCodigo, impuestoCodigoPorcentaje, impuestoTarifa, impuestoBaseImponible, impuestoValor;
        //infoAdicional
        string infoAdicionalNombre, infoAdicionalValor;
        //Motivo (Nota de Debito)
        string motivoRazon, motivoValor;
        #endregion


        string tipoImpuesto;
        string impuestotipoImpuesto;
        #region totales
        string subtotal12;
        string subtotal0;
        string subtotalNoSujeto;
        string ICE;
        string IVA12;
        string importeAPagar;

        //Información Adicional CIMA
        string termino, proforma, domicilio, telefono, pedido;

        #endregion


        string idDestinatario = "";
        string idDetallesTemp = "";
        ArrayList arraylDetalles;
        ArrayList arraylImpuestosDetalles;
        ArrayList arraylDetallesAdicionales;
        ArrayList arrayInfoAdicionales;
        ArrayList arraylTotalImpuestos;
        ArrayList arraylTotalImpuestosRetenciones;
        ArrayList arraylMotivos;
        ArrayList arraylDestinatarios;
								ArrayList arraylPagos;
        String[] asDetalles;
        String[] asImpuestosDetalles;
        String[] asDetallesAdicionales;
        String[] asInfoAdicionales;
        String[] asTotalImpuestos;
        String[] asMotivos;
        String[] asTotalImpuestosRetenciones;
        String[] asDestinatarios;
								String[] asPagos;

								private string forma_pago = "";
								private string plazo = "";
								private string totalPago = "0";
								private string unidadTiempo = "";
        public Spool()
        {
            var DB = new BasesDatos();
            try
            {

                numA = new NumerosALetras();
                log = new Log();
                //Parametros Generales
                DB.Conectar();
                DB.CrearComando(@"select servidorSMTP,puertoSMTP,sslSMTP,userSMTP,passSMTP,
                              dirdocs,dirtxt,dirrespaldo,dircertificados,dirllaves,emailEnvio 
                              from ParametrosSistema WITH (NOLOCK) ");
                using (DbDataReader DR = DB.EjecutarConsulta())
                {
                    while (DR.Read())
                    {
                        servidor = DR[0].ToString();
                        puerto = Convert.ToInt32(DR[1]);
                        ssl = Convert.ToBoolean(DR[2]);
                        emailCredencial = DR[3].ToString();
                        passCredencial = DR[4].ToString();
                        RutaDOC = DR[5].ToString();
                        RutaTXT = DR[6].ToString();
                        RutaBCK = DR[7].ToString();
                        RutaCER = DR[8].ToString();
                        RutaKEY = DR[9].ToString();
                        emailEnviar = DR[10].ToString();
                    }
                }

                DB.Desconectar();
                //Fin de Parametros Generales.
                tipoComprobante = ""; idComprobante = ""; version = "";
                //Informacion Tributaria
                ambiente = ""; tipoEmision = ""; razonSocial = ""; nombreComercial = ""; ruc = ""; claveAcceso = ""; codDoc = ""; estab = ""; ptoEmi = ""; secuencial = ""; dirMatriz = "";
                //Informacion del Documento(Factura="";guia="";notas="";retenciones)
                fechaEmision = ""; dirEstablecimiento = ""; contribuyenteEspecial = ""; obligadoContabilidad = ""; tipoIdentificacionComprador = "";
                guiaRemision = ""; razonSocialComprador = ""; identificacionComprador = ""; direccionComprador = ""; totalSinImpuestos = "0"; totalDescuento = ""; propina = "0"; importeTotal = "0"; moneda = "0";
                dirPartida = ""; razonSocialTransportista = ""; tipoIdentificacionTransportista = ""; rucTransportista = ""; rise = ""; fechaIniTransporte = ""; fechaFinTransporte = ""; placa = "";//Guia de Remision
                codDocModificado = ""; numDocModificado = ""; fechaEmisionDocSustentoNota = ""; valorModificacion = "0"; motivo = "";//Nota de Credito
                valorTotal = "";
                //Nota de Debito
                tipoIdentificacionSujetoRetenido = ""; razonSocialSujetoRetenido = ""; identificacionSujetoRetenido = ""; periodoFiscal = "";
                //Destinatario Para Guia de Remision
                identificacionDestinatario = ""; razonSocialDestinatario = ""; dirDestinatario = ""; motivoTraslado = ""; docAduaneroUnico = ""; codEstabDestino = ""; ruta = ""; codDocSustentoDestinatario = ""; numDocSustentoDestinatario = ""; numAutDocSustento = ""; fechaEmisionDocSustentoDestinatario = "";
                //Total de Impuestos
                codigo = ""; codigoPorcentaje = ""; baseImponible = "0"; tarifa = "0"; valor = "0";
                codigoRetencion = ""; porcentajeRetener = ""; valorRetenido = ""; codDocSustento = ""; numDocSustento = ""; fechaEmisionDocSustento = ""; //Retenciones
                                                                                                                                                          //detalles
                codigoPrincipal = ""; codigoAuxiliar = ""; descripcion = ""; cantidad = ""; precioUnitario = ""; descuento = ""; precioTotalSinImpuesto = "";
                codigoInterno = ""; codigoAdicional = "";
                //detalles Adicionales
                detAdicionalNombre = ""; detAdicionalValor = "";
                //Impuestos Detalles
                impuestoCodigo = ""; impuestoCodigoPorcentaje = ""; impuestoTarifa = ""; impuestoBaseImponible = ""; impuestoValor = "";
                //infoAdicional
                infoAdicionalNombre = ""; infoAdicionalValor = "";
                //Motivo (Nota de Debito)
                motivoRazon = ""; motivoValor = "";
                idDestinatario = "";

                forma_pago = ""; plazo = ""; totalPago = "0"; unidadTiempo = "";

                arraylDetalles = new ArrayList();
                arraylImpuestosDetalles = new ArrayList();
                arraylDetallesAdicionales = new ArrayList();
                arrayInfoAdicionales = new ArrayList();
                arraylTotalImpuestos = new ArrayList();
                arraylMotivos = new ArrayList();
                arraylTotalImpuestosRetenciones = new ArrayList();
                arraylDestinatarios = new ArrayList();
                arraylPagos = new ArrayList();
                asDetalles = new String[1];
                asImpuestosDetalles = new String[1];
                asDetallesAdicionales = new String[1];
                asInfoAdicionales = new String[1];
                asTotalImpuestos = new String[1];
                asMotivos = new String[1];
                asTotalImpuestosRetenciones = new String[1];
                asDestinatarios = new String[1];
                asPagos = new String[1];

                //Info Adicional CIMA
                termino = ""; proforma = ""; domicilio = ""; telefono = ""; pedido = "";
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

        public void xmlComprobante(string version = "1.0", string idComprobante = "Comprobante")
        {
            this.version = version;
            this.idComprobante = idComprobante;
        }

        public void InformacionTributaria(string ambiente, string tipoEmision, string razonSocial, string nombreComercial, string ruc,
            string claveAcceso, string codDoc, string estab, string ptoEmi, string secuencial, string dirMatriz,string emails)
        {
            this.ambiente = ambiente; this.tipoEmision = tipoEmision; this.razonSocial = razonSocial;
            this.nombreComercial = nombreComercial; this.ruc = ruc; this.claveAcceso = claveAcceso;
            this.codDoc = codDoc; this.estab = estab; this.ptoEmi = ptoEmi; this.secuencial = secuencial;
            this.dirMatriz = dirMatriz; this.emails=emails;
        }

								public void infromacionDocumentoFactura(string fechaEmision, string dirEstablecimiento, string contribuyenteEspecial, string obligadoContabilidad, string tipoIdentificacionComprador,
											string guiaRemision, string razonSocialComprador, string identificacionComprador, string moneda,
											string dirPartida, string razonSocialTransportista, string tipoIdentificacionTransportista, string rucTransportista, string rise, string fechaIniTransporte, string fechaFinTransporte, string placa,//Guia de Remision
											string codDocModificado, string numDocModificado, string fechaEmisionDocSustentoNota, string valorModificacion, string motivo, string direccionComprador)
								{
												this.fechaEmision = fechaEmision; this.dirEstablecimiento = dirEstablecimiento; this.contribuyenteEspecial = contribuyenteEspecial;
												this.obligadoContabilidad = obligadoContabilidad; this.tipoIdentificacionComprador = tipoIdentificacionComprador;
												this.guiaRemision = guiaRemision; this.razonSocialComprador = razonSocialComprador; this.identificacionComprador = identificacionComprador; this.direccionComprador = direccionComprador;
												this.moneda = moneda;
												this.dirPartida = dirPartida; this.razonSocialTransportista = razonSocialTransportista; this.tipoIdentificacionTransportista = tipoIdentificacionTransportista;
												this.rucTransportista = rucTransportista; this.rise = rise; this.fechaIniTransporte = fechaIniTransporte; this.fechaFinTransporte = fechaFinTransporte;
												this.placa = placa; this.codDocModificado = codDocModificado; this.numDocModificado = numDocModificado;
												this.fechaEmisionDocSustentoNota = fechaEmisionDocSustentoNota; this.valorModificacion = valorModificacion; this.motivo = motivo;
								}
        public void infromacionDocumento(string fechaEmision, string dirEstablecimiento, string contribuyenteEspecial, string obligadoContabilidad, string tipoIdentificacionComprador,
            string guiaRemision, string razonSocialComprador, string identificacionComprador, string moneda,
            string dirPartida, string razonSocialTransportista, string tipoIdentificacionTransportista, string rucTransportista, string rise, string fechaIniTransporte, string fechaFinTransporte, string placa,//Guia de Remision
            string codDocModificado, string numDocModificado, string fechaEmisionDocSustentoNota, string valorModificacion, string motivo)
        {
            this.fechaEmision = fechaEmision; this.dirEstablecimiento = dirEstablecimiento; this.contribuyenteEspecial = contribuyenteEspecial;
            this.obligadoContabilidad = obligadoContabilidad; this.tipoIdentificacionComprador = tipoIdentificacionComprador;
            this.guiaRemision = guiaRemision; this.razonSocialComprador = razonSocialComprador; this.identificacionComprador = identificacionComprador;
            this.moneda = moneda;
            this.dirPartida = dirPartida; this.razonSocialTransportista = razonSocialTransportista; this.tipoIdentificacionTransportista = tipoIdentificacionTransportista;
            this.rucTransportista = rucTransportista; this.rise = rise; this.fechaIniTransporte = fechaIniTransporte; this.fechaFinTransporte = fechaFinTransporte;
            this.placa = placa; this.codDocModificado = codDocModificado; this.numDocModificado = numDocModificado;
            this.fechaEmisionDocSustentoNota = fechaEmisionDocSustentoNota; this.valorModificacion = valorModificacion; this.motivo = motivo;
        }
        public void retencionesPeriodoFiscal(string periodoFiscal)
        {
            this.periodoFiscal = periodoFiscal;
        }
        public void cantidades(string subtotal12, string subtotal0, string subtotalNoSujeto, string totalSinImpuestos,
                       string totalDescuento, string ICE, string IVA12, string importeTotal, string propina, string importeAPagar)
        {
            this.totalSinImpuestos = totalSinImpuestos;
            this.totalDescuento = totalDescuento;
            this.propina = propina;
            this.importeTotal = importeTotal;
            this.subtotal12 = subtotal12;
            this.subtotal0 = subtotal0;
            this.subtotalNoSujeto = subtotalNoSujeto;
            this.ICE = ICE;
            this.IVA12 = IVA12;
            this.importeAPagar = importeAPagar;
        }
        public void destinatarios(string idUser)
        {
            var DB = new BasesDatos();
            try
            {

                arraylDestinatarios = new ArrayList();
                idDestinatario = "";
                DB.Conectar();
                DB.CrearComando(@"SELECT identificacionDestinatario,razonSocialDestinatario,dirDestinatario,motivoTraslado,docAduaneroUnico,
                              codEstabDestino,ruta,codDocSustento,numDocSustento,numAutDocSustento,fechaEmisionDocSustento,idDestinatarioTemp
                            FROM 
                                DestinatariosTemp WITH (NOLOCK) 
                            WHERE
                                id_Empleado = @id_Empleado");
                DB.AsignarParametroCadena("@id_Empleado", idUser);
                using (DbDataReader DRDeta = DB.EjecutarConsulta())
                {
                    while (DRDeta.Read())
                    {
                        asDestinatarios = new String[12];
                        identificacionDestinatario = DRDeta[0].ToString();
                        razonSocialDestinatario = DRDeta[1].ToString();
                        dirDestinatario = DRDeta[2].ToString();
                        motivoTraslado = DRDeta[3].ToString();
                        docAduaneroUnico = DRDeta[4].ToString();
                        codEstabDestino = DRDeta[5].ToString();
                        ruta = DRDeta[6].ToString();
                        codDocSustento = DRDeta[7].ToString();
                        numDocSustento = DRDeta[8].ToString();
                        numAutDocSustento = DRDeta[9].ToString();
                        fechaEmisionDocSustento = Convert.ToDateTime(DRDeta[10]).ToString("dd/MM/yyyy");
                        idDestinatario = DRDeta[11].ToString();
                        asDestinatarios[0] = identificacionDestinatario; asDestinatarios[1] = razonSocialDestinatario; asDestinatarios[2] = dirDestinatario;
                        asDestinatarios[3] = motivoTraslado; asDestinatarios[4] = docAduaneroUnico; asDestinatarios[5] = codEstabDestino;
                        asDestinatarios[6] = ruta; asDestinatarios[7] = codDocSustento; asDestinatarios[8] = numDocSustento;
                        asDestinatarios[9] = numAutDocSustento; asDestinatarios[10] = fechaEmisionDocSustento; asDestinatarios[11] = idDestinatario;
                        arraylDestinatarios.Add(asDestinatarios);
                    }
                   
                }

                DB.Desconectar();
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }

        }

        public void detalles(string idUser)
        {
            var DB = new BasesDatos();
            try
            {

                idDestinatario = "";
                arraylDetalles = new ArrayList();
                DB.Conectar();
                DB.CrearComando(@"SELECT codigoPrincipal,codigoAuxiliar,descripcion,cantidad,precioUnitario,
                                     descuento,precioTotalSinImpuestos,idDetallesTemp,id_Destinatario
                              FROM DetallesTemp WITH (NOLOCK) WHERE id_Empleado=@empleado");
                DB.AsignarParametroCadena("@empleado", idUser);
                using (DbDataReader DRDeta = DB.EjecutarConsulta())
                {
                    while (DRDeta.Read())
                    {
                        asDetalles = new String[11];
                        codigoPrincipal = DRDeta[0].ToString();
                        codigoAuxiliar = DRDeta[1].ToString();
                        descripcion = DRDeta[2].ToString();
                        cantidad = DRDeta[3].ToString();
                        precioUnitario = DRDeta[4].ToString();
                        descuento = DRDeta[5].ToString();
                        precioTotalSinImpuesto = DRDeta[6].ToString();
                        idDetallesTemp = DRDeta[7].ToString();
                        idDestinatario = DRDeta[8].ToString();
                        asDetalles[0] = codigoPrincipal; asDetalles[1] = codigoAuxiliar; asDetalles[2] = descripcion;
                        asDetalles[3] = cantidad; asDetalles[4] = precioUnitario; asDetalles[5] = descuento;
                        asDetalles[6] = precioTotalSinImpuesto; asDetalles[7] = codigoAuxiliar; asDetalles[8] = descripcion;
                        asDetalles[9] = idDetallesTemp; asDetalles[10] = idDestinatario;
                        arraylDetalles.Add(asDetalles);
                    }
                }

                DB.Desconectar();
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }

        }

        public void impuestos(string idUser)
        {
            var DB = new BasesDatos();
            try
            {

                string codigoTemp = "";
                arraylImpuestosDetalles = new ArrayList();
                DB.Conectar();
                DB.CrearComando(@"SELECT codigo,codigoPorcentaje,baseImponible,tarifa,valor,
                                     codigoTemp,id_Empleado,tipo
                              FROM ImpuestosDetallesTemp WITH (NOLOCK)  WHERE id_Empleado=@empleado");
                DB.AsignarParametroCadena("@empleado", idUser);
                using (DbDataReader DRDeta = DB.EjecutarConsulta())
                {
                    while (DRDeta.Read())
                    {
                        asImpuestosDetalles = new String[8];
                        impuestoCodigo = DRDeta[0].ToString();
                        impuestoCodigoPorcentaje = DRDeta[1].ToString();
                        impuestoBaseImponible = DRDeta[2].ToString();
                        impuestoTarifa = DRDeta[3].ToString();
                        impuestoValor = DRDeta[4].ToString();
                        codigoTemp = DRDeta[5].ToString();
                        impuestotipoImpuesto = DRDeta[7].ToString();
                        asImpuestosDetalles[0] = impuestoCodigo; asImpuestosDetalles[1] = impuestoCodigoPorcentaje; asImpuestosDetalles[2] = impuestoBaseImponible;
                        asImpuestosDetalles[3] = impuestoTarifa; asImpuestosDetalles[4] = impuestoValor; asImpuestosDetalles[5] = codigoTemp;
                        asImpuestosDetalles[6] = impuestotipoImpuesto;
                        arraylImpuestosDetalles.Add(asImpuestosDetalles);
                    }
                   
                }

                DB.Desconectar();
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }

        }

								public void Pagos(string idUser)
								{
                                        var DB = new BasesDatos();
                                        try
            {

                string codigopag = "";
                arraylPagos = new ArrayList();
                DB.Conectar();
                DB.CrearComando(@"SELECT formaPagoTemp,plazoTemp,totalPagoTemp,unidadTiempoTemp
                                                          FROM pagoTemp WITH (NOLOCK)  WHERE id_Empleado=@empleado");
                DB.AsignarParametroCadena("@empleado", idUser);
                using (DbDataReader DRPA = DB.EjecutarConsulta())
                {
                    while (DRPA.Read())
                    {
                        asPagos = new String[5];
                        forma_pago = DRPA[0].ToString();
                        plazo = DRPA[1].ToString();
                        totalPago = DRPA[2].ToString();
                        unidadTiempo = DRPA[3].ToString();
                        asPagos[0] = forma_pago; asPagos[1] = plazo;
                        asPagos[2] = totalPago; asPagos[3] = unidadTiempo;
                        arraylPagos.Add(asPagos);
                    }
                 
                }

                DB.Desconectar();
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
                                        finally
                                        {
                                            DB.Desconectar();
                                        }

                                 }

        public void totalImpuestosRetenciones(string idUser)
        {
            var DB = new BasesDatos();
            try
            {

                arraylTotalImpuestosRetenciones = new ArrayList();
                DB.Conectar();
                DB.CrearComando(@"SELECT 
                                codigo,codigoRetencion,baseImponible,porcentajeRetener,valorRetenido,
                                codDocSustento,numDocSustento,fechaEmisionDocSustento,id_Empleado
                            FROM TotalConImpuestosTemp WITH (NOLOCK)   
                            WHERE id_Empleado=@empleado");
                DB.AsignarParametroCadena("@empleado", idUser);
                using (DbDataReader DRRet = DB.EjecutarConsulta())
                {
                    while (DRRet.Read())
                    {
                        asTotalImpuestosRetenciones = new String[8];
                        codigo = DRRet[0].ToString();
                        codigoRetencion = DRRet[1].ToString();
                        baseImponible = DRRet[2].ToString();
                        porcentajeRetener = DRRet[3].ToString();
                        valorRetenido = DRRet[4].ToString();
                        codDocSustento = DRRet[5].ToString();
                        numDocSustento = DRRet[6].ToString();
                        fechaEmisionDocSustento = Convert.ToDateTime(DRRet[7]).ToString("dd/MM/yyyy");
                        asTotalImpuestosRetenciones[0] = codigo; asTotalImpuestosRetenciones[1] = codigoRetencion; asTotalImpuestosRetenciones[2] = baseImponible;
                        asTotalImpuestosRetenciones[3] = porcentajeRetener; asTotalImpuestosRetenciones[4] = valorRetenido; asTotalImpuestosRetenciones[5] = codDocSustento;
                        asTotalImpuestosRetenciones[6] = numDocSustento; asTotalImpuestosRetenciones[7] = fechaEmisionDocSustento;
                        arraylTotalImpuestosRetenciones.Add(asTotalImpuestosRetenciones);
                    }
                }

                DB.Desconectar();
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }

        }

        public void totalImpuestos(string idUser)
        {

            var DB = new BasesDatos();
            try
            {
                arraylTotalImpuestos = new ArrayList();
                DB.Conectar();
                DB.CrearComando(@"SELECT codigo,codigoPorcentaje,sum(baseImponible) as baseImponible,tarifa,sum(valor) as valor,tipo
                              FROM ImpuestosDetallesTemp WITH (NOLOCK) WHERE id_Empleado=@empleado
                                GROUP BY codigo,codigoPorcentaje,tarifa,tipo");
                DB.AsignarParametroCadena("@empleado", idUser);
                using (DbDataReader DRDeta = DB.EjecutarConsulta())
                {
                    while (DRDeta.Read())
                    {
                        asTotalImpuestos = new String[8];
                        codigo = DRDeta[0].ToString();
                        codigoPorcentaje = DRDeta[1].ToString();
                        baseImponible = DRDeta[2].ToString();
                        tarifa = DRDeta[3].ToString();
                        valor = DRDeta[4].ToString();
                        tipoImpuesto = DRDeta[5].ToString();
                        asTotalImpuestos[0] = codigo; asTotalImpuestos[1] = codigoPorcentaje; asTotalImpuestos[2] = baseImponible;
                        asTotalImpuestos[3] = tarifa; asTotalImpuestos[4] = valor; asTotalImpuestos[5] = tipoImpuesto;
                        arraylTotalImpuestos.Add(asTotalImpuestos);
                    }
                }

                DB.Desconectar();
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }

        }

        public void totalMotivos(string idUser)
        {
            var DB = new BasesDatos();
            try
            {

                arraylTotalImpuestos = new ArrayList();
                arraylMotivos = new ArrayList();
                DB.Conectar();
                DB.CrearComando(@"SELECT codigo,codigoPorcentaje,baseImponible,tarifa,sum(valor),tipo
                              FROM MotivosDebitoTemp WITH (NOLOCK) WHERE id_Empleado=@empleado
                                GROUP BY codigo,codigoPorcentaje,baseImponible,tarifa,tipo");
                DB.AsignarParametroCadena("@empleado", idUser);
                using (DbDataReader DRDeta = DB.EjecutarConsulta())
                {
                    while (DRDeta.Read())
                    {
                        asTotalImpuestos = new String[8];
                        codigo = DRDeta[0].ToString();
                        codigoPorcentaje = DRDeta[1].ToString();
                        baseImponible = DRDeta[2].ToString();
                        tarifa = DRDeta[3].ToString();
                        valor = DRDeta[4].ToString();
                        tipoImpuesto = DRDeta[5].ToString();
                        asTotalImpuestos[0] = codigo; asTotalImpuestos[1] = codigoPorcentaje; asTotalImpuestos[2] = baseImponible;
                        asTotalImpuestos[3] = tarifa; asTotalImpuestos[4] = valor; asTotalImpuestos[5] = tipoImpuesto;
                        arraylTotalImpuestos.Add(asTotalImpuestos);
                    }
                }

                DB.Desconectar();
                DB.Conectar();
                DB.CrearComando(@"SELECT razon,baseImponible+valor  FROM MotivosDebitoTemp WITH (NOLOCK)  WHERE id_Empleado=@empleado");
                DB.AsignarParametroCadena("@empleado", idUser);
                using (DbDataReader DRMot = DB.EjecutarConsulta())
                {
                    while (DRMot.Read())
                    {
                        asMotivos = new String[8];
                        motivoRazon = DRMot[0].ToString();
                        motivoValor = DRMot[1].ToString();
                        asMotivos[0] = motivoRazon; asMotivos[1] = motivoValor;
                        arraylMotivos.Add(asMotivos);
                    }
                   
                }

                DB.Desconectar();
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }
        }

        public void detallesAdicionales(string idUser)
        {
            var DB = new BasesDatos();
            try
            {

                string codigoTemp = "";
                idDetallesTemp = "";
                arraylDetallesAdicionales = new ArrayList();

                DB.Conectar();
                DB.CrearComando(@"SELECT nombre,valor,codigoTemp,id_DetallesTemp
                              FROM DetallesAdicionalesTemp WITH (NOLOCK)  WHERE id_Empleado=@empleado");
                DB.AsignarParametroCadena("@empleado", idUser);
                using (DbDataReader DRDeta = DB.EjecutarConsulta())
                {
                    while (DRDeta.Read())
                    {
                        asDetallesAdicionales = new String[4];
                        detAdicionalNombre = DRDeta[0].ToString();
                        detAdicionalValor = DRDeta[1].ToString();
                        codigoTemp = DRDeta[2].ToString();
                        idDetallesTemp = DRDeta[3].ToString();
                        asDetallesAdicionales[0] = detAdicionalNombre; asDetallesAdicionales[1] = detAdicionalValor;
                        asDetallesAdicionales[2] = codigoTemp; asDetallesAdicionales[3] = idDetallesTemp;
                        arraylDetallesAdicionales.Add(asDetallesAdicionales);
                    }
                }

                DB.Desconectar();
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }
        }
        
        public void informacionAdicional(string idUser)
        {
            var DB = new BasesDatos();
            try
            {

                arrayInfoAdicionales = new ArrayList();
                DB.Conectar();
                DB.CrearComando(@"SELECT nombre,valor
                              FROM InfoAdicionalTemp WITH (NOLOCK)  WHERE id_Empleado=@empleado");
                DB.AsignarParametroCadena("@empleado", idUser);
                using (DbDataReader DRDeta = DB.EjecutarConsulta())
                {
                    while (DRDeta.Read())
                    {
                        asInfoAdicionales = new String[2];
                        infoAdicionalNombre = DRDeta[0].ToString();
                        infoAdicionalValor = DRDeta[1].ToString();
                        asInfoAdicionales[0] = infoAdicionalNombre; asInfoAdicionales[1] = infoAdicionalValor;
                        arrayInfoAdicionales.Add(asInfoAdicionales);
                    }
                }

                DB.Desconectar();
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }

        }
        /*      public void detalles(string codigoPrincipal, string codigoAuxiliar, string descripcion, string cantidad, string precioUnitario,
          string descuento, string precioTotalSinImpuesto, string codigoInterno, string codigoAdicional)
              {
                  this.codigoPrincipal = codigoPrincipal; this.codigoAuxiliar = codigoAuxiliar; this.descripcion = descripcion; this.cantidad = cantidad;
                  this.precioUnitario = precioUnitario; this.descuento = descuento; this.precioTotalSinImpuesto = precioTotalSinImpuesto;
                  this.codigoInterno = codigoInterno; this.codigoAdicional = codigoAdicional;
              }
              */


        public void infromacionAdicionalCima(string termino, string proforma, string pedido, string domicilio, string telefono)
        {
            this.termino = termino; this.proforma = proforma; this.pedido = pedido; this.domicilio = domicilio; this.telefono = telefono;
        }

        public string generarDocumento()
        {
            string linea = "";
            string codigoControl = codDoc + estab + ptoEmi + Convert.ToDateTime(fechaEmision).ToString("yyyyMMddHHmmss");
            try
            {
                StreamWriter sw = new StreamWriter(RutaTXT + codigoControl + ".txt");
                //Versión
                linea = "VE|" + tipoComprobante + "|" + idComprobante + "|" + version + "|";
                sw.WriteLine(linea);
                //Informacion Tributaria
                linea = "IT|" + ambiente + "|" + tipoEmision + "|" + razonSocial + "|" + nombreComercial + "|" + ruc + "|" + claveAcceso + "|";
                linea += codDoc + "|" + estab + "|" + ptoEmi + "|" + secuencial + "|" + dirMatriz + "|" + emails + "|";
                sw.WriteLine(linea);
                //Iformación del Comprobante
                linea = "IC|" + fechaEmision + "|" + dirEstablecimiento + "|" + contribuyenteEspecial + "|" + obligadoContabilidad + "|";
                linea += tipoIdentificacionComprador + "|" + guiaRemision + "|" + razonSocialComprador + "|" + identificacionComprador + "|";
                linea += moneda + "|" + rise + "|" + codDocModificado + "|" + numDocModificado + "|" + fechaEmisionDocSustentoNota + "|" + valorModificacion + "|";
                linea += motivo + "|" + periodoFiscal + "|" + dirPartida + "|" + razonSocialTransportista + "|" + tipoIdentificacionTransportista + "|" + rucTransportista + "|";
																linea += rise + "|" + contribuyenteEspecial + "|" + fechaIniTransporte + "|" + fechaFinTransporte + "|" + placa + "|" + direccionComprador + "|";
                sw.WriteLine(linea);
                //Totales
                linea = "T|" + subtotal12 + "|" + subtotal0 + "|" + subtotalNoSujeto + "|" + totalSinImpuestos + "|" + totalDescuento + "|" + ICE + "|" + IVA12 + "|";
                linea += importeTotal + "|" + propina + "|" + importeAPagar + "|";
                sw.WriteLine(linea);
                //Total de Impuestos
                foreach (String[] ti in arraylTotalImpuestos)
                {         //"IT |" + Codigo + "|" + CodigoPorcentaje + "|" + Tarifa + "|" + BaseImponible + "|" + Valor + "|"+Impuestos +"|";
                    linea = "TI|" + ti[0] + "|" + ti[1] + "|" + ti[2] + "|" + ti[3] + "|" + ti[4] + "|" + ti[5] + "|";
                    sw.WriteLine(linea);
                }
																//Pagos
																foreach (String[] pa in arraylPagos)
																{      
																				linea = "PA|" + pa[0] + "|" + pa[1] + "|" + pa[2] + "|" + pa[3] + "|";
																				sw.WriteLine(linea);
																}
                //Total de Impuestos Retenidos
                foreach (String[] tir in arraylTotalImpuestosRetenciones)
                {
                    //TIR | codigo | codigoRetencion | baseImponible | porcentajeRetener | valorRetenido | codDocSustento | numDocSustento | fechaEmisionDocSustento |
                    linea = "TIR|" + tir[0] + "|" + tir[1] + "|" + tir[2] + "|" + tir[3] + "|" + tir[4] + "|" + tir[5] + "|" + tir[6] + "|" + tir[7] + "|";
                    sw.WriteLine(linea);
                }

                //Destinatarios
                foreach (String[] dest in arraylDestinatarios)
                {   //identificacionDestinatario|razonSocialDestinatario|dirDestinatario |motivoTraslado |docAduaneroUnico|codEstabDestino|
                    //ruta|codDocSustento|numDocSustento|numAutDocSustento |fechaEmisionDocSustento |idDestinatario|
                    linea = "DEST|" + dest[0] + "|" + dest[1] + "|" + dest[2] + "|" + dest[3] + "|" + dest[4] + "|" + dest[5] + "|";
                    linea += dest[6] + "|" + dest[7] + "|" + dest[8] + "|" + dest[9] + "|" + dest[10] + "|";// +dest[11] + "|";
                    sw.WriteLine(linea);
                    //Detalles destinatario
                    foreach (String[] d in arraylDetalles)
                    {         //"IM |" + impuestoCodigo + "|" + impuestoCodigoPorcentaje + "|" + impuestoTarifa + "|" + impuestoBaseImponible + "|" + impuestoValor + "|"+tipoImpuestos +"|"+idDestinatario+"|";
                        if (dest[11].Equals(d[10]))
                        {
                            linea = "DE|" + d[0] + "|" + d[1] + "|" + d[2] + "|" + d[3] + "|" + d[4] + "|" + d[6] + "|" + d[7] + "|";
                            sw.WriteLine(linea);
                            //Detalles Adicionales
                            foreach (String[] da in arraylDetallesAdicionales)
                            {         //"DA|" + detAdicionalNombre + "|" + detAdicionalValor+ "|" + idDetallesTemp+"|";
                                if (d[9].Equals(da[3]))
                                {
                                    linea = "DA|" + da[0] + "|" + da[1] + "|";
                                    sw.WriteLine(linea);
                                }
                            } // fin detalles adicionales.
                        }
                    } // fin detalles
                    arraylDetalles = new ArrayList();

                }//fin destinatarios

                //Motivos
                foreach (String[] am in arraylMotivos)
                {         //MO|motivoRazon|motivoValor|
                    linea = "MO|" + am[0] + "|" + am[1]+"|";
                    sw.WriteLine(linea);
                }
                //Detalles
                foreach (String[] d in arraylDetalles)
                {         //"DE|" + codigoPrincipal + "|" + codigoAuxiliar + "|" + descripcion + "|" + cantidad + "|" + precioUnitario + "|" + descuento + "|" + precioTotalSinImpuesto + "|";
                    linea = "DE|" + d[0] + "|" + d[1] + "|" + d[2] + "|" + d[3] + "|" + d[4] + "|" + d[5] + "|" + d[6] + "|";
                    sw.WriteLine(linea);
                    //Impuestos por Detalles
                    foreach (String[] id in arraylImpuestosDetalles)
                    {         //"IM |" + impuestoCodigo + "|" + impuestoCodigoPorcentaje + "|" + impuestoTarifa + "|" + impuestoBaseImponible + "|" + impuestoValor + "|"+tipoImpuestos +"|";
                        if (d[0].Equals(id[5]))
                        {
                            linea = "IM|" + id[0] + "|" + id[1] + "|" + id[2] + "|" + id[3] + "|" + id[4] + "|" + id[6] + "|";
                            sw.WriteLine(linea);
                        }
                    }
                    //Detalles Adicionales
                    foreach (String[] da in arraylDetallesAdicionales)
                    {         //"DA|" + detAdicionalNombre + "|" + detAdicionalValor;
                        if (d[0].Equals(da[2]))
                        {
                            linea = "DA|" + da[0] + "|" + da[1] + "|";
                            sw.WriteLine(linea);
                        }
                    }
                }
                //Información Adicional
                foreach (String[] ia in arrayInfoAdicionales)
                {       //IA |" + infoAdicionalNombre + "|" + infoAdicionalValor + "|";
                    linea = "IA|" + ia[0] + "|" + ia[1] + "|";
                    sw.WriteLine(linea);
                }
                //Información Adicional CIMA
                linea = "CIMAIT|" + this.termino + "|" + this.proforma + "|" + this.pedido + "|" + this.domicilio +"|" + this.telefono +"|";
                sw.WriteLine(linea);

                //Cerrar Archivo.
                sw.Close();
                return codigoControl;
            }
            catch (Exception ex)
            {
                return "";
                log.mensajesLog("ES004", "", ex.Message, "", codigoControl);
            }
            finally
            {

            }
        }

        private void mensajeEmail(string asunto, string codigo, string mensaje2, string emails,string rucReglas,string rucReceptor,string adjuntar)
        {
            var DB = new BasesDatos();
            try
            {

                DB.Conectar();
                DB.CrearComando("select emailsRegla from EmailsReglas WITH (NOLOCK)   where Receptor=@rfcrec and estadoRegla=1");
                DB.AsignarParametroCadena("@rfcrec", rucReglas);
                using (DbDataReader DR3 = DB.EjecutarConsulta())
                {
                    if (DR3.Read())
                    {
                        emails = emails.Trim(',') + "," + DR3[0].ToString().Trim(',') + "";
                    }
                   
                }

                DB.Desconectar();
                string[] array;
                string mensaje;
                array = log.PA_mensajes(codigo);
                if (emails.Length > 5)
                {
                    em = new EnviarMail();
                    em.servidorSTMP(servidor, puerto, ssl, emailCredencial, passCredencial);
                    if (String.IsNullOrEmpty(asunto))
                    {
                        asunto = "Información sobre la factura que enviaste con Folio. " + secuencial + ". ";
                    }
                    mensaje = @"Buen dia! <br>
                            La factura que enviaste con fecha: " + secuencial + @"<br>
                            y  folio " + secuencial + ".";
                    mensaje += "<br>" + array[0];
                    mensaje += "<br>" + array[1];
                    mensaje += "<br><br><br>Saludos cordiales. ";
                    mensaje += "<br>Impulsora ";
                    mensaje += "<br>Servicio proporcionado por DataExpress";
                    em.llenarEmail(emailEnviar, emails.Trim(','), "", "", asunto, mensaje);
                    if (adjuntar.Equals("1"))
                    {
                        em.adjuntar(RutaDOC + rucReceptor + "_" + secuencial + ".xml");
                    }
                    if (adjuntar.Equals("2"))
                    {
                        em.adjuntar(RutaDOC + rucReceptor + "_" + secuencial + ".pdf");
                    }
                    if (adjuntar.Equals("3"))
                    {
                        em.adjuntar(RutaDOC + rucReceptor + "_" + secuencial + ".xml");
                        em.adjuntar(RutaDOC + rucReceptor + "_" + secuencial + ".pdf");
                    }
                    try
                    {
                        em.enviarEmail();
                        //msj = msj + "E-mail enviado";
                    }
                    catch (System.Net.Mail.SmtpException ex)
                    {
                        log.mensajesLog("EM001", msj, ex.Message, "", secuencial);
                    }
                }
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }

        }

       
        

    }
}
