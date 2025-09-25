using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using Control;
using clibLogger;

namespace DataExpressWeb
{
    public partial class crearRetencion : System.Web.UI.Page
    {
        private Spool spoolComprobante;
        private string idUser = "";
        private string rucEmisor = "";
        private string sucursalUser = "";
        private int count = 0;
        private string msj = "";
        private string codigoControl = "";
        private Log log = new Log();
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
                        llenarPeriodoFiscal();
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
                    idUser = Session["idUser"].ToString();
                    sucursalUser = Session["sucursalUser"].ToString();
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
            lMsjDocumento.Text = "";
            codigoControl = "";
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
                    ddlTipoIdentificacion.SelectedValue, "", tbRazonSocialComprador.Text, tbIdentificacionComprador.Text, "USD",
                    "", "", "", "", "", "", "", "", "", "", "", "0", "");
                spoolComprobante.retencionesPeriodoFiscal(ddlPeriodoFiscal.SelectedValue);
                spoolComprobante.cantidades("0", "0", "0", "0",
                    "0", "0", "0", "0", "0", "0");
                spoolComprobante.totalImpuestosRetenciones(idUser);
                spoolComprobante.detalles(idUser);
                spoolComprobante.impuestos(idUser);
                spoolComprobante.detallesAdicionales(idUser);
                spoolComprobante.informacionAdicional(idUser);
                spoolComprobante.infromacionAdicionalCima("", "", "", txt_dir_cli.Text, "");
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
                lMsjDocumento.Text = msj;
                log.mensajesLog("EM011", "", ex.Message, "Crear Retención", "");
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
                DB.CrearComando(@"SELECT count(id_Empleado) FROM DetallesTemp WITH (NOLOCK)  WHERE id_Empleado=@id_Empleado");
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
                if (aux.ToString().Length == 8) { code = aux.ToString(); }
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
                if (tbTarifa.Text != "No Sujeto")
                {
                    valImpuesto = tbTarifa.Text;
                    if (tbValor.Text.Length > 0)
                    {
                        DB.Conectar();
                        DB.CrearComando(@"insert into TotalConImpuestosTemp 
                           (codigo,codigoRetencion,baseImponible,porcentajeRetener,valorRetenido,
                            codDocSustento,numDocSustento,fechaEmisionDocSustento,id_Empleado) 
                           values 
                           (@codigo,@codigoRetencion,@baseImponible,@porcentajeRetener,@valorRetenido,
                            @codDocSustento,@numDocSustento,@fechaEmisionDocSustento,@id_Empleado)");
                        DB.AsignarParametroCadena("@codigo", tbCodigoID.Text);
                        DB.AsignarParametroCadena("@codigoRetencion", tbCodigoIDP.Text);
                        DB.AsignarParametroCadena("@baseImponible", tbBaseImponible.Text);
                        DB.AsignarParametroCadena("@porcentajeRetener", valImpuesto);
                        DB.AsignarParametroCadena("@valorRetenido", tbValor.Text);
                        DB.AsignarParametroCadena("@codDocSustento", ddlcodDocSustento.SelectedValue);
                        DB.AsignarParametroCadena("@numDocSustento", tbNumDocModificado.Text);
                        DB.AsignarParametroCadena("@fechaEmisionDocSustento", tbFechaEmisionDocSustento.Text);
                        DB.AsignarParametroCadena("@id_Empleado", idUser);
                        DB.EjecutarConsulta1();
                        DB.Desconectar();
                        SqlDataImpuestosConceptos.SelectParameters[0].DefaultValue = Session["idUser"].ToString();
                        SqlDataImpuestosConceptos.DataBind();
                        gvTotalImpuestos.DataBind();
                    }
                }
                else
                {
                    valImpuesto = "0";
                    if (tbValor.Text.Length > 0)
                    {
                        DB.Conectar();
                        DB.CrearComando(@"insert into TotalConImpuestosTemp 
                           (codigo,codigoRetencion,baseImponible,porcentajeRetener,valorRetenido,
                            codDocSustento,numDocSustento,fechaEmisionDocSustento,id_Empleado) 
                           values 
                           (@codigo,@codigoRetencion,@baseImponible,@porcentajeRetener,@valorRetenido,
                            @codDocSustento,@numDocSustento,@fechaEmisionDocSustento,@id_Empleado)");
                        DB.AsignarParametroCadena("@codigo", tbCodigoID.Text);
                        DB.AsignarParametroCadena("@codigoRetencion", tbCodigoIDP.Text);
                        DB.AsignarParametroCadena("@baseImponible", tbBaseImponible.Text);
                        DB.AsignarParametroCadena("@porcentajeRetener", valImpuesto);
                        DB.AsignarParametroCadena("@valorRetenido", tbValor.Text);
                        DB.AsignarParametroCadena("@codDocSustento", ddlcodDocSustento.SelectedValue);
                        DB.AsignarParametroCadena("@numDocSustento", tbNumDocModificado.Text);
                        DB.AsignarParametroCadena("@fechaEmisionDocSustento", tbFechaEmisionDocSustento.Text);
                        DB.AsignarParametroCadena("@id_Empleado", idUser);
                        DB.EjecutarConsulta1();
                        DB.Desconectar();
                        SqlDataImpuestosConceptos.SelectParameters[0].DefaultValue = Session["idUser"].ToString();
                        SqlDataImpuestosConceptos.DataBind();
                        gvTotalImpuestos.DataBind();
                    }
                }
                tbCodigoID.Text = "";
                tbCodigoIDP.Text = "";
                tbTarifa.Text = "0";
                tbValor.Text = "0";
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }
        }



