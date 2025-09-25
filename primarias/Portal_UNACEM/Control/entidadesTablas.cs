using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Datos;
using clibLogger;

namespace Control
{
    public  class Sucursales
    {
        public int idSucursal { get; set; }
        public String clave { get; set; }
        public String sucursal { get; set; }
        public String domicilio { get; set; }
        public Boolean estado { get; set; }
        public List<detallePuntoEmision> destalles { get; set; }
    }

    public class detallePuntoEmision
    {
        public int idCaja { get; set; }
        public int idSucursal { get; set; }
        public String descripcion { get; set; }
        public String ptoEmi { get; set; }
        public Boolean estado { get; set; }
    }

    public class extraerDatos
    {
        //private Datos.BasesDatos DB = new Datos.BasesDatos();
        public  List<Sucursales> extraerDatosSucursales(String idEmpresa, String querySucursales, String queryPuntoEmision)
        {
            var DB = new BasesDatos();
            List<Sucursales> list = new List<Sucursales>();
            try
            {
               
                DB.Conectar();
                DataTable dt = DB.TraerDataSetConsulta(querySucursales + idEmpresa, new Object[] { }).Tables[0];
                foreach (DataRow dr in dt.Rows)
                    list.Add(loadSucursales(dr, queryPuntoEmision));
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


            return list;
        }

        private Sucursales loadSucursales(DataRow dr, String queryPuntoEmision) 
        {
            Sucursales sucursales = new Sucursales();
            sucursales.idSucursal = Convert.ToInt32(dr["idSucursal"]);
            sucursales.clave = dr["clave"].ToString();
            sucursales.sucursal = dr["sucursal"].ToString();
            sucursales.domicilio = dr["domicilio"].ToString();
            sucursales.estado = Convert.ToBoolean(dr["estado"]);
            sucursales.destalles = extraerDatosPuntoEmision(dr["idSucursal"].ToString(), queryPuntoEmision);
            return sucursales;
        }

        private List<detallePuntoEmision> extraerDatosPuntoEmision(String idSucursal, String queryPuntoEmision)
        {
            var DB = new BasesDatos();
            List<detallePuntoEmision> list = new List<detallePuntoEmision>();
            try
            {
                DB.Conectar();
                DataTable dt = DB.TraerDataSetConsulta(queryPuntoEmision + idSucursal, new Object[] { }).Tables[0];
                foreach (DataRow dr in dt.Rows)
                    list.Add(loadPuntoEmision(dr));
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

            return list;
        }

        private  detallePuntoEmision loadPuntoEmision(DataRow dr)
        {
            detallePuntoEmision emision = new detallePuntoEmision();
            emision.idSucursal = Convert.ToInt32(dr["idSucursal"]);
            emision.idCaja = Convert.ToInt32(dr["idCaja"]);
            emision.descripcion = dr["descripcion"].ToString();
            emision.ptoEmi = dr["ptoEmi"].ToString();
            emision.estado = Convert.ToBoolean(dr["estado"]);
            return emision;
        }
    }
}
