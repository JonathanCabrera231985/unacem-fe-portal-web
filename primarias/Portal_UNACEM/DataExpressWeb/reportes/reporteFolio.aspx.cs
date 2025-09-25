using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using clibLogger;

namespace DataExpressWeb
{
    public partial class reporteFolio : System.Web.UI.Page
    {
        string fi;
        string fechafin;
        string fechai;
        string nomfecha;
        string hora;
        string texto;
        string archivo;
        string rfcEmisor;
        DateTime FechaMinima;
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

                DB.Conectar();
                DB.CrearComando(@"SELECT rfcEmisor from ParametrosSistema WITH (NOLOCK) ");
                using (DbDataReader DR = DB.EjecutarConsulta())
                {
                    if (DR.Read())
                    {
                        rfcEmisor = DR[0].ToString();
                    }
                }
                DB.Desconectar();
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

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            fechafin = Calendar1.SelectedDate.ToShortDateString() + " 23:59:59.997";
            FechaMinima = Convert.ToDateTime(Convert.ToString(1) + "/" + Convert.ToString(Calendar1.SelectedDate.Month) + "/" + Convert.ToString(Calendar1.SelectedDate.Year));
            Label2.Text = FechaMinima.ToString() + "-" + fechafin;
        }

        protected void bGenerar_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            string rfcReceptor;
            string serie;
            string folio;
            string compueso;
            string fecha;
            string monto;
            string iva;
            string edo;
            string pedimentos;
            string fechapedimento;
            string aduana;
            string efecto;
            try
            {
                if (!Calendar1.SelectedDate.ToShortDateString().Equals("01/01/0001"))
                {
                    texto = "";
                    fechafin = Calendar1.SelectedDate.ToShortDateString() + " 23:59:59.997";
                    FechaMinima = Convert.ToDateTime(Convert.ToString(1) + "/" + Convert.ToString(Calendar1.SelectedDate.Month) + "/" + Convert.ToString(Calendar1.SelectedDate.Year));
                    fi = Convert.ToString(FechaMinima.ToShortDateString());
                    hora = Convert.ToString(FechaMinima.ToShortDateString() + FechaMinima.ToShortTimeString());
                    DB.Conectar();
                    DB.CrearComandoProcedimiento("PA_reporte_folios_full");
                    DB.AsignarParametroProcedimiento("@FECHAI", System.Data.DbType.String, fi);
                    DB.AsignarParametroProcedimiento("@FECHAF", System.Data.DbType.String, Calendar1.SelectedDate.ToShortDateString() + " 23:59:59.997");
                    using (DbDataReader DR = DB.EjecutarConsulta())
                    {
                        while (DR.Read())
                        {
                            rfcReceptor = DR[0].ToString();
                            serie = DR[1].ToString();
                            folio = DR[2].ToString();
                            compueso = DR[3].ToString();
                            fecha = Convert.ToDateTime(DR[4]).ToString("dd/MM/yyyy HH:mm:ss");
                            monto = DR[5].ToString();
                            iva = DR[6].ToString();
                            edo = DR[7].ToString();
                            pedimentos = DR[8].ToString();
                            fechapedimento = DR[9].ToString();
                            aduana = DR[10].ToString();
                            if (serie == "C")
                            {
                                efecto = "E";
                            }
                            else
                            {
                                efecto = "I";
                            }
                            if (edo == "0")
                            {
                                rfcReceptor = rfcReceptor.Replace("-", "");
                                texto += "|" + rfcReceptor + "|" + serie + "|" + folio + "|" + compueso + "|" + fecha.Trim() + "|" + monto + "|" + iva + "|" + 1 + "|" + efecto + "|" + pedimentos.Trim(',') + "|" + fechapedimento.Trim(',') + "|" + aduana.Trim(',') + "|" + "\r\n";
                                texto += "|" + rfcReceptor + "|" + serie + "|" + folio + "|" + compueso + "|" + fecha.Trim() + "|" + monto + "|" + iva + "|" + 0 + "|" + efecto + "|" + pedimentos.Trim(',') + "|" + fechapedimento.Trim(',') + "|" + aduana.Trim(',') + "|" + "\r\n";
                            }
                            else
                            {
                                rfcReceptor = rfcReceptor.Replace("-", "");
                                texto += "|" + rfcReceptor + "|" + serie + "|" + folio + "|" + compueso + "|" + fecha.Trim() + "|" + monto + "|" + iva + "|" + edo + "|" + efecto + "|" + pedimentos.Trim(',') + "|" + fechapedimento.Trim(',') + "|" + aduana.Trim(',') + "|" + "\r\n";
                            }
                        }
                    }
                    DB.Desconectar();
                    nomfecha = "1" + rfcEmisor + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yyyy");
                    archivo = System.AppDomain.CurrentDomain.BaseDirectory + @"reportes\docs\" + nomfecha + ".txt";
                    texto = texto.Trim();
                    texto = texto.Trim('\n');
                    texto = texto.Trim('\r');
                    using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(archivo))
                    {
                        if (texto.Length > 0)
                        {
                            escritor.WriteLine(texto);
                        }
                        else
                        {
                            Label2.Text = "No existen facturas en este mes.";
                        }
                    }
                    if (texto.Length > 0)
                    {
                        if (!String.IsNullOrEmpty(archivo))
                        {
                            String path = Server.MapPath(@"docs/" + nomfecha + ".txt");
                            System.IO.FileInfo toDownload =
                                         new System.IO.FileInfo(path);
                            if (toDownload.Exists)
                            {
                                Response.Clear();
                                Response.Buffer = true;
                                Response.AppendHeader("Content-Disposition",
                                           "attachment; filename=" + toDownload.Name);
                                Response.AddHeader("Content-Length",
                                           (toDownload.Length - 2).ToString());
                                Response.ContentType = "application/octet-stream";
                                Response.WriteFile(path);
                                Response.Flush();
                                Response.Close();
                            }
                            if (System.IO.File.Exists(archivo))
                                System.IO.File.Delete(archivo);
                        }
                    }
                }
                else
                {
                    Label2.Text = "Tienes que seleccionar una fecha.";
                }
            }
            catch (Exception ae)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ae.Message);
                Label2.Text = ae.Message;
            }
            finally
            {
                DB.Desconectar();
            }
        }
    }
}
