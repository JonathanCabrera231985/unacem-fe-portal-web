using System;
using System.IO;
using System.Data.Common;
using Datos;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using clibLogger;

namespace DataExpressWeb
{
    public partial class CargaContigencia : System.Web.UI.Page
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

        protected void bSubir_Click2(object sender, EventArgs e)
        {
            if (this.exami.HasFile)
            {
                if (!exami.FileName.Contains(".txt"))//aument
                {
                    msj.Text = "El Archivo no es formato txt";
                    process.Enabled = false;
                }
                else
                {
                    try
                    {
                        if (this.exami.HasFile)
                        {
                            exami.SaveAs(Server.MapPath("\\manual\\docu.txt"));
                            msj.Text = ("El archivo: "
                                        + (exami.FileName + (" fue recibido satisfactoriamente. (Tamaño: "
                                        + (exami.PostedFile.ContentLength + " Bytes)<br> El archivo fue renombrado a <b>docu.txt</b>"))));
                            msj.Visible = true;
                            process.Enabled = true;
                            habilitaBotones(false);
                            if (!verificaRuc())
                            {
                                msj.Text = "El RUC del archivo no coincide con el de la empresa";
                                process.Enabled = false;
                                eliminaArchivo();
                            }
                        }
                        else
                        {
                            msj.Text = "El archivo no fue recibido.";
                            process.Enabled = false;
                            habilitaBotones(true);
                        }
                    }
                    catch (Exception ex)
                    {
                        msj.Text = "Error al cargar el archivo: " + ex.Message.ToString();
                    }
                }
            }
            else
            {
                msj.Text = "No hay algún archivo seleccionado.";
            }
        }

        protected void process_Click(object sender, EventArgs e)
        {
            String cadenaconexion = ConfigurationManager.ConnectionStrings["dataexpressConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(cadenaconexion);
            DataTable dt = new DataTable();
            DataColumn colInt32 = new DataColumn("idClaveContingencia");
            colInt32.DataType = System.Type.GetType("System.Int32");
            dt.Columns.Add(colInt32);
            DataColumn claveString = new DataColumn("clave");
            claveString.DataType = System.Type.GetType("System.String");
            dt.Columns.Add(claveString);
            DataColumn estadoString = new DataColumn("estado");
            claveString.DataType = System.Type.GetType("System.String");
            dt.Columns.Add(estadoString);
            DataColumn colDateTime = new DataColumn("uso");
            colDateTime.DataType = System.Type.GetType("System.DateTime");
            dt.Columns.Add(colDateTime);
            DataColumn rucString = new DataColumn("ruc");
            rucString.DataType = System.Type.GetType("System.String");
            dt.Columns.Add(rucString);
            DataColumn tipoString = new DataColumn("tipo");
            rucString.DataType = System.Type.GetType("System.String");
            dt.Columns.Add(tipoString);
            DataRow row;
            string filepath = Server.MapPath("\\manual\\docu.txt");
            StreamReader sr = new StreamReader(filepath);
            string linea = sr.ReadLine();
            if (!string.IsNullOrEmpty(linea))
            {
                if (linea.Length == 37)
                {
                    if (!string.IsNullOrEmpty(ejecuta_query1("select top 1 * From ClavesContignencia WITH (NOLOCK) where clave='" + linea.Trim() + "'")))
                    {
                        msj.Text = "El Archivo ya fue cargado";
                        process.Enabled = false;
                    }
                    else
                    {
                        row = dt.NewRow();
                        row["clave"] = linea;
                        row["estado"] = "0";
                        row["uso"] = DateTime.Now;
                        row["ruc"] = linea.Substring(0, 13);
                        row["tipo"] = linea.Substring(13, 1);
                        dt.Rows.Add(row);

                        while (!sr.EndOfStream)
                        {
                            linea = sr.ReadLine();
                            if (!string.IsNullOrEmpty(linea))
                            {
                                row = dt.NewRow();
                                row["clave"] = linea;
                                row["estado"] = "0";
                                row["uso"] = DateTime.Now;
                                row["ruc"] = linea.Substring(0, 13);
                                row["tipo"] = linea.Substring(13, 1);
                                dt.Rows.Add(row);
                            }
                        }
                        SqlBulkCopy bc = new SqlBulkCopy(cadenaconexion, SqlBulkCopyOptions.TableLock);
                        try
                        {
                            bc.DestinationTableName = "ClavesContignencia";
                            bc.BatchSize = dt.Rows.Count;
                            con.Open();
                            bc.WriteToServer(dt);
                            msj.Text = "Archivo cargado con éxito.";
                            process.Enabled = false;
                        }
                        catch (Exception ex)
                        {
                            msj.Text = "Error al insertar registros: " + ex.Message.ToString();
                        }
                        finally
                        {
                            bc.Close();
                            con.Close();
                        }
                    }
                }
                else
                {
                    msj.Text = "El dato del archivo no es válido. Tamaño: " + linea.Length;
                }
            }
            else
            {
                msj.Text = "El archivo no tiene datos.";
            }
            sr.Dispose();
            sr.Close();
            eliminaArchivo();
        }
        private Boolean verificaRuc()
        {
            Boolean rpt = false;
            if (System.IO.File.Exists(Server.MapPath("\\manual\\docu.txt")))
            {
                StreamReader sr = new StreamReader(Server.MapPath("\\manual\\docu.txt"));
                try
                {
                    string va = sr.ReadLine();//aument
                    if (!string.IsNullOrEmpty(va))
                    {
                        string verificarchivo = va.Substring(0, 13);

                        if (!string.IsNullOrEmpty(verificarchivo))
                        {
                            if (!string.IsNullOrEmpty(ejecuta_query1("select top 1 * From EMISOR WITH (NOLOCK) where RFCEMI='" + verificarchivo.Trim() + "'")))
                            {
                                rpt = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    msj.Text = "Error en verificar RUC: " + ex.Message;
                }
                finally
                {
                    sr.Dispose();
                    sr.Close();
                }
            }
            else
            {
                msj.Text = "El archivo: " + Server.MapPath("\\manual\\docu.txt") + " no existe";
            }

            return rpt;
        }

        private void eliminaArchivo()
        {
            if (System.IO.File.Exists(Server.MapPath("\\manual\\docu.txt")))
            {
                try
                {
                    System.IO.File.Delete(Server.MapPath("\\manual\\docu.txt"));
                }
                catch (System.IO.IOException ex)
                {
                    msj.Text += "Error al borrar el archivo: " + ex.Message;
                }
                habilitaBotones(true);
            }
        }

        private String ejecuta_query1(String query)
        {
            var DB = new BasesDatos();
            String retorno = "";
            try
            {
                DB = new BasesDatos();
                DB.Conectar();
                DB.CrearComando(query);
                using (DbDataReader DR = DB.EjecutarConsulta())
                {
                    while (DR.Read())
                    {
                        retorno = DR[0].ToString();
                    }
                    DB.Desconectar();
                }
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
            return retorno;
        }

        private void habilitaBotones(bool p_habilita)
        {
            exami.Enabled = p_habilita;
            bSubir.Enabled = p_habilita;
        }
    }
}