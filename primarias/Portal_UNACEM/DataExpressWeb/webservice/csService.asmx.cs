using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using Datos;
using System.Data.Common;
using System.IO;
using System.Collections;
using System.Data;
using System.Xml;
using System.Text.RegularExpressions;
using Control;
using System.Configuration;
using System.Web.Services.Protocols;
using clibLogger;
using System.Data.SqlClient;

namespace DataExpressWeb.webservice
{
    /// <summary>
    /// Summary description for csService
    /// </summary>
    [WebService(Namespace = "http://CIMAIT.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class csService : ServicioSeguro //System.Web.Services.WebService
    //public class csService : System.Web.Services.WebService
    {
        CriptoSimetrica.AES cripto = new CriptoSimetrica.AES();
        Control.Log logControl = new Control.Log();
        string RutaTXT = "";
        string RutaDocs = "";
        string RucEmisor = "";
        //BasesDatos DB = new BasesDatos();
       // DbDataReader DR = null;
        StreamWriter sw;
        DataTable table = new DataTable("TablaRespuesta");
        private DataExpressWeb.General clGeneral = new DataExpressWeb.General();

        private static String userWS, passWS;

        public csService()
        {
            var DB = new BasesDatos();
            RutaTXT = "";
            RutaDocs = "";
            sw = null;
            userWS = obtener_codigo("userWS");
            passWS = obtener_codigo("passWS");

            try
            {
                DB.Desconectar();
                DB.Configurar();
                DB.Conectar();
                DB.CrearComando(@"select servidorSMTP,puertoSMTP,sslSMTP,userSMTP,passSMTP,
                              dirdocs,dirtxt,dirrespaldo,dircertificados,dirllaves,emailEnvio,rfcEmisor 
                              from ParametrosSistema WITH (NOLOCK) ");
                using (DbDataReader DR = DB.EjecutarConsulta())
                {
                    if (DR.Read())
                    {
                        RutaDocs = DR[5].ToString();
                        RutaTXT = DR[6].ToString();
                        RucEmisor = DR[11].ToString();
                    }
                }

                DB.Desconectar();
            }
            catch (Exception ex)
            {

                DB.Desconectar();
                clsLogger.Graba_Log_Error(ex.Message);
                logService(ex.Message, "No se pudo conectar a la base", "Matriz Web Service", "error", "E");

            }
            finally
            {
                DB.Desconectar();
            }

            // Create a new DataTable.

            // Declare variables for DataColumn and DataRow objects.
            DataColumn column;


            // Create new DataColumn, set DataType, 
            // ColumnName and add to DataTable.    
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "id";
            column.ReadOnly = true;
            column.Unique = true;
            column.AutoIncrement = true;
            column.AutoIncrementSeed = 0;
            column.AutoIncrementStep = 1;
            // Add the Column to the DataColumnCollection.
            table.Columns.Add(column);

            // Create second column.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "codDoc";
            column.AutoIncrement = false;
            column.Caption = "codDoc";
            column.ReadOnly = false;
            column.Unique = false;
            // Add the column to the table.
            table.Columns.Add(column);

            // Create 3 column.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "numDoc";
            column.AutoIncrement = false;
            column.Caption = "numDoc";
            column.ReadOnly = false;
            column.Unique = false;
            // Add the column to the table.
            table.Columns.Add(column);

            // Create 4 column.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Boolean");
            column.ColumnName = "autorizado";
            column.AutoIncrement = false;
            column.Caption = "autorizado";
            column.ReadOnly = false;
            column.Unique = false;
            // Add the column to the table.
            table.Columns.Add(column);

            // Create 3 column.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "fechaAut";
            column.AutoIncrement = false;
            column.Caption = "fechaAut";
            column.ReadOnly = false;
            column.Unique = false;
            // Add the column to the table.
            table.Columns.Add(column);

            // Create 3 column.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "numAut";
            column.AutoIncrement = false;
            column.Caption = "numAut";
            column.ReadOnly = false;
            column.Unique = false;
            // Add the column to the table.
            table.Columns.Add(column);

            // Create 3 column.
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "xml";
            column.AutoIncrement = false;
            column.Caption = "xml";
            column.ReadOnly = false;
            column.Unique = false;
            // Add the column to the table.
            table.Columns.Add(column);

            // Make the ID column the primary key column.
            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = table.Columns["id"];
            table.PrimaryKey = PrimaryKeyColumns;
        }


      


        private Boolean recibeTxt(string cadena, string centroSolucion)
        {
            string infoTxt = "";
            try
            {
                if (!String.IsNullOrEmpty(cadena) && !String.IsNullOrEmpty(centroSolucion))
                {
                    // infoTxt = cripto.desencriptar(cadena, "DEServientrega");
                    infoTxt = cadena.Replace("'", "");
                    sw = new StreamWriter(RutaTXT + centroSolucion + "_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");
                    sw.WriteLine(infoTxt);
                    sw.Close();
                    logService("información Recibida.", infoTxt, centroSolucion, "mensaje", "E");
                    return true;
                }
                else
                {
                    logService("Información Incompleta.", infoTxt, centroSolucion, "error", "E");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logService(ex.Message, cadena.Replace("'", ""), centroSolucion, "error", "E");
                return false;
            }
        }

        private void logService(string mensaje, string infoAdicional, string centroSolucion, string tipo, string estado)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Desconectar();
                DB.Configurar();
                DB.Conectar();
                DB.CrearComando(@"INSERT INTO [LogWebService]
                        ([mensaje],[fecha],[infoAdicional],[centroSolucion],[tipo],[estado])
                        VALUES
                        (@mensaje,@fecha,@infoAdicional,@centroSolucion,@tipo,@estado)");
                DB.AsignarParametroCadena("@mensaje", mensaje);
                DB.AsignarParametroCadena("@fecha", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                DB.AsignarParametroCadena("@infoAdicional", infoAdicional);
                DB.AsignarParametroCadena("@centroSolucion", centroSolucion);
                DB.AsignarParametroCadena("@tipo", tipo);
                DB.AsignarParametroCadena("@estado", estado);
                using (var x = DB.EjecutarConsulta())
                {
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

        private string generarClaveAcceso(string secuencial, string fechaEmision, string codDoc, string estab, string ptoEmi, string ambiente, string tipoEmision, string ruc)
        {
            try
            {
                string clave, codigoNumerico;
                codigoNumerico = secuencial.Substring(1);//digitoVerificador(aleat3.ToString().Trim());
                DateTime tiempo = DateTime.ParseExact(fechaEmision, "dd/MM/yyyy", null);
                clave = tiempo.ToString("ddMMyyyy") + codDoc + ruc;
                clave += ambiente + (estab + ptoEmi) + secuencial + codigoNumerico + tipoEmision;
                clave = digitoVerificador(clave);
                return clave;
            }
            catch (Exception ex)
            {
                //msj = "";
                //msjT = ex.Message;
                //log.mensajesLog("EM014", "", msjT, "", codigoControl, "Leer.cs");
                return "";
            }
        }

        private static string digitoVerificador(string number)
        {
            int Sum = 0;
            for (int i = number.Length - 1, Multiplier = 2; i >= 0; i--)
            {
                Sum += (int)char.GetNumericValue(number[i]) * Multiplier;

                if (++Multiplier == 8) Multiplier = 2;
            }
            string Validator = (11 - (Sum % 11)).ToString().Trim();

            if (Validator == "11") Validator = "0";
            else if (Validator == "10") Validator = "1";

            return number + Validator;
        }

        private void logService2(string mensaje, string infoAdicional, string centroSolucion, string tipo, string estado)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Desconectar();
                DB.Configurar2();
                DB.Conectar();
                DB.CrearComando(@"INSERT INTO [LogWebService]
                        ([mensaje],[fecha],[infoAdicional],[centroSolucion],[tipo],[estado])
                        VALUES
                        (@mensaje,@fecha,@infoAdicional,@centroSolucion,@tipo,@estado)");
                DB.AsignarParametroCadena("@mensaje", mensaje);
                DB.AsignarParametroCadena("@fecha", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                DB.AsignarParametroCadena("@infoAdicional", infoAdicional);
                DB.AsignarParametroCadena("@centroSolucion", centroSolucion);
                DB.AsignarParametroCadena("@tipo", tipo);
                DB.AsignarParametroCadena("@estado", estado);
                using (var x = DB.EjecutarConsulta())
                {
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
                DB.Configurar();
                DB.Desconectar();
            }
        }


        //[WebMethod, SoapHeader("CredencialAutenticacion")]
        ////[WebMethod]
        //public DataTable ConsultaAutSRI(DataTable p_tabla)
        //{
        //    DataRow fila;
        //    string infoTxt = "";
        //    string v_xml = "";
        //    table.Clear();
        //    if (VerificarPermisos(CredencialAutenticacion))
        //    //if (true)
        //    {
        //        try
        //        {
        //            if (p_tabla.Rows.Count > 0)
        //            //if (!String.IsNullOrEmpty(cadena) && !String.IsNullOrEmpty(centroSolucion))
        //            {
        //                // infoTxt = cripto.desencriptar(cadena, "DECopece");

        //                foreach (DataRow row in p_tabla.Rows)
        //                {
        //                    string codDoc = row["codDoc"].ToString();
        //                    string numDoc = row["numDoc"].ToString();

        //                    if (!String.IsNullOrEmpty(codDoc) && !String.IsNullOrEmpty(numDoc))
        //                    {
        //                        DB.Conectar();
        //                        DB.CrearComandoProcedimiento("SP_CONS_FE");
        //                        DB.AsignarParametroProcedimiento("@pr_codDoc", System.Data.DbType.String, codDoc);
        //                        DB.AsignarParametroProcedimiento("@pr_numDoc", System.Data.DbType.String, numDoc);
        //                        DR = DB.EjecutarConsulta();
        //                        while (DR.Read())
        //                        {
        //                            fila = table.NewRow();
        //                            fila["codDoc"] = DR[1].ToString();
        //                            fila["numDoc"] = DR[2].ToString();
        //                            fila["autorizado"] = DR[3].ToString();
        //                            fila["fechaAut"] = DR[4].ToString();
        //                            fila["NumAut"] = DR[5].ToString();
        //                            v_xml = DR[6].ToString();

        //                            string file_xml = "";
        //                            bool v_autorizado = bool.Parse(DR[3].ToString());

        //                            if (v_autorizado)
        //                            {
        //                                if (System.IO.File.Exists(RutaDocs + v_xml))
        //                                {
        //                                    file_xml = System.IO.File.ReadAllText(RutaDocs + v_xml);
        //                                }
        //                                else
        //                                {
        //                                    file_xml = "Archivo xml no Existe";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                file_xml = v_xml;
        //                            }

        //                            fila["xml"] = file_xml;

        //                            table.Rows.Add(fila);

        //                        }
        //                        DB.Desconectar();
        //                    }
        //                    else
        //                    {
        //                        infoTxt = "No hay información de entrada : codDoc= " + codDoc + " numDoc:" + numDoc;
        //                    }

        //                }
        //            }
        //            else
        //            {
        //                logService("No hay Información en DataTable.", infoTxt, "", "error", "E");

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            logService(ex.Message, infoTxt, "", "error", "E");
        //        }
        //    }
        //    else
        //    {
        //        //p_mensaje = "Credenciales Incorrectas";
        //        //logService("Credenciales Incorrectas", p_mensaje, "", "error", "E");
        //    }

        //    return table;
        //}


        //[WebMethod, SoapHeader("CredencialAutenticacion")]
        ////[WebMethod]
        //public Boolean recibeDocs(DataTable p_data, string p_sucursal, out string p_mensaje)
        //{
        //    Boolean v_retorno = false;
        //    int cantDocs = p_data.Rows.Count, cantDocsOk = 0;
        //    string infoTxt = "", msj = "", msjT = "", codDoc = "", doc_version = "";
        //    string docEstado = "", docRuc = "", docEstab = "", docPtoEmi = "", docSecuencial = "", docCodigoControl = "";
        //    XmlDocument xmlbase;

        //    p_mensaje = "";
        //    if (VerificarPermisos(CredencialAutenticacion))
        //    //if (true)
        //    {
        //        try
        //        {
        //            if (cantDocs > 0 && !String.IsNullOrEmpty(p_sucursal))
        //            {
        //                // infoTxt = cripto.desencriptar(cadena, "CIMAITkey");
        //                int contador = 0;
        //                foreach (DataRow row in p_data.Rows)
        //                {
        //                    string txtDatos = row["txtDatos"].ToString();
        //                    string nombre = row["nombre"].ToString() + contador.ToString();
        //                    infoTxt = nombre;
        //                    //if (recibeTxt(txtDatos, nombre))

        //                    if (!String.IsNullOrEmpty(txtDatos))
        //                    {
        //                        xmlbase = new XmlDocument();
        //                        xmlbase.LoadXml(txtDatos);
        //                        xmlbase.InnerXml = Regex.Replace(xmlbase.InnerXml, @"\t|\n|\r", "");
        //                        xmlbase.InnerXml = clGeneral.VerificaAcentos(xmlbase.InnerXml);

        //                        XmlNode root = xmlbase.DocumentElement;

        //                        codDoc = clGeneral.lee_nodo_xml(root, "codDoc");
        //                        doc_version = clGeneral.lee_atributo_nodo_xml(root, "version");
        //                        docRuc = clGeneral.lee_nodo_xml(root, "ruc");
        //                        docEstab = clGeneral.lee_nodo_xml(root, "estab");
        //                        docPtoEmi = clGeneral.lee_nodo_xml(root, "ptoEmi");
        //                        docSecuencial = clGeneral.lee_nodo_xml(root, "secuencial");
        //                        docSecuencial = clGeneral.completaSecuencial(docSecuencial);

        //                        docCodigoControl = docRuc + codDoc + docEstab + docPtoEmi + docSecuencial + System.DateTime.Now.ToString("yyyyMMddHHmm");

        //                        if (!String.IsNullOrEmpty(codDoc) && !String.IsNullOrEmpty(doc_version))
        //                        {
        //                            if (clGeneral.ValidarEstructuraXSD(xmlbase.InnerXml, codDoc, doc_version, out msj, out msjT))
        //                            {
        //                                docEstado = "P";
        //                                logService("RECIBIDO", xmlbase.InnerXml, nombre, docCodigoControl, docEstado);
        //                                p_mensaje = "RECIBIDO";
        //                                cantDocsOk++;
        //                            }
        //                            else
        //                            {
        //                                docEstado = "E";
        //                                msj = msj.Replace("'", "''");
        //                                msjT = msjT.Replace("'", "''");
        //                                logService(msj + msjT, xmlbase.InnerXml, nombre, docCodigoControl, docEstado);
        //                                logControl.mensajesLog("US001", msj, msjT, nombre, docCodigoControl);
        //                                p_mensaje += nombre + ":" + msj + msjT + "|";
        //                            }

        //                        }
        //                        else
        //                        {
        //                            docEstado = "I";
        //                            msj = msj.Replace("'", "''");
        //                            msjT = msjT.Replace("'", "''");
        //                            logService(msj + msjT, xmlbase.InnerXml, nombre, docCodigoControl, docEstado);
        //                            logControl.mensajesLog("US001", msj, msjT, nombre, docCodigoControl);
        //                            p_mensaje += nombre + ": Documento incompleto, falta atributo versión o la etiqueta código del documento <codDoc>.|";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        p_mensaje += nombre + ": No hay datos del documento.|";
        //                    }
        //                    contador++;
        //                }
        //                v_retorno = true;
        //            }
        //            else
        //            {
        //                p_mensaje += "Información Incompleta en DataTable. " + infoTxt + " : " + p_sucursal;
        //                v_retorno = false;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            p_mensaje = "Error: " + ex.Message + infoTxt + " : " + p_sucursal;
        //            v_retorno = false;
        //        }
        //    }
        //    else
        //    {
        //        p_mensaje = "Credenciales Incorrectas";
        //        //logService("Credenciales Incorrectas", p_mensaje, "", "error", "E");
        //    }

        //    return v_retorno;

        //}

        //[WebMethod, SoapHeader("CredencialAutenticacion")]
        [WebMethod]

        public Boolean recibeDocsString(string p_doc, string p_nombre, string p_sucursal, out string p_mensaje, string user, string pass, out string P_ClaveAcceso)
        {
            Boolean v_retorno = false;

            string msj = "", msjT = "", codDoc = "", doc_version = "", codDocA = "", ambiente = "";
            string docEstado = "", docRuc = "", docEstab = "", docPtoEmi = "", docSecuencial = "", docCodigoControl = "";
            string p_fechaEmision = "", p_tipoEmision = "";
            XmlDocument xmlbase;
            P_ClaveAcceso = "";

            p_mensaje = "";
            userWS = obtener_codigo("userWS");
            passWS = obtener_codigo("passWS");
            if (user.Equals(userWS) && pass.Equals(passWS))
            //if (true)
            {
                try
                {
                    if (!String.IsNullOrEmpty(p_doc) && !String.IsNullOrEmpty(p_nombre) && !String.IsNullOrEmpty(p_sucursal))
                    {
                        string txtDatos = p_doc;
                        string nombre = p_nombre;

                        if (!String.IsNullOrEmpty(txtDatos))
                        {
                            //txtDatos = validaCaracterEspecial(txtDatos);

                            xmlbase = new XmlDocument();
                            xmlbase.LoadXml(txtDatos);
                            xmlbase.InnerXml = Regex.Replace(xmlbase.InnerXml, @"\t|\n|\r", "");
                            xmlbase.InnerXml = clGeneral.VerificaAcentos(xmlbase.InnerXml);

                            XmlNode root = xmlbase.DocumentElement;

                            ambiente = clGeneral.lee_nodo_xml(root, "ambiente");
                            codDoc = clGeneral.lee_nodo_xml(root, "codDoc");
                            codDoc = obtener_codDoc(codDoc);
                            doc_version = clGeneral.lee_atributo_nodo_xml(root, "version");
                            docRuc = clGeneral.lee_nodo_xml(root, "ruc");
                            docEstab = clGeneral.lee_nodo_xml(root, "estab");
                            docPtoEmi = clGeneral.lee_nodo_xml(root, "ptoEmi");
                            docSecuencial = clGeneral.lee_nodo_xml(root, "secuencial");
                            docSecuencial = clGeneral.completaSecuencial(docSecuencial);
                            p_fechaEmision = clGeneral.lee_nodo_xml(root, codDoc == "06" ? "fechaEmisionDocSustento" : "fechaEmision");
                            p_tipoEmision = clGeneral.lee_nodo_xml(root, "tipoEmision");

                            docCodigoControl = docRuc + codDoc.Substring(0, 2) + docEstab + docPtoEmi + docSecuencial + System.DateTime.Now.ToString("yyyyMMddHHmm");

                            string estabGR = ConfigurationManager.AppSettings.Get("estabGC");
                            string ptoEmiGR = ConfigurationManager.AppSettings.Get("ptoemiGC");

                            if (docEstab == estabGR && docPtoEmi == ptoEmiGR && codDoc == "06")
                            {
                                xmlbase.InnerXml = Regex.Replace(xmlbase.InnerXml, 
                                    ConfigurationManager.AppSettings.Get("leyendaGranContrOld"), 
                                    ConfigurationManager.AppSettings.Get("leyendaGranContrNew"));
                            }

                            if (!String.IsNullOrEmpty(codDoc) && !String.IsNullOrEmpty(doc_version))
                            {
                                if (clGeneral.ValidarEstructuraXSD(xmlbase.InnerXml, codDoc, doc_version, out msj, out msjT, ambiente))
                                {
                                    P_ClaveAcceso = generarClaveAcceso(docSecuencial, p_fechaEmision, codDoc, docEstab, docPtoEmi, ambiente, p_tipoEmision, docRuc);
                                    docEstado = "P";
                                    bool unacem = docRuc.Equals(RucEmisor);
                                    
                                    if (docRuc.Equals(RucEmisor)) {
                                        //unacem
                                        if (IsAuthorized(codDoc, docPtoEmi, docEstab, docSecuencial)) {
                                            UpdateForSendSap(codDoc, docPtoEmi, docEstab, docSecuencial); 
                                        }
                                        else{
                                            logService("RECEIVED", xmlbase.InnerXml, nombre, docCodigoControl, docEstado);
                                        }

                                    }
                                    else {
                                        //cantivol
                                        if (IsAuthorizedCantivol(codDoc, docPtoEmi, docEstab, docSecuencial))
                                        {
                                            UpdateForSendSapCantivol(codDoc, docPtoEmi, docEstab, docSecuencial);
                                        }
                                        else
                                        {
                                            logService2("RECEIVED", xmlbase.InnerXml, nombre, docCodigoControl, docEstado);
                                        }
                                    }
                                    
                                    p_mensaje = obtener_codigo("RECEIVED");
                                }
                                else
                                {
                                    docEstado = "E";
                                    msj = msj.Replace("'", "''");
                                    msjT = msjT.Replace("'", "''");
                                    logService(msj + msjT, xmlbase.InnerXml, nombre, docCodigoControl, docEstado);
                                    logControl.mensajesLog("US001", msj, msjT, nombre, docCodigoControl);
                                    p_mensaje += obtener_codigo("ERROR") + ":" + nombre + ":" + msj + msjT + "|";
                                    P_ClaveAcceso = "";
                                }

                            }
                            else
                            {
                                docEstado = "I";
                                msj = msj.Replace("'", "''");
                                msjT = msjT.Replace("'", "''");
                                logService(msj + msjT, xmlbase.InnerXml, nombre, docCodigoControl, docEstado);
                                logControl.mensajesLog("US001", msj, msjT, nombre, docCodigoControl);
                                p_mensaje += obtener_codigo("ERROR") + ":" + nombre + ": Documento incompleto, falta el atributo versión o la etiqueta código del documento <codDoc>.|";
                                P_ClaveAcceso = "";
                            }
                        }
                        else
                        {
                            p_mensaje += obtener_codigo("ERROR") + ":" + nombre + ": No hay datos del documento.|";
                            P_ClaveAcceso = "";
                        }


                        v_retorno = true;
                    }
                    else
                    {
                        p_mensaje += obtener_codigo("ERROR") + ":" + "Información Incompleta. " + p_nombre + " : " + p_sucursal;
                        v_retorno = false;
                        P_ClaveAcceso = "";

                    }
                }
                catch (Exception ex)
                {
                    p_mensaje = obtener_codigo("ERROR") + ":" + "Error: " + ex.Message + p_nombre + " : " + p_sucursal;
                    clsLogger.Graba_Log_Error(ex.Message);
                    v_retorno = false;
                    P_ClaveAcceso = "";
                }
            }
            else
            {
                p_mensaje = "Credenciales Incorrectas";
                P_ClaveAcceso = "";
                //logService("Credenciales Incorrectas", p_mensaje, "", "error", "E");
            }

            return v_retorno;

        }
        //                    logControl.mensajesLog("US001", ex.Message, $"{ex.Message}///{ex.InnerException}///{ex.StackTrace}", "", "");
        private bool IsAuthorized(string type, string emision, string local, string secuential)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["dataexpressConnectionString"].ToString();
            var queryString =
                "SELECT COUNT(1) FROM GENERAL WITH (NOLOCK) "
                    + "WHERE codDoc = @type AND ptoEmi = @emision AND estab = @local AND secuencial = @secuential AND estado = 1 AND tipo = 'E'";

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@emision", emision);
                command.Parameters.AddWithValue("@local", local);
                command.Parameters.AddWithValue("@secuential", secuential);

                try
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    var result = 0;
                    while (reader.Read())
                        result = Convert.ToInt32(reader[0]);
                    reader.Close();
                    return result == 1;
                }
                catch (Exception ex)
                {
                    logControl.mensajesLog("US001", ex.Message, $"{ex.Message}///{ex.InnerException}///{ex.StackTrace}", "", "");
                    return false;
                }
            }
        }

        private bool IsAuthorizedCantivol(string type, string emision, string local, string secuential)
        {
            var DB = new BasesDatos();
            bool resultado = false;

            try
            {
                DB.Desconectar();
                DB.Configurar2();
                DB.Conectar();
                DB.CrearComando(@"
    SELECT COUNT(1) FROM GENERAL WITH (NOLOCK) WHERE codDoc = @type AND ptoEmi = @emision AND estab = @local AND secuencial = @secuential AND estado = 1 AND tipo = 'E'
    ");
                DB.AsignarParametroCadena("@type", type);
                DB.AsignarParametroCadena("@emision", emision);
                DB.AsignarParametroCadena("@local", local);
                DB.AsignarParametroCadena("@secuential", secuential);

                using (var reader = DB.EjecutarConsulta())
                {
                    var result = 0;
                    while (reader.Read())
                        result = Convert.ToInt32(reader[0]);
                    reader.Close();
                    resultado = result == 1;
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
                DB.Configurar();
                DB.Desconectar();
            }

            return resultado;
        }

        private void UpdateForSendSap(string type, string emision, string local, string secuential)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["dataexpressConnectionString"].ToString();
            var queryString =
                "UPDATE GENERAL SET estadoWLF = 'S', archivoWLF = 'RC'"
                   + "WHERE codDoc = @type AND ptoEmi = @emision AND estab = @local AND secuencial = @secuential AND estado = 1 AND tipo = 'E'";

            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@type", type);
                command.Parameters.AddWithValue("@emision", emision);
                command.Parameters.AddWithValue("@local", local);
                command.Parameters.AddWithValue("@secuential", secuential);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    logControl.mensajesLog("US001", ex.Message, $"{ex.Message}///{ex.InnerException}///{ex.StackTrace}", "", "");
                }
            }
        }

        private void UpdateForSendSapCantivol(string type, string emision, string local, string secuential)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Desconectar();
                DB.Configurar2();
                DB.Conectar();
                DB.CrearComando(@"
    UPDATE GENERAL SET estadoWLF = 'S', archivoWLF = 'RC' 
    WHERE codDoc = @type AND ptoEmi = @emision AND estab = @local AND secuencial = @secuential AND estado = 1 AND tipo = 'E'
    ");
                DB.AsignarParametroCadena("@type", type);
                DB.AsignarParametroCadena("@emision", emision);
                DB.AsignarParametroCadena("@local", local);
                DB.AsignarParametroCadena("@secuential", secuential);

                using (var x = DB.EjecutarConsulta())
                {
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
                DB.Configurar();
                DB.Desconectar();
            }

        }
        private string validaCaracterEspecial(string cadena) 
        {
            string nuevaCadena = "", bandera ="0";
            cadena = Regex.Replace(cadena, @"\t|\n|\r", "");
            cadena = cadena.Replace("<descripcion>", "|");
            cadena = cadena.Replace("</descripcion>", "|");

            string[] cadenasXml = cadena.Split('|');

            foreach (var cadenas in cadenasXml)
            {
                bandera = "0";
                var primerCaracter = cadenas.Substring(0, 1);
                if (!primerCaracter.Equals("<") && !cadenas.Contains("</")) 
                {
                    var desc = "";
                    desc = cadenas.Replace("<", "");
                    desc = desc.Replace(">", "");
                    nuevaCadena += "<descripcion>" + desc + "</descripcion>";
                    bandera = "1";
                }
                if (!bandera.Equals("1")) 
                {
                    nuevaCadena += cadenas;
                }
                
            }

            return nuevaCadena;
        }


        //[WebMethod, SoapHeader("CredencialAutenticacion")]
        //[WebMethod]
        //public Boolean ConsultaAutSRIString(String p_codDoc, String p_numDoc, out string p_autorizacion, out string p_fechaAut, out string p_xml, out string p_mensaje)
        //{
        //    Boolean rpt = false;
        //    p_autorizacion = "";
        //    p_fechaAut = "";
        //    p_xml = "";
        //    p_mensaje = "";

        //    if (VerificarPermisos(CredencialAutenticacion))
        //    //if (true)
        //    {
        //        try
        //        {
        //            string codDoc = p_codDoc; // row["codDoc"].ToString();
        //            string numDoc = p_numDoc; // row["numDoc"].ToString();

        //            if (!String.IsNullOrEmpty(codDoc) && !String.IsNullOrEmpty(numDoc))
        //            {
        //                DB.Conectar();
        //                DB.CrearComandoProcedimiento("SP_CONS_FE");
        //                DB.AsignarParametroProcedimiento("@pr_codDoc", System.Data.DbType.String, codDoc);
        //                DB.AsignarParametroProcedimiento("@pr_numDoc", System.Data.DbType.String, numDoc);
        //                DR = DB.EjecutarConsulta();
        //                while (DR.Read())
        //                {
        //                    p_mensaje = DR[3].ToString();
        //                    p_fechaAut = DR[4].ToString();
        //                    p_autorizacion = DR[5].ToString();
        //                    p_xml = DR[6].ToString();

        //                    string file_xml = "";

        //                    switch (DR[3].ToString().Trim())
        //                    {
        //                        case "1":
        //                            if (!String.IsNullOrEmpty(p_autorizacion))
        //                            {
        //                                if (System.IO.File.Exists(RutaDocs + p_xml))
        //                                {
        //                                    file_xml = System.IO.File.ReadAllText(RutaDocs + p_xml);
        //                                    p_xml = file_xml;
        //                                    //p_mensaje = "E1: Documento generado con Exito";
        //                                    p_mensaje = obtener_codigo("AUTHORIZED");
        //                                }
        //                                else
        //                                {
        //                                    //p_mensaje = "E2: Documento generado con Exito sin disponibilidad de xml";
        //                                    p_mensaje = obtener_codigo("AUTHORIZED");
        //                                }
        //                                rpt = true;
        //                            }
        //                            else
        //                            {
        //                                //p_mensaje = "E3: Documento con estado pendiente de autorización";
        //                                p_mensaje = obtener_codigo("PENDING");

        //                            }
        //                            break;
        //                        case "0":
        //                            if (String.IsNullOrEmpty(p_autorizacion))
        //                            {
        //                                //p_mensaje = "E4: Documento NO Autorizado";
        //                                p_mensaje = obtener_codigo("REJECTED");
        //                            }
        //                            else
        //                            {
        //                                //p_mensaje = "E5: Documento previamente Autorizado, pero posteriormente afectado";
        //                                p_mensaje = obtener_codigo("AUTHORIZED");

        //                            }
        //                            break;
        //                        case "2":
        //                            //p_mensaje = "E6: Documento emitido en contingencia";
        //                            p_mensaje = obtener_codigo("CONTINGENCY");
        //                            break;
        //                        case "N":
        //                            //p_mensaje = "E7: Documento no procesado en el sistema";
        //                            p_mensaje = obtener_codigo("ERROR");
        //                            break;
        //                        default:
        //                            //p_mensaje = "E8: Documento no consultado por indisponibilidad";
        //                            p_mensaje = obtener_codigo("ERROR");
        //                            break;
        //                    }

        //                }
        //                DB.Desconectar();
        //            }
        //            else
        //            {
        //                //p_mensaje = "E9: Parámetros de entrada incompletos: Código o número del documento";
        //                p_mensaje = obtener_codigo("ERROR") + ":" + "E9: Parámetros de entrada incompletos: Código o número del documento";
        //                //infoTxt = "No hay información de entrada : codDoc= " + codDoc + " numDoc:" + numDoc;
        //            }


        //        }
        //        catch (Exception ex)
        //        {
        //            logService(ex.Message, p_mensaje, "", "error", "E");
        //            p_mensaje = obtener_codigo("ERROR") + ":" + "E10: Error en proceso: " + ex.Message;
        //        }
        //    }
        //    else
        //    {
        //        p_mensaje = "Credenciales Incorrectas";
        //        //logService("Credenciales Incorrectas", p_mensaje, "", "error", "E");
        //    }

        //    return rpt;
        //}

        private string obtener_codDoc(string v_parametro)
        {
            //int EF;
            string retorno = ConfigurationManager.AppSettings.Get(v_parametro);
            //string retorno = ConfigurationManager.ConnectionStrings[v_parametro].ConnectionString;
            //EF = 01;
            //squareVal(EF);
            //Console.WriteLine(EF);
            //enum retorno long{EF= 01, EC = 05, E1 = 06,E8 = 05,EB = 04,EG = 06 }
            //int EF = 01, EC = 05, E1 = 06, E8 = 05, EB = 04, EG = 06;
            //int caseSwitch = 1;
            //string str = Console.ReadLine();
            //switch (str)
            //{
            //    case EF=01:
            //        retorno 
            //        break;
            //    case EC:
            //        retorno = 05;
            //        break;
            //        case E1:
            //        retorno = 06;
            //        break;
            //    case E8:
            //        retorno = 05;
            //        break;
            //    case EB:
            //        retorno = 04;
            //        break;
            //    case EG:
            //        retorno = 06;
            //        break;
            //    default:
            //        Console.WriteLine("Default case");
            //        break;
            //}
            return retorno;
        }
        private string obtener_codigo(string a_parametro)
        {
            string retorna = ConfigurationManager.AppSettings.Get(a_parametro);

            return retorna;
        }

        public static Boolean VerificarPermisos(Autenticacion value)
        {
            if (value == null)
            {
                return false;
            }
            else
            {
                //Verifica los permiso Ej. Consulta a BD 
                if (value.UsuarioNombre == userWS && value.UsuarioClave == passWS)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public String[] ConsultaGuia(string Cod_vehiculo)
        {
            var DB = new BasesDatos();
            DataSet ds = new DataSet();
            string[] result = null;
            try
            {
                if (String.IsNullOrEmpty(Cod_vehiculo))
                {
                    throw new ArgumentException("Número de Vehiculo incorrecto");
                }
                else
                {
                    DB.Desconectar();
                    DB.Conectar();
                    ds = DB.TraerDataSetConsulta(@"select 
                    cast(g.xmlGenerado.value('(//comprobante/node())[1]', 'varchar(max)') as xml) xmlGene
                    from general g  WITH (NOLOCK) inner join
					              	InfoAdicional WITH (NOLOCK) on id_Comprobante=g.idComprobante
                    where g.fecha>='2016-jun-06' and InfoAdicional.nombre='Num_Vehiculo' and InfoAdicional.valor='" + Cod_vehiculo + "' and g.codDoc='06' and g.estado='1' and tipo='E' and g.numeroAutorizacion is not null and g.fechaAutorizacion is not null", new Object[] { });//Estado = 'R' and 
                    int cont = ds.Tables[0].Rows.Count;
                    result = new string[cont];
                    DataView view = new DataView(ds.Tables[0]);
                    DataTable dtPagos = new DataTable();
                    dtPagos = view.ToTable(true, "xmlGene");
                    int c = 0;
                    foreach (DataRow pagos in dtPagos.Rows)
                    {
                        result[c] = pagos["xmlGene"].ToString();
                        c++;
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
            return result;
        }

    }
}