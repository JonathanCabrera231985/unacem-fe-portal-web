using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using clibLogger;
using DataExpressWeb;

namespace Administracion
{
    public partial class modificar_roles : System.Web.UI.Page
    {
        string idRol;
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

                if (!Page.IsPostBack)
                {
                    if (Session["idUser"] != null)
                    {
                        int id_int;
                        if (int.TryParse(Request.QueryString["id"].ToString(), out id_int))
                        {
                            idRol = id_int.ToString();
                            DB.Conectar();
                            DB.CrearComandoProcedimiento("PA_consulta_rol");
                            DB.AsignarParametroProcedimiento("@idRol", System.Data.DbType.String, idRol);
                            using (DbDataReader DR = DB.EjecutarConsulta())
                            {
                                if (DR.Read())
                                {
                                    tbRol.Text = DR[1].ToString();
                                    cbCrear_cliente.Checked = Convert.ToBoolean(DR[2].ToString());
                                    cbCrear_admin.Checked = Convert.ToBoolean(DR[3].ToString());
                                    cbConsulta_propias.Checked = Convert.ToBoolean(DR[4].ToString());
                                    cbConsulta_todas.Checked = Convert.ToBoolean(DR[5].ToString());
                                    cbReportesSucursales.Checked = Convert.ToBoolean(DR[6].ToString());
                                    cbReportesGlobales.Checked = Convert.ToBoolean(DR[7].ToString());
                                    cbModificarEmpleado.Checked = Convert.ToBoolean(DR[8].ToString());
                                    cbAsignar_rol.Checked = Convert.ToBoolean(DR[9].ToString());
                                    cbEnvio_fac.Checked = Convert.ToBoolean(DR[10].ToString());
                                    cbAgregar_doc.Checked = Convert.ToBoolean(DR[11].ToString());
                                    cbValidarFacturas.Checked = Convert.ToBoolean(DR[14].ToString());
                                    cbAceptarFacturas.Checked = Convert.ToBoolean(DR[15].ToString());
                                    cbVerFacturasRecibidas.Checked = Convert.ToBoolean(DR[13].ToString());
                                }
                            }
                            DB.Desconectar();
                        }
                    }
                }
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }

        }

        protected void bModificar_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                int id_int;
                if (int.TryParse(Request.QueryString["id"].ToString(), out id_int))
                {
                    idRol = id_int.ToString();
                    if (!consultaRol(tbRol.Text, idRol))
                    {
                        if (cbCrear_cliente.Checked || cbCrear_admin.Checked || cbConsulta_propias.Checked || cbConsulta_todas.Checked ||
                            cbReportesSucursales.Checked || cbReportesGlobales.Checked || cbAsignar_rol.Checked || cbEnvio_fac.Checked ||
                            cbAgregar_doc.Checked || cbModificarEmpleado.Checked ||
                            cbValidarFacturas.Checked || cbAceptarFacturas.Checked || cbVerFacturasRecibidas.Checked)
                        {
                            DB.Conectar();
                            DB.CrearComandoProcedimiento("PA_modificar_rol");
                            DB.AsignarParametroProcedimiento("@idRol", System.Data.DbType.String, idRol);
                            DB.AsignarParametroProcedimiento("@descripcion", System.Data.DbType.String, tbRol.Text);
                            DB.AsignarParametroProcedimiento("@crear_cliente", System.Data.DbType.Byte, Convert.ToByte(cbCrear_cliente.Checked));
                            DB.AsignarParametroProcedimiento("@crear_admin_sucursal", System.Data.DbType.Byte, Convert.ToByte(cbCrear_admin.Checked));
                            DB.AsignarParametroProcedimiento("@consultar_facturas_propias", System.Data.DbType.Byte, Convert.ToByte(cbConsulta_propias.Checked));
                            DB.AsignarParametroProcedimiento("@consultar_todas_facturas", System.Data.DbType.Byte, Convert.ToByte(cbConsulta_todas.Checked));
                            DB.AsignarParametroProcedimiento("@reportesSucursales", System.Data.DbType.Byte, Convert.ToByte(cbReportesSucursales.Checked));
                            DB.AsignarParametroProcedimiento("@reportesGlobales", System.Data.DbType.Byte, Convert.ToByte(cbReportesGlobales.Checked));
                            DB.AsignarParametroProcedimiento("@modificarEmpleado", System.Data.DbType.Byte, Convert.ToByte(cbModificarEmpleado.Checked));
                            DB.AsignarParametroProcedimiento("@asignacion_roles", System.Data.DbType.Byte, Convert.ToByte(cbAsignar_rol.Checked));
                            DB.AsignarParametroProcedimiento("@envio_facturas_email", System.Data.DbType.Byte, Convert.ToByte(cbEnvio_fac.Checked));
                            DB.AsignarParametroProcedimiento("@agregar_documento", System.Data.DbType.Byte, Convert.ToByte(cbAgregar_doc.Checked));
                            DB.AsignarParametroProcedimiento("@validarFactura", System.Data.DbType.Byte, Convert.ToByte(cbValidarFacturas.Checked));
                            DB.AsignarParametroProcedimiento("@aceptarFactura", System.Data.DbType.Byte, Convert.ToByte(cbAceptarFacturas.Checked));
                            DB.AsignarParametroProcedimiento("@Recepcion", System.Data.DbType.Byte, Convert.ToByte(cbVerFacturasRecibidas.Checked));
                            DB.EjecutarConsulta1();
                            DB.Desconectar();
                            Response.Redirect(Server.HtmlEncode("roles.aspx"));
                        }
                        else
                        {
                            lMsj.Text = "Seleccionar una opción";
                        }
                    }
                    else
                    {
                        lMsj.Text = "El Rol ya Existe.";
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
        public Boolean consultaRol(string descripcion, string idRol)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Conectar();
                DB.CrearComando("select descripcion from Roles WITH (NOLOCK)  where descripcion=@descripcion  and idRol<>@idRol");
                DB.AsignarParametroCadena("@descripcion", descripcion);
                DB.AsignarParametroCadena("@idRol", idRol);
                using (DbDataReader DR = DB.EjecutarConsulta())
                {
                    while (DR.Read())
                    {
                        DB.Desconectar();
                        return true;
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
            return false;
        }
        protected void bCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(Server.HtmlEncode("roles.aspx"));
        }
    }
}