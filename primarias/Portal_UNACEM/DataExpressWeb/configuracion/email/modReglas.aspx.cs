using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using clibLogger;

namespace DataExpressWeb
{
    public partial class modReglas : System.Web.UI.Page
    {
        string idRegla;
        string user = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            idRegla = Request.QueryString.Get("regladi");
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
                    DB.CrearComando("select nombreRegla,estadoRegla,emailsRegla,receptor from EmailsReglas WITH (NOLOCK)  where  idEmailRegla=@idRegla");
                    DB.AsignarParametroCadena("@idRegla", idRegla);
                    using (DbDataReader DR = DB.EjecutarConsulta())
                    {
                        while (DR.Read())
                        {
                            tbNombre.Text = DR[0].ToString();
                            //ddlEstado.SelectedValue = DR[1].ToString();
                            //Label1.Text = DR[1].ToString();
                            tbEmail.Text = DR[2].ToString();
                            tbRFC.Text = DR[3].ToString();
                            if (Convert.ToBoolean(DR[1]))
                            {
                                ddlEstado.SelectedValue = "1";
                            }
                            else
                            {
                                ddlEstado.SelectedValue = "0";
                            }
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

        protected void bActualizar_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            string a = "";
            try
            {
                DB.Conectar();
                DB.CrearComando("select IDEREC from Receptor WITH (NOLOCK)  where RFCREC=@RFC");
                DB.AsignarParametroCadena("@RFC", tbRFC.Text);
                using (DbDataReader DR = DB.EjecutarConsulta())
                {
                    while (DR.Read())
                    {
                        a = DR[0].ToString();
                    }
                }
                DB.Desconectar();
                if (!String.IsNullOrEmpty(a))
                {
                    DB.Conectar();
                    DB.CrearComandoProcedimiento("PA_modifica_ReglasEmail");
                    DB.AsignarParametroProcedimiento("@idRegla", System.Data.DbType.Int32, Convert.ToInt32(idRegla));
                    DB.AsignarParametroProcedimiento("@nombreRegla", System.Data.DbType.String, tbNombre.Text);
                    DB.AsignarParametroProcedimiento("@estado", System.Data.DbType.Byte, ddlEstado.SelectedValue);
                    DB.AsignarParametroProcedimiento("@emailsRegla", System.Data.DbType.String, tbEmail.Text);
                    DB.AsignarParametroProcedimiento("@rfcrec", System.Data.DbType.String, tbRFC.Text);
                    DB.EjecutarConsulta1();
                    DB.Desconectar();
                    Response.Redirect("reglas.aspx");
                }
                else
                {
                    lMensaje.Text = "El RUC proporcionado no Existe";
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