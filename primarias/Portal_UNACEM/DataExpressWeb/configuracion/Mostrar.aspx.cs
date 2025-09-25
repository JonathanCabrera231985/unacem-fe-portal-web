using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using CriptoSimetrica;
using clibLogger;
using DataExpressWeb;

namespace ups
{
    public partial class Mostrar : System.Web.UI.Page
    {
        private AES Cs = new AES();
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
                this.tbDirllaves.Attributes["type"] = "password";
                this.txtPwdRecepcion.Attributes["type"] = "password";
                if (!Page.IsPostBack)
                {
                    DB.Conectar();
                    DB.CrearComandoProcedimiento("PA_consultarParametros");
                    DB.AsignarParametroProcedimiento("@idparametro", System.Data.DbType.String, 3);
                    using (DbDataReader DR = DB.EjecutarConsulta())
                    {
                        while (DR.Read())
                        {
                            tbDirdocs.Text = DR[1].ToString();
                            tbDirtxt.Text = DR[2].ToString();
                            tbDirrespaldo.Text = DR[3].ToString();
                            txtXmlBase.Text = DR[15].ToString();
                            tbDircerti.Text = DR[13].ToString();
                            if (!string.IsNullOrEmpty(DR[14].ToString()))
                                tbDirllaves.Text = Cs.desencriptar(DR[14].ToString(), "CIMAIT");
                            txtIntentos.Text = DR[16].ToString();
                            txtmailRecep.Text = DR[18].ToString();
                            txtPwdRecepcion.Text = DR[19].ToString();
                        }
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

        protected void bModificar_Click(object sender, EventArgs e)
        {
            tbDirtxt.ReadOnly = false;
            tbDirdocs.ReadOnly = false;
            tbDirtxt.ReadOnly = false;
            tbDirrespaldo.ReadOnly = false;
            tbDircerti.ReadOnly = false;
            tbDirllaves.ReadOnly = false;
            txtXmlBase.ReadOnly = false;
            txtIntentos.ReadOnly = false;
            txtmailRecep.ReadOnly = false;
            txtPwdRecepcion.ReadOnly = false;
            bModificar.Visible = false;
            bActualizar.Visible = true;
        }

        protected void bActualizar_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                DB.CrearComandoProcedimiento("PA_modificarParametros");
                DB.AsignarParametroProcedimiento("@idparametro", System.Data.DbType.Int16, 0);
                DB.AsignarParametroProcedimiento("@dirdocs", System.Data.DbType.String, tbDirdocs.Text);
                DB.AsignarParametroProcedimiento("@dirtxt", System.Data.DbType.String, tbDirtxt.Text);
                DB.AsignarParametroProcedimiento("@dirrespaldo", System.Data.DbType.String, tbDirrespaldo.Text);
                DB.AsignarParametroProcedimiento("@dircertificados", System.Data.DbType.String, tbDircerti.Text);
                DB.AsignarParametroProcedimiento("@dirllaves", System.Data.DbType.String, Cs.encriptar(tbDirllaves.Text, "CIMAIT"));
                DB.AsignarParametroProcedimiento("@dirXMLbase", System.Data.DbType.String, txtXmlBase.Text);
                DB.AsignarParametroProcedimiento("@intentosautorizacion", System.Data.DbType.Int32, this.txtIntentos.Text);
                DB.AsignarParametroProcedimiento("@dirRecepcion", System.Data.DbType.String, this.txtDirRecep.Text);
                DB.AsignarParametroProcedimiento("@correoRecepcion", System.Data.DbType.String, this.txtmailRecep.Text);
                DB.AsignarParametroProcedimiento("@passRecepcion", System.Data.DbType.String, this.txtPwdRecepcion.Text);
                using (var x = DB.EjecutarConsulta())
                {
                }
                DB.Desconectar();
                tbDirtxt.ReadOnly = true;
                tbDirdocs.ReadOnly = true;
                tbDirtxt.ReadOnly = true;
                tbDirrespaldo.ReadOnly = true;
                tbDircerti.ReadOnly = true;
                tbDirllaves.ReadOnly = true;
                txtXmlBase.ReadOnly = true;
                txtIntentos.ReadOnly = true;
                txtmailRecep.ReadOnly = true;
                txtPwdRecepcion.ReadOnly = true;
                bActualizar.Visible = false;
                bModificar.Visible = true;
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

        protected void tbDirtxt_TextChanged(object sender, EventArgs e)
        {
        }
    }
}