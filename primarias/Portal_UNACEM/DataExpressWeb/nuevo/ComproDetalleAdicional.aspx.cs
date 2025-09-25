using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Common;
using clibLogger;
using CriptoSimetrica;

namespace DataExpressWeb.nuevo
{
    public partial class ComproDetalleAdicional : System.Web.UI.Page
    {
        private String cod_principal;
        private String idUser;
        string user = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                cod_principal = Control.Encriptaciondes.ClaseEncriptacion.Instance.Decrypt(Request.QueryString.Get("Cod_Prod"));
                if (Session["idUser"] != null)
                {
                    idUser = Session["idUser"].ToString();
                    if (!Page.IsPostBack)
                    {
                        int id_DetallesTemp = ConsultaIdDetalleTemp();
                        DB.Conectar();
                        DB.CrearComando(@"SELECT nombre, valor FROM DetallesAdicionalesTemp WITH (NOLOCK) 
                                  where codigoTemp = @codigoTemp and id_Empleado = @id_Empleado and id_DetallesTemp = @id_DetallesTemp");
                        DB.AsignarParametroCadena("@id_DetallesTemp", id_DetallesTemp.ToString());
                        DB.AsignarParametroCadena("@codigoTemp", cod_principal);
                        DB.AsignarParametroCadena("@id_Empleado", idUser);
                        using (DbDataReader DRCont = DB.EjecutarConsulta())
                        {
                            while (DRCont.Read())
                            {
                                if (DRCont[0].ToString().Trim().Equals("B/L Nº"))
                                    txt_bl.Text = DRCont[1].ToString();
                                if (DRCont[0].ToString().Trim().Equals("BUQUE"))
                                    txt_buque.Text = DRCont[1].ToString();
                                if (DRCont[0].ToString().Trim().Equals("VIAJE"))
                                    txt_viaje.Text = DRCont[1].ToString();
                            }
                        }
                        DB.Desconectar();
                    }
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
        }
        private Int32 ConsultaIdDetalleTemp()
        {
            var DB = new BasesDatos();
            int idCodigoTempDetalle = 0;
            try
            {
                DB.Conectar();
                DB.CrearComando(@"SELECT idDetallesTemp FROM DetallesTemp WITH (NOLOCK)  WHERE id_Empleado = @id_Empleado and codigoPrincipal=@codigoPrincipal");
                DB.AsignarParametroCadena("@id_Empleado", idUser);
                DB.AsignarParametroCadena("@codigoPrincipal", cod_principal);
                using (DbDataReader DRCont = DB.EjecutarConsulta())
                {
                    if (DRCont.Read())
                    {
                        idCodigoTempDetalle = Convert.ToInt32(DRCont[0]);
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
            return idCodigoTempDetalle;
        }

        private Boolean InsertarDetalleAdicional(int idCodigoTempDetalle, String Valor, String nombre)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Conectar();
                DB.CrearComando(@"INSERT INTO DetallesAdicionalesTemp
                                    (nombre     ,valor        ,id_DetallesTemp            ,codigoTemp
                                    ,id_Empleado)
                                VALUES(
                                    @nombre,    @valor,      @id_DetallesTemp            ,@codigoTemp,
                                    @id_Empleado
                                    ) ");
                DB.AsignarParametroCadena("@nombre", nombre);
                DB.AsignarParametroCadena("@valor", Valor);
                DB.AsignarParametroCadena("@id_DetallesTemp", idCodigoTempDetalle.ToString());
                DB.AsignarParametroCadena("@codigoTemp", cod_principal);
                DB.AsignarParametroCadena("@id_Empleado", idUser);
                DB.EjecutarConsulta1();
                DB.Desconectar();
                return true;
            }
            catch (Exception ex)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ex.Message);
                return false;
            }
            finally
            {
                DB.Desconectar();
            }
        }

        private Boolean UpdateDetalleAdicional(int idCodigoTempDetalle, String Valor, String nombre)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Conectar();
                DB.CrearComando(@"UPDATE DetallesAdicionalesTemp
                                     set valor  = @valor 
                                where codigoTemp = @codigoTemp and id_Empleado = @id_Empleado and nombre = @nombre and id_DetallesTemp = @id_DetallesTemp
                                   ");
                DB.AsignarParametroCadena("@nombre", nombre);
                DB.AsignarParametroCadena("@valor", Valor);
                DB.AsignarParametroCadena("@id_DetallesTemp", idCodigoTempDetalle.ToString());
                DB.AsignarParametroCadena("@codigoTemp", cod_principal);
                DB.AsignarParametroCadena("@id_Empleado", idUser);
                DB.EjecutarConsulta1();
                DB.Desconectar();
                return true;
            }
            catch (Exception ex)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ex.Message);
                return false;
            }
            finally
            {
                DB.Desconectar();
            }
        }

        public Int32 ConsultDetalleAdicional(int idCodigoTempDetalle, String nombre)
        {
            var DB = new BasesDatos();
            int contador = 0;
            try
            {
                DB.Conectar();
                DB.CrearComando(@"SELECT count(*) FROM DetallesAdicionalesTemp WITH (NOLOCK) 
                     where codigoTemp = @codigoTemp and id_Empleado = @id_Empleado and nombre = @nombre and id_DetallesTemp = @id_DetallesTemp");
                DB.AsignarParametroCadena("@nombre", nombre);
                DB.AsignarParametroCadena("@id_DetallesTemp", idCodigoTempDetalle.ToString());
                DB.AsignarParametroCadena("@codigoTemp", cod_principal);
                DB.AsignarParametroCadena("@id_Empleado", idUser);
                using (DbDataReader DRCont = DB.EjecutarConsulta())
                {
                    if (DRCont.Read())
                    {
                        contador = Convert.ToInt32(DRCont[0]);
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
            return contador;
        }

        protected void bto_asignar_Click(object sender, EventArgs e)
        {
            try
            {
                int idCodigoTempDetalle = 0;
                if (txt_bl.Text.Trim().Equals("") & txt_buque.Text.Trim().Equals("") & txt_viaje.Text.Trim().Equals(""))
                {
                    String Script = "<script language='javascript'>" +
                            "alert('Debe ingresar al menos 1 detalle adicional');" +
                            "</script>";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ajax", Script, false);
                    return;
                }
                idCodigoTempDetalle = ConsultaIdDetalleTemp();
                if (idCodigoTempDetalle > 0)
                {
                    if (ConsultDetalleAdicional(idCodigoTempDetalle, "B/L Nº") == 0)
                        InsertarDetalleAdicional(idCodigoTempDetalle, txt_bl.Text, "B/L Nº");
                    else
                        UpdateDetalleAdicional(idCodigoTempDetalle, txt_bl.Text, "B/L Nº");

                    if (ConsultDetalleAdicional(idCodigoTempDetalle, "BUQUE") == 0)
                        InsertarDetalleAdicional(idCodigoTempDetalle, txt_buque.Text, "BUQUE");
                    else
                        UpdateDetalleAdicional(idCodigoTempDetalle, txt_buque.Text, "BUQUE");

                    if (ConsultDetalleAdicional(idCodigoTempDetalle, "VIAJE") == 0)
                        InsertarDetalleAdicional(idCodigoTempDetalle, txt_viaje.Text, "VIAJE");
                    else
                        UpdateDetalleAdicional(idCodigoTempDetalle, txt_viaje.Text, "VIAJE");

                    String Script = "<script language='javascript'>" +
                        "alert('Datos Ingresados con exito'); window.close(); " +
                    "</script>";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ajax", Script, false);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}