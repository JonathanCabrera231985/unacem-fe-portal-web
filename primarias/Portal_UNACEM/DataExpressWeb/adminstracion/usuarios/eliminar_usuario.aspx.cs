using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using clibLogger;
using DataExpressWeb;

namespace Administracion
{
    public partial class eliminar_usuario : System.Web.UI.Page
    {
        string idEmpleado, idCliente;
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
                idEmpleado = Request.QueryString.Get("idmrdxbdi");
                idCliente = Request.QueryString.Get("idmbdi");

                if (!String.IsNullOrEmpty(idEmpleado))
                {
                    //elimnar

                    DB.Conectar();
                    DB.CrearComandoProcedimiento("PA_eliminar_empleado");
                    DB.AsignarParametroProcedimiento("@idEmpleado", System.Data.DbType.String, idEmpleado);
                    DB.EjecutarConsulta1();
                    DB.Desconectar();
                    Response.Redirect("empleados.aspx");

                }
                if (!String.IsNullOrEmpty(idCliente))
                {
                    //elimnar

                    DB.Conectar();
                    DB.CrearComandoProcedimiento("PA_eliminar_cliente");
                    DB.AsignarParametroProcedimiento("@idCliente", System.Data.DbType.String, idCliente);
                    DB.EjecutarConsulta1();
                    DB.Desconectar();
                    Response.Redirect("clientes.aspx");

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