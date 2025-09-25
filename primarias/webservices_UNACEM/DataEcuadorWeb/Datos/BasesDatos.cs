using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
namespace Datos
{
				public class BasesDatos
				{
								private DbConnection conexion = null;
								private DbConnection conexion2 = null;
								private DbCommand comando = null;
								private DbTransaction transaccion = null;
								private string cadenaConexion;
								private string cadenaConexion2;
								private static DbProviderFactory factory = null;
								protected IDbTransaction mTransaccion;
								public BasesDatos()
								{
												this.Configurar();
								}
								public void Configurar()
								{
												try
												{
																string proveedor = ConfigurationManager.AppSettings.Get("PROVEEDOR_ADONET");
																this.cadenaConexion = ConfigurationManager.AppSettings.Get("CADENA_CONEXION");
																this.cadenaConexion2 = ConfigurationManager.AppSettings.Get("CADENA_CONEXION2");
																BasesDatos.factory = DbProviderFactories.GetFactory(proveedor);
												}
												catch (ConfigurationException ex)
												{
																throw new BaseDatosException("Error al cargar la configuración del acceso a datos.", ex);
												}
								}

								public void Desconectar()
								{
												if (this.conexion.State.Equals(ConnectionState.Open))
												{
																this.conexion.Close();
												}
								}

								public void Desconectar2()
								{
												if (this.conexion2.State.Equals(ConnectionState.Open))
												{
																this.conexion2.Close();
												}
								}

								public void Conectar()
								{
												if (this.conexion != null && !this.conexion.State.Equals(ConnectionState.Closed))
												{
																throw new BaseDatosException("La conexión ya se encuentra abierta.");
												}
												try
												{
																if (this.conexion == null)
																{
																				this.conexion = BasesDatos.factory.CreateConnection();
																				this.conexion.ConnectionString = this.cadenaConexion;
																}
																this.conexion.Open();
												}
												catch (DataException ex)
												{
																throw new BaseDatosException("Error al conectarse a la base de datos.", ex);
												}
								}

								public void Conectar2()
								{
												if (this.conexion2 != null && !this.conexion2.State.Equals(ConnectionState.Closed))
												{
																throw new BaseDatosException("La conexión ya se encuentra abierta.");
												}
												try
												{
																if (this.conexion2 == null)
																{
																				this.conexion2 = BasesDatos.factory.CreateConnection();
																				this.conexion2.ConnectionString = this.cadenaConexion2;
																}
																this.conexion2.Open();
												}
												catch (DataException ex)
												{
																throw new BaseDatosException("Error al conectarse a la base de datos2.", ex);
												}
								}

								public void CrearComando(string sentenciaSQL)
								{
												this.comando = BasesDatos.factory.CreateCommand();
												this.comando.Connection = this.conexion;
												this.comando.CommandType = CommandType.Text;
												this.comando.CommandText = sentenciaSQL;
												if (this.transaccion != null)
												{
																this.comando.Transaction = this.transaccion;
												}
								}

								public void CrearComandoProcedimiento(string sentenciaSQL)
								{
												this.comando = BasesDatos.factory.CreateCommand();
												this.comando.Connection = this.conexion;
												this.comando.CommandType = CommandType.StoredProcedure;
												this.comando.CommandText = sentenciaSQL;
												if (this.transaccion != null)
												{
																this.comando.Transaction = this.transaccion;
												}
								}

								public void CrearComandoProcedimiento2(string sentenciaSQL)
								{
												this.comando = BasesDatos.factory.CreateCommand();
												this.comando.Connection = this.conexion2;
												this.comando.CommandType = CommandType.StoredProcedure;
												this.comando.CommandText = sentenciaSQL;
												if (this.transaccion != null)
												{
																this.comando.Transaction = this.transaccion;
												}
								}

								public DataTable EjecutarConsulta11()
								{
												DataTable tb = new DataTable();
												tb.Load(this.comando.ExecuteReader());
												return tb;
								}

								public void AsignarParametroProcedimiento(string nombre, DbType tipo, object valor)
								{
												DbParameter param = this.comando.CreateParameter();
												param.ParameterName = nombre;
												param.DbType = tipo;
												param.Value = valor;
												this.comando.Parameters.Add(param);
								}

								public void AsignarParametroProcedimiento(string nombre, DbType tipo, int size, bool salida)
								{
												DbParameter param = this.comando.CreateParameter();
												param.ParameterName = nombre;
												param.DbType = tipo;
												param.Size = size;
												if (salida)
												{
																param.Direction = ParameterDirection.Output;
												}
												else
												{
																param.Direction = ParameterDirection.Input;
												}
												this.comando.Parameters.Add(param);
								}

								public object devolverParametroProcedimiento(string nombre)
								{
												return this.comando.Parameters[nombre].Value;
								}

								public void AsignarParametroFlotante(string nombre, string valor)
								{
												if (valor == " ")
												{
																valor = "0";
												}
												this.AsignarParametro(nombre, "", valor.ToString());
								}