        private void llenarlista(String ruc, String tipo)
        {
            var DB = new BasesDatos();
            try
            {
                String sql = "";
                if (tipo == "emi") { sql = @"SELECT * FROM EMISOR WITH (NOLOCK)  WHERE RFCEMI=@ruc"; }
                if (tipo == "rec") { sql = @"SELECT * FROM RECEPTOR WITH (NOLOCK)  WHERE RFCREC=@ruc"; }
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
        protected void tbTarifa_TextChanged(object sender, EventArgs e)
        {
            importeConcepto();
        }
        protected void ddlTipoImpuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem lt = new ListItem();
            lt.Value = "0";
            lt.Text = "Selecciona el impuesto";
            if (ddlTipoImpuesto.SelectedValue == "1")
            {
                ddlTasa.Visible = true;
                ddlTasa.Items.Clear();
                SqlDataPorcentajeRetencion.SelectParameters[0].DefaultValue = "AIRretencion";
                SqlDataPorcentajeRetencion.DataBind();
                ddlTasa.DataBind();
                tbTarifa.Visible = false;
                RequiredFieldValidator21.Visible = true;
                tbCodigoIDP.Text = "";
                tbTarifa.Text = "0";
            }
            else if (ddlTipoImpuesto.SelectedValue == "6")
            {
                ddlTasa.Visible = true;
                tbTarifa.Visible = false;
                RequiredFieldValidator21.Visible = true;
                tbCodigoIDP.Text = "";
                tbTarifa.Text = String.Format("{0:f}", 1);
            }
            else if (ddlTipoImpuesto.SelectedValue == "0")
            {
                ddlTasa.Items.Clear();
            }
            else
            {
                ddlTasa.Items.Clear();
                SqlDataPorcentajeRetencion.SelectParameters[0].DefaultValue = "IVARetencion";
                SqlDataPorcentajeRetencion.DataBind();
                ddlTasa.DataBind();
                ddlTasa.Visible = true;
                tbTarifa.Visible = false;
                RequiredFieldValidator21.Visible = false;
            }
            ddlTasa.Items.Add(lt);
            ddlTasa.SelectedValue = "0";
            importeConcepto();
        }

        private void importeConcepto()
        {
            var DB = new BasesDatos();
            try
            {
                tbCodigoID.Text = ddlTipoImpuesto.SelectedValue;
                DB.Conectar();
                DB.CrearComando(@"SELECT valor,codigo FROM CatImpuestos_C WITH (NOLOCK)  WHERE id=@id");
                DB.AsignarParametroCadena("@id", ddlTasa.SelectedValue);
                using (DbDataReader DRCont = DB.EjecutarConsulta())
                {
                    if (DRCont.Read())
                    {
                        tbTarifa.Text = DRCont[0].ToString();
                        tbCodigoIDP.Text = DRCont[1].ToString();
                        calcularImpuesto();
                    }
                    else
                    {
                        tbValor.Text = "0";
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

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            importeConcepto();
        }

        protected void StepNextButton_Click(object sender, WizardNavigationEventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                count = 0;
                switch (Wizard1.ActiveStepIndex)
                {
                    case 1:
                        DB.Conectar();
                        DB.CrearComando(@"SELECT count(*) FROM TotalConImpuestosTemp WITH (NOLOCK)  WHERE id_Empleado=@empleado");
                        DB.AsignarParametroCadena("@empleado", idUser);
                        using (DbDataReader DRCont = DB.EjecutarConsulta())
                        {
                            if (DRCont.Read())
                            {
                                count = Convert.ToInt32(DRCont[0]);
                            }
                        }
                        DB.Desconectar();
                        if (count < 1) { lMsjRetencion.Text = "Necesitas Agregar al menos una Retención."; e.Cancel = true; } else { lMsjRetencion.Text = ""; }
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

        private void llenarPeriodoFiscal()
        {
            ddlPeriodoFiscal.Items.Add("12/2012");
            string[] mes = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            string ano = System.DateTime.Now.ToString("yyyy");
            ddlPeriodoFiscal.Items.Clear();
            for (int i = 0; i < 12; i++)
            {
                ddlPeriodoFiscal.Items.Add(mes[i] + "/" + ano);
            }
            ddlPeriodoFiscal.SelectedValue = System.DateTime.Now.ToString("MM/yyyy");
        }
        protected void tbBaseImponible_TextChanged(object sender, EventArgs e)
        {
            importeConcepto();
        }

        private void calcularImpuesto()
        {
            decimal tarifa = 0;
            try
            {
                tarifa = Convert.ToDecimal(tbTarifa.Text);
            }
            catch (Exception ex)
            {
                lMsjRetencion.Text = "No es un numero Válido.";
            }
            tbValor.Text = String.Format("{0:f}", (tarifa * Convert.ToDecimal(tbBaseImponible.Text)) / 100);
        }
    }
}

