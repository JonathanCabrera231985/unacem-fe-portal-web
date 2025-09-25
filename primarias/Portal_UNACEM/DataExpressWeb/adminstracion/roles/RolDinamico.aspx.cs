using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using clibLogger;

namespace DataExpressWeb.adminstracion.roles
{
    public partial class RolDinamico : System.Web.UI.Page
    {
        string user = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                user = Session["idUser"].ToString();
                if (!string.IsNullOrEmpty(user))
                {
                    ValidarPermisos vP = new ValidarPermisos();
                    vP.DB = DB;
                    bool Permiso = vP.PermisoAccederFormulario(user);
                    if (!Permiso)
                    {
                        ContentPlaceHolder mpContentPlaceHolder = (ContentPlaceHolder)Master.FindControl("MainContent");
                        mpContentPlaceHolder.Visible = false;
                        string script = vP.AgregarAlertaRedireccionar();
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "confirm", script.ToString(), true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "alertaNoPermisos()", true);
                    }
                }
            }
            catch (Exception ex)
            {
                clsLogger.Graba_Log_Error(ex.Message);
                lbmensaje.Text = "Error:" + ex.Message;
                DB.Desconectar();
                throw;
            }
            finally
            {
                DB.Desconectar();
            }
        }

        protected void PopulateNode(TreeView node1, String dato)
        {
            var DB = new BasesDatos();
            try
            {
                node1.Nodes.Clear();
                DataSet dsDataSet = new DataSet();
                DB.Conectar();
                dsDataSet = DB.TraerDataSetConsulta(@"select A.id_menu_option, A.description, A.id_parent_menu_option, A.url from Menu_Options A WITH (NOLOCK) 
                        inner join Menu_Roles B WITH (NOLOCK) on	A.id_menu_option = B.id_menu_option
                        inner join Roles C WITH (NOLOCK)  on B.id_idRol = C.idRol where Estado = 0 and C.idRol = @p1 and C.eliminado = 0", new Object[] { dato });

                if (dsDataSet.Tables[0] != null && dsDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drDataRow in dsDataSet.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(drDataRow[0]) == Convert.ToInt32(drDataRow[2]))
                        {
                            TreeNode miMenuItem = new TreeNode(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), "", "", Convert.ToString(drDataRow[2]));
                            node1.Nodes.Add(miMenuItem);
                            AddChildItem(ref miMenuItem, dsDataSet.Tables[0]);
                        }
                    }
                }
                DB.Desconectar();
            }
            catch (Exception ex) { DB.Desconectar(); clsLogger.Graba_Log_Error(ex.Message); throw; }
            finally
            {
                DB.Desconectar();
            }
        }

        protected void AddChildItem(ref TreeNode miMenuItem, DataTable dtDataTable)
        {
            foreach (DataRow drDataRow in dtDataTable.Rows)
            {
                if (Convert.ToInt32(drDataRow[2]) == Convert.ToInt32(miMenuItem.Value) && Convert.ToInt32(drDataRow[0]) != Convert.ToInt32(drDataRow[2]))
                {
                    TreeNode miMenuItemChild = new TreeNode(Convert.ToString(drDataRow[1]), Convert.ToString(drDataRow[0]), "", "", Convert.ToString(drDataRow[2]));

                    miMenuItem.ChildNodes.Add(miMenuItemChild);
                    AddChildItem(ref miMenuItemChild, dtDataTable);
                }
            }
        }

        protected void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateNode(LinksTreeView, ListBox1.SelectedValue);
            }
            catch (Exception ex)
            {
                lbmensaje.Text = "Error: " + ex.Message;
            }
        }

        protected void btoEliminar_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                DB.Conectar();
                DB.TraerDataSetConsulta(@"UPDATE Roles SET eliminado='true' WHERE (idRol = @p1) UPDATE Empleados SET eliminado='true' WHERE (id_Rol=@p2)", new Object[] { ListBox1.SelectedValue, Session["idUser"].ToString() });
                DB.Desconectar();
                Response.Redirect("RolDinamico.aspx", false);
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

        protected void btoEditar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/adminstracion/roles/NuevoRolDinamicos.aspx?idRol=" + ListBox1.SelectedValue);
        }
    }
}