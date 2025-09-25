using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using clibLogger;
using DataExpressWeb;

namespace Administracion
{
    public partial class modificarSucursal : System.Web.UI.Page
    {
        string idSucursal;
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
                    int id_int;
                    if (int.TryParse(Request.QueryString["id"].ToString(), out id_int))
                    {
                        idSucursal = id_int.ToString();// Request.QueryString.Get("id");
                        DB.Conectar();
                        DB.CrearComandoProcedimiento("PA_consultaSucursal");
                        DB.AsignarParametroProcedimiento("@idSucursal", System.Data.DbType.String, idSucursal);
                        using (DbDataReader DR = DB.EjecutarConsulta())
                        {
                            DR.Read();
                            tbClave.Text = DR["clave"].ToString();
                            tbSucursal.Text = DR["sucursal"].ToString();
                            tbDireccion.Text = DR["domicilio"].ToString();
                            txtCorreos.Text = DR["correo"].ToString();
                        }
                        DB.Desconectar();
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

        protected void bModificar_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                int id_int;
                if (int.TryParse(Request.QueryString["id"].ToString(), out id_int))
                {
                    idSucursal = id_int.ToString();// Request.QueryString.Get("id");
                    if (!ValidarSucursal(tbClave.Text, idSucursal))
                    {
                        DB.Conectar();
                        DB.CrearComandoProcedimiento("PA_modificarSucursal");
                        DB.AsignarParametroProcedimiento("@idSucursal", System.Data.DbType.String, idSucursal);
                        DB.AsignarParametroProcedimiento("@clave", System.Data.DbType.String, tbClave.Text);
                        DB.AsignarParametroProcedimiento("@sucursal", System.Data.DbType.String, tbSucursal.Text);
                        DB.AsignarParametroProcedimiento("@domicilio", System.Data.DbType.String, tbDireccion.Text);
                        DB.AsignarParametroProcedimiento("@correo", System.Data.DbType.String, txtCorreos.Text);
                        DB.EjecutarConsulta1();
                        DB.Desconectar();
                        Response.Redirect(Server.HtmlEncode("sucursales.aspx"));
                    }
                    else
                    {
                        lMsj.Text = "La clave de sucursal ya existe.";
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
        private Boolean ValidarSucursal(string claveSucursal, string idSuc)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Conectar();
                DB.CrearComando("select * from sucursales WITH (NOLOCK)  where clave=@clave and idSucursal<>@idSuc and eliminado='False' ");
                DB.AsignarParametroCadena("@clave", claveSucursal);
                DB.AsignarParametroCadena("@idSuc", idSuc);
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
            Response.Redirect(Server.HtmlEncode("sucursales.aspx"));
        }
    }
}