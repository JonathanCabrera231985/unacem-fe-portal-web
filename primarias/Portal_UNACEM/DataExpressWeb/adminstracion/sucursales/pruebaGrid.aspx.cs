using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using clibLogger;

namespace DataExpressWeb.adminstracion.sucursales
{
    public partial class pruebaGrid : System.Web.UI.Page
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
                DB.Desconectar();
                throw;
            }
            finally
            {
                DB.Desconectar();
            }
        }

        protected void gvSuppliers_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                GridViewRow gvr = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);

                TableCell cell = new TableCell();
                cell.Text = "Action";
                gvr.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "Company Name";
                gvr.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "Phone";
                gvr.Cells.Add(cell);

                gvSuppliers.Controls[0].Controls.AddAt(0, gvr);
            }
        }

        protected void gvSuppliers_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
            if ((e.Exception == null) && e.AffectedRows.Equals(1))
            {
                lblResults.Text = "Supplier successfully updated.";
            }
            else
            {
                lblResults.Text = "Unable to successfully update supplier.";
                e.ExceptionHandled = true;
            }
        }

        protected void gvSuppliers_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
            if ((e.Exception == null) && e.AffectedRows.Equals(1))
            {
                lblResults.Text = "Supplier successfully deleted.";
            }
            else
            {
                lblResults.Text = "Unable to successfully delete supplier.";
                e.ExceptionHandled = true;
            }
        }

        protected void gvSuppliers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("EmptyDataTemplateInsert"))
            {
                GridViewRow gvr = gvSuppliers.Controls[0].Controls[1] as GridViewRow;

                if (gvr == null) { return; }
                TextBox txtCompanyName = gvr.FindControl("txtCompanyName") as TextBox;
                TextBox txtPhone = gvr.FindControl("txtPhone") as TextBox;
                if (txtCompanyName == null) { return; }
                if (txtPhone == null) { return; }
                sdsSuppliers.InsertParameters["CompanyName"].DefaultValue = txtCompanyName.Text;
                sdsSuppliers.InsertParameters["Phone"].DefaultValue = txtPhone.Text;
                sdsSuppliers.Insert();
            }
            else if (e.CommandName.Equals("FooterInsert"))
            {
                TextBox txtCompanyName = gvSuppliers.FooterRow.FindControl("txtCompanyName") as TextBox;
                TextBox txtPhone = gvSuppliers.FooterRow.FindControl("txtPhone") as TextBox;
                if (txtCompanyName == null) { return; }
                if (txtPhone == null) { return; }
                sdsSuppliers.InsertParameters["CompanyName"].DefaultValue = txtCompanyName.Text;
                sdsSuppliers.InsertParameters["Phone"].DefaultValue = txtPhone.Text;
                sdsSuppliers.Insert();
            }
        }

        protected void sdsSuppliers_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            if ((e.Exception == null) && e.AffectedRows.Equals(1))
            {
                lblResults.Text = "Supplier successfully added.";
            }
            else
            {
                lblResults.Text = "Unable to successfully add supplier.";
                e.ExceptionHandled = true;
            }
        }
    }
}