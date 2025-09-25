using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Datos;
using CriptoSimetrica;
using clibLogger;

namespace EncriptadoMasivo
{
    public partial class Form1 : Form
    {

        public AES256 Cs256 = new AES256();
        public AES Cs = new AES();

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_empleados(object sender, EventArgs e)
        {
            string Antes, Despues;
            bool encr;
            var DB = new BasesDatos();
            DB.Conectar();
            DataSet ds = DB.TraerDataSetConsulta(@"SELECT IdEmpleado, userEmpleado, claveEmpleado FROM Empleados");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Antes = "";
                    Despues = "";
                    encr = true;
                    if (ds.Tables[0].Rows[i]["claveEmpleado"].ToString() == ds.Tables[0].Rows[i]["userEmpleado"].ToString())
                    {
                        clsLogger.Graba_Log("Encriptado DIRECTO", "EncMasivoUnacem");
                        clsLogger.Graba_Log("IDEMPLEADO:" + ds.Tables[0].Rows[i]["IdEmpleado"].ToString() + "claveEmpleadoENC:" + ds.Tables[0].Rows[i]["claveEmpleado"].ToString(), "EncMasivoUnacem");
                        try
                        {
                            Antes = ds.Tables[0].Rows[i]["claveEmpleado"].ToString();
                            Despues = Cs256.encriptar(Antes, "CIMAIT");
                        }
                        catch (Exception ex)
                        {
                            clsLogger.Graba_Log_Error("NO PUEDE ENCRYPTARSE/DESENCRYPTARSE IDEMPLEADO:" + ds.Tables[0].Rows[i]["IdEmpleado"].ToString());
                            encr = false;
                        }
                    }
                    else
                    {
                        clsLogger.Graba_Log("Encriptado PUENTE", "EncMasivoUnacem");
                        clsLogger.Graba_Log("IDEMPLEADO:" + ds.Tables[0].Rows[i]["IdEmpleado"].ToString() + "claveEmpleadoENC:" + ds.Tables[0].Rows[i]["claveEmpleado"].ToString(), "EncMasivoUnacem");
                        try
                        {
                            Antes = ds.Tables[0].Rows[i]["claveEmpleado"].ToString();
                            Antes = Cs.desencriptar(Antes, "CIMAIT");
                            Despues = Cs256.encriptar(Antes, "CIMAIT");
                        }
                        catch (Exception ex)
                        {
                            clsLogger.Graba_Log_Error("NO PUEDE ENCRYPTARSE/DESENCRYPTARSE IDEMPLEADO:" + ds.Tables[0].Rows[i]["IdEmpleado"].ToString());
                            encr = false;
                        }
                    }
                    if (encr)
                    {
                        var DBGrabar = new BasesDatos();
                        DBGrabar.Conectar();
                        DBGrabar.TraerDataSetConsulta(@"update EMPLEADOS set claveEmpleado = @p1 where IdEmpleado = @p2", new Object[] { Despues, ds.Tables[0].Rows[i]["IdEmpleado"] });
                        DBGrabar.Desconectar();
                        clsLogger.Graba_Log("IDEMPLEADO:" + ds.Tables[0].Rows[i]["IdEmpleado"].ToString() + "claveEmpleadoENC256:" + Despues, "EncMasivoUnacem");
                    }
                    else
                    {
                        clsLogger.Graba_Log("IDEMPLEADO:" + ds.Tables[0].Rows[i]["IdEmpleado"].ToString() + "NOMODIFICADO", "EncMasivoUnacem");
                    }
                }
            }
            DB.Desconectar();
            MessageBox.Show("ACTUALIZACION EMPLEADOS FINALIZADA.");
        }

        private void btn_clientes(object sender, EventArgs e)
        {
            string Antes, Despues;
            bool encr;
            var DB = new BasesDatos();
            DB.Conectar();
            DataSet ds = DB.TraerDataSetConsulta(@"SELECT idCliente, userCliente, claveCliente FROM Clientes");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Antes = "";
                    Despues = "";
                    encr = true;
                    if (ds.Tables[0].Rows[i]["claveCliente"].ToString() == ds.Tables[0].Rows[i]["userCliente"].ToString())
                    {
                        clsLogger.Graba_Log("Encriptado DIRECTO", "EncMasivoUnacem");
                        clsLogger.Graba_Log("IDCLIENTE:" + ds.Tables[0].Rows[i]["idCliente"].ToString() + "claveClienteENC:" + ds.Tables[0].Rows[i]["claveCliente"].ToString(), "EncMasivoUnacem");
                        try
                        {
                            Antes = ds.Tables[0].Rows[i]["claveCliente"].ToString();
                            Despues = Cs256.encriptar(Antes, "CIMAIT");
                        }
                        catch (Exception ex)
                        {
                            clsLogger.Graba_Log_Error("NO PUEDE ENCRYPTARSE/DESENCRYPTARSE IDCLIENTE:" + ds.Tables[0].Rows[i]["idCliente"].ToString());
                            encr = false;
                        }
                    }
                    else
                    {
                        clsLogger.Graba_Log("Encriptado PUENTE", "EncMasivoUnacem");
                        clsLogger.Graba_Log("IDCLIENTE:" + ds.Tables[0].Rows[i]["idCliente"].ToString() + "claveClienteENC:" + ds.Tables[0].Rows[i]["claveCliente"].ToString(), "EncMasivoUnacem");
                        try
                        {
                            Antes = ds.Tables[0].Rows[i]["claveCliente"].ToString();
                            Antes = Cs.desencriptar(Antes, "CIMAIT");
                            Despues = Cs256.encriptar(Antes, "CIMAIT");
                        }
                        catch (Exception ex)
                        {
                            clsLogger.Graba_Log_Error("NO PUEDE ENCRYPTARSE/DESENCRYPTARSE IDCLIENTE:" + ds.Tables[0].Rows[i]["idCliente"].ToString());
                            encr = false;
                        }
                    }
                    if (encr)
                    {
                        var DBGrabar = new BasesDatos();
                        DBGrabar.Conectar();
                        DBGrabar.TraerDataSetConsulta(@"update CLIENTES set claveCliente = @p1 where idCliente = @p2", new Object[] { Despues, ds.Tables[0].Rows[i]["idCliente"] });
                        DBGrabar.Desconectar();
                        clsLogger.Graba_Log("IDCLIENTE:" + ds.Tables[0].Rows[i]["idCliente"].ToString() + "claveClienteENC256:" + Despues, "EncMasivoUnacem");
                    }
                    else
                    {
                        clsLogger.Graba_Log("IDCLIENTE:" + ds.Tables[0].Rows[i]["idCliente"].ToString() + "NOMODIFICADO", "EncMasivoUnacem");
                    }
                }
            }
            DB.Desconectar();
            MessageBox.Show("ACTUALIZACION CLIENTES FINALIZADA.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string Antes, Despues;
            bool encr;
            var DB = new BasesDatos();
            DB.Conectar();
            DataSet ds = DB.TraerDataSetConsulta(@"SELECT IDEEMI, passp12 FROM Emisor");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Antes = "";
                    Despues = "";
                    encr = true;
                                        
                    clsLogger.Graba_Log("Encriptado PUENTE", "EncMasivoUnacem");
                    clsLogger.Graba_Log("IDEEMI:" + ds.Tables[0].Rows[i]["IDEEMI"].ToString() + "PASSP12:" + ds.Tables[0].Rows[i]["Passp12"].ToString(), "EncMasivoUnacem");
                    try
                    {
                        Antes = ds.Tables[0].Rows[i]["Passp12"].ToString();
                        Antes = Cs.desencriptar(Antes, "CIMAIT");
                        Despues = Cs256.encriptar(Antes, "CIMAIT");
                    }
                    catch (Exception ex)
                    {
                        clsLogger.Graba_Log_Error("NO PUEDE ENCRYPTARSE/DESENCRYPTARSE IDEEMI:" + ds.Tables[0].Rows[i]["IDEEMI"].ToString());
                        encr = false;
                    }

                    if (encr)
                    {
                        var DBGrabar = new BasesDatos();
                        DBGrabar.Conectar();
                        DBGrabar.TraerDataSetConsulta(@"update EMISOR set Passp12 = @p1 where IDEEMI = @p2", new Object[] { Despues, ds.Tables[0].Rows[i]["IDEEMI"] });
                        DBGrabar.Desconectar();
                        clsLogger.Graba_Log("IDEEMI:" + ds.Tables[0].Rows[i]["IDEEMI"].ToString() + "claveClienteENC256:" + Despues, "EncMasivoUnacem");
                    }
                    else
                    {
                        clsLogger.Graba_Log("IDEEMI:" + ds.Tables[0].Rows[i]["IDEEMI"].ToString() + "NOMODIFICADO", "EncMasivoUnacem");
                    }
                }
            }
            DB.Desconectar();

            DB.Conectar();
            DataSet ds2 = DB.TraerDataSetConsulta(@"SELECT idparametro, passp12 FROM ParametrosSistema");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                {
                    Antes = "";
                    Despues = "";
                    encr = true;

                    clsLogger.Graba_Log("Encriptado PUENTE", "EncMasivoUnacem");
                    clsLogger.Graba_Log("idparametro:" + ds2.Tables[0].Rows[i]["idparametro"].ToString() + "PASSP12:" + ds2.Tables[0].Rows[i]["Passp12"].ToString(), "EncMasivoUnacem");
                    try
                    {
                        Antes = ds2.Tables[0].Rows[i]["Passp12"].ToString();
                        Antes = Cs.desencriptar(Antes, "CIMAIT");
                        Despues = Cs256.encriptar(Antes, "CIMAIT");
                    }
                    catch (Exception ex)
                    {
                        clsLogger.Graba_Log_Error("NO PUEDE ENCRYPTARSE/DESENCRYPTARSE idparametro:" + ds2.Tables[0].Rows[i]["idparametro"].ToString());
                        encr = false;
                    }

                    if (encr)
                    {
                        var DBGrabar = new BasesDatos();
                        DBGrabar.Conectar();
                        DBGrabar.TraerDataSetConsulta(@"update ParametrosSistema set Passp12 = @p1 where idparametro = @p2", new Object[] { Despues, ds2.Tables[0].Rows[i]["idparametro"] });
                        DBGrabar.Desconectar();
                        clsLogger.Graba_Log("idparametro:" + ds2.Tables[0].Rows[i]["idparametro"].ToString() + "claveClienteENC256:" + Despues, "EncMasivoUnacem");
                    }
                    else
                    {
                        clsLogger.Graba_Log("idparametro:" + ds2.Tables[0].Rows[i]["idparametro"].ToString() + "NOMODIFICADO", "EncMasivoUnacem");
                    }
                }
            }
            DB.Desconectar();

            MessageBox.Show("ACTUALIZACION CERTIFICADOS FINALIZADA.");
        }

        private void btn_nocrypt_Click(object sender, EventArgs e)
        {
            string Antes, Despues;
            bool encr;
            var DB = new BasesDatos();
            DB.Conectar();
            DataSet ds = DB.TraerDataSetConsulta(@"SELECT IDEEMI, passRecepcion, passSMTP FROM Emisor");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Antes = "";
                    Despues = "";
                    encr = true;

                    clsLogger.Graba_Log("Encriptado PUENTE", "EncMasivoUnacem");
                    clsLogger.Graba_Log("IDEEMI:" + ds.Tables[0].Rows[i]["IDEEMI"].ToString() + "passRecepcion:" + ds.Tables[0].Rows[i]["passRecepcion"].ToString(), "EncMasivoUnacem");
                    try
                    {
                        Antes = ds.Tables[0].Rows[i]["passRecepcion"].ToString();
                        Antes = Cs256.desencriptar(Antes, "CIMAIT");
                        Despues = Cs256.encriptar(Antes, "CIMAIT");
                    }
                    catch (Exception ex)
                    {
                        clsLogger.Graba_Log_Error("NO PUEDE ENCRYPTARSE/DESENCRYPTARSE passRecepcion de IDEEMI:" + ds.Tables[0].Rows[i]["IDEEMI"].ToString());
                        encr = false;
                    }

                    if (encr)
                    {
                        var DBGrabar = new BasesDatos();
                        DBGrabar.Conectar();
                        DBGrabar.TraerDataSetConsulta(@"update EMISOR set passRecepcion = @p1 where IDEEMI = @p2", new Object[] { Despues, ds.Tables[0].Rows[i]["IDEEMI"] });
                        DBGrabar.Desconectar();
                        clsLogger.Graba_Log("IDEEMI:" + ds.Tables[0].Rows[i]["IDEEMI"].ToString() + "claveClienteENC256:" + Despues, "EncMasivoUnacem");
                    }
                    else
                    {
                        Antes = ds.Tables[0].Rows[i]["passRecepcion"].ToString();
                        Despues = Cs256.encriptar(Antes, "CIMAIT");
                        var DBGrabar = new BasesDatos();
                        DBGrabar.Conectar();
                        DBGrabar.TraerDataSetConsulta(@"update EMISOR set passRecepcion = @p1 where IDEEMI = @p2", new Object[] { Despues, ds.Tables[0].Rows[i]["IDEEMI"] });
                        DBGrabar.Desconectar();
                        clsLogger.Graba_Log("IDEEMI:" + ds.Tables[0].Rows[i]["IDEEMI"].ToString() + "claveClienteENC256:" + Despues, "EncMasivoUnacem");
                    }
                }
            }
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Antes = "";
                    Despues = "";
                    encr = true;

                    clsLogger.Graba_Log("Encriptado PUENTE", "EncMasivoUnacem");
                    clsLogger.Graba_Log("IDEEMI:" + ds.Tables[0].Rows[i]["IDEEMI"].ToString() + "passSMTP:" + ds.Tables[0].Rows[i]["passSMTP"].ToString(), "EncMasivoUnacem");
                    try
                    {
                        Antes = ds.Tables[0].Rows[i]["passSMTP"].ToString();
                        Antes = Cs256.desencriptar(Antes, "CIMAIT");
                        Despues = Cs256.encriptar(Antes, "CIMAIT");
                    }
                    catch (Exception ex)
                    {
                        clsLogger.Graba_Log_Error("NO PUEDE ENCRYPTARSE/DESENCRYPTARSE passSMTP de IDEEMI:" + ds.Tables[0].Rows[i]["IDEEMI"].ToString());
                        encr = false;
                    }

                    if (encr)
                    {
                        var DBGrabar = new BasesDatos();
                        DBGrabar.Conectar();
                        DBGrabar.TraerDataSetConsulta(@"update EMISOR set passSMTP = @p1 where IDEEMI = @p2", new Object[] { Despues, ds.Tables[0].Rows[i]["IDEEMI"] });
                        DBGrabar.Desconectar();
                        clsLogger.Graba_Log("IDEEMI:" + ds.Tables[0].Rows[i]["IDEEMI"].ToString() + "claveClienteENC256:" + Despues, "EncMasivoUnacem");
                    }
                    else
                    {
                        Antes = ds.Tables[0].Rows[i]["passSMTP"].ToString();
                        Despues = Cs256.encriptar(Antes, "CIMAIT");
                        var DBGrabar = new BasesDatos();
                        DBGrabar.Conectar();
                        DBGrabar.TraerDataSetConsulta(@"update EMISOR set passSMTP = @p1 where IDEEMI = @p2", new Object[] { Despues, ds.Tables[0].Rows[i]["IDEEMI"] });
                        DBGrabar.Desconectar();
                        clsLogger.Graba_Log("IDEEMI:" + ds.Tables[0].Rows[i]["IDEEMI"].ToString() + "claveClienteENC256:" + Despues, "EncMasivoUnacem");
                    }
                }
            }
            DB.Desconectar();

            DB.Conectar();
            DataSet ds2 = DB.TraerDataSetConsulta(@"SELECT idparametro, passRecepcion, passSMTP FROM ParametrosSistema");
            if (ds2.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                {
                    Antes = "";
                    Despues = "";
                    encr = true;

                    clsLogger.Graba_Log("Encriptado PUENTE", "EncMasivoUnacem");
                    clsLogger.Graba_Log("idparametro:" + ds2.Tables[0].Rows[i]["idparametro"].ToString() + "passRecepcion:" + ds2.Tables[0].Rows[i]["passRecepcion"].ToString(), "EncMasivoUnacem");
                    try
                    {
                        Antes = ds2.Tables[0].Rows[i]["passRecepcion"].ToString();
                        Antes = Cs256.desencriptar(Antes, "CIMAIT");
                        Despues = Cs256.encriptar(Antes, "CIMAIT");
                    }
                    catch (Exception ex)
                    {
                        clsLogger.Graba_Log_Error("NO PUEDE ENCRYPTARSE/DESENCRYPTARSE passRecepcion de idparametro:" + ds2.Tables[0].Rows[i]["idparametro"].ToString());
                        encr = false;
                    }

                    if (encr)
                    {
                        var DBGrabar = new BasesDatos();
                        DBGrabar.Conectar();
                        DBGrabar.TraerDataSetConsulta(@"update ParametrosSistema set passRecepcion = @p1 where idparametro = @p2", new Object[] { Despues, ds2.Tables[0].Rows[i]["idparametro"] });
                        DBGrabar.Desconectar();
                        clsLogger.Graba_Log("idparametro:" + ds2.Tables[0].Rows[i]["idparametro"].ToString() + "claveClienteENC256:" + Despues, "EncMasivoUnacem");
                    }
                    else
                    {
                        Antes = ds2.Tables[0].Rows[i]["passRecepcion"].ToString();
                        Despues = Cs256.encriptar(Antes, "CIMAIT");
                        var DBGrabar = new BasesDatos();
                        DBGrabar.Conectar();
                        DBGrabar.TraerDataSetConsulta(@"update ParametrosSistema set passRecepcion = @p1 where idparametro = @p2", new Object[] { Despues, ds2.Tables[0].Rows[i]["idparametro"] });
                        DBGrabar.Desconectar();
                        clsLogger.Graba_Log("idparametro:" + ds2.Tables[0].Rows[i]["idparametro"].ToString() + "claveClienteENC256:" + Despues, "EncMasivoUnacem");
                    }
                }
            }
            if (ds2.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                {
                    Antes = "";
                    Despues = "";
                    encr = true;

                    clsLogger.Graba_Log("Encriptado PUENTE", "EncMasivoUnacem");
                    clsLogger.Graba_Log("idparametro:" + ds2.Tables[0].Rows[i]["idparametro"].ToString() + "passSMTP:" + ds2.Tables[0].Rows[i]["passSMTP"].ToString(), "EncMasivoUnacem");
                    try
                    {
                        Antes = ds2.Tables[0].Rows[i]["passSMTP"].ToString();
                        Antes = Cs256.desencriptar(Antes, "CIMAIT");
                        Despues = Cs256.encriptar(Antes, "CIMAIT");
                    }
                    catch (Exception ex)
                    {
                        clsLogger.Graba_Log_Error("NO PUEDE ENCRYPTARSE/DESENCRYPTARSE passSMTP de idparametro:" + ds2.Tables[0].Rows[i]["idparametro"].ToString());
                        encr = false;
                    }

                    if (encr)
                    {
                        var DBGrabar = new BasesDatos();
                        DBGrabar.Conectar();
                        DBGrabar.TraerDataSetConsulta(@"update ParametrosSistema set passSMTP = @p1 where idparametro = @p2", new Object[] { Despues, ds2.Tables[0].Rows[i]["idparametro"] });
                        DBGrabar.Desconectar();
                        clsLogger.Graba_Log("idparametro:" + ds2.Tables[0].Rows[i]["idparametro"].ToString() + "claveClienteENC256:" + Despues, "EncMasivoUnacem");
                    }
                    else
                    {
                        Antes = ds2.Tables[0].Rows[i]["passSMTP"].ToString();
                        Despues = Cs256.encriptar(Antes, "CIMAIT");
                        var DBGrabar = new BasesDatos();
                        DBGrabar.Conectar();
                        DBGrabar.TraerDataSetConsulta(@"update ParametrosSistema set passSMTP = @p1 where idparametro = @p2", new Object[] { Despues, ds2.Tables[0].Rows[i]["idparametro"] });
                        DBGrabar.Desconectar();
                        clsLogger.Graba_Log("idparametro:" + ds2.Tables[0].Rows[i]["idparametro"].ToString() + "claveClienteENC256:" + Despues, "EncMasivoUnacem");
                    }
                }
            }
            DB.Desconectar();
            MessageBox.Show("ACTUALIZACION Crypts FINALIZADA.");
        }
    }
}
