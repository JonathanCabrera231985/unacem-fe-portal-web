using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Datos;
using System.IO;
using clibLogger;

namespace Control
{
   public class Log
    {
       //private BasesDatos DB;
        public Log() {
            var DB = new BasesDatos();
        }

         public void mensajesLog(string codigo, string mensaje, string mensajeTecnico,string nombreArchivo,string noFolio)
        {
            var DB = new BasesDatos();
            try
            {
                string[] array = new string[2];
                array = PA_mensajes(codigo);
                if (String.IsNullOrEmpty(mensaje))
                {
                    mensaje = "";
                }
                DB.Conectar();
                DB.CrearComando(@"insert into LogErrorFacturas
                                (detalle,fecha,archivo,linea,numeroDocumento,tipo,detalleTecnico) 
                                values 
                                (@detalle,@fecha,@archivo,@linea,@numeroDocumento,@tipo,@detalleTecnico)");
                DB.AsignarParametroCadena("@detalle", array[0].Replace("'", "''") + Environment.NewLine + mensaje.Replace("'", "''"));
                DB.AsignarParametroCadena("@fecha", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                DB.AsignarParametroCadena("@archivo", nombreArchivo.Replace("'", "''"));
                DB.AsignarParametroCadena("@linea", "-");
                DB.AsignarParametroCadena("@tipo", array[1].Replace("'", "''"));
                DB.AsignarParametroCadena("@numeroDocumento", noFolio.Replace("'", "''"));
                DB.AsignarParametroCadena("@detalleTecnico", mensajeTecnico.Replace("'", "''"));
                DB.EjecutarConsulta1();
                DB.Desconectar();
            }
            catch (Exception ex )
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ex.Message);
              
            }
            finally
            {
                DB.Desconectar();
            }

        }

         public void mensajesLogRecepcion(string codigo, string mensaje, string mensajeTecnico, string nombreArchivo, string noFolio)
         {
            var DB = new BasesDatos();
            try
            {
                string[] array = new string[2];
                array = PA_mensajes(codigo);
                if (String.IsNullOrEmpty(mensaje))
                {
                    mensaje = "";
                }
                DB.Conectar();
                DB.CrearComando(@"insert into LogErrorFacturas
                                (detalle,fecha,archivo,linea,numeroDocumento,tipo,detalleTecnico) 
                                values 
                                (@detalle,@fecha,@archivo,@linea,@numeroDocumento,@tipo,@detalleTecnico)");
                DB.AsignarParametroCadena("@detalle", array[0].Replace("'", "''") + Environment.NewLine + mensaje.Replace("'", "''"));
                DB.AsignarParametroCadena("@fecha", System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                DB.AsignarParametroCadena("@archivo", nombreArchivo.Replace("'", "''"));
                DB.AsignarParametroCadena("@linea", "-");
                DB.AsignarParametroCadena("@tipo", array[1].Replace("'", "''"));
                DB.AsignarParametroCadena("@numeroDocumento", noFolio.Replace("'", "''"));
                DB.AsignarParametroCadena("@detalleTecnico", mensajeTecnico.Replace("'", "''"));
                DB.EjecutarConsulta1();
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

        }

         public String[] PA_mensajes(string codigo)
         {
            var DB = new BasesDatos();
            string[] array;
            array = new string[2];
            try
            {
                DB.Conectar();
                DB.CrearComandoProcedimiento("PA_Errores");
                DB.AsignarParametroProcedimiento("@CODIGO", System.Data.DbType.String, codigo);
                using (DbDataReader DRE = DB.EjecutarConsulta())
                {
                    if (DRE.Read())
                    {
                        array[0] = codigo + ": " + DRE[0].ToString();
                        array[1] = DRE[1].ToString();
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

            return array;
         }

      

    }
}
