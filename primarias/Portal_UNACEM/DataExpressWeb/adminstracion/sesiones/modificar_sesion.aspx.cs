using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using clibLogger;
using DataExpressWeb;

namespace Administracion
{
    public partial class modificar_sesion : System.Web.UI.Page
    {
        string idSesion;
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
                    idSesion = Request.QueryString.Get("id");
                    DB.Conectar();
                    DB.CrearComandoProcedimiento("PA_consulta_sesion");
                    DB.AsignarParametroProcedimiento("@idSesion", System.Data.DbType.String, idSesion);
                    using (DbDataReader DR = DB.EjecutarConsulta())
                    {
                        DR.Read();
                        tbDescripcion.Text = DR[1].ToString();
                        ddlConexiones.SelectedValue = DR[2].ToString();
                        ddlDuracion.SelectedValue = DR[3].ToString();
                        ddlIntentos.SelectedValue = DR[4].ToString();
                    }
                    DB.Desconectar();
                }
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }
        }

        protected void bModificarsesion_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                idSesion = Request.QueryString.Get("id");
                DB.Conectar();
                DB.CrearComandoProcedimiento("PA_modificar_sesion");
                DB.AsignarParametroProcedimiento("@idSesion", System.Data.DbType.String, idSesion);
                DB.AsignarParametroProcedimiento("@descripcion", System.Data.DbType.String, tbDescripcion.Text);
                DB.AsignarParametroProcedimiento("@conexiones_simultaneas", System.Data.DbType.Int16, ddlConexiones.SelectedValue);
                DB.AsignarParametroProcedimiento("@duracion_sesion", System.Data.DbType.String, ddlDuracion.SelectedValue);
                DB.AsignarParametroProcedimiento("@intentos", System.Data.DbType.Int16, ddlIntentos.SelectedValue);
                DB.EjecutarConsulta1();
                DB.Desconectar();
                Response.Redirect("sesiones.aspx");
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

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
        }

        protected void bCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("sesiones.aspx");
        }
    }
}