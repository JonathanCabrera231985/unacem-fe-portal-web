using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using Control;
using clibLogger;

namespace DataExpressWeb
{
    public partial class crearGuia : System.Web.UI.Page
    {
        private Spool spoolComprobante;
        private string idUser = "";
        private string rucEmisor = "";
        private string sucursalUser = "";
        private int count = 0;
        private int countDest = 0;
        private string formatCero = "0.00";
        private string msj = "";
        private Log log = new Log();
        private string codigoControl;
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
                    }
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
                    tbDirPartida.Text, tbRazonSocialComprador.Text, ddlTipoIdentificacion.SelectedValue, tbIdentificacionComprador.Text,
                    tbRise.Text, tbFechaIniTransporte.Text, tbFechaFinTransporte.Text, tbPlaca.Text, "", "", "", formatCero, "");
                spoolComprobante.cantidades(formatCero, formatCero, formatCero, formatCero, formatCero, formatCero, formatCero, formatCero, formatCero, formatCero);
                spoolComprobante.destinatarios(idUser);
                spoolComprobante.detalles(idUser);
                spoolComprobante.detallesAdicionales(idUser);
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
                log.mensajesLog("EM011", "", ex.Message, "Crear Guía", "");
            }
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
        private void llenarlista(String ruc, String tipo)
        {
            var DB = new BasesDatos();
            try
            {
                String sql = "";
                if (tipo == "emi") { sql = @"SELECT * FROM EMISOR WITH (NOLOCK) WHERE RFCEMI=@ruc"; }
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

        protected void StepNextButton_Click(object sender, WizardNavigationEventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                switch (Wizard1.ActiveStepIndex)
                {
                    case 1:
                        DB.Conectar();
                        DB.CrearComando(@"SELECT count(*) FROM DestinatariosTemp WITH (NOLOCK)  WHERE id_Empleado=@empleado");
                        DB.AsignarParametroCadena("@empleado", idUser);
                        using (DbDataReader DRContDest1 = DB.EjecutarConsulta())
                        {
                            if (DRContDest1.Read())
                            {
                                countDest = Convert.ToInt32(DRContDest1[0]);
                            }
                        }
                        DB.Desconectar();
                        if (countDest < 1) { lMsjDetinatario.Text = "Necesitas Agregar al menos un Destinatario."; Wizard1.MoveTo(Wizard1.WizardSteps[1]); } else { lMsjDetinatario.Text = ""; }
                        break;
                    case 2:
                        DB.Conectar();
                        DB.CrearComando(@"SELECT count(*) FROM DestinatariosTemp WITH (NOLOCK) WHERE id_Empleado=@empleado");
                        DB.AsignarParametroCadena("@empleado", idUser);
                        using (DbDataReader DRContDest = DB.EjecutarConsulta())
                        {
                            if (DRContDest.Read())
                            {
                                countDest = Convert.ToInt32(DRContDest[0]);
                            }
                        }
                        DB.Desconectar();
                        DB.Conectar();
                        DB.CrearComando(@"SELECT count(id_Destinatario)
                                  FROM  DestinatariosTemp WITH (NOLOCK)  INNER JOIN
                                        DetallesTemp WITH (NOLOCK) ON DestinatariosTemp.idDestinatarioTemp = DetallesTemp.id_Destinatario
                                  WHERE DestinatariosTemp.id_Empleado=@empleado");
                        DB.AsignarParametroCadena("@empleado", idUser);
                        using (DbDataReader DRCont = DB.EjecutarConsulta())
                        {
                            if (DRCont.Read())
                            {
                                count = Convert.ToInt32(DRCont[0]);
                            }
                        }
                        DB.Desconectar();
                        if (count < 1) { lMsjDetallesDest.Text = "Necesitas Agregar al menos un Detalle."; Wizard1.MoveTo(Wizard1.WizardSteps[2]); } else { lMsjDetallesDest.Text = ""; }
                        if (count == countDest) { lMsjDetallesDest.Text = "No todos los Destinatarios contienen al menos un Detalle."; Wizard1.MoveTo(Wizard1.WizardSteps[2]); } else { lMsjDetallesDest.Text = ""; }
                        break;
                    case 3:
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
            countDest = 0;
            count = 0;
            //verificar destinatarios y detalles
            if (Wizard1.ActiveStepIndex == 3)
            {
            }
            else { lMsjDetinatario.Text = ""; }
            if (Wizard1.ActiveStepIndex == 2)
            {
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            lMsjDetinatario.Text = "";
            try
            {
                DB.Conectar();
                DB.CrearComando(@"INSERT INTO DestinatariosTemp
           (identificacionDestinatario,razonSocialDestinatario,dirDestinatario,motivoTraslado,docAduaneroUnico
           ,codEstabDestino,ruta,codDocSustento,numDocSustento,numAutDocSustento,fechaEmisionDocSustento,id_Empleado)
     VALUES
			(@identificacionDestinatario,@razonSocialDestinatario,@dirDestinatario,@motivoTraslado,@docAduaneroUnico
           ,@codEstabDestino,@ruta,@codDocSustento,@numDocSustento,@numAutDocSustento,@fechaEmisionDocSustento,@id_Empleado)");
                DB.AsignarParametroCadena("@identificacionDestinatario", tbIdentificacion.Text);
                DB.AsignarParametroCadena("@razonSocialDestinatario", tbRazonDestinatario.Text);
                DB.AsignarParametroCadena("@dirDestinatario", tbDireccionDestinatario.Text);
                DB.AsignarParametroCadena("@motivoTraslado", tbMotivoTraslado.Text);
                DB.AsignarParametroCadena("@docAduaneroUnico", tbDocAduaneroUnico.Text);
                DB.AsignarParametroCadena("@codEstabDestino", tbCodigoEstabl.Text);
                DB.AsignarParametroCadena("@ruta", tbRutaTraslado.Text);
                DB.AsignarParametroCadena("@codDocSustento", ddlcodDocSustento.SelectedValue);
                DB.AsignarParametroCadena("@numDocSustento", tbNumDocSustento.Text);
                DB.AsignarParametroCadena("@numAutDocSustento", tbNumAutoDocSustento.Text);
                DB.AsignarParametroCadena("@fechaEmisionDocSustento", tbfechaEmisionDocSustento.Text);
                DB.AsignarParametroCadena("@id_Empleado", idUser);
                DB.EjecutarConsulta1();
                DB.Desconectar();
                SqlDataDestinatarios.SelectParameters[0].DefaultValue = idUser.ToString();
                SqlDataDestinatarios.DataBind();
                gvDestinatario.DataBind();
                lMsjDetinatario.Text = "Registro Correcto";
                tbIdentificacion.Text = "";
                tbRazonDestinatario.Text = "";
                tbNumDocSustento.Text = "";
                tbDireccionDestinatario.Text = "";
                tbMotivoTraslado.Text = "";
                tbRutaTraslado.Text = "";
                tbDocAduaneroUnico.Text = "";
                tbCodigoEstabl.Text = "001";
                tbNumAutoDocSustento.Text = "";
                tbfechaEmisionDocSustento.Text = "";
                ddlcodDocSustento.SelectedValue = "0";
            }
            catch (Exception ex)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ex.Message);
                lMsjDetinatario.Text = "No de puede insertar el registro" + ex.Message;
            }
            finally
            {
                DB.Desconectar();
            }
        }


        protected void bAgregarDetalleDest_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            lMsjDetallesDest.Text = "";
            if (!ddlDestinatario.SelectedValue.ToString().Equals("0"))
            {
                ListItem lt = new ListItem();
                lt.Value = "0";
                lt.Text = "Selecciona un Detalle";
                try
                {
                    DB.Conectar();
                    DB.CrearComando(@"insert into DetallesTemp 
                           (codigoPrincipal,codigoAuxiliar,descripcion,cantidad,precioUnitario,
                            descuento,precioTotalSinImpuestos,id_Empleado,id_Destinatario) 
                           values 
                           (@codigoPrincipal,@codigoAuxiliar,@descripcion,@cantidad,@precioUnitario,
                            @descuento,@precioTotalSinImpuestos,@id_Empleado,@id_Destinatario)");
                    DB.AsignarParametroCadena("@codigoPrincipal", tbCodigoInterno.Text);
                    DB.AsignarParametroCadena("@codigoAuxiliar", tbCodigoAdicional.Text);
                    DB.AsignarParametroCadena("@descripcion", tbDescripcion.Text);
                    DB.AsignarParametroCadena("@cantidad", tbCantidad.Text);
                    DB.AsignarParametroCadena("@precioUnitario", "0");
                    DB.AsignarParametroCadena("@descuento", "0");
                    DB.AsignarParametroCadena("@precioTotalSinImpuestos", "0");
                    DB.AsignarParametroCadena("@id_Destinatario", ddlDestinatario.SelectedValue);
                    DB.AsignarParametroCadena("@id_Empleado", idUser);
                    DB.EjecutarConsulta1();
                    DB.Desconectar();
                    SqlDataDetallesDest.SelectParameters[0].DefaultValue = idUser.ToString(); //idDestinatario
                    SqlDataDetallesDest.SelectParameters[1].DefaultValue = ddlDestinatario.SelectedValue;
                    SqlDataDetallesDest.DataBind();
                    gvDetallesDestinatario.DataBind();
                    lMsjDetallesDest.Text = "Registro Correcto";
                    tbCodigoAdicional.Text = "";
                    tbCodigoInterno.Text = "";
                    tbDescripcion.Text = "";
                }
                catch (Exception ex)
                {
                    DB.Desconectar();
                    clsLogger.Graba_Log_Error(ex.Message);
                    lMsjDetallesDest.Text = "No se puede insertar el registro" + ex.Message;
                }
                finally
                {
                    DB.Desconectar();
                }
            }
            else
            {
                lMsjDetallesDest.Text = "Tienes que seleccionar algún Destinatario.";
            }
        }
    }
}