using System;
using System.Text;
using System.IO;
using System.Data.Common;
using Datos;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;
using Control;
using clibLogger;

namespace DataExpressWeb
{
    public partial class Validar : System.Web.UI.Page
    {
        string arc, pdf, bck;
        private Recepcion rece_doc;
        string user = "";

        string detalle = "", fecha = "", clavelog = "", detalletec = "";
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
                rece_doc = new Recepcion();
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

        protected void bSubir_Click(object sender, EventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();
            lMsj.Text = "";
            if (fuXML.HasFile)
            {
                Log log1 = new Log();
                clsLogger.Graba_Log_Info("ingresando al metodo de de subirDocvClick validar.cs");
                String fnXML = fuXML.FileName;
                clsLogger.Graba_Log_Info("recogiendo el nombre del documento seleccionado:: " + fnXML + "  vaida.cs");
                String feXML = System.IO.Path.GetExtension(fuXML.FileName).ToLower();
                clsLogger.Graba_Log_Info("recogiendo la extencion del documento seleccionado:: " + feXML + " vaida.cs");
                if (feXML == ".xml")
                {
                    try
                    {
                        clsLogger.Graba_Log_Info("entrando si es un archivo .xml vaida.cs");
                        string docXML = Encoding.UTF8.GetString(fuXML.FileBytes);
                        clsLogger.Graba_Log_Info("verificar el encoding del archivo seleccionado:: vaida.cs");
                        MemoryStream ms = new MemoryStream(fuXML.FileBytes);
                        clsLogger.Graba_Log_Info("guardando el archivo en la memoria del servidor vaida.cs");
                        ms.Flush();
                        ms.Position = 0;
                        clsLogger.Graba_Log_Info("cargando el documento vaida.cs");
                        xDoc.Load(ms);
                        clsLogger.Graba_Log_Info("terminando de cargar el documento xml vaida.cs");
                        rece_doc.procesarRecepcion(xDoc, "");
                        clsLogger.Graba_Log_Info("terminando de validar el documento de procesar recepcion vaida.cs");
                        tbMsj.Text = "Documento recibido. verificar su estado en la bandeja de recepción.";
                    }
                    catch (Exception ex)
                    {
                        clsLogger.Graba_Log_Error(ex.Message);
                        tbMsj.Text = "No se pudieron validar los archivos " + ex.Message;
                    }
                }
                else
                {
                    lMsj.Text = "Extension de archivo no reconocida";
                }
            }
            else
            {
                lMsj.Text = "No se encuentra disponible la ruta";
            }
        }

        private void consultarLog(string clave)
        {
            var DB = new BasesDatos();
            try
            {
                tbMsj.Text += "Resultado de la Validación. " + Environment.NewLine + Environment.NewLine;
                tbMsj.Text += "Clave de acceso: " + clave + Environment.NewLine + Environment.NewLine;
                DB.Conectar();
                DB.CrearComando(@"SELECT  detalle, fecha, numeroDocumento, detalleTecnico FROM LogErrorRecepcion WITH (NOLOCK)  WHERE numeroDocumento=@claveAcceso");
                DB.AsignarParametroCadena("@claveAcceso", clave);
                using (DbDataReader DR1 = DB.EjecutarConsulta())
                {
                    if (DR1.Read())
                    {
                        detalle = DR1[0].ToString();
                        fecha = DR1[1].ToString();
                        clavelog = DR1[2].ToString();
                        detalletec = DR1[3].ToString();

                        tbMsj.Text += fecha + Environment.NewLine;
                        tbMsj.Text += detalle + Environment.NewLine;
                        //tbMsj.Text += claveAcceso + Environment.NewLine + Environment.NewLine;
                        tbMsj.Text += detalletec + Environment.NewLine + Environment.NewLine;
                    }
                }
                DB.Desconectar();
            }
            catch (DbException e)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(e.Message);
                lMsj.Text = "No se puede consultar: " + e.Message;
            }
            finally
            {
                DB.Desconectar();
            }
        }

        private String consultarID1(string valor1, string valor2, string campo1, string campo2, string consulta)
        {
            var DB = new BasesDatos();
            try
            {
                String ide;
                DB.Conectar();
                DB.CrearComando(consulta + " " + campo1 + "=@a and " + campo2 + "=@b");
                DB.AsignarParametroCadena("@a", valor1.Replace("'", "''"));
                DB.AsignarParametroCadena("@b", valor1.Replace("'", "''"));
                using (DbDataReader DR = DB.EjecutarConsulta())
                {
                    while (DR.Read())
                    {
                        ide = DR[0].ToString();
                        DB.Desconectar();
                        return ide;
                    }
                }
                DB.Desconectar();
                return null;
            }
            catch (DbException e) { clsLogger.Graba_Log_Error(e.Message); return null; }
            finally
            {
                DB.Desconectar();
            }
        }
    }
}