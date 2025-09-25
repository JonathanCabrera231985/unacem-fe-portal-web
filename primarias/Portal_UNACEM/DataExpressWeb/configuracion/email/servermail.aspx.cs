using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using clibLogger;
using CriptoSimetrica;

namespace DataExpressWeb.configuracion.email
{
    public partial class servermail : System.Web.UI.Page
    {
        string user = "";
        private AES cs = new AES();
        string psmtp = "";
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
                    DB.Conectar();
                    DB.CrearComandoProcedimiento("PA_consultarParametros");
                    DB.AsignarParametroProcedimiento("@idparametro", System.Data.DbType.String, 3);
                    using (DbDataReader DR = DB.EjecutarConsulta())
                    {
                        while (DR.Read())
                        {
                            tbServidor.Text = DR[6].ToString();
                            tbPuerto.Text = DR[7].ToString();
                            tbUsuario.Text = DR[9].ToString();
                            psmtp = cs.desencriptar(DR[10].ToString(), "CIMAIT");
                            tbPassword.Text = psmtp;
                            tbEmailEnvio.Text = DR[11].ToString();
                            cbSSL.Checked = Convert.ToBoolean(DR[8].ToString());
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
        protected void bModificar_Click(object sender, EventArgs e)
        {
            tbServidor.ReadOnly = false;
            tbPuerto.ReadOnly = false;
            tbUsuario.ReadOnly = false;
            tbPassword.ReadOnly = false;
            tbEmailEnvio.ReadOnly = false;
            cbSSL.Enabled = true;
            bModificar.Visible = false;
            bActualizar.Visible = true;
        }

        protected void bActualizar_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Conectar();
                DB.CrearComandoProcedimiento("PA_modificarParametrosEmail");
                DB.AsignarParametroProcedimiento("@idparametro", System.Data.DbType.Int16, 0);
                DB.AsignarParametroProcedimiento("@servidor", System.Data.DbType.String, tbServidor.Text);
                DB.AsignarParametroProcedimiento("@puerto", System.Data.DbType.String, tbPuerto.Text);
                DB.AsignarParametroProcedimiento("@usuario", System.Data.DbType.String, tbUsuario.Text);
                if(!tbPassword.Text.Equals(""))
                {
                    DB.AsignarParametroProcedimiento("@password", System.Data.DbType.String, cs.encriptar(tbPassword.Text, "CIMAIT"));
                }
                else
                    DB.AsignarParametroProcedimiento("@password", System.Data.DbType.String, cs.encriptar(psmtp, "CIMAIT"));
                DB.AsignarParametroProcedimiento("@emailenvio", System.Data.DbType.String, tbEmailEnvio.Text);
                DB.AsignarParametroProcedimiento("@ssl", System.Data.DbType.Byte, cbSSL.Checked);
                using (var x = DB.EjecutarConsulta())
                {
                }

                DB.Desconectar();
                tbServidor.ReadOnly = true;
                tbPuerto.ReadOnly = true;
                tbUsuario.ReadOnly = true;
                tbPassword.ReadOnly = true;
                tbEmailEnvio.ReadOnly = true;
                cbSSL.Enabled = false;
                bModificar.Visible = true;
                bActualizar.Visible = false;
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