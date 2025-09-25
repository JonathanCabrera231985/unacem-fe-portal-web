using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using Control;
using clibLogger;

namespace DataExpressWeb
{
    public partial class crearFactura : System.Web.UI.Page
    {
        private BasesDatos DBTimer = new BasesDatos();
        private Spool spoolComprobante;
        private string idUser = "";
        private string rucEmisor = "";
        private string sucursalUser = "";
        private int count = 0;
        private string msj = "";
        string codigoControl = "";
        private string formatCero = "0.00";
        private Log log = new Log();
        private int countTimer = 0;
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
                lMsjImpuestos.Text = "";
                if (Session["idUser"] != null)
                {
                    Object dad = Convert.ToString(Session["codigoTemp"]);
                    idUser = Session["idUser"].ToString();
                    sucursalUser = Session["sucursalUser"].ToString();
                    if (!Page.IsPostBack)
                    {
                        Session["codigoTemp"] = codigoPrincipal();
                        DB.Conectar();
                        DB.CrearComando(@"SELECT rfcemisor from ParametrosSistema WITH (NOLOCK) ");
                        using (DbDataReader DRSum = DB.EjecutarConsulta())
                        {
                            if (DRSum.Read())
                            {
                                rucEmisor = DRSum[0].ToString();
                            }
                        }
                        DB.Desconectar();
                        DB.Conectar();
                        DB.CrearComando(@"SELECT domicilio from Sucursales WITH (NOLOCK)  where idSucursal = @idSucursal ");
                        DB.AsignarParametroCadena("@idSucursal", sucursalUser);
                        using (DbDataReader DRSuc = DB.EjecutarConsulta())
                        {
                            if (DRSuc.Read())
                            {
                                tbDirEstablecimiento.Text = DRSuc[0].ToString();
                            }
                        }
                        DB.Desconectar();
                        tbFechaEmision.Text = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    llenarTotales();
                    llenarlista(rucEmisor, "emi");
                }
                tbCodigoP.Text = codigoPrincipal();
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }
        }
        protected void FinishButton_Click(object sender, EventArgs e)
        {
            codigoControl = "";
            lMsjDocumento.Text = "";
            string obligadocontabilidad = "";
            string guiaRemision;
            codigoControl = "";
            if (String.IsNullOrEmpty(ddlSucursal.SelectedValue))
            {
                ddlSucursal.Items.Clear();
                ddlSucursal.DataBind();
            }
            if (String.IsNullOrEmpty(ddlEmision.SelectedValue))
            {
                ddlEmision.Items.Clear();
                ddlEmision.DataBind();
            }
            if (String.IsNullOrEmpty(ddlAmbiente.SelectedValue))
            {
                ddlAmbiente.Items.Clear();
                ddlAmbiente.DataBind();
            }
            if (String.IsNullOrEmpty(ddlComprobante.SelectedValue))
            {
                ddlComprobante.Items.Clear();
                ddlComprobante.DataBind();
            }
            if (String.IsNullOrEmpty(ddlPtoEmi.SelectedValue))
            {
                ddlPtoEmi.Items.Clear();
                ddlPtoEmi.DataBind();
            }
            if (cbObligado.Checked)
            {
                obligadocontabilidad = "SI";
            }
            else
            {
                obligadocontabilidad = "NO";
            }
            try
            {
                spoolComprobante = new Spool();
                spoolComprobante.xmlComprobante();
                spoolComprobante.InformacionTributaria(ddlAmbiente.SelectedValue, ddlEmision.SelectedValue, tbRazonSocial.Text, tbNombreComercial.Text,
                 tbRuc.Text, "", ddlComprobante.SelectedValue, ddlSucursal.SelectedValue, ddlPtoEmi.SelectedValue, "", tbDirMatriz.Text, tbEmail.Text);
                spoolComprobante.infromacionDocumentoFactura(tbFechaEmision.Text, tbDirEstablecimiento.Text, tbContribuyenteEspecial.Text, obligadocontabilidad,
ddlTipoIdentificacion.SelectedValue, "", tbRazonSocialComprador.Text, tbIdentificacionComprador.Text, tbMoneda.Text,
                                "", "", "", "", "", "", "", "", "", "", "", formatCero, "", txt_dir_cli.Text);
                spoolComprobante.cantidades(tbSubtotal12.Text, tbSubtotal0.Text, tbSubtotalNoSujeto.Text, tbTotalSinImpuestos.Text,
                    tbTotalDescuento.Text, tbICE.Text, tbIVA12.Text, tbImporteTotal.Text, tbPropinas.Text, tbImporteaPagar.Text);
                spoolComprobante.totalImpuestos(idUser);
                spoolComprobante.detalles(idUser);
                spoolComprobante.impuestos(idUser);
                spoolComprobante.Pagos(idUser);
                spoolComprobante.detallesAdicionales(idUser);
                spoolComprobante.informacionAdicional(idUser);
                //CIMA
                spoolComprobante.infromacionAdicionalCima(this.txt_termino.Text, this.txt_proforma.Text, this.txt_pedido.Text, this.txt_dir_cli.Text, this.txt_fono.Text);

                codigoControl = spoolComprobante.generarDocumento();
                if (!String.IsNullOrEmpty(codigoControl))
                {
                    Session["codigoControl"] = codigoControl;
                    Response.Redirect("~/Procesando.aspx", false);
                }
                else
                {
                    lMsjDocumento.Text = "No se pudo crear el Comprobante.";
                }
            }
            catch (Exception ex)
            {
                msj = log.PA_mensajes("EM011")[0];
                clsLogger.Graba_Log_Error(ex.Message);
                lMsjDocumento.Text = msj;
                log.mensajesLog("EM011", "", ex.Message, "Crear Factura", "");
            }
        }

