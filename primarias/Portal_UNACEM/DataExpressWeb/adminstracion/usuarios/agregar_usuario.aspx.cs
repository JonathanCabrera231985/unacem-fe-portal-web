using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using Control;
using System.Data;
using CriptoSimetrica;
using System.Xml.Linq;
using clibLogger;
using DataExpressWeb;

namespace Administracion
{
    public partial class agregar_usuario : System.Web.UI.Page
    {
        private string rfcEmisor = "";
        private String idSucursal = "0";
        ValidaRUC rucValida = new ValidaRUC();
        private AES Cs = new AES();
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
                if (Session["sucursalUser"] != null)
                {
                    idSucursal = Session["sucursalUser"].ToString();
                    DB.Conectar();
                    DB.CrearComando(@"select RFCEMI from EMISOR WITH (NOLOCK)  where IDEEMI = @IDEEMI");
                    DB.AsignarParametroCadena("@IDEEMI", Session["rucEmpresa"].ToString());
                    using (DbDataReader DR = DB.EjecutarConsulta())
                    {
                        if (DR.Read())
                        {
                            rfcEmisor = DR[0].ToString();
                        }
                    }

                    DB.Desconectar();
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

        private DataTable dtEmpleado()
        {
            DataTable dt = new DataTable("PA_insertar_empleados");
            dt.Columns.Add("nombreEmpleado", typeof(String));
            dt.Columns.Add("userEmpleado", typeof(String));
            dt.Columns.Add("claveEmpleado", typeof(String));
            dt.Columns.Add("status", typeof(Byte));
            dt.Columns.Add("RFC", typeof(String));
            dt.Columns.Add("id_Rol", typeof(Int32));
            dt.Columns.Add("id_Sesion", typeof(Int32));
            dt.Columns.Add("id_Sucursal", typeof(Int32));
            dt.Columns.Add("eliminado", typeof(Byte));
            dt.Columns.Add("documentoXML", typeof(String));
            dt.Columns.Add("correo", typeof(String));
            return dt;
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            string rfc = "";
            try
            {
                if (IsComplexPassword())
                {
                    if (ddlTipoUsuario.SelectedValue == "1")
                    {
                        tbRFC.Visible = false;
                        lRfc.Visible = false;
                        if (ddlRol.SelectedValue != "0")
                        {
                            tbRFC.Text = rfcEmisor;
                            DataSet ds = new DataSet();
                            DataTable dt = dtEmpleado();
                            DataRow dr = dt.NewRow();
                            if (tbUsername.Text.Length < 6)
                                tbUsername.Text = "EMPLE" + tbUsername.Text;
                            dr["nombreEmpleado"] = tbNombre.Text;
                            dr["userEmpleado"] = tbUsername.Text.Substring(0, 5).ToUpper().Equals("EMPLE") ? tbUsername.Text : "EMPLE" + tbUsername.Text;
                            dr["claveEmpleado"] = Cs.encriptar(tbContrasena.Text, "CIMAIT");
                            dr["status"] = ddlStatus.SelectedValue;
                            dr["RFC"] = cboEmpresa.SelectedValue.ToString();
                            dr["id_Rol"] = ddlRol.SelectedValue;
                            dr["id_Sesion"] = 1;
                            string idSucur = "";

                            foreach (GridViewRow row in gvSucursales.Rows)
                            {
                                CheckBox chk_Seleccionar = (CheckBox)row.FindControl("check");
                                if (chk_Seleccionar.Checked)
                                {
                                    idSucur = ((HiddenField)row.FindControl("checkLinea")).Value;
                                    break;
                                }
                            }

                            if (string.IsNullOrEmpty(idSucur))
                            {
                                DB.Conectar();
                                DataTable dtSucursal = DB.TraerDataSetConsulta("select top 1 idSucursal from Sucursales WITH (NOLOCK) ", new Object[] { }).Tables[0];
                                dr["id_Sucursal"] = dtSucursal.Rows[0][0].ToString();
                                DB.Desconectar();
                            }
                            else
                            {
                                dr["id_Sucursal"] = idSucur;
                            }

                            dr["eliminado"] = false;
                            dr["correo"] = tbEmail.Text;
                            XElement INSTRUCCION = new XElement("INSTRUCCION");
                            foreach (GridViewRow row in gvSucursales.Rows)
                            {
                                CheckBox chk_Seleccionar = (CheckBox)row.FindControl("check");
                                GridView gvPuntoEmision = (GridView)row.FindControl("gvPuntoEmision");
                                int contador = gvPuntoEmision.Rows.Count;
                                foreach (GridViewRow rowEmision in gvPuntoEmision.Rows)
                                {
                                    CheckBox chk_SeleccionarEmision = (CheckBox)rowEmision.FindControl("check");
                                    if (!chk_SeleccionarEmision.Checked)
                                        contador--;
                                }
                                if (contador < gvPuntoEmision.Rows.Count & contador > 0 & chk_Seleccionar.Checked)
                                    chk_Seleccionar.Checked = false;

                                foreach (GridViewRow rowEmision in gvPuntoEmision.Rows)
                                {
                                    CheckBox chk_SeleccionarEmision = (CheckBox)rowEmision.FindControl("check");
                                    if (chk_SeleccionarEmision.Checked | chk_Seleccionar.Checked)
                                    {
                                        INSTRUCCION.Add(new XElement("SUCURSALES",
                                            new XElement("idSucursal", ((HiddenField)row.FindControl("checkLinea")).Value),
                                            new XElement("idEmison", ((HiddenField)rowEmision.FindControl("checkCaja")).Value)));
                                    }
                                }
                            }
                            XDocument xDocument = new XDocument(INSTRUCCION);
                            dr["documentoXML"] = xDocument.ToString();
                            dt.Rows.Add(dr);
                            ds.Tables.Add(dt);
                            DB.Conectar();
                            DB.TraerDataset(ds);
                            DB.Desconectar();
                            Response.Redirect("empleados.aspx");
                        }
                        else
                        {
                            lMsj.Text = "Selecciona un tipo de Empleado o no existe el RUC.";
                        }
                    }
                    else if (ddlTipoUsuario.SelectedValue == "2")
                    {
                        if (!rucValida.ValidarNumeroIdentificacion(tbRFC.Text))
                        {
                            lMsj.Text = "Numero de Identificación Incorrecto";
                        }
                        else
                        {
                            DB.Conectar();
                            DB.CrearComando(@"SELECT RFCREC from Receptor WITH (NOLOCK)  where RFCREC=@RFC");
                            DB.AsignarParametroCadena("@RFC", tbRFC.Text);
                            using (DbDataReader DR = DB.EjecutarConsulta())
                            {
                                if (DR.Read())
                                {
                                    rfc = DR[0].ToString();
                                }
                                DB.Desconectar();
                                if (!String.IsNullOrEmpty(rfc))
                                {
                                    if (tbUsername.Text.Length < 6)
                                        tbUsername.Text = "CLIEN" + tbUsername.Text;
                                    DB.Conectar();
                                    DB.CrearComandoProcedimiento("PA_insertar_clientes");
                                    DB.AsignarParametroProcedimiento("@nombreCliente", System.Data.DbType.String, tbNombre.Text);
                                    DB.AsignarParametroProcedimiento("@userCliente", System.Data.DbType.String, tbUsername.Text.Substring(0, 5).ToUpper().Equals("CLIEN") ? tbUsername.Text : "CLIEN" + tbUsername.Text);
                                    DB.AsignarParametroProcedimiento("@claveCliente", System.Data.DbType.String, Cs.encriptar(tbContrasena.Text, "CIMAIT"));
                                    DB.AsignarParametroProcedimiento("@status", System.Data.DbType.String, ddlStatus.SelectedValue);
                                    DB.AsignarParametroProcedimiento("@RFC", System.Data.DbType.String, tbRFC.Text);
                                    DB.AsignarParametroProcedimiento("@id_Rol", System.Data.DbType.Int16, 1);
                                    DB.AsignarParametroProcedimiento("@id_Sesion", System.Data.DbType.Int16, 1);
                                    DB.AsignarParametroProcedimiento("@email", System.Data.DbType.String, tbEmail.Text);
                                    DB.AsignarParametroProcedimiento("@eliminado", System.Data.DbType.Byte, false);
                                    DB.AsignarParametroProcedimiento("@id_Sucursal", System.Data.DbType.Int16, int.Parse(idSucursal));
                                    DB.EjecutarConsulta1();
                                    DB.Desconectar();
                                    Response.Redirect("clientes.aspx");
                                }
                                else
                                {
                                    lMsj.Text = "El RUC no existe.";
                                }
                            }
                        }
                    }
                    else
                    {
                        lMsj.Text = "Selecciona un tipo de Clientes";
                    }

                }
                else
                {
                    lMsj.Text = "La contraseña no cumple con las especificaciones\nDebe tener de 8 a 14 carácteres.\nAl menos una letra en mayúscula.\nAl menos una letra en minúscula.\nNo espacios en blancos.\nAl menos un carácter especial.";
                }
            }
            catch (Exception ex)
            {
                lMsj.Text = ex.Message;
                clsLogger.Graba_Log_Error(ex.Message);
                DB.Desconectar();
            }
            finally
            {
                DB.Desconectar();
            }
        }

        private bool IsComplexPassword()
        {
            var password = tbContrasena.Text;
            if (password.Length < 8 || password.Length > 14)
                return false;
            if (!password.Any(char.IsUpper))
                return false;
            if (!password.Any(char.IsLower))
                return false;
            if (password.Contains(" "))
                return false;
            var specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
            char[] specialChArray = specialCh.ToCharArray();
            foreach (char ch in specialChArray)
            {
                if (password.Contains(ch))
                    return true;
            }
            return false;
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTipoUsuario.SelectedValue == "1")
            {
                columnEmpresa.Visible = true;
                lNombre.Text = "EMPLE120000";
                tbRFC.Text = rfcEmisor;
                ddlRol.Visible = true;
                SqlDataSourceRol.SelectCommand = "SELECT [idRol], [descripcion] FROM [Roles] WITH (NOLOCK) WHERE idRol <> 1 AND eliminado ='False'";
                SqlDataSourceRol.DataBind();
                ddlRol.DataBind();
                tbRFC.ReadOnly = true;
                ddlRol.Visible = true;
                lRol.Visible = true;
                lSucursal.Visible = true;
                tbEmail.Visible = false;
                lEmail.Visible = false;
                gvSucursales.Visible = true;
            }
            if (ddlTipoUsuario.SelectedValue == "2")
            {
                columnEmpresa.Visible = false;
                lNombre.Text = "CLIEN120000";
                tbRFC.Visible = true;
                tbRFC.ReadOnly = false;
                lRfc.Visible = true;
                tbRFC.Text = "";
                ddlRol.Visible = false;
                lRol.Visible = false;
                lSucursal.Visible = false;
                tbEmail.Visible = false;
                lEmail.Visible = false;
                gvSucursales.Visible = false;
            }
        }

