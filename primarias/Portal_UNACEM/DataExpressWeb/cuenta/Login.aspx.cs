using System;
using Datos;
using System.Data.Common;
using CriptoSimetrica;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace DataExpressWeb
{


    public partial class Login : System.Web.UI.Page
    {
        private BasesDatos DB = new BasesDatos();
        private String msj;
        private string idUsuario, idSucursal, nombreEmpleado, nombreSucursal, rfcCliente, rucEmpresa;
        private string categoria;
        private int rol;
        private AES Cs = new AES();


        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (ViewState["tbPass"] != null)
            {
                tbPass.Attributes["value"] = ViewState["tbPass"].ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            msj = "";
            categoria = "Login";
            rol = 0;



            if (!IsPostBack)
            {
                HttpCookie cookie = Request.Cookies["cookiesAceptadas"];
                if (cookie != null && cookie.Value == "true")
                {
                    modalCookies.Style["display"] = "none"; // Oculta el modal
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mostrarCookies", "mostrarModalCookies();", true);
                }
            }
            else
            {

                if (!checkPolitica.Checked)
                {
                    if (!valida_Politica_Privacidad(tbRfcuser.Text.Trim()))
                    {
                        if (string.IsNullOrWhiteSpace(tbRfcuser.Text) || string.IsNullOrWhiteSpace(tbPass.Text))
                        {
                            return;
                        }
                        ScriptManager.RegisterStartupScript(upLogin, upLogin.GetType(), "mostrarPrivacidad", "mostrarModalPrivacidad();", true);
                        lMensaje.Visible = false;
                        //RestaurarValorPassword();
                        return;
                    }
                }

            }



        }

        protected void bSesion_Click(object sender, EventArgs e)
        {

            if (!valida_Politica_Privacidad(tbRfcuser.Text.Trim()))
            {
                if (!checkPolitica.Checked)
                {
                    if (string.IsNullOrWhiteSpace(tbRfcuser.Text) || string.IsNullOrWhiteSpace(tbPass.Text))
                    {
                        return;
                    }
                    ScriptManager.RegisterStartupScript(upLogin, upLogin.GetType(), "mostrarPrivacidad", "mostrarModalPrivacidad();", true);
                    lMensaje.Visible = false;
                    GuardarValorPassword();
                    return;
                }


            }

            string deviceId;
            if (Request.Cookies["DeviceID"] == null)
            {
                deviceId = Guid.NewGuid().ToString();
                HttpCookie cookie = new HttpCookie("DeviceID", deviceId);
                cookie.Expires = DateTime.Now.AddYears(1); // Persistente por 1 año
                Response.Cookies.Add(cookie);
            }
            //agregar un cookie como aceptado
            else
            {
                deviceId = Request.Cookies["DeviceID"].Value;
            }

            string ip = Request.UserHostAddress;
            string userAgent = Request.UserAgent;

            string latitud = hfLatitud.Value;
            string longitud = hfLongitud.Value;

            string login = tbPass.Text.Trim();


            if (!login.Equals(tbRfcuser.Text.Trim()))
            {
                login = Cs.encriptar(tbPass.Text, "CIMAIT");
            }
            else
            {
                Session["rfcUser"] = tbRfcuser.Text.Trim();
                Response.Redirect(Server.HtmlEncode("../cuenta/Registro.aspx"));
            }

            double id_int; string user = "";
            if (double.TryParse(tbRfcuser.Text, out id_int))
            {
                user = tbRfcuser.Text;
            }
            else
            {
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
            DB.CrearComandoProcedimiento("PA_Login");
            DB.AsignarParametroProcedimiento("@USER", System.Data.DbType.String, user);
            DB.AsignarParametroProcedimiento("@PASSWORD", System.Data.DbType.String, login);
            DbDataReader DR = DB.EjecutarConsulta();
            DR.Read();
            msj = DR[0].ToString();
            rol = Convert.ToInt16(DR[1].ToString());
            idUsuario = DR[2].ToString();
            idSucursal = DR[3].ToString();
            nombreEmpleado = DR[4].ToString();
            nombreSucursal = DR[5].ToString();
            rfcCliente = DR[6].ToString();
            rucEmpresa = DR["RucEmpresa"].ToString();
            if (String.IsNullOrEmpty(rfcCliente))
            {
                rfcCliente = "R---";
            }
            DB.Desconectar();

            if (!msj.Equals("00000"))
            {
                DB.Conectar();
                DB.CrearComandoProcedimiento("PA_Errores");
                DB.AsignarParametroProcedimiento("@CODIGO", System.Data.DbType.String, msj);
                DbDataReader DRE = DB.EjecutarConsulta();
                DRE.Read();
                lMensaje.Text = DRE[0].ToString();
                lMensaje.Visible = true;
                DB.Desconectar();

                return; // No continuar si hay error
            }

            // Usuario autenticado
            Session["rfcUser"] = tbRfcuser.Text;
            Session["rolUser"] = rol;
            Session["idUser"] = idUsuario;
            Session["sucursalUser"] = idSucursal;
            Session["nombreSucursalUser"] = nombreSucursal;
            Session["nombreEmpleado"] = nombreEmpleado;
            Session["rfcCliente"] = rfcCliente;
            Session["rucEmpresa"] = rucEmpresa;

            DB.Conectar();
            DB.CrearComandoProcedimiento("PA_consulta_rol");
            DB.AsignarParametroProcedimiento("@idRol", System.Data.DbType.Int16, rol);
            DbDataReader DRR = DB.EjecutarConsulta();
            DRR.Read();
            Session["crCliPermisos"] = DRR[2].ToString();
            Session["crAdminPermiso"] = DRR[3].ToString();
            Session["coFactPropias"] = DRR[4].ToString();
            Session["coFactTodas"] = DRR[5].ToString();
            Session["repSucursales"] = DRR[6].ToString();
            Session["repGlobales"] = DRR[7].ToString();
            Session["moEmpleado"] = DRR[8].ToString();
            Session["asRoles"] = DRR[9].ToString();
            Session["enFacturasEmail"] = DRR[10].ToString();
            Session["crDocumento"] = DRR[11].ToString();
            Session["Recepcion"] = DRR[13].ToString();
            Session["validarFactura"] = DRR[14].ToString();
            Session["aceptarFactura"] = DRR[15].ToString();
            DB.Desconectar();

            bool userConectado = false;
            string us = user.Substring(0, 5).ToUpper();
            if (user.Length >= 6)
            {
                DB.Conectar();
                if (us.Equals("EMPLE"))
                {
                    DB.CrearComando("select conectado from Empleados where userEmpleado = @userCliente");
                }
                else
                {
                    DB.CrearComando("select conectado from Clientes where userCliente = @userCliente");
                }

                DB.AsignarParametroCadena("@userCliente", user);
                DR = DB.EjecutarConsulta();
                while (DR.Read())
                {
                    if (DR["conectado"].ToString() == "0")
                        userConectado = true;
                }

                DB.Desconectar();
            }




            DB.Conectar();
            DB.CrearComandoProcedimiento("Insert_PoliticaPrivacidad");

            DB.AsignarParametroProcedimiento("@id_empleado", System.Data.DbType.Int32, Convert.ToInt32(idUsuario));
            DB.AsignarParametroProcedimiento("@username", System.Data.DbType.String, user);
            DB.AsignarParametroProcedimiento("@cedula_ruc", System.Data.DbType.String, rfcCliente);
            DB.AsignarParametroProcedimiento("@correo", System.Data.DbType.String, ""); // Puedes agregar correo si lo tienes
            DB.AsignarParametroProcedimiento("@fecha_inicio_sesion", System.Data.DbType.DateTime, DateTime.Now);
            DB.AsignarParametroProcedimiento("@accion_usuario", System.Data.DbType.String, "Aceptar");
            DB.AsignarParametroProcedimiento("@fecha_accion_usuario", System.Data.DbType.DateTime, DateTime.Now);
            DB.AsignarParametroProcedimiento("@latitud", System.Data.DbType.String, latitud);
            DB.AsignarParametroProcedimiento("@longitud", System.Data.DbType.String, longitud);
            DB.AsignarParametroProcedimiento("@id_dispositivo", System.Data.DbType.String, deviceId);
            DB.AsignarParametroProcedimiento("@ip", System.Data.DbType.String, ip);
            DB.AsignarParametroProcedimiento("@user_agent", System.Data.DbType.String, userAgent);
            DB.AsignarParametroProcedimiento("@id_sesion", System.Data.DbType.String, Session.SessionID);

            DB.EjecutarConsulta();
            DB.Desconectar();




            if (userConectado)
            {
                DB.Conectar();
                DB.CrearComando(@"update Clientes set conectado =1 where userCliente = @userCliente");
                DB.AsignarParametroCadena("@userCliente", user);
                DB.EjecutarConsulta();
                DB.Desconectar();
                Response.Redirect(Server.HtmlEncode("../cuenta/Registro.aspx"));

            }

            Response.Redirect(Server.HtmlEncode("../Default.aspx"));
        }


        protected void btnAcceptCookies_Click(object sender, EventArgs e)
        {
            if (Session["loginCorrecto"] != null && (bool)Session["loginCorrecto"])
            {
                Response.Redirect(Server.HtmlEncode("../Default.aspx"));
            }
        }

        private bool valida_Politica_Privacidad(string user)
        {
            bool acepto = false;
            try
            {
                DB.Conectar();
                DB.CrearComando(@"SELECT COUNT(*) FROM Politica_Privacidad 
                          WHERE username = @username AND accion_usuario = 'Aceptar'");
                DB.AsignarParametroProcedimiento("@username", System.Data.DbType.String, user);
                DbDataReader dr = DB.EjecutarConsulta();
                if (dr.Read() && Convert.ToInt32(dr[0]) > 0)
                {
                    acepto = true;
                }
                DB.Desconectar();
            }
            catch (Exception)
            {
                DB.Desconectar();
            }

            return acepto;
        }

        // Guarda manualmente el valor de cualquier TextBox Password en ViewState
        private void GuardarValorPassword()
        {
            ViewState["tbPass"] = tbPass.Text;
        }

        // Restaura el valor en el HTML del campo, ya que ASP.NET lo borra automáticamente
        private void RestaurarValorPassword()
        {
            if (ViewState["tbPass"] != null)
            {
                tbPass.Text = ViewState["tbPass"].ToString();
            }
        }

    }

}