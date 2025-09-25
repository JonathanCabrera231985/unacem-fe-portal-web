using Datos;
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using WebServicesCimaUnacem.webservice;
namespace WebServicesCimaUnacem.webService
{
				[ToolboxItem(false), WebService(Namespace = "http://cimait.org/"), WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
				public class WebCimaUnacem : ServicioSeguro
				{
								private static string userWS;
								private static string passWS;
								private BasesDatos DB = new BasesDatos();
								private DbDataReader DR = null;
								private DataTable table = new DataTable("TablaRespuesta");
								private string[] asinfo;
								private ArrayList arrayxmlLote;
								private string[] asinfo3;
								private ArrayList arrayxmlresult;
								private string[] conexion;
								private ArrayList arrayConexion;
								private string conexionBase;
								private string fechaInicio;
								private string fechaFin;
								public WebCimaUnacem()
								{
												WebCimaUnacem.userWS = this.obtener_codigo("userWS");
												WebCimaUnacem.passWS = this.obtener_codigo("passWS");
								}
								[SoapHeader("CredencialAutenticacion"), WebMethod]
								public XmlDocument ConsultaDatosGuia(string Num_Vehiculo, string fechaInicio, string fechaFin)
								{
												XmlDocument xmlDocument = new XmlDocument();
												XmlDocument result;
												if (!string.IsNullOrEmpty(Num_Vehiculo))
												{
																if (Convert.ToDateTime(fechaFin) >= Convert.ToDateTime(fechaInicio))
																{
																				this.fechaInicio = fechaInicio;
																				this.fechaFin = fechaFin.Replace("00:00:00", "23:59:59");
																				try
																				{
																								this.arrayxmlLote = new ArrayList();
																								this.arrayxmlresult = new ArrayList();
																								this.arrayConexion = new ArrayList();
																								if (WebCimaUnacem.VerificarPermisos(this.CredencialAutenticacion))
																								{
																												this.ConsultaInformacionGuias(Num_Vehiculo);
																												xmlDocument = this.xmlInfoGuialote();
																								}
																								result = xmlDocument;
																								return result;
																				}
																				catch (Exception ex)
																				{
																								xmlDocument = this.xmlInfoError(ex.Message);
																								result = xmlDocument;
																								return result;
																				}
																}
																xmlDocument = this.xmlInfoError("La fecha de inicio de consulta es posterior a la fecha fin de consulta");
																result = xmlDocument;
												}
												else
												{
																xmlDocument = this.xmlInfoError("Ingresar numero de vehículo.");
																result = xmlDocument;
												}
												return result;
								}
								public void ConsultaInformacionGuias(string Num_Vehiculo)
								{
												this.conexion = new string[1];
												this.conexion[0] = this.obtener_codigo("CantServ");
												this.arrayConexion.Add(this.conexion);
												this.conexion = new string[1];
												this.conexion[0] = this.obtener_codigo("UnacemPrimarias");
												this.arrayConexion.Add(this.conexion);
												foreach (string[] array in this.arrayConexion)
												{
																this.conexionBase = array[0];
																XmlDocument xmlDocument = new XmlDocument();
																if (this.conexionBase.Equals("CADENA_CONEXION"))
																{
																				this.DB.Conectar();
																}
																if (this.conexionBase.Equals("CADENA_CONEXION2"))
																{
																				this.DB.Conectar2();
																}
																if (this.conexionBase.Equals("CADENA_CONEXION"))
																{
																				this.DB.CrearComandoProcedimiento("SP_Consulta_Datos_Guia");
																}
																if (this.conexionBase.Equals("CADENA_CONEXION2"))
																{
																				this.DB.CrearComandoProcedimiento2("SP_Consulta_Datos_Guia");
																}
																this.DB.AsignarParametroProcedimiento("@numeroVehiculo", DbType.String, Num_Vehiculo);
																this.DB.AsignarParametroProcedimiento("@opcion", DbType.String, "1");
																this.DB.AsignarParametroProcedimiento("@idcomprobante", DbType.Int32, 0);
																this.DB.AsignarParametroProcedimiento("@fechaInicio", DbType.String, this.fechaInicio);
																this.DB.AsignarParametroProcedimiento("@fechaFin", DbType.String,this.fechaFin);
																this.DR = this.DB.EjecutarConsulta();
																while (this.DR.Read())
																{
																				this.asinfo3 = new string[4];
																				this.asinfo3[0] = this.DR["idComprobante"].ToString();
																				this.asinfo3[1] = this.DR["xmlBase"].ToString();
																				this.asinfo3[2] = this.DR["numeroAutorizacion"].ToString();
																				this.asinfo3[3] = this.DR["fechaAutorizacion"].ToString();
																				this.arrayxmlresult.Add(this.asinfo3);
																}
																if (this.conexionBase.Equals("CADENA_CONEXION"))
																{
																				this.DB.Desconectar();
																}
																if (this.conexionBase.Equals("CADENA_CONEXION2"))
																{
																				this.DB.Desconectar2();
																}
																foreach (string[] array2 in this.arrayxmlresult)
																{
																				xmlDocument.LoadXml(array2[1]);
																				xmlDocument.SelectNodes("guiaRemision").Item(0).RemoveChild(xmlDocument.SelectNodes("guiaRemision/infoAdicional").Item(0));
																				XmlDocument xmlDocument2 = this.agregaInfoAdicional(array2[0]);
																				XmlDocument xmlDocument3 = this.crearNodos("numeroAutorizacion", array2[2]);
																				XmlDocument xmlDocument4 = this.crearNodos("fechaAutorizacion", array2[3]);
																				this.asinfo = new string[1];
																				this.asinfo[0] = xmlDocument3.InnerXml + xmlDocument4.InnerXml + xmlDocument.InnerXml + xmlDocument2.InnerXml;
																				this.arrayxmlLote.Add(this.asinfo);
																}
																this.arrayxmlresult = new ArrayList();
												}
								}
								public static bool VerificarPermisos(Autenticacion value)
								{
												return value != null && (value.UsuarioNombre == WebCimaUnacem.userWS && value.UsuarioClave == WebCimaUnacem.passWS);
								}
								private string obtener_codigo(string a_parametro)
								{
												return ConfigurationManager.AppSettings.Get(a_parametro);
								}
								public XmlDocument agregaInfoAdicional(string idcomprobante)
								{
												XmlDocument xmlDocument = new XmlDocument();
												StringWriter stringWriter = new StringWriter();
												XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
												if (this.conexionBase.Equals("CADENA_CONEXION"))
												{
																this.DB.Conectar();
												}
												if (this.conexionBase.Equals("CADENA_CONEXION2"))
												{
																this.DB.Conectar2();
												}
												if (this.conexionBase.Equals("CADENA_CONEXION"))
												{
																this.DB.CrearComandoProcedimiento("SP_Consulta_Datos_Guia");
												}
												if (this.conexionBase.Equals("CADENA_CONEXION2"))
												{
																this.DB.CrearComandoProcedimiento2("SP_Consulta_Datos_Guia");
												}
												this.DB.AsignarParametroProcedimiento("@numeroVehiculo", DbType.String, "");
												this.DB.AsignarParametroProcedimiento("@opcion", DbType.String, "2");
												this.DB.AsignarParametroProcedimiento("@idcomprobante", DbType.Int32, idcomprobante);
												this.DB.AsignarParametroProcedimiento("@fechaInicio", DbType.String, this.fechaInicio);
												this.DB.AsignarParametroProcedimiento("@fechaFin", DbType.String, this.fechaFin);
												this.DR = this.DB.EjecutarConsulta();
												xmlTextWriter.WriteStartElement("infoAdicional");
												while (this.DR.Read())
												{
																xmlTextWriter.WriteStartElement(this.DR["nombre"].ToString());
																xmlTextWriter.WriteString(this.DR["valor"].ToString());
																xmlTextWriter.WriteEndElement();
												}
												xmlTextWriter.WriteEndElement();
												xmlTextWriter.Flush();
												xmlDocument.InnerXml = stringWriter.ToString();
												if (this.conexionBase.Equals("CADENA_CONEXION"))
												{
																this.DB.Desconectar();
												}
												if (this.conexionBase.Equals("CADENA_CONEXION2"))
												{
																this.DB.Desconectar2();
												}
												return xmlDocument;
								}
								public XmlDocument crearNodos(string Nombre, string valor)
								{
												XmlDocument xmlDocument = new XmlDocument();
												StringWriter stringWriter = new StringWriter();
												XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
												xmlTextWriter.WriteStartElement(Nombre);
												xmlTextWriter.WriteString(valor);
												xmlTextWriter.WriteEndElement();
												xmlTextWriter.Flush();
												xmlDocument.InnerXml = stringWriter.ToString();
												this.DB.Desconectar();
												return xmlDocument;
								}
								public XmlDocument xmlInfoGuialote()
								{
												XmlDocument xmlDocument = new XmlDocument();
												StringWriter stringWriter = new StringWriter();
												XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
												XmlDocument result;
												try
												{
																xmlTextWriter.Formatting = Formatting.Indented;
																xmlTextWriter.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");
																if (this.arrayxmlLote != null && this.arrayxmlLote.Count > 0)
																{
																				xmlTextWriter.WriteStartElement("comprobantes");
																				foreach (string[] array in this.arrayxmlLote)
																				{
																								xmlTextWriter.WriteStartElement("comprobanteAutorizado");
																								xmlTextWriter.WriteString(array[0].ToString());
																								xmlTextWriter.WriteEndElement();
																				}
																				xmlTextWriter.WriteEndElement();
																}
																xmlTextWriter.Flush();
																string text = stringWriter.ToString();
																text = text.Replace("&lt;", "<");
																text = text.Replace("&gt;", ">");
																byte[] bytes = Encoding.Default.GetBytes(text);
																text = Encoding.UTF8.GetString(bytes);
																xmlDocument.LoadXml(text);
																xmlDocument.InnerXml = text;
																result = xmlDocument;
												}
												catch (Exception ex)
												{
																result = this.xmlInfoError(ex.Message);
												}
												return result;
								}
								public XmlDocument xmlInfoError(string error)
								{
												XmlDocument xmlDocument = new XmlDocument();
												StringWriter stringWriter = new StringWriter();
												XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
												xmlTextWriter.Formatting = Formatting.Indented;
												xmlTextWriter.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"UTF-8\"");
												xmlTextWriter.WriteStartElement("comprobantes");
												xmlTextWriter.WriteStartElement("descripcionError");
												xmlTextWriter.WriteString(error);
												xmlTextWriter.WriteEndElement();
												xmlTextWriter.WriteEndElement();
												xmlTextWriter.Flush();
												xmlDocument.InnerXml = stringWriter.ToString();
												return xmlDocument;
								}
				}
}
