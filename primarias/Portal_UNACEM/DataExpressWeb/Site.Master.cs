using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Control;
using System.Data;
using Datos;
using System.Text;
using clibLogger;

namespace DataExpressWeb
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        MenuItem menuPrincipal, menuConsulta,
                 menuUsuario, menuRoles, menuConfig,
                 menuSucursal, menuManual, menuNuevoDocumento, menuRecepcion;
        
        protected void AddChildItem(ref MenuItem miMenuItem, DataTable dtDataTable)
        {
            foreach (DataRow drDataRow in dtDataTable.Rows)
            {
                if (Convert.ToInt32(drDataRow[2]) == Convert.ToInt32(miMenuItem.Value) && Convert.ToInt32(drDataRow[0]) != Convert.ToInt32(drDataRow[2]))
                {
                    MenuItem miMenuItemChild = new MenuItem(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[3]));
                    miMenuItem.ChildItems.Add(miMenuItemChild);
                    AddChildItem(ref miMenuItemChild, dtDataTable);
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            String pathRequLogin, pathRespLogin;
            pathRequLogin = System.Configuration.ConfigurationManager.AppSettings["pathRequLogin"];
            pathRespLogin = System.Configuration.ConfigurationManager.AppSettings["pathRespLogin"];
            var DB = new BasesDatos();
            try
            {
                if (Session["rfcUser"] != null)
                {
                    // menuManual.Enabled = false;
                    lRfc.Text = Session["rfcUser"].ToString() + "<br>" +
                                Session["nombreEmpleado"].ToString() + "<br>" +
                                Session["nombreSucursalUser"].ToString() + "<br>";
                    DataSet dsDataSet = new DataSet();
                    DB.Conectar();
                    StringBuilder documentoXML = new StringBuilder("");
                    documentoXML.Append("<INSTRUCCION>");
                    documentoXML.Append("<Filtro>");
                    documentoXML.Append("<Opcion>3</Opcion>");
                    documentoXML.Append("<id_Role>" + Session["rolUser"].ToString() + "</id_Role>");
                    documentoXML.Append("</Filtro>");
                    documentoXML.Append("</INSTRUCCION>");
                    dsDataSet = DB.TraerDataset("PA_RolMenu_AMC", documentoXML.ToString());
                    if (dsDataSet.Tables[0] != null && dsDataSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drDataRow in dsDataSet.Tables[0].Rows)
                        {
                            if (Convert.ToInt32(drDataRow[0]) == Convert.ToInt32(drDataRow[2]))
                            {
                                MenuItem miMenuItem = new MenuItem(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), String.Empty, Convert.ToString(drDataRow[3]));
                                this.nmMenu.Items.Add(miMenuItem);
                                AddChildItem(ref miMenuItem, dsDataSet.Tables[0]);
                            }
                        }
                    } 
                }
                else
                {
                    if (Request.Path != pathRequLogin)
                    {
                        Response.Redirect(pathRespLogin);
                    }
                }
            }
            catch (Exception ae)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ae.Message);
            }
            finally
            {
                DB.Desconectar();
            }
        }

        protected void NavigationMenu_MenuItemClick(object sender, MenuEventArgs e)
        {

        }

    }
}