        protected void tbPU_TextChanged(object sender, EventArgs e)
        {
            importeConcepto();
        }

        protected void tbDescuento_TextChanged(object sender, EventArgs e)
        {
            importeConcepto();
        }

        private string codigoPrincipal()
        {
            var DB = new BasesDatos();
            int aux = 0;
            string code = "";
            try
            {
                DB.Conectar();
                DB.CrearComando(@"SELECT count(id_Empleado) FROM DetallesTemp WITH (NOLOCK) WHERE id_Empleado=@id_Empleado");
                DB.AsignarParametroCadena("@id_Empleado", idUser);
                using (DbDataReader DRDT = DB.EjecutarConsulta())
                {
                    if (DRDT.Read())
                    {
                        aux = Convert.ToInt32(DRDT[0]) + 1;
                    }
                }
                DB.Desconectar();
                if (aux.ToString().Length == 1) { code = "00000000" + aux.ToString(); }
                if (aux.ToString().Length == 2) { code = "0000000" + aux.ToString(); }
                if (aux.ToString().Length == 3) { code = "000000" + aux.ToString(); }
                if (aux.ToString().Length == 4) { code = "00000" + aux.ToString(); }
                if (aux.ToString().Length == 5) { code = "0000" + aux.ToString(); }
                if (aux.ToString().Length == 6) { code = "000" + aux.ToString(); }
                if (aux.ToString().Length == 7) { code = "00" + aux.ToString(); }
                if (aux.ToString().Length == 8) { code = "0" + aux.ToString(); }
                if (aux.ToString().Length == 9) { code = aux.ToString(); }
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
            return code;
        }

        protected void bAgregarImpuesto_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            string valImpuesto = "";
            try
            {
                if (Convert.ToDecimal(tbImporteConcepto.Text) > 0)
                {
                    if (tbTarifa.Text != "No Sujeto")
                    {
                        valImpuesto = tbTarifa.Text;
                        if (Convert.ToInt32(tbValor.Text.Length) > 0)
                        {
                            DB.Conectar();
                            DB.CrearComando(@"insert into ImpuestosDetallesTemp 
                           (codigo,codigoPorcentaje,baseImponible,tarifa,valor,id_DetallesTemp,codigoTemp,id_Empleado,tipo) 
                           values 
                           (@codigo,@codigoPorcentaje,@baseImponible,@tarifa,@valor,@id_DetallesTemp,@codigoTemp,@id_Empleado,@tipo)");
                            DB.AsignarParametroCadena("@codigo", "2");
                            DB.AsignarParametroCadena("@codigoPorcentaje", tbCodigoIDP.Text);
                            DB.AsignarParametroCadena("@baseImponible", tbBaseImponible.Text);
                            DB.AsignarParametroCadena("@tarifa", valImpuesto);
                            DB.AsignarParametroCadena("@valor", tbValor.Text);
                            DB.AsignarParametroCadena("@id_DetallesTemp", "");
                            DB.AsignarParametroCadena("@codigoTemp", tbCodigoP.Text);
                            DB.AsignarParametroCadena("@id_Empleado", idUser);
                            DB.AsignarParametroCadena("@tipo", ddlTipoImpuesto.SelectedItem.ToString());
                            DB.EjecutarConsulta1();
                            DB.Desconectar();
                            SqlDataImpuestosConceptos.SelectParameters[0].DefaultValue = Session["idUser"].ToString();
                            SqlDataImpuestosConceptos.SelectParameters[1].DefaultValue = tbCodigoP.Text;
                            SqlDataImpuestosConceptos.DataBind();
                            gvImpuestosDetalles.DataBind();
                        }
                    }
                    else
                    {
                        valImpuesto = "0";
                        if (tbValor.Text.Length > 0)
                        {
                            DB.Conectar();
                            DB.CrearComando(@"insert into ImpuestosDetallesTemp 
                           (codigo,codigoPorcentaje,baseImponible,valor,id_DetallesTemp,codigoTemp,id_Empleado,tipo) 
                           values 
                           (@codigo,@codigoPorcentaje,@baseImponible,@valor,@id_DetallesTemp,@codigoTemp,@id_Empleado,@tipo)");
                            DB.AsignarParametroCadena("@codigo", tbCodigoID.Text);
                            DB.AsignarParametroCadena("@codigoPorcentaje", tbCodigoIDP.Text);
                            DB.AsignarParametroCadena("@baseImponible", tbBaseImponible.Text);
                            DB.AsignarParametroCadena("@valor", valImpuesto);
                            DB.AsignarParametroCadena("@id_DetallesTemp", "");
                            DB.AsignarParametroCadena("@codigoTemp", tbCodigoP.Text);
                            DB.AsignarParametroCadena("@id_Empleado", idUser);
                            DB.AsignarParametroCadena("@tipo", ddlTipoImpuesto.SelectedItem.ToString());
                            DB.EjecutarConsulta1();
                            DB.Desconectar();
                            SqlDataImpuestosConceptos.SelectParameters[0].DefaultValue = Session["idUser"].ToString();
                            SqlDataImpuestosConceptos.SelectParameters[1].DefaultValue = tbCodigoP.Text;
                            SqlDataImpuestosConceptos.DataBind();
                            gvImpuestosDetalles.DataBind();
                        }
                    }
                    tbCodigoID.Text = "";
                    tbCodigoIDP.Text = "";
                    tbTarifa.Text = formatCero;
                    tbValor.Text = formatCero;
                    bAgregarConcepto_Click();
                }
                else
                {
                    lMsjImpuestos.Text = "Debe corregir Subtotal";
                }
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }

        }

        protected void bAgregarConcepto_Click()
        {
            var DB = new BasesDatos();
            int countImp = 0;
            Page.Validate("Deta");
            try
            {
                if (Page.IsValid)
                {
                    DB.Conectar();
                    DB.CrearComando(@"SELECT count(*) FROM ImpuestosDetallesTemp WITH (NOLOCK)  WHERE id_Empleado=@empleado and codigoTemp=@codigoPrincipal");
                    DB.AsignarParametroCadena("@empleado", idUser);
                    DB.AsignarParametroCadena("@codigoPrincipal", tbCodigoP.Text);
                    using (DbDataReader DRCont = DB.EjecutarConsulta())
                    {
                        if (DRCont.Read())
                        {
                            countImp = Convert.ToInt32(DRCont[0]);
                        }
                    }

                    DB.Desconectar();
                    if (countImp > 0)
                    {
                        DB.Conectar();
                        DB.CrearComando(@"insert into DetallesTemp 
                           (codigoPrincipal,codigoAuxiliar,descripcion,cantidad,precioUnitario,
                            descuento,precioTotalSinImpuestos,id_Empleado) 
                           values 
                           (@codigoPrincipal,@codigoAuxiliar,@descripcion,@cantidad,@precioUnitario,
                            @descuento,@precioTotalSinImpuestos,@id_Empleado)");
                        DB.AsignarParametroCadena("@codigoPrincipal", tbCodigoP.Text);
                        DB.AsignarParametroCadena("@codigoAuxiliar", "");//tbCodigoA.Text);
                        DB.AsignarParametroCadena("@descripcion", tbCodigoA0.Text);
                        DB.AsignarParametroCadena("@cantidad", tbCantidad.Text);
                        DB.AsignarParametroCadena("@precioUnitario", tbPU.Text);
                        DB.AsignarParametroCadena("@descuento", tbDescuento.Text);
                        DB.AsignarParametroCadena("@precioTotalSinImpuestos", tbImporteConcepto.Text);
                        DB.AsignarParametroCadena("@id_Empleado", idUser);
                        DB.EjecutarConsulta1();
                        DB.Desconectar();
                        SqlDataDetalles.SelectParameters[0].DefaultValue = Session["idUser"].ToString();
                        SqlDataDetalles.DataBind();
                        gvDetalles.DataBind();
                        SqlDataImpuestosConceptos.SelectParameters[0].DefaultValue = Session["idUser"].ToString();
                        SqlDataImpuestosConceptos.SelectParameters[1].DefaultValue = Session["idUser"].ToString();
                        SqlDataImpuestosConceptos.DataBind();
                        gvImpuestosDetalles.DataBind();
                        tbCantidad.Text = String.Format("{0:f}", 1);
                        tbCodigoP.Text = codigoPrincipal();
                        tbCodigoA0.Text = "";
                        tbPU.Text = "0";
                        tbDescuento.Text = "0";
                        tbImporteConcepto.Text = "0";
                        tbBaseImponible.Text = "0";
                    }
                    else
                    {
                        lMsjImpuestos.Text = "Necesitas Agregar al menos un Impuesto.";
                    }
                }
                codigoPrincipal();
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }
        }

        protected void bAgregarCA_Click(object sender, EventArgs e)
        {

        }
        protected void tbTarifa_TextChanged(object sender, EventArgs e)
        {
            calcularImpuesto();
        }

        private void llenarlista(String ruc, String tipo)
        {
            var DB = new BasesDatos();
            String sql = "";
            if (tipo == "emi") { sql = @"SELECT * FROM EMISOR WITH (NOLOCK) WHERE RFCEMI=@ruc"; }
            if (tipo == "rec") { sql = @"SELECT * FROM RECEPTOR WITH (NOLOCK)  WHERE RFCREC=@ruc"; }
            try
            {
                DB.Conectar();
                DB.CrearComando(sql);
                DB.AsignarParametroCadena("@ruc", ruc);
                using (DbDataReader DRSum = DB.EjecutarConsulta())
                {
                    while (DRSum.Read())
                    {
                        if (tipo == "emi")
                        {
                            tbRuc.Text = DRSum[1].ToString();
                            tbRazonSocial.Text = DRSum[2].ToString();
                            tbNombreComercial.Text = DRSum[3].ToString();
                            tbDirMatriz.Text = DRSum[4].ToString();
                        }
                        if (tipo == "rec")
                        {
                            tbIdentificacionComprador.Text = DRSum[1].ToString();
                            tbRazonSocialComprador.Text = DRSum[2].ToString();
                            ddlTipoIdentificacion.SelectedValue = DRSum[5].ToString();
                            tbEmail.Text = DRSum[6].ToString();
                            txt_dir_cli.Text = DRSum[7].ToString();
                            txt_fono.Text = DRSum[8].ToString();
                        }
                    }
                }

                DB.Desconectar();

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

        protected void tbIdentificacionComprador_TextChanged(object sender, EventArgs e)
        {
            if (tbIdentificacionComprador.Text.Length > 0)
            {
                llenarlista(tbIdentificacionComprador.Text, "rec");
            }
        }

        public void llenarTotales()
        {
            var DB = new BasesDatos();
            try
            {
                tbSubtotal0.Text = formatCero;
                tbSubtotal12.Text = formatCero;
                tbSubtotalNoSujeto.Text = formatCero;
                tbTotalSinImpuestos.Text = formatCero;
                tbTotalDescuento.Text = formatCero;
                tbICE.Text = formatCero;
                tbIVA12.Text = formatCero;
                tbImporteTotal.Text = formatCero;
                if (Convert.ToDecimal(tbPropinas.Text) == 0)
                {
                    tbPropinas.Text = formatCero;
                }
                tbImporteaPagar.Text = formatCero;
                DB.Conectar();
                DB.CrearComando(@"select ISNULL(sum(precioTotalSinImpuestos),0),ISNULL(sum(descuento),0) from DetallesTemp WITH (NOLOCK) WHERE id_Empleado=@empleado");
                DB.AsignarParametroCadena("@empleado", idUser);
                using (DbDataReader DRSum = DB.EjecutarConsulta())
                {
                    if (DRSum.Read())
                    {
                        tbTotalSinImpuestos.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum[0].ToString()));
                        tbTotalDescuento.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum[1].ToString()));
                    }
                }
                DB.Desconectar();
                DB.Conectar();
                DB.CrearComando(@"SELECT ISNULL(sum(ImpuestosDetallesTemp.valor),0),ISNULL(sum(DetallesTemp.precioTotalSinImpuestos),0)
                              FROM  ImpuestosDetallesTemp WITH (NOLOCK) INNER JOIN
                                    DetallesTemp WITH (NOLOCK) ON ImpuestosDetallesTemp.codigoTemp = DetallesTemp.codigoPrincipal
                              WHERE   (CAST(tarifa AS INT)='14' OR CAST(tarifa AS INT)='12' ) AND ImpuestosDetallesTemp.tipo='IVA' AND ImpuestosDetallesTemp.id_Empleado=@empleado");
                DB.AsignarParametroCadena("@empleado", idUser);
                using (DbDataReader DRSum5 = DB.EjecutarConsulta())
                {
                    if (DRSum5.Read())
                    {
                        tbIVA12.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum5[0].ToString()));
                        tbSubtotal12.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum5[1].ToString()));

                    }
                }
                DB.Desconectar();
                DB.Conectar();
                DB.CrearComando(@"SELECT ISNULL(sum(DetallesTemp.precioTotalSinImpuestos),0)
                              FROM  ImpuestosDetallesTemp WITH (NOLOCK)  INNER JOIN
                                    DetallesTemp WITH (NOLOCK) ON ImpuestosDetallesTemp.codigoTemp = DetallesTemp.codigoPrincipal
                              WHERE   CAST(tarifa AS INT)='0' AND ImpuestosDetallesTemp.tipo='IVA' AND ImpuestosDetallesTemp.id_Empleado=@empleado");
                DB.AsignarParametroCadena("@empleado", idUser);
                using (DbDataReader DRSum2 = DB.EjecutarConsulta())
                {
                    if (DRSum2.Read())
                    {
                        tbSubtotal0.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum2[0].ToString()));
                    }
                }
                DB.Desconectar();
                DB.Conectar();
                DB.CrearComando(@"SELECT ISNULL(sum(DetallesTemp.precioTotalSinImpuestos),0)  
                              FROM  ImpuestosDetallesTemp WITH (NOLOCK)  INNER JOIN
                                    DetallesTemp WITH (NOLOCK)  ON ImpuestosDetallesTemp.codigoTemp = DetallesTemp.codigoPrincipal
                              WHERE   tarifa IS NULL AND ImpuestosDetallesTemp.tipo='IVA' AND  ImpuestosDetallesTemp.id_Empleado=@empleado");
                DB.AsignarParametroCadena("@empleado", idUser);
                using (DbDataReader DRSum3 = DB.EjecutarConsulta())
                {
                    if (DRSum3.Read())
                    {
                        tbSubtotalNoSujeto.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum3[0].ToString()));
                    }
                }
                DB.Desconectar();
                DB.Conectar();
                DB.CrearComando(@"SELECT ISNULL(sum(ImpuestosDetallesTemp.valor),0) 
                              FROM  ImpuestosDetallesTemp WITH (NOLOCK)  INNER JOIN
                                    DetallesTemp WITH (NOLOCK) ON ImpuestosDetallesTemp.codigoTemp = DetallesTemp.codigoPrincipal
                              WHERE   ImpuestosDetallesTemp.tipo='ICE' AND ImpuestosDetallesTemp.id_Empleado=@empleado");
                DB.AsignarParametroCadena("@empleado", idUser);
                using (DbDataReader DRSum4 = DB.EjecutarConsulta())
                {
                    if (DRSum4.Read())
                    {
                        tbICE.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum4[0].ToString()));
                    }
                }
                DB.Desconectar();
                tbImporteTotal.Text = String.Format("{0:f}", Convert.ToDecimal(tbTotalSinImpuestos.Text) - Convert.ToDecimal(tbDescuento.Text) +
                                       Convert.ToDecimal(tbICE.Text) + Convert.ToDecimal(tbIVA12.Text));
                tbImporteaPagar.Text = String.Format("{0:f}", Convert.ToDecimal(tbImporteTotal.Text) + Convert.ToDecimal(tbPropinas.Text));
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

        protected void ddlTipoImpuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoImpuesto.SelectedValue == "3")
            {
                ddlTasaIVA.Visible = false;
                tbTarifa.Visible = true;
                RegularExpressionValidator2.Visible = true;
                RequiredFieldValidator16.Visible = true;
                tbCodigoIDP.Text = "";
                tbTarifa.Text = formatCero;
            }
            else
            {
                ddlTasaIVA.Visible = true;
                tbTarifa.Visible = false;
                RegularExpressionValidator2.Visible = false;
                RequiredFieldValidator16.Visible = false;
            }
            importeConcepto();
        }

        private void calcularImpuesto()
        {
            decimal tarifa = 0;
            if (tbTarifa.Text != "No Sujeto")
            {
                try
                {
                    tarifa = Convert.ToDecimal(cc(tbTarifa.Text));
                }
                catch (Exception ex)
                {
                    lMsjImpuestos.Text = "No es un numero Válido.";
                }
                tbValor.Text = String.Format("{0:f}", (tarifa * Convert.ToDecimal(tbBaseImponible.Text)) / 100);
            }
            else
            {
                tbValor.Text = "0";
            }
        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            importeConcepto();
        }

        private void importeConcepto()
        {
            lMsjImpuestos.Text = "";
            tbImporteConcepto.Text = String.Format("{0:f}", (Convert.ToDecimal(cc(tbPU.Text)) * Convert.ToDecimal(cc(tbCantidad.Text))) - Convert.ToDecimal(cc(tbDescuento.Text)));
            tbBaseImponible.Text = tbImporteConcepto.Text;
            tbCodigoID.Text = ddlTipoImpuesto.SelectedValue;
            tbTarifa.Text = ddlTasaIVA.SelectedValue;
            tbTarifa.ReadOnly = false;
            if (tbTarifa.Text == "14" || tbTarifa.Text == "14,00" || tbTarifa.Text == "14.00")
            {
                tbCodigoIDP.Text = "3";
            }
            if (tbTarifa.Text == "12" || tbTarifa.Text == "12,00" || tbTarifa.Text == "12.00")
            {
                tbCodigoIDP.Text = "2";
            }
            if (tbTarifa.Text == "0" || tbTarifa.Text == "0.00" || tbTarifa.Text == "0,00")
            {
                tbCodigoIDP.Text = "0";
            }
            if (tbTarifa.Text == "No Sujeto")
            {
                tbCodigoIDP.Text = "6";
            }
            if (ddlTipoImpuesto.SelectedValue == "3")
            {
                tbCodigoIDP.Text = "";
                tbTarifa.Text = formatCero;
                tbValor.Text = formatCero;
            }
            else
            {
                calcularImpuesto();
            }
        }

        protected void StepNextButton_Click(object sender, WizardNavigationEventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                switch (Wizard1.ActiveStepIndex)
                {
                    case 1:
                        DB.Conectar();
                        DB.CrearComando(@"SELECT count(*) FROM DetallesTemp WITH (NOLOCK)  WHERE id_Empleado=@empleado");
                        DB.AsignarParametroCadena("@empleado", idUser);
                        using (DbDataReader DRCont = DB.EjecutarConsulta())
                        {
                            if (DRCont.Read())
                            {
                                count = Convert.ToInt32(DRCont[0]);
                            }
                            DRCont.Dispose();
                        }
                        DB.Desconectar();
                        if (count < 1) { lMsjDetalles.Text = "Necesitas Agregar al menos un Detalle."; e.Cancel = true; } else { lMsjDetalles.Text = ""; }
                        break;
                    case 2:
                        Page.Validate("Receptor");
                        if (!Page.IsValid)
                        {
                            e.Cancel = true;
                        }
                        break;
                    default:
                        break;
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

        private string cc(string numero)
        {
            if (String.IsNullOrEmpty(numero))
            {
                numero = formatCero;
            }
            return numero;
        }
        protected void Wizard1_ActiveStepChanged(object sender, EventArgs e)
        {
        }

        protected void tbPropinas_TextChanged(object sender, EventArgs e)
        {
            llenarTotales();
        }

        protected void tbCantidad_TextChanged(object sender, EventArgs e)
        {
            importeConcepto();
        }

        protected void FinishButton_Click(object sender, WizardNavigationEventArgs e)//(object sender, EventArgs e)
        {
            Page.Validate("Receptor");
            if (!Page.IsValid)
            {
                e.Cancel = true;
            }
            else
            {
                codigoControl = "";
            }
        }
        protected void gvDetalles_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gvDetalles.SelectedRow;
            String Script = "<script language='javascript'>" +
                    " var url = \"../ComproDetalleAdicional.aspx?Cod_Prod=" + Control.Encriptaciondes.ClaseEncriptacion.Instance.Encrypt(gvDetalles.SelectedValue.ToString()) + "\";" +
                    "window.open(url, '', 'width=500, height=450 , left=300,  top=220, status=0');" +
                    "</script>";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ajax", Script, false);
        }
        protected void Btn_pagos_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                string valImpuesto = "";
                int countImppa = 0;
                string codep = "";
                Page.Validate("Deta");
                if (Convert.ToDecimal(tbtotal_pago.Text) > 0)
                {
                    int auxp = 0, contp = 0;
                    string code = "";
                    DB.Conectar();
                    DB.CrearComando(@"SELECT count(*)+1 as cod FROM pagoTemp WITH (NOLOCK)  WHERE id_Empleado=@id_Empleado");
                    DB.AsignarParametroCadena("@id_Empleado", idUser);
                    using (DbDataReader DRDT1P = DB.EjecutarConsulta())
                    {
                        if (DRDT1P.Read())
                        {
                            contp = Convert.ToInt32(DRDT1P["cod"].ToString());
                        }
                    }
                    DB.Desconectar();
                    if (contp == 1)
                    {
                        auxp = 1;
                    }
                    else
                    {
                        DB.Conectar();
                        DB.CrearComando(@"SELECT max(codigoTempp)+1 FROM pagoTemp WITH (NOLOCK) WHERE id_Empleado=@empleado");
                        DB.AsignarParametroCadena("@empleado", idUser);
                        using (DbDataReader DRContp = DB.EjecutarConsulta())
                        {
                            if (DRContp.Read())
                            {
                                auxp = Convert.ToInt32(DRContp[0]);
                            }
                        }
                        DB.Desconectar();
                    }
                    if (auxp.ToString().Length == 1) { codep = "00000000" + auxp.ToString(); }
                    if (auxp.ToString().Length == 2) { codep = "0000000" + auxp.ToString(); }
                    if (auxp.ToString().Length == 3) { codep = "000000" + auxp.ToString(); }
                    if (auxp.ToString().Length == 4) { codep = "00000" + auxp.ToString(); }
                    if (auxp.ToString().Length == 5) { codep = "0000" + auxp.ToString(); }
                    if (auxp.ToString().Length == 6) { codep = "000" + auxp.ToString(); }
                    if (auxp.ToString().Length == 7) { codep = "00" + auxp.ToString(); }
                    if (auxp.ToString().Length == 8) { codep = "0" + auxp.ToString(); }
                    if (auxp.ToString().Length == 9) { codep = auxp.ToString(); }
                    DB.Conectar();
                    DB.CrearComando(@"insert into pagoTemp 
                           (formaPagoTemp,plazoTemp,totalpagoTemp,codigoTempp,unidadTiempoTemp,id_empleado) 
                           values 
                           (@formapago,@plazo,@totalpago,@codigotempp,@unidadTiempo,@idempleado)");
                    DB.AsignarParametroCadena("@formapago", ddr_pagos.SelectedValue.ToString());
                    DB.AsignarParametroCadena("@plazo", tbplazo.Text);
                    DB.AsignarParametroCadena("@totalpago", tbtotal_pago.Text);
                    DB.AsignarParametroCadena("@codigotempp", codep.ToString());
                    DB.AsignarParametroCadena("@unidadTiempo", tbUMedida.Text);
                    DB.AsignarParametroCadena("@idempleado", Session["idUser"].ToString());
                    DB.EjecutarConsulta1();
                    DB.Desconectar();
                    SqlDataSourcedetallePago.SelectParameters[0].DefaultValue = Session["idUser"].ToString();
                    SqlDataSourcedetallePago.DataBind();
                    GridDetPago.DataBind();
                    tbplazo.Text = "0";
                    tbtotal_pago.Text = "0";
                    tbUMedida.Text = "";
                }
                else
                {
                    lMsjImpuestos.Text = "Debe corregir TotalPago";
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
    }
}