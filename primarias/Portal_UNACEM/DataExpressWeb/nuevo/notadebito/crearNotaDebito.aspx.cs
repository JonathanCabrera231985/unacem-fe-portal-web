using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using Control;
using clibLogger;

namespace DataExpressWeb
{
    public partial class crearNotaDebito : System.Web.UI.Page
    {
        private Spool spoolComprobante;
        private string idUser = "";
        private string rucEmisor = "";
        private string sucursalUser = "";
        private int count = 0;
        private string msj = "";
        private Log log = new Log();
        private string codigoControl = "";
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
                if (Session["idUser"] != null)
                {
                    idUser = Session["idUser"].ToString();
                    sucursalUser = Session["sucursalUser"].ToString();
                    if (!Page.IsPostBack)
                    {
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
                        this.tbFechaEmisionDocSustento.Text = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    idUser = Session["idUser"].ToString();
                    sucursalUser = Session["sucursalUser"].ToString();
                    llenarTotales();
                    llenarlista(rucEmisor, "emi");
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

        protected void FinishButton_Click(object sender, EventArgs e)
        {
            codigoControl = "";
            lMsjDocumento.Text = "";
            string obligadocontabilidad = "";
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
                spoolComprobante.infromacionDocumento(tbFechaEmision.Text, tbDirEstablecimiento.Text, tbContribuyenteEspecial.Text, obligadocontabilidad,
                    ddlTipoIdentificacion.SelectedValue, "", tbRazonSocialComprador.Text, tbIdentificacionComprador.Text, tbMoneda.Text,
                    "", "", "", "", "", "", "", "", ddlComprobanteMod.SelectedValue, tbNumDocModificado.Text, tbFechaEmisionDocSustento.Text, tbImporteTotal.Text, "");
                spoolComprobante.cantidades(tbSubtotal12.Text, tbSubtotal0.Text, tbSubtotalNoSujeto.Text, tbTotalSinImpuestos.Text,
                    "0", tbICE.Text, tbIVA12.Text, tbImporteTotal.Text, "0", tbImporteaPagar.Text);
                spoolComprobante.totalImpuestos(idUser);
                spoolComprobante.totalMotivos(idUser);
                spoolComprobante.informacionAdicional(idUser);
                codigoControl = spoolComprobante.generarDocumento();
                if (!String.IsNullOrEmpty(codigoControl))
                {
                    Session["codigoControl"] = codigoControl;
                    Response.Redirect("~/Procesando.aspx");
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
                log.mensajesLog("EM011", "", ex.Message, "Crear Nota de Débito", "");
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
        protected void bAgregarMotivo_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            string valImpuesto = "";
            try
            {
                if (tbTarifa.Text != "No Sujeto")
                {
                    valImpuesto = tbTarifa.Text;
                    if (tbValor.Text.Length > 0)
                    {
                        DB.Conectar();
                        DB.CrearComando(@"insert into MotivosDebitoTemp 
                           (razon,codigo,codigoPorcentaje,baseImponible,tarifa,valor,id_Empleado,tipo) 
                           values 
                           (@razon,@codigo,@codigoPorcentaje,@baseImponible,@tarifa,@valor,@id_Empleado,@tipo)");
                        DB.AsignarParametroCadena("@razon", tbRazon.Text);
                        DB.AsignarParametroCadena("@codigo", tbCodigoID.Text);
                        DB.AsignarParametroCadena("@codigoPorcentaje", tbCodigoIDP.Text);
                        DB.AsignarParametroCadena("@baseImponible", tbBaseImponible.Text);
                        DB.AsignarParametroCadena("@tarifa", valImpuesto);
                        DB.AsignarParametroCadena("@valor", tbValor.Text);
                        DB.AsignarParametroCadena("@id_Empleado", idUser);
                        DB.AsignarParametroCadena("@tipo", ddlTipoImpuesto.SelectedItem.ToString());
                        DB.EjecutarConsulta1();
                        DB.Desconectar();
                        SqlDataMotivos.SelectParameters[0].DefaultValue = Session["idUser"].ToString();
                        SqlDataMotivos.DataBind();
                        gvMotivos.DataBind();
                    }
                }
                else
                {
                    valImpuesto = "0";
                    if (tbValor.Text.Length > 0)
                    {
                        DB.Conectar();
                        DB.CrearComando(@"insert into MotivosDebitoTemp 
                           (razon,codigo,codigoPorcentaje,baseImponible,tarifa,valor,id_Empleado,tipo) 
                           values 
                           (@razon,@codigo,@codigoPorcentaje,@baseImponible,@tarifa,@valor,@id_Empleado,@tipo)");
                        DB.AsignarParametroCadena("@razon", tbRazon.Text);
                        DB.AsignarParametroCadena("@codigo", tbCodigoID.Text);
                        DB.AsignarParametroCadena("@codigoPorcentaje", tbCodigoIDP.Text);
                        DB.AsignarParametroCadena("@baseImponible", tbBaseImponible.Text);
                        DB.AsignarParametroCadena("@tarifa", valImpuesto);
                        DB.AsignarParametroCadena("@valor", tbValor.Text);
                        DB.AsignarParametroCadena("@id_Empleado", idUser);
                        DB.AsignarParametroCadena("@tipo", ddlTipoImpuesto.SelectedItem.ToString());
                        DB.EjecutarConsulta1();
                        DB.Desconectar();
                        SqlDataMotivos.SelectParameters[0].DefaultValue = Session["idUser"].ToString();
                        SqlDataMotivos.DataBind();
                        gvMotivos.DataBind();
                    }
                }
                tbCodigoID.Text = "";
                tbCodigoIDP.Text = "";
                tbTarifa.Text = "0";
                tbValor.Text = "0";
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
        protected void tbTarifa_TextChanged(object sender, EventArgs e)
        {
            calcularImpuesto();
        }

        private void llenarlista(String ruc, String tipo)
        {
            var DB = new BasesDatos();
            try
            {
                String sql = "";
                if (tipo == "emi") { sql = @"SELECT * FROM EMISOR WITH (NOLOCK)  WHERE RFCEMI=@ruc"; }
                if (tipo == "rec") { sql = @"SELECT * FROM RECEPTOR WITH (NOLOCK) WHERE RFCREC=@ruc"; }

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
                        }
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

        protected void tbIdentificacionComprador_TextChanged(object sender, EventArgs e)
        {
            llenarlista(tbIdentificacionComprador.Text, "rec");
        }

        public void llenarTotales()
        {
            var DB = new BasesDatos();
            try
            {
                tbSubtotal0.Text = "0";
                tbSubtotal12.Text = "0";
                tbSubtotalNoSujeto.Text = "0";
                tbTotalSinImpuestos.Text = "0";
                tbICE.Text = "0";
                tbIVA12.Text = "0";
                tbImporteaPagar.Text = "0";
                DB.Conectar();
                DB.CrearComando(@"select ISNULL(sum(baseImponible),0) from MotivosDebitoTemp WITH (NOLOCK)  WHERE id_Empleado=@empleado");
                DB.AsignarParametroCadena("@empleado", idUser);
                using (DbDataReader DRSum = DB.EjecutarConsulta())
                {
                    if (DRSum.Read())
                    {
                        tbTotalSinImpuestos.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum[0].ToString()));
                    }
                }
                DB.Desconectar();
                DB.Conectar();
                DB.CrearComando(@"SELECT ISNULL(sum(valor),0),ISNULL(sum(baseImponible),0)
                              FROM  MotivosDebitoTemp WITH (NOLOCK)  WHERE   (CAST(tarifa AS INT)='14' OR CAST(tarifa AS INT)='12') AND tipo='IVA' AND id_Empleado=@empleado");
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
                DB.CrearComando(@"SELECT ISNULL(sum(baseImponible),0)
                              FROM  MotivosDebitoTemp WITH (NOLOCK)  WHERE   CAST(tarifa AS INT)='0' AND tipo='IVA' AND id_Empleado=@empleado");
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
                DB.CrearComando(@"SELECT ISNULL(sum(valor),0),ISNULL(sum(baseImponible),0)
                              FROM  MotivosDebitoTemp WITH (NOLOCK)  WHERE   tarifa IS NULL AND tipo='IVA' AND id_Empleado=@empleado");
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
                DB.CrearComando(@"SELECT ISNULL(sum(valor),0),ISNULL(sum(baseImponible),0)
                              FROM  MotivosDebitoTemp WITH (NOLOCK)  WHERE   tipo='ICE' AND id_Empleado=@empleado");
                DB.AsignarParametroCadena("@empleado", idUser);
                using (DbDataReader DRSum4 = DB.EjecutarConsulta())
                {
                    if (DRSum4.Read())
                    {
                        tbICE.Text = String.Format("{0:f}", Convert.ToDecimal(DRSum4[0].ToString()));
                    }
                }
                DB.Desconectar();
                tbImporteTotal.Text = String.Format("{0:f}", Convert.ToDecimal(tbTotalSinImpuestos.Text) +
                                       Convert.ToDecimal(tbICE.Text) + Convert.ToDecimal(tbIVA12.Text));
                tbImporteaPagar.Text = String.Format("{0:f}", Convert.ToDecimal(tbImporteTotal.Text));
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
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
                tbTarifa.Text = "0";
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
                    tarifa = Convert.ToDecimal(tbTarifa.Text);
                }
                catch (Exception ex)
                {
                    lMsjMotivos.Text = "No es un numero Válido.";
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
            tbCodigoID.Text = ddlTipoImpuesto.SelectedValue;
            tbTarifa.Text = ddlTasaIVA.SelectedValue;
            tbTarifa.ReadOnly = false;
            if (tbTarifa.Text == "14.00" || tbTarifa.Text == "14" || tbTarifa.Text == "14,00")
            {
                tbCodigoIDP.Text = "3";
            }
            if (tbTarifa.Text == "12.00" || tbTarifa.Text == "12" || tbTarifa.Text == "12,00")
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
                tbTarifa.Text = "0";
                tbValor.Text = "0";
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
                        DB.CrearComando(@"SELECT count(*) FROM MotivosDebitoTemp WITH (NOLOCK)  WHERE id_Empleado=@empleado");
                        DB.AsignarParametroCadena("@empleado", idUser);
                        using (DbDataReader DRCont = DB.EjecutarConsulta())
                        {
                            if (DRCont.Read())
                            {
                                count = Convert.ToInt32(DRCont[0]);
                            }

                        }
                        DB.Desconectar();
                        if (count < 1) { lMsjMotivos.Text = "Necesitas Agregar al menos un Motivo."; e.Cancel = true; } else { lMsjMotivos.Text = ""; }
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
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }
        }

        protected void Wizard1_ActiveStepChanged(object sender, EventArgs e)
        {
        }

        protected void tbBaseImponible_TextChanged(object sender, EventArgs e)
        {
            calcularImpuesto();
        }
    }
}