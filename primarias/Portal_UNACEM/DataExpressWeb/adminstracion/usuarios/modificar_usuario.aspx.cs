using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using System.Data;
using CriptoSimetrica;
using Control;
using System.Xml.Linq;
using clibLogger;
using DataExpressWeb;

namespace Administracion
{
    public partial class modificar_usuario : System.Web.UI.Page
    {
        private AES Cs = new AES();
        string idEmpleado, idCliente, idSucursal = "0";
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
                }

                if (!Page.IsPostBack)
                {
                    idEmpleado = Request.QueryString.Get("idmrdxbdi");
                    idCliente = Request.QueryString.Get("idmbdi");

                    if (!String.IsNullOrEmpty(idEmpleado))
                    {
                        lRol.Visible = true;
                        lRol.Visible = true;
                        SqlDataSource2.SelectCommand = "SELECT [idRol], [descripcion] FROM [Roles] WITH (NOLOCK)  WHERE idRol <> 1  AND eliminado ='False'";
                        SqlDataSource2.DataBind();
                        ddlRol.DataBind();
                        DB.Conectar();
                        DB.CrearComandoProcedimiento("PA_consulta_empleadosUpdate");
                        DB.AsignarParametroProcedimiento("@idEmpleado", System.Data.DbType.String, idEmpleado);
                        using (DbDataReader DR = DB.EjecutarConsulta())
                        {
                            DR.Read();
                            tbNombre.Text = DR[1].ToString();
                            tbUsername.Text = DR[2].ToString();
                            tbContraseña.Attributes.Add("Value", Cs.desencriptar(DR[3].ToString(), "CIMAIT"));
                           // tbContraseña.Text = Cs.desencriptar(DR[3].ToString(), "CIMAIT");
                            ddlStatus.SelectedValue = DR[7].ToString();
                            ddlRol.SelectedValue = DR[4].ToString();
                            ddlSesion.SelectedValue = DR[9].ToString();
                            ddlSucursal.SelectedValue = DR[10].ToString();
                            tbEmail.Text = DR["correo"].ToString();
                        }
                        DB.Desconectar();
                        extraerDatos datos = new extraerDatos();
                        gvSucursales.DataSource = datos.extraerDatosSucursales(Session["rucEmpresa"].ToString(),
                            @"select DISTINCT Sucursales.idSucursal, clave,sucursal,domicilio,  case when isnull( empleado_Sucursal.idSucursal, '') = '' then 0 else 1 end estado  from Sucursales WITH (NOLOCK) 
                        left join empleado_Sucursal WITH (NOLOCK)  on Sucursales.idSucursal = empleado_Sucursal.idSucursal and empleado_Sucursal.idEmpleado = " + idEmpleado + @" where eliminado = 0 and Sucursales.IDEEMI =",
                            @"SELECT DISTINCT A.idCaja, A.idSucursal, b.descripcion, A.ptoEmi,case when isnull( C.idSucursal, '') = '' then 0 else 1 end estado
                        FROM dbo.CajaSucursal A WITH (NOLOCK)  INNER JOIN dbo.Catalogo1_C B WITH (NOLOCK)  ON A.NumeroRentas = B.codigo AND b.tipo = 'Comprobante' left join dbo.empleado_Sucursal C WITH (NOLOCK)  ON A.idSucursal = C.idSucursal and C.idEmpleado =  " + idEmpleado + @"
                        AND C.idPuntoEmision = A.idCaja WHERE A.idSucursal = ");
                        gvSucursales.DataBind();
                        gvSucursales.Visible = true;
                    }
                    if (!String.IsNullOrEmpty(idCliente))
                    {
                        lRol.Visible = false;
                        lRol.Visible = false;
                        ddlRol.Visible = false;
                        DB.Conectar();
                        DB.CrearComandoProcedimiento("PA_consulta_clientesUpdate");
                        DB.AsignarParametroProcedimiento("@idCliente", System.Data.DbType.String, idCliente);
                        using (DbDataReader DR = DB.EjecutarConsulta())
                        {
                            DR.Read();
                            string clave = "";
                            try
                            {
                                clave = Cs.desencriptar(DR[3].ToString(), "CIMAIT");
                            }
                            catch (Exception)
                            {
                                clave = DR[2].ToString();
                            }
                            tbNombre.Text = DR[1].ToString();
                            tbUsername.Text = DR[2].ToString();
                            tbContraseña.Attributes.Add("Value", clave);
                           // tbContraseña.Text = clave;
                            ddlStatus.SelectedValue = DR[8].ToString();
                            ddlRol.SelectedValue = DR[4].ToString();
                            ddlSesion.SelectedValue = DR[9].ToString();
                            ddlSucursal.SelectedValue = DR[10].ToString();
                            tbEmail.Text = DR["correo"].ToString();
                        }
                        DB.Desconectar();
                        gvSucursales.Visible = false;
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

        private DataTable dtEmpleado()
        {
            DataTable dt = new DataTable("PA_modificar_empleado");
            dt.Columns.Add("idEmpleado", typeof(String));
            dt.Columns.Add("nombreEmpleado", typeof(String));
            dt.Columns.Add("userEmpleado", typeof(String));
            dt.Columns.Add("claveEmpleado", typeof(String));
            dt.Columns.Add("id_Rol", typeof(Int16));
            dt.Columns.Add("status", typeof(Byte));
            dt.Columns.Add("id_Sesion", typeof(Int32));
            dt.Columns.Add("id_Sucursal", typeof(Int32));
            dt.Columns.Add("documentoXML", typeof(String));
            dt.Columns.Add("correo", typeof(String));
            return dt;
        }

        protected void bModificar_Click1(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            Control.Log log1 = new Control.Log();
            idEmpleado = Request.QueryString.Get("idmrdxbdi");
            idCliente = Request.QueryString.Get("idmbdi");
            clsLogger.Graba_Log_Info("recuperando datos idEmpleado " + idEmpleado + " idCliente " + idCliente);
            try
            {
                if (!String.IsNullOrEmpty(idEmpleado))
                {
                    DataSet ds = new DataSet();
                    DataTable dt = dtEmpleado();
                    DataRow dr = dt.NewRow();
                    dr["idEmpleado"] = idEmpleado;
                    dr["nombreEmpleado"] = tbNombre.Text;
                    dr["userEmpleado"] = tbUsername.Text;
                    dr["claveEmpleado"] = Cs.encriptar(tbContraseña.Text, "CIMAIT");
                    dr["id_Rol"] = ddlRol.SelectedValue;
                    dr["status"] = ddlStatus.SelectedValue;
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
                    }
                    else
                    {
                        dr["id_Sucursal"] = idSucur;
                    }
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
                if (!String.IsNullOrEmpty(idCliente))
                {
                    clsLogger.Graba_Log_Info("Ejecuta SP PA_modificar_cliente");
                    try
                    {
                        DB.Conectar();
                        DB.CrearComandoProcedimiento("PA_modificar_cliente");
                        DB.AsignarParametroProcedimiento("@idCliente", System.Data.DbType.String, idCliente);
                        DB.AsignarParametroProcedimiento("@nombreCliente", System.Data.DbType.String, tbNombre.Text);
                        DB.AsignarParametroProcedimiento("@userCliente", System.Data.DbType.String, tbUsername.Text);
                        DB.AsignarParametroProcedimiento("@claveCliente", System.Data.DbType.String, Cs.encriptar(tbContraseña.Text, "CIMAIT"));
                        DB.AsignarParametroProcedimiento("@id_Rol", System.Data.DbType.Int16, 1);
                        DB.AsignarParametroProcedimiento("@status", System.Data.DbType.String, ddlStatus.SelectedValue);
                        DB.AsignarParametroProcedimiento("@id_Sesion", System.Data.DbType.Int16, 1);
                        DB.AsignarParametroProcedimiento("@id_Sucursal", System.Data.DbType.Int16, idSucursal);
                        DB.AsignarParametroProcedimiento("@correo", System.Data.DbType.String, tbEmail.Text);
                        DB.EjecutarConsulta1();
                        DB.Desconectar();
                        Response.Redirect("clientes.aspx");
                    }
                    catch (Exception ex)
                    {
                        clsLogger.Graba_Log_Info("Error PA_modificar_cliente" + ex.ToString());
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

        protected void bCancelar_Click(object sender, EventArgs e)
        {
            idEmpleado = Request.QueryString.Get("idmrdxbdi");
            idCliente = Request.QueryString.Get("idmbdi");
            if (!String.IsNullOrEmpty(idEmpleado))
            {
                Response.Redirect("empleados.aspx");
            }
            if (!String.IsNullOrEmpty(idCliente))
            {
                Response.Redirect("clientes.aspx");
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
            gvSucursales.PageIndex = e.NewPageIndex;
        }
    }
}