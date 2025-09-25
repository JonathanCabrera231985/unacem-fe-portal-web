using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace DataExpressWeb
{
    public class Fileuploadhandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var clGeneral = new General();
            HttpPostedFile file = context.Request.Files[0];
            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.Load(file.InputStream);
            doc.InnerXml = Regex.Replace(doc.InnerXml, @"\t|\n|\r", "");
            doc.InnerXml = clGeneral.VerificaAcentos(doc.InnerXml);
            XmlNode root = doc.DocumentElement;
            var  codDoc = clGeneral.lee_nodo_xml(root, "codDoc");
            if (codDoc != "07")
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("Archivo no es comprobante de retención 2.0");
                return;
            }
            else
            {
                var version = clGeneral.lee_atributo_nodo_xml(root, "version");
                if (version != "2.0.0")
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Archivo no es comprobante de retención 2.0");
                    return;
                }
            }
            var docRuc = clGeneral.lee_nodo_xml(root, "ruc");
            var docEstab = clGeneral.lee_nodo_xml(root, "estab");
            var docPtoEmi = clGeneral.lee_nodo_xml(root, "ptoEmi");
            var docSecuencial = clGeneral.lee_nodo_xml(root, "secuencial");
            docSecuencial = clGeneral.completaSecuencial(docSecuencial);
            var docCodigoControl = $"{docRuc}{codDoc}{docEstab}{docPtoEmi}{docSecuencial}{DateTime.Now:yyyyMMddHHmm}";
            
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["invoicecConnectionString"].ToString()))
            {
                var cmd = new SqlCommand()
                {
                    CommandText = "InsertarLogWebService",
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure
                };
                var messageParameter = new SqlParameter
                {
                    ParameterName = "@Mensaje",
                    SqlDbType = SqlDbType.Text,
                    Value = "RECEIVED",
                    Direction = ParameterDirection.Input
                };
                cmd.Parameters.Add(messageParameter);
                var aditionalInformationParameter = new SqlParameter
                {
                    ParameterName = "@InfoAdicional",
                    SqlDbType = SqlDbType.Text,
                    Value = doc.InnerXml,
                    Direction = ParameterDirection.Input
                };
                cmd.Parameters.Add(aditionalInformationParameter);
                var solutionCenterParameter = new SqlParameter
                {
                    ParameterName = "@CentroSolucion",
                    SqlDbType = SqlDbType.VarChar,
                    Value = "Ingreso retención 2.0 vía portal web",
                    Direction = ParameterDirection.Input
                };
                cmd.Parameters.Add(solutionCenterParameter);
                var typeParameter = new SqlParameter
                {
                    ParameterName = "@Tipo",
                    SqlDbType = SqlDbType.VarChar,
                    Value = docCodigoControl,
                    Direction = ParameterDirection.Input
                };
                cmd.Parameters.Add(typeParameter);
                var stateParameter = new SqlParameter
                {
                    ParameterName = "@Estado",
                    SqlDbType = SqlDbType.Char,
                    Value = "P",
                    Direction = ParameterDirection.Input
                };
                cmd.Parameters.Add(stateParameter);
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write("Archivos cargados sastifactoriamente!");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}