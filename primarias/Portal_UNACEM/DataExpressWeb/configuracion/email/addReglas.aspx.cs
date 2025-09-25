using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using clibLogger;

namespace DataExpressWeb
{
    public partial class addReglas : System.Web.UI.Page
    {
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
            string a = "";
            try
            {
                if (!ValidarRegla(tbRFC.Text) && !tbRFC.Text.Equals("9999999999999"))
                {
                    DB.Conectar();
                    DB.CrearComandoProcedimiento("PA_insertar_ReglasEmail");
                    DB.AsignarParametroProcedimiento("@nombreRegla", System.Data.DbType.String, tbNombre.Text);
                    DB.AsignarParametroProcedimiento("@estado", System.Data.DbType.Byte, ddlEstado.SelectedValue);
                    DB.AsignarParametroProcedimiento("@emailsRegla", System.Data.DbType.String, tbEmail.Text);
                    DB.AsignarParametroProcedimiento("@rfcrec", System.Data.DbType.String, tbRFC.Text);
                    DB.AsignarParametroProcedimiento("@eliminado", System.Data.DbType.Byte, false);
                    DB.EjecutarConsulta1();
                    DB.Desconectar();
                    Response.Redirect("reglas.aspx");
                }
                else
                {
                    lMensaje.Text = "Ya existe una regla ligada a este RUC.";
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

        private Boolean ValidarRegla(string rfc)
        {
            return false;
        }
    }
}