using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Datos;
using System.Data.Common;
using clibLogger;

namespace DataExpressWeb.nuevo
{
    /// <summary>
    /// Descripción breve de autoRuc
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class autoRuc : System.Web.Services.WebService
    {

        //BasesDatos DB;
        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public string[] getRuc(string prefixText)
        {
            var DB = new BasesDatos();
            int count = 0;
            string[] a = new String[1];
            DB = new BasesDatos();
            string sql1 = "SELECT TOP 10 RFCREC FROM RECEPTOR WITH (NOLOCK)  where RFCREC LIKE @rfc";
            int Contador = 0;
            try
            {
                DB.Conectar();
                DB.CrearComando("SELECT TOP 10 COUNT(RFCREC) FROM RECEPTOR WITH (NOLOCK) where RFCREC LIKE @rfc ");
                DB.AsignarParametroCadena("@rfc", prefixText + "%"); ;
                using (DbDataReader DRTot = DB.EjecutarConsulta())
                {
                    DRTot.Read();
                    count = Convert.ToInt32(DRTot[0].ToString());
                }
                DB.Desconectar();

                DB.Conectar();
                DB.CrearComando(sql1);
                DB.AsignarParametroCadena("@rfc", prefixText + "%");
                using (DbDataReader DRSum = DB.EjecutarConsulta())
                {
                    string[] items = new string[count];
                    while (DRSum.Read())
                    {
                        items[Contador] = DRSum[0].ToString();
                        Contador++;
                    }

                    DB.Desconectar();
                    if (count == 0) { a[0] = "No existen registros"; return a; }
                    else { return items; }
                }
            }
            catch (Exception e) { clsLogger.Graba_Log_Error(e.Message); DB.Desconectar(); a[0] = e.ToString(); return a; }
            finally
            {
                DB.Desconectar();
            }

        }
    }
}
