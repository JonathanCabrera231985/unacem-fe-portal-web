using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Datos;
using System.Data.Common;
using Control;
using clibLogger;

namespace DataExpressWeb
{
    public partial class Formulario_web11 : Generic
    {
        String filename = "";
        private CrearPDF cPDF;
        private CrearPDFold cPDFold;
        string user = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            filename = Request.QueryString.Get("file");
            if (!String.IsNullOrEmpty(filename))
            {
                filename = Decrypt(filename.Replace(" ","+"));
                String codigoControl = filename.Replace("docus/", "").Replace(".xml", "").Replace(".pdf", "").Replace("file", "").Replace("=", "");

                    if (filename.IndexOf(".xml") > 0)
                    {
                        if (Documentos.historicad.Equals("2"))
                        {
                            cPDFold = new CrearPDFold();
                            descarga_xml2(codigoControl);
                        }
                        else
                        {
                            cPDF = new CrearPDF();
                            descarga_xml(codigoControl);
                        }
                    }
                    else if (filename.IndexOf(".pdf") > 0)
                    {
                        if (Documentos.historicad.Equals("2"))
                        {
                            cPDFold = new CrearPDFold();
                            descarga_pdf2(codigoControl);
                        }
                        else
                        {
                            cPDF = new CrearPDF();
                            descarga_pdf(codigoControl);
                        }
                    }
                    else
                    {
                        descarga_xml3(codigoControl);

                    }
                
            }
        }

        public void descarga_xml(string p_codControl)
        {
            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment; filename=" + p_codControl + ".xml");
            Response.BinaryWrite(consulta_archivo_xml(p_codControl, 7).ToArray());
            Response.End();
        }
        public void descarga_xml2(string p_codControl)
        {
            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment; filename=" + p_codControl + ".xml");
            Response.BinaryWrite(consulta_archivo_xml2(p_codControl, 7).ToArray());
            Response.End();
        }

        public void descarga_pdf(string p_codControl)
        {
            var auxAmbiente = System.Configuration.ConfigurationManager.AppSettings["Ambiente"];
            if (auxAmbiente.Equals("1"))
            {
                Response.Clear();
                Response.ContentType = "application/force-download";
                Response.AddHeader("content-disposition", "attachment; filename=" + p_codControl + ".pdf");
                Response.BinaryWrite(cPDF.msPDF(p_codControl).ToArray());
                Response.End();
            }
            else
            {
                Log lg1 = new Log();
                clsLogger.Graba_Log_Info("descarga_pdf paso 1");
                System.IO.Stream d = null;
                clsLogger.Graba_Log_Info("descarga_pdf paso 2");
                d = cPDF.msPDF(p_codControl);
                clsLogger.Graba_Log_Info("descarga_pdf paso 3");
                byte[] byteArray = null;
                clsLogger.Graba_Log_Info("descarga_pdf paso 4");
                byteArray = new byte[d.Length];
                clsLogger.Graba_Log_Info("descarga_pdf paso 5");
                d.Read(byteArray, 0, Convert.ToInt32(d.Length - 1));
                clsLogger.Graba_Log_Info("descarga_pdf paso 6");
                Response.Clear();
                Response.ContentType = "application/force-download";
                Response.AddHeader("Content-Length", d.Length.ToString());
                Response.AddHeader("Content-disposition", "inline; filename= " + p_codControl + ".pdf");
                Response.BinaryWrite(byteArray);
                Response.Flush();
                Response.Close();
                clsLogger.Graba_Log_Info("descarga_pdf paso 7");
            }
        }

        public void descarga_pdf2(string p_codControl)
        {
            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment; filename=" + p_codControl + ".pdf");
            Response.BinaryWrite(cPDFold.msPDF(p_codControl).ToArray());
            Response.End();
        }

        public void descarga_xml3(string p_codControl)
        {
            Response.Clear();
            Response.ContentType = "application/force-download";
            Response.AddHeader("content-disposition", "attachment; filename=" + p_codControl + ".xml");
            Response.BinaryWrite(consulta_archivo_xml(p_codControl, 10).ToArray());
            Response.End();
        }

        public MemoryStream consulta_archivo_xml(string p_codigoControl, int p_opcion)
        {
            var DB = new BasesDatos();
            MemoryStream rpt = new MemoryStream();
            string doc = "";
            try
            {
                DB.Conectar();
                DB.CrearComandoProcedimiento("PA_ARCHIVO_XML");
                DB.AsignarParametroProcedimiento("@documentoXML", System.Data.DbType.Xml, "0");
                DB.AsignarParametroProcedimiento("@codigoControl", System.Data.DbType.String, p_codigoControl);
                DB.AsignarParametroProcedimiento("@idComprobante", System.Data.DbType.Int32, "0");
                DB.AsignarParametroProcedimiento("@opcion", System.Data.DbType.Int32, p_opcion);
                using (DbDataReader dr = DB.EjecutarConsulta())
                {
                    if (dr.Read())
                    {
                        doc = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + dr[0].ToString();
                        rpt = GenerateStreamFromString(doc);
                    }
                }
                DB.Desconectar();
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
            return rpt;
        }

        public MemoryStream consulta_archivo_xml2(string p_codigoControl, int p_opcion)
        {
            var DB = new BasesDatos();
            MemoryStream rpt = new MemoryStream();
            string doc = "";
            try
            {
                DB.Conectar2();
                DB.CrearComandoProcedimiento2("PA_ARCHIVO_XML");
                DB.AsignarParametroProcedimiento("@documentoXML", System.Data.DbType.Xml, "0");
                DB.AsignarParametroProcedimiento("@codigoControl", System.Data.DbType.String, p_codigoControl);
                DB.AsignarParametroProcedimiento("@idComprobante", System.Data.DbType.Int32, "0");
                DB.AsignarParametroProcedimiento("@opcion", System.Data.DbType.Int32, p_opcion);
                using (DbDataReader dr = DB.EjecutarConsulta())
                {
                    if (dr.Read())
                    {
                        doc = @"<?xml version=""1.0"" encoding=""UTF-8""?>" + dr[0].ToString();
                        rpt = GenerateStreamFromString(doc);
                    }
                }

                DB.Desconectar2();
            }
            catch (Exception ex)
            {
                DB.Desconectar2();
                clsLogger.Graba_Log_Error(ex.Message);
            }
            finally
            {
                DB.Desconectar2();
            }
            return rpt;
        }

        private MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(value ?? ""));
        }
    }
}