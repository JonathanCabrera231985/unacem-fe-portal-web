using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Datos;
using clibLogger;
using DataExpressWeb;

namespace Administracion
{
    public partial class sucursales : System.Web.UI.Page
    {
        private String separador = "|";
        String rucEmpresa = "";
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
                if (Session["rucEmpresa"] != null)
                    rucEmpresa = Session["rucEmpresa"].ToString();
            }
            catch (Exception ex)
            {
                clsLogger.Graba_Log_Error(ex.Message);
                DB.Desconectar();
                throw;
            }
            finally
            {
                DB.Desconectar();
            }
        }

        protected void bActualizar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/adminstracion/sucursales/sucursales.aspx", false);
        }

        protected void bBuscarReg_Click(object sender, EventArgs e)
        {
            buscar();
        }

        private void buscar()
        {
            int fecha = 0;
            string msjbuscar = "";
            string consulta = "";

            if (tbClave.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "CL" + tbClave.Text + separador; }
                else { consulta = "CL" + tbClave.Text + separador; }
            }
            if (tbSucursal.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "SU" + tbSucursal.Text + separador; }
                else { consulta = "SU" + tbSucursal.Text + separador; }
            }
            if (tbDomicilio.Text.Length != 0)
            {
                if (consulta.Length != 0) { consulta = consulta + "DO" + tbDomicilio.Text + separador; }
                else { consulta = "DO" + tbDomicilio.Text + separador; }
            }
            if (consulta.Length > 0)
            {

            }
            else
            {
                consulta = "-";
            }
            if (Convert.ToBoolean(Session["asRoles"]))
            {
                SqlDataSource1.SelectParameters["QUERY"].DefaultValue = consulta;
                SqlDataSource1.SelectParameters["idEmpresa"].DefaultValue = rucEmpresa;
            }
            else
            {
                SqlDataSource1.SelectParameters["QUERY"].DefaultValue = "-";
                SqlDataSource1.SelectParameters["idEmpresa"].DefaultValue = rucEmpresa;
            }
            SqlDataSource1.DataBind();
            gvSucursales.DataBind();
            consulta = "";
            lMensaje.Text = msjbuscar;
        }

        protected void grid_cajaSucursal_DataBinding(object sender, EventArgs e)
        {
            if (gvSucursales.SelectedIndex.Equals(-1))
            {
                gv_cajaSucursal.EmptyDataText = "";
            }
            else
            {
                gv_cajaSucursal.EmptyDataText = "No hay cajas registradas en esta sucursal";
            }

            if (!gvSucursales.SelectedIndex.Equals(-1))
            {
                this.Panel_cajaSucursal.Visible = true;
            }
            else
            {
                this.Panel_cajaSucursal.Visible = false;
            }
        }

        protected void grid_sucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
            gv_cajaSucursal.SelectedIndex = -1;
            try
            {
                gv_cajaSucursal.DataBind();
                Show(false);
            }
            catch (Exception ex)
            {
            }
        }


        protected void grid_sucursales_PageIndexChanged(object sender, EventArgs e)
        {
            gvSucursales.SelectedIndex = -1;
        }

        protected void btnCajas_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("idCaja");
            dt.Columns.Add("idSucursal");
            dt.Columns.Add("NumeroSisposnet");
            dt.Columns.Add("NumeroRentas");
            dt.Columns.Add("estab");
            dt.Columns.Add("ptoEmi");
            dt.Columns.Add("secuencial");
            dt.Columns.Add("estado");
            dt.Columns.Add("estadoFE");
            dt.Columns.Add("estadoPro");
            BindEmptyRow(dt, gv_cajaSucursal);
        }

        protected void lbnuevo_Click(object sender, EventArgs e)
        {
            Show(true);
        }

        protected void lbcancel_Click(object sender, EventArgs e)
        {
            Show(false);
        }

        protected void lbinsert_Click(object sender, EventArgs e)
        {
            string Column1 = ((TextBox)gv_cajaSucursal.FooterRow.FindControl("txt_i_numsispo")).Text;
            string Column2 = ((DropDownList)gv_cajaSucursal.FooterRow.FindControl("ddl_i_nrentas")).SelectedValue.ToString();
            string Column3 = ((TextBox)gv_cajaSucursal.FooterRow.FindControl("txt_i_estab")).Text;
            string Column4 = ((TextBox)gv_cajaSucursal.FooterRow.FindControl("txt_i_ptoemi")).Text;
            string Column5 = ((TextBox)gv_cajaSucursal.FooterRow.FindControl("txt_i_secuencial")).Text;
            string Column6 = ((DropDownList)gv_cajaSucursal.FooterRow.FindControl("ddl_i_estado")).SelectedValue.ToString();
            string Column7 = ((DropDownList)gv_cajaSucursal.FooterRow.FindControl("ddl_i_estadofe")).SelectedValue.ToString();
            string Column8 = ((DropDownList)gv_cajaSucursal.FooterRow.FindControl("ddl_i_estadopro")).SelectedValue.ToString();
            ds_CajaSucursal.InsertParameters.Add(new Parameter("NumeroSisposnet", TypeCode.String, Column1));
            ds_CajaSucursal.InsertParameters.Add(new Parameter("NumeroRentas", TypeCode.String, Column2));
            ds_CajaSucursal.InsertParameters.Add(new Parameter("estab", TypeCode.String, Column3));
            ds_CajaSucursal.InsertParameters.Add(new Parameter("ptoEmi", TypeCode.String, Column4));
            ds_CajaSucursal.InsertParameters.Add(new Parameter("secuencial", TypeCode.String, Column5));
            ds_CajaSucursal.InsertParameters.Add(new Parameter("estado", TypeCode.String, Column6));
            ds_CajaSucursal.InsertParameters.Add(new Parameter("estadoFE", TypeCode.String, Column7));
            ds_CajaSucursal.InsertParameters.Add(new Parameter("estadoPro", TypeCode.String, Column8));
            ds_CajaSucursal.Insert();
            gv_cajaSucursal.DataSourceID = "ds_CajaSucursal";
            gv_cajaSucursal.DataBind();
            Show(false);
        }

        protected void Show(bool visible)
        {
            LinkButton Insert = (LinkButton)gv_cajaSucursal.FooterRow.FindControl("lbinsert");
            LinkButton Cancel = (LinkButton)gv_cajaSucursal.FooterRow.FindControl("lbcancel");
            LinkButton New = (LinkButton)gv_cajaSucursal.FooterRow.FindControl("lbnuevo");
            TextBox Column1 = (TextBox)gv_cajaSucursal.FooterRow.FindControl("txt_i_numsispo");
            DropDownList Column2 = (DropDownList)gv_cajaSucursal.FooterRow.FindControl("ddl_i_nrentas");
            TextBox Column3 = (TextBox)gv_cajaSucursal.FooterRow.FindControl("txt_i_estab");
            TextBox Column4 = (TextBox)gv_cajaSucursal.FooterRow.FindControl("txt_i_ptoemi");
            TextBox Column5 = (TextBox)gv_cajaSucursal.FooterRow.FindControl("txt_i_secuencial");
            DropDownList Column6 = (DropDownList)gv_cajaSucursal.FooterRow.FindControl("ddl_i_estado");
            DropDownList Column7 = (DropDownList)gv_cajaSucursal.FooterRow.FindControl("ddl_i_estadofe");
            DropDownList Column8 = (DropDownList)gv_cajaSucursal.FooterRow.FindControl("ddl_i_estadopro");
            New.Visible = !visible;
            Insert.Visible = visible;
            Cancel.Visible = visible;
            Column1.Text = string.Empty;
            Column3.Text = string.Empty;
            Column4.Text = string.Empty;
            Column5.Text = string.Empty;
            Column1.Visible = visible;
            Column2.Visible = visible;
            Column3.Visible = visible;
            Column4.Visible = visible;
            Column5.Visible = visible;
            Column6.Visible = visible;
            Column7.Visible = visible;
            Column8.Visible = visible;
        }

        private void ShowNoResultFound(DataTable source, GridView gv)
        {
            source.Rows.Add(source.NewRow());
            gv.DataSourceID = null;
            gv.DataSource = source;
            gv.DataBind();
            int columnsCount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = columnsCount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RESULT FOUND!";
        }

        private void BindEmptyRow(DataTable source, GridView gv)
        {
            gv.Dispose();
            gv.DataSourceID = null;
            DataTable t = new DataTable();
            t = source.Clone();
            foreach (DataColumn c in t.Columns)
                c.AllowDBNull = true;
            t.Rows.Add(t.NewRow());
            gv.DataSource = t;
            gv.DataBind();
            gv.Rows[0].Visible = false;
            gv.Rows[0].Controls.Clear();
        }
    }
}