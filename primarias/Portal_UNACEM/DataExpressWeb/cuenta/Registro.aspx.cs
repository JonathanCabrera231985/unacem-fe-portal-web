using System;
using Datos;
using CriptoSimetrica;
using System.Data.Common;
using clibLogger;

namespace DataExpressWeb
{
    public partial class Registro : System.Web.UI.Page
    {
        private AES Cs = new AES();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["rfcUser"] != null)
                this.tbRfcuser.Text = Session["rfcUser"].ToString();
            else
            {
                Response.Redirect(Server.HtmlEncode("../Default.aspx"));
            }
        }

        protected void bRegistrarse_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                string login = this.tbClaveActual.Text;
                double id_int;
                string user = "";
                if (double.TryParse(tbRfcuser.Text, out id_int))
                {
                    user = tbRfcuser.Text;
                    if (!login.Equals(tbRfcuser.Text.Trim()))
                        login = Cs.encriptar(tbClaveActual.Text, "CIMAIT");
                }
                else
                {
                    login = Cs.encriptar(tbClaveActual.Text, "CIMAIT");
                    string p = tbRfcuser.Text.Substring(0, 5).ToUpper();
                    if (p.Equals("EMPLE"))
                    {
                        user = tbRfcuser.Text;
                    }
                    else
                    {
                        user = "EMPLE" + tbRfcuser.Text;
                    }
                }
                DB.Conectar();
                DB.CrearComandoProcedimiento("PA_modificar_usuario");
                DB.AsignarParametroProcedimiento("@userEmpleado", System.Data.DbType.String, user);
                DB.AsignarParametroProcedimiento("@claveActual", System.Data.DbType.String, login);
                DB.AsignarParametroProcedimiento("@claveEmpleado", System.Data.DbType.String, Cs.encriptar(tbPass.Text, "CIMAIT"));
                using (DbDataReader DR1 = DB.EjecutarConsulta())
                {
                    if (DR1.Read())
                    {
                        lblMensaje.Text = "Clave modificada con éxito.";
                    }
                }
                DB.Desconectar();
                Session["rfcUser"] = null;
                Session.Abandon();
                Response.Redirect(Server.HtmlEncode("../Default.aspx"), false);
            }
            catch (Exception ex)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ex.Message);
                lblMensaje.Text = "Error: " + ex.Message;
            }
            finally
            {
                DB.Desconectar();
            }
        }

        private Boolean valida_claveUser(string p_login, string p_clave)
        {
            var DB = new BasesDatos();
            Boolean rpt = false;
            try
            {
                DB.Conectar();
                DB.CrearComandoProcedimiento("PA_Login");
                DB.AsignarParametroProcedimiento("@USER", System.Data.DbType.String, p_login);
                DB.AsignarParametroProcedimiento("@PASSWORD", System.Data.DbType.String, p_clave);
                using (DbDataReader DR = DB.EjecutarConsulta())
                {
                    DR.Read();
                    string msj = DR[0].ToString();
                    DB.Desconectar();
                    if (msj.Equals("00000"))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogger.Graba_Log_Error(ex.Message);
                DB.Desconectar();
            }
            finally
            {
                DB.Desconectar();
            }
            return rpt;
        }
    }
}