        protected void tbRFC_TextChanged(object sender, EventArgs e)
        {
            if (tbRFC.Text.Length > 9)
            {
                if (!rucValida.ValidarNumeroIdentificacion(tbRFC.Text))
                    lMsj.Text = "Numero de Identificación Incorrecto";
                else
                    lMsj.Text = "";
            }
        }

        /// <summary>
        /// Método del evento del grid agregado....ojo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cboEmpresaList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                extraerDatos datos = new extraerDatos();
                gvSucursales.DataSource = datos.extraerDatosSucursales(cboEmpresa.SelectedValue, "select idSucursal,clave,sucursal, domicilio,0 estado from Sucursales WITH (NOLOCK) where eliminado = 0 and IDEEMI = ",
                    @"SELECT A.idCaja, A.idSucursal, b.descripcion, A.ptoEmi, 0 estado FROM dbo.CajaSucursal A WITH (NOLOCK)  INNER JOIN dbo.Catalogo1_C B WITH (NOLOCK) ON A.NumeroRentas = B.codigo AND b.tipo = 'Comprobante'
                    WHERE A.idSucursal = ");
                gvSucursales.DataBind();
            }
            catch (Exception ex)
            {
                lMsj.Text = ex.Message;
            }
        }

        protected void gvSucursales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
                return;
            Sucursales datos = (Sucursales)e.Row.DataItem;
            GridView gvPuntoEmision = (GridView)e.Row.FindControl("gvPuntoEmision");

            gvPuntoEmision.DataSource = datos.destalles;
            gvPuntoEmision.DataBind();
        }

        protected void gvSucursales_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }
    }
}