								public void AsignarParametroEntero(string nombre, int valor)
								{
												this.AsignarParametro(nombre, "", valor.ToString());
								}

								public void AsignarParametroCadena(string nombre, string valor)
								{
												this.AsignarParametro(nombre, "'", valor);
								}

								public void AsignarParametroFecha(string nombre, string valor)
								{
												this.AsignarParametro(nombre, "'", valor.ToString());
								}

								private void AsignarParametro(string nombre, string separador, string valor)
								{
												int indice = this.comando.CommandText.IndexOf(nombre);
												string prefijo = this.comando.CommandText.Substring(0, indice);
												string sufijo = this.comando.CommandText.Substring(indice + nombre.Length);
												this.comando.CommandText = string.Concat(new string[]
			{
				prefijo,
				separador,
				valor,
				separador,
				sufijo
			});
								}

								public DbDataReader EjecutarConsulta()
								{
												return this.comando.ExecuteReader();
								}

								public int EjecutarConsultaFilasafectadas()
								{
												return this.comando.ExecuteNonQuery();
								}

								public void EjecutarConsulta1()
								{
												this.comando.ExecuteReader();
								}

								public DataSet TraerDataset(string ProcedimientoAlmacenado, params object[] Args)
								{
												DataSet mDataSet = new DataSet();
												this.mTransaccion = this.conexion.BeginTransaction(IsolationLevel.Serializable);
												IDataAdapter ida = this.CrearDataAdapter(ProcedimientoAlmacenado, Args);
												ida.Fill(mDataSet);
												this.mTransaccion.Commit();
												this.Desconectar();
												return mDataSet;
								}

								public void TraerDataset(DataSet ds)
								{
												try
												{
																if (ds != null)
																{
																				foreach (DataTable dt in ds.Tables)
																				{
																								foreach (DataRow dr in dt.Rows)
																								{
																												this.TraerDataset(dt.TableName, dr.ItemArray);
																								}
																				}
																}
												}
												catch (Exception ex)
												{
																throw ex;
												}
								}

								public DataSet TraerDataSetConsulta(string ComandoSelect, params object[] Args)
								{
												DataSet mDataASet = new DataSet();
												this.mTransaccion = this.conexion.BeginTransaction(IsolationLevel.Serializable);
												IDataAdapter ida = this.CrearDataAdapterSelect(ComandoSelect, Args);
												ida.Fill(mDataASet);
												this.mTransaccion.Commit();
												this.Desconectar();
												return mDataASet;
								}

								protected IDataAdapter CrearDataAdapter(string ProcedimientoAlmacenado, params object[] Args)
								{
												SqlDataAdapter Da = new SqlDataAdapter((SqlCommand)this.Comandos(ProcedimientoAlmacenado));
												if (Args.Length != 0)
												{
																this.CargarParametros(Da.SelectCommand, Args);
												}
												return Da;
								}

								protected IDataAdapter CrearDataAdapterSelect(string SecuenciaSelect, params object[] Args)
								{
												SqlDataAdapter Da = new SqlDataAdapter((SqlCommand)this.Comandosselect(SecuenciaSelect));
												if (Args.Length != 0)
												{
																this.AsignarParametroSelect("'", Da.SelectCommand, Args);
												}
												return Da;
								}

								protected IDbCommand Comandos(string ProcedimientoAlmacenado)
								{
												SqlCommand Com = new SqlCommand(ProcedimientoAlmacenado, (SqlConnection)this.conexion);
												Com.Transaction = (SqlTransaction)this.mTransaccion;
												Com.CommandType = CommandType.StoredProcedure;
												SqlCommandBuilder.DeriveParameters(Com);
												return Com;
								}

								protected IDbCommand Comandosselect(string SecuenciaSelect)
								{
												return new SqlCommand(SecuenciaSelect, (SqlConnection)this.conexion)
												{
																Transaction = (SqlTransaction)this.mTransaccion,
																CommandType = CommandType.Text
												};
								}

								protected void CargarParametros(IDbCommand Com, object[] Args)
								{
												int Limite = Com.Parameters.Count;
												for (int i = 1; i < Com.Parameters.Count; i++)
												{
																SqlParameter P = (SqlParameter)Com.Parameters[i - 1];
																SqlParameter P2 = (SqlParameter)Com.Parameters[i];
																if (i <= Args.Length)
																{
																				P2.Value = Args[i - 1];
																}
																else
																{
																				P2.Value = null;
																}
												}
								}

								private void AsignarParametroSelect(string separador, IDbCommand Com, object[] Args)
								{
												for (int i = 1; i < Args.Length + 1; i++)
												{
																int indice = Com.CommandText.IndexOf("@p" + i.ToString());
																string prefijo = Com.CommandText.Substring(0, indice);
																string sufijo = Com.CommandText.Substring(indice + ("@p" + i).ToString().Length);
																Com.CommandText = string.Concat(new string[]
				{
					prefijo,
					separador,
					Args[i - 1].ToString(),
					separador,
					sufijo
				});
												}
								}
				}
}
