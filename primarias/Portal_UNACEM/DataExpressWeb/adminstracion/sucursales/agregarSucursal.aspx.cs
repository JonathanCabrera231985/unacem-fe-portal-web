using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using clibLogger;
using DataExpressWeb;

namespace Administracion
{
    public partial class agregarSucursal : System.Web.UI.Page
    {
        string user = "";
        String rucEmpresa = "";
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
                if (Session["rucEmpresa"] != null)
                    rucEmpresa = Session["rucEmpresa"].ToString();
            }
            catch (Exception ex)
            {
                clsLogger.Graba_Log_Error(ex.Message);
                DB.Desconectar();
                throw;
            }
            finally
            {
                DB.Desconectar();
            }
        }

        protected void bGuardar_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                if (!ValidarSucursal(tbClave.Text))
                {
                    DB.Conectar();
                    DB.CrearComandoProcedimiento("PA_inserta_sucursal");
                    DB.AsignarParametroProcedimiento("@clave", System.Data.DbType.String, tbClave.Text);
                    DB.AsignarParametroProcedimiento("@sucursal", System.Data.DbType.String, tbSucursal.Text);
                    DB.AsignarParametroProcedimiento("@domicilio", System.Data.DbType.String, tbDireccion.Text);
                    DB.AsignarParametroProcedimiento("@eliminado", System.Data.DbType.Byte, false);
                    DB.AsignarParametroProcedimiento("@correo", System.Data.DbType.String, txtCorreos.Text);
                    DB.AsignarParametroProcedimiento("@IDEEMI", System.Data.DbType.String, rucEmpresa);
                    DB.EjecutarConsulta1();
                    DB.Desconectar();
                    Response.Redirect("sucursales.aspx");
                }
                else
                {
                    lMsj.Text = "La sucursal ya existe.";
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

        private Boolean ValidarSucursal(string claveSucursal)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Conectar();
                DB.CrearComando("select * from sucursales WITH (NOLOCK)  where clave=@clave and eliminado='False' and IDEEMI = @IDEEMI ");
                DB.AsignarParametroCadena("@clave", claveSucursal);
                DB.AsignarParametroCadena("@IDEEMI", rucEmpresa);
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
    }
}