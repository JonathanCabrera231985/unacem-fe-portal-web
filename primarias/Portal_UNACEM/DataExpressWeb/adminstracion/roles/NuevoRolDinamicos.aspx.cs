using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Datos;
using clibLogger;

namespace DataExpressWeb.adminstracion.roles
{
    public partial class NuevoRolDinamicos : System.Web.UI.Page
    {
        DataTable dtMenu_Rol = new DataTable();
        DataTable dtcheckRoles;
        String idRol = "";
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
                lbmensaje.Text = "";
                if (Request.QueryString["idRol"] != null)
                {
                    lbtextocabecera.Text = "Modificar Rol";
                    txtdescripcionRol.Enabled = true;
                    idRol = Request.QueryString["idRol"].ToString();
                    DB.Conectar();
                    DataTable dtdescripcion = DB.TraerDataSetConsulta("select descripcion from Roles WITH (NOLOCK)  where idRol = @p1", idRol).Tables[0];
                    DB.Desconectar();
                    if (dtdescripcion.Rows.Count > 0)
                        txtdescripcionRol.Text = dtdescripcion.Rows[0]["descripcion"].ToString();
                }
                {
                    lbtextocabecera.Text = "Crear Rol";
                }
                if (!Page.IsPostBack)
                    PopulateNode(LinksTreeView, idRol);
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
                DB.Conectar();
                dtMenu_Rol = DB.TraerDataSetConsulta("select id_menu_option from menu_Roles WITH (NOLOCK)  where id_idRol = @p1", dato).Tables[0];
                DB.Desconectar();
                node1.Nodes.Clear();
                DataSet dsDataSet = new DataSet();
                DB.Conectar();
                dsDataSet = DB.TraerDataSetConsulta("select A.id_menu_option, A.description, A.id_parent_menu_option, A.url from Menu_Options A WITH (NOLOCK)  where A.Estado = 0", new Object[] { });

                if (dsDataSet.Tables[0] != null && dsDataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow drDataRow in dsDataSet.Tables[0].Rows)
                    {
                        if (Convert.ToInt32(drDataRow[0]) == Convert.ToInt32(drDataRow[2]))
                        {
                            TreeNode miMenuItem = new TreeNode(Convert.ToString(drDataRow["description"]), Convert.ToString(drDataRow["id_menu_option"]), "", "", Convert.ToString(drDataRow["id_parent_menu_option"]));
                            miMenuItem.ShowCheckBox = true;

                            if (dtMenu_Rol.Rows.Count > 0)
                                foreach (DataRow dr in dtMenu_Rol.Rows)
                                    if (dr["id_menu_option"].ToString().Equals(drDataRow["id_menu_option"].ToString()))
                                        miMenuItem.Checked = true;

                            node1.Nodes.Add(miMenuItem);
                            AddChildItem(ref miMenuItem, dsDataSet.Tables[0]);
                        }
                    }
                }
                DB.Desconectar();
            }
            catch (Exception ex)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ex.Message);
                lbmensaje.Text = "Error:" + ex.Message;
                throw;
            }
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
                    TreeNode miMenuItemChild = new TreeNode(Convert.ToString(drDataRow["description"]), Convert.ToString(drDataRow["id_menu_option"]), "", "", Convert.ToString(drDataRow["id_parent_menu_option"]));
                    miMenuItemChild.ShowCheckBox = true;

                    if (dtMenu_Rol.Rows.Count > 0)
                        foreach (DataRow dr in dtMenu_Rol.Rows)
                            if (dr["id_menu_option"].ToString().Equals(drDataRow["id_menu_option"].ToString()))
                                miMenuItemChild.Checked = true;

                    miMenuItem.ChildNodes.Add(miMenuItemChild);
                    AddChildItem(ref miMenuItemChild, dtDataTable);
                }
            }
        }

        protected void btoGuardar_Click(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                StringBuilder documentoXML = new StringBuilder("");
                documentoXML.Append("<INSTRUCCION>");
                documentoXML.Append("<Filtro>");
                documentoXML.Append("<Opcion>" + (idRol.Equals("") ? "1" : "2") + "</Opcion>");
                documentoXML.Append("</Filtro>");
                documentoXML.Append("<Roles>");
                documentoXML.Append("<descripcion>" + txtdescripcionRol.Text + "</descripcion>");
                documentoXML.Append("<eliminado>0</eliminado>");
                documentoXML.Append("<id_Role>" + idRol + "</id_Role>");
                documentoXML.Append("</Roles>");
                #region "ingresando datos del rol"
                dtcheckRoles = new DataTable("Menu");
                dtcheckRoles.Columns.Add("codMenu", typeof(String));
                for (int i = 0; i < LinksTreeView.Nodes.Count; i++)
                    DisplayChildNodeText(LinksTreeView.Nodes[i]);
                foreach (DataRow dr in dtcheckRoles.Rows)
                {
                    documentoXML.Append("<menuRol>");
                    documentoXML.Append("<id_menu_option>" + dr["codMenu"].ToString() + "</id_menu_option>");
                    documentoXML.Append("<id_Role>" + idRol + "</id_Role>");
                    documentoXML.Append("</menuRol>");
                }
                #endregion
                documentoXML.Append("</INSTRUCCION>");
                DB.Conectar();
                DB.TraerDataset("PA_RolMenu_AMC", documentoXML.ToString());
                DB.Desconectar();
                Response.Redirect("~/adminstracion/roles/RolDinamico.aspx");
            }
            catch (Exception ex)
            {
                DB.Desconectar();
                clsLogger.Graba_Log_Error(ex.Message);
                lbmensaje.Text = "Error: " + ex.Message;
                throw;
            }
            finally
            {
                DB.Desconectar();
            }
        }
        protected void DisplayChildNodeText(TreeNode node)
        {
            if (!node.Value.Equals("Menú"))
                if (node.Checked)
                {
                    DataRow dr = dtcheckRoles.NewRow();
                    dr["codMenu"] = node.Value;
                    dtcheckRoles.Rows.Add(dr);
                }
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                DisplayChildNodeText(node.ChildNodes[i]);
            }
        }

        protected void seleccionaCampos(TreeNodeCollection p_miMenuItem, Boolean p_check, String p_nodoSeleccionado)
        {
            foreach (TreeNode hijos in p_miMenuItem)
            {
                hijos.Checked = p_check;
                if (hijos.Target.Equals(p_nodoSeleccionado))
                    seleccionaCampos(hijos.ChildNodes, hijos.Checked, hijos.Value);
            }
        }

        protected void LinksTreeView_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            seleccionaCampos(e.Node.ChildNodes, e.Node.Checked, e.Node.Value);
        }
    }
}