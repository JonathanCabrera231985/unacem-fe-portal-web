using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Datos;
using System.Configuration;
using Reportes;
namespace Control
{
    public class RepSucursal
    {
        String error = "";
        public RepSucursal(String rutadoc, DataSet dsDatods)
        {
												try
												{
																dsDatods.Tables["Table"].TableName = "RepGeneralSC";
																RepGeneralSC rpt = new RepGeneralSC();
																rpt.SetDataSource(dsDatods);
																rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, rutadoc + ".xls");
																rpt.Close();
												}
												catch (Exception ex)
												{

																String archivo = System.AppDomain.CurrentDomain.BaseDirectory + @"reportes\docs\" + rutadoc + ".txt";
																using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(rutadoc + ".txt"))
																{
																				escritor.WriteLine(ex.ToString());
																}
												}
        }
								public RepSucursal(String rutadoc, DataSet dsDatods, string codDoc)
								{

												try
												{
																if (codDoc.Equals("07"))
																{
																				dsDatods.Tables["Table"].TableName = "RepGeneralSC";
																				RepGeneralSC_Ret rpt = new RepGeneralSC_Ret();
																				rpt.SetDataSource(dsDatods);
																				rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, rutadoc + ".xls");
																				rpt.Close();
																}
																else
																{
																				dsDatods.Tables["Table"].TableName = "RepGeneralSC";
																				RepGeneralSC rpt = new RepGeneralSC();
																				rpt.SetDataSource(dsDatods);
																				rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, rutadoc + ".xls");
																				rpt.Close();
																}
												}
												catch (Exception ex)
												{

																String archivo = System.AppDomain.CurrentDomain.BaseDirectory + @"reportes\docs\" + rutadoc + ".txt";
																using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(rutadoc + ".txt"))
																{
																				escritor.WriteLine(ex.ToString());
																}
												}
								}
        public String getError() {
            return error;  
        }
    }

}