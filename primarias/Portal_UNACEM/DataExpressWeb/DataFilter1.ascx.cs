using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace DataExpressWeb
{

    public partial class DataFilter1 : System.Web.UI.UserControl
    {
        public delegate void RefreshDataGridView();

        public event RefreshDataGridView OnFilterAdded;

        DataTable filteredTable;
        string filterOperator;

        string filterSessionID;

        public string FilterSessionID
        {
            get { return filterSessionID; }
            set { filterSessionID = value; }
        }

        int filtersCounter;

        Dictionary<string, Filter> filterPanelsDict;


        List<Panel> filterPanelsList;
        Object dataSource;
        DataControlFieldCollection dataColumns;

        public Object DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        public DataControlFieldCollection DataColumns
        {
            get { return dataColumns; }
            set { dataColumns = value; }
        }


        private List<Filter> filters;

        private List<Filter> Filters
        {
            get { return filters; }
            set { filters = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {


                Session.Remove(FilterSessionID + "filtersCounter");
                Session.Remove(FilterSessionID + "filterPanelsList");

                filtersCounter = 0;
                Session.Add(FilterSessionID + "filtersCounter", filtersCounter);

                filterPanelsDict = new Dictionary<string, Filter>();
                Session.Add(FilterSessionID + "filterPanelsDict", filterPanelsDict);

                filterPanelsList = new List<Panel>();
                Session.Add(FilterSessionID + "filterPanelsList", filterPanelsList);




            }
            else
            {
                filtersCounter = Int32.Parse(Session[FilterSessionID + "filtersCounter"].ToString());

                filterPanelsDict = (Dictionary<string, Filter>)Session[FilterSessionID + "filterPanelsDict"];

                filterPanelsList = (List<Panel>)Session[FilterSessionID + "filterPanelsList"];
            }


            LoadPanels(filterPanelsList);

        }

        public DataTable FilterDataTable()
        {

            string filterString = BuildFilter();
            DataRow[] filteredRows = ((DataTable)DataSource).Select(filterString);
            filteredTable = ((DataTable)DataSource).Clone();
            foreach (DataRow dr in filteredRows)
            {
                filteredTable.ImportRow(dr);
            }
            return filteredTable;
        }
        public void FilterDataSource()
        {
            string filterString = BuildFilter();
            ((SqlDataSource)DataSource).FilterExpression = filterString;
            //((SqlDataSource)DataSource).FilterExpression = "StartDate = #05/23/2009#";
        }

        private string BuildFilter()
        {
            StringBuilder filtersString = new StringBuilder();

            int filterPanelsDictCounter = 1;
            foreach (Filter filter in filterPanelsDict.Values)
            {
                if (filterPanelsDictCounter < filterPanelsDict.Count)
                {
                    //filtersString.Append(filter.ToString() + " AND ");
                    filtersString.Append(filter.ToString());
                    filterPanelsDictCounter++;
                }
                else if (filterPanelsDictCounter == filterPanelsDict.Count)
                {
                    filtersString.Append(filter.ToString());
                }

            }
            return filtersString.ToString();
        }

        private void AddNewFilter()
        {
            //Creating The Panel
            Panel newFilter = new Panel();
            filtersCounter = Int32.Parse(Session[FilterSessionID + "filtersCounter"].ToString());
            filtersCounter++;
            filterPanelsList = (List<Panel>)Session[FilterSessionID + "filterPanelsList"];
            newFilter.ID = "filterPanel-" + filtersCounter.ToString();
            //newFilter.Direction = ContentDirection.LeftToRight;
            //newFilter.Width = Unit.Percentage(100);


            //Creating Submit Button
            Button submitFilterButton = new Button();
            submitFilterButton.Text = "Añadir Filtro";
            submitFilterButton.ID = "btnSubmit-" + filtersCounter.ToString();
            submitFilterButton.CssClass = "ilbuttons";
            submitFilterButton.Click += new EventHandler(submitFilterButtonHandler_Click);

            //Creating Cancel Button
            Button cancelFilterButton = new Button();
            cancelFilterButton.Text = "Eliminar";
            cancelFilterButton.CssClass = "ilbuttons";
            cancelFilterButton.ID = "btnCancel-" + filtersCounter.ToString();
            cancelFilterButton.Click += new EventHandler(cancelFilterButtonHandler_Click);

            //Creating Culomn Name Drop Down List
            DropDownList ddlColumnName = new DropDownList();
            ddlColumnName.ID = "ddlColumnName-" + filtersCounter.ToString();
            ddlColumnName.Items.AddRange(BuildDataColumns());
            ddlColumnName.Style.Add(HtmlTextWriterStyle.Direction, "ltr");
            //ddlColumnName.CssClass = "checkBoxs";


            //Creating Culomn Operation Drop Down List
            DropDownList ddlColumnOperation = new DropDownList();
            ddlColumnOperation.ID = "ddlColumnOperation-" + filtersCounter.ToString();
            ddlColumnOperation.Items.Add("=");
            ddlColumnOperation.Items.Add("LIKE");
            ddlColumnOperation.Items.Add("<>");
            ddlColumnOperation.Items.Add(">");
            ddlColumnOperation.Items.Add("<");
            ddlColumnOperation.Style.Add(HtmlTextWriterStyle.Direction, "rtl");
            //ddlColumnOperation.CssClass = "checkBoxs";

            //Create Value Textbox
            TextBox txtColumnValue = new TextBox();
            txtColumnValue.ID = "txtColumnValue-" + filtersCounter.ToString();
            txtColumnValue.CssClass = "iltxtFields";
            newFilter.Controls.Add(ddlColumnName);
            newFilter.Controls.Add(ddlColumnOperation);
            newFilter.Controls.Add(txtColumnValue);


            newFilter.Controls.Add(submitFilterButton);
            newFilter.Controls.Add(cancelFilterButton);

            //filterPanelsDict.Add(newFilter.ID, new Filter());
            //Session.Add("filterPanelsDict", filterPanelsDict);


            Session.Add(FilterSessionID + "filtersCounter", filtersCounter);

            filterPanelsList.Add(newFilter);
            Session.Add(FilterSessionID + "filterPanelsList", filterPanelsList);
            LoadPanels(filterPanelsList);

        }

        private ListItem[] BuildDataColumns()
        {

            ListItem[] li = new ListItem[CountBoundDataColumns()];

            int i = 0;
            int j = 0;
            while (i < DataColumns.Count)
            {
                if (DataColumns[i].GetType() == typeof(BoundField))
                {
                    li[j] = new ListItem(((BoundField)DataColumns[i]).HeaderText, ((BoundField)DataColumns[i]).DataField);
                    j++;
                }
                i++;
            }

            return li;
        }

        private int CountBoundDataColumns()
        {
            try
            {
                int counter = 0;
                for (int i = 0; i < DataColumns.Count; i++)
                {
                    if (DataColumns[i].GetType() == typeof(BoundField))
                    {
                        counter++;
                    }
                }
                return counter;
            }
            catch
            {
                throw new Exception("Can not add filters");
            }
        }

        private void LoadPanels(List<Panel> flPanel)
        {


            foreach (Panel x in flPanel)
            {
                foreach (System.Web.UI.Control c in x.Controls)
                {
                    if (c.GetType() == typeof(Button))
                    {
                        Button b = (Button)c;
                        if (b != null)
                        {
                            if (b.ID.StartsWith("btnS"))
                            {
                                b.Click += new EventHandler(this.submitFilterButtonHandler_Click);
                            }
                            else if (b.ID.StartsWith("btnC"))
                            {
                                b.Click += new EventHandler(this.cancelFilterButtonHandler_Click);
                            }

                        }
                    }
                }
                pnlNewFilter.Controls.Add(x);
            }

        }

        public void AddNewFilter(string filterCulomnName, string filterColumnOperation, string filterValue)
        {
            //Creating The Panel
            Panel newFilter = new Panel();
            filtersCounter = Int32.Parse(Session[FilterSessionID + "filtersCounter"].ToString());
            filtersCounter++;
            filterPanelsList = (List<Panel>)Session[FilterSessionID + "filterPanelsList"];
            newFilter.ID = "filterPanel-" + filtersCounter.ToString();
            //newFilter.Direction = ContentDirection.LeftToRight;
            //newFilter.Width = Unit.Percentage(100);


            //Creating Submit Button
            Button submitFilterButton = new Button();
            submitFilterButton.Text = "Añadir Filtro";
            submitFilterButton.ID = "btnSubmit-" + filtersCounter.ToString();
            submitFilterButton.CssClass = "ilbuttons";
            submitFilterButton.Click += new EventHandler(submitFilterButtonHandler_Click);

            //Creating Cancel Button
            Button cancelFilterButton = new Button();
            cancelFilterButton.Text = "Eliminar";
            cancelFilterButton.CssClass = "ilbuttons";
            cancelFilterButton.ID = "btnCancel-" + filtersCounter.ToString();
            cancelFilterButton.Click += new EventHandler(cancelFilterButtonHandler_Click);

            //Creating Culomn Name Drop Down List
            DropDownList ddlColumnName = new DropDownList();
            ddlColumnName.ID = "ddlColumnName-" + filtersCounter.ToString();
            ddlColumnName.Items.AddRange(BuildDataColumns());
            ddlColumnName.Style.Add(HtmlTextWriterStyle.Direction, "ltr");
            ddlColumnName.SelectedValue = filterCulomnName;
            //ddlColumnName.CssClass = "checkBoxs";


            //Creating Culomn Operation Drop Down List
            DropDownList ddlColumnOperation = new DropDownList();
            ddlColumnOperation.ID = "ddlColumnOperation-" + filtersCounter.ToString();
            ddlColumnOperation.Items.Add("=");
            ddlColumnOperation.Items.Add("LIKE");
            ddlColumnOperation.Items.Add("<>");
            ddlColumnOperation.Items.Add(">");
            ddlColumnOperation.Items.Add("<");
            ddlColumnOperation.Style.Add(HtmlTextWriterStyle.Direction, "rtl");
            ddlColumnOperation.SelectedValue = filterColumnOperation;
            //ddlColumnOperation.CssClass = "checkBoxs";

            //Create Value Textbox
            TextBox txtColumnValue = new TextBox();
            txtColumnValue.ID = "txtColumnValue-" + filtersCounter.ToString();
            txtColumnValue.CssClass = "iltxtFields";
            txtColumnValue.Text = filterValue;
            newFilter.Controls.Add(ddlColumnName);
            newFilter.Controls.Add(ddlColumnOperation);
            newFilter.Controls.Add(txtColumnValue);


            newFilter.Controls.Add(submitFilterButton);
            newFilter.Controls.Add(cancelFilterButton);

            //filterPanelsDict.Add(newFilter.ID, new Filter());
            //Session.Add("filterPanelsDict", filterPanelsDict);


            Session.Add(FilterSessionID + "filtersCounter", filtersCounter);

            filterPanelsList.Add(newFilter);
            Session.Add(FilterSessionID + "filterPanelsList", filterPanelsList);
            LoadPanels(filterPanelsList);

            submitFilter(submitFilterButton);

        }

        protected void submitFilterButtonHandler_Click(object sender, EventArgs e)
        {


            int panelID = Int32.Parse(((Button)sender).ID.Split('-')[1]);
            string columnName = "";
            string operation = "";
            string columnValue = "";

            foreach (Panel p in ((List<Panel>)(Session[FilterSessionID + "filterPanelsList"])))
            {
                if (p.ID == "filterPanel-" + panelID.ToString())
                {
                    foreach (System.Web.UI.Control c in p.Controls)
                    {
                        if (c.GetType() == typeof(DropDownList))
                        {
                            if (c.ID.StartsWith("ddlColumnName"))
                            {
                                //columnName = Session[FilterSessionID+"filterOperator"].ToString() + ((DropDownList)c).SelectedValue;
                                columnName = ((DropDownList)c).SelectedValue;
                            }
                            else
                            {
                                operation = ((DropDownList)c).Text;
                            }
                        }
                        if (c.GetType() == typeof(TextBox))
                        {
                            string[] columnValues = ((TextBox)c).Text.Split(';');
                            if (columnValues.Length > 1)
                            {
                                for (int ii = 0; ii < columnValues.Length; ii++)
                                {
                                    Regex r = new Regex("^([0]?[1-9]|[1|2][0-9]|[3][0|1])[/]([0]?[1-9]|[1][0-2])[/]([0-9]{4}|[0-9]{2})$");

                                    if (r.IsMatch(columnValues[ii].Trim()))
                                    {
                                        DateTime dt = new DateTime();
                                        string day = columnValues[ii].Trim().Split(new char[] { '/' })[0].ToString();
                                        string month = columnValues[ii].Trim().Split(new char[] { '/' })[1].ToString();
                                        string year = columnValues[ii].Trim().Split(new char[] { '/' })[2].ToString();

                                        string ds = month + "/" + day + "/" + year;


                                        dt = DateTime.Parse(ds);
                                        if (ii == 0)
                                        {
                                            columnValue += "#" + dt.ToString() + "# OR " + columnName + " " + operation;
                                        }
                                        else if (ii == columnValues.Length - 1)
                                        {
                                            columnValue += " #" + dt.ToString() + "# )";
                                        }
                                        else
                                        {
                                            columnValue += " #" + dt.ToString() + "# OR " + columnName + " " + operation;
                                        }
                                    }
                                    else
                                    {
                                        if (ii == 0)
                                        {
                                            if (operation != "LIKE")

                                                columnValue += "'" + columnValues[ii].Trim() + "' OR " + columnName + " " + operation;
                                            else
                                                columnValue += "'%" + columnValues[ii].Trim() + "%' OR " + columnName + " " + operation;
                                        }
                                        else if (ii == columnValues.Length - 1)
                                        {
                                            if (operation != "LIKE")

                                                columnValue += " '" + columnValues[ii].Trim() + "' )";
                                            else
                                                columnValue += " '%" + columnValues[ii].Trim() + "%' )";
                                        }
                                        else
                                        {
                                            if (operation != "LIKE")

                                                columnValue += "'" + columnValues[ii].Trim() + "' OR " + columnName + " " + operation;
                                            else
                                                columnValue += "'%" + columnValues[ii].Trim() + "%' OR " + columnName + " " + operation;
                                        }

                                    }

                                }

                                columnName = Session[FilterSessionID + "filterOperator"].ToString() + "(" + columnName;

                            }
                            else
                            {
                                for (int ii = 0; ii < columnValues.Length; ii++)
                                {
                                    Regex r = new Regex("^([0]?[1-9]|[1|2][0-9]|[3][0|1])[/]([0]?[1-9]|[1][0-2])[/]([0-9]{4}|[0-9]{2})$");

                                    if (r.IsMatch(columnValues[ii].Trim()))
                                    {
                                        DateTime dt = new DateTime();
                                        string day = columnValues[ii].Trim().Split(new char[] { '/' })[0].ToString();
                                        string month = columnValues[ii].Trim().Split(new char[] { '/' })[1].ToString();
                                        string year = columnValues[ii].Trim().Split(new char[] { '/' })[2].ToString();

                                        string ds = month + "/" + day + "/" + year;


                                        dt = DateTime.Parse(ds);
                                        columnValue = "#" + dt.ToString() + "#";
                                    }
                                    else
                                    {
                                        if (operation != "LIKE")

                                            columnValue = "'" + columnValues[ii].Trim() + "'";
                                        else
                                            columnValue = "'%" + columnValues[ii].Trim() + "%'";
                                    }

                                }
                                columnName = Session[FilterSessionID + "filterOperator"].ToString() + columnName;
                            }

                        }
                        if (c.GetType() == typeof(Button))
                        {
                            if (((Button)c).ID.StartsWith("btnS"))
                            {
                                ((Button)c).Visible = false;
                            }
                        }
                    }
                    filterPanelsDict = (Dictionary<string, Filter>)Session[FilterSessionID + "filterPanelsDict"];
                    filterPanelsDict.Add("filterPanel-" + panelID.ToString(), new Filter(columnName, operation, columnValue));
                    Session.Add(FilterSessionID + "filterPanelsDict", filterPanelsDict);
                    if (filterPanelsDict.Count > 0)
                    {
                        btnAddNewFilter.Visible = false;
                        btnAndNewFilter.Visible = true;
                        btnOrNewFilter.Visible = true;
                    }
                    OnFilterAdded();

                }
            }


        }

        private void submitFilter(object sender)
        {


            int panelID = Int32.Parse(((Button)sender).ID.Split('-')[1]);
            string columnName = "";
            string operation = "";
            string columnValue = "";

            foreach (Panel p in ((List<Panel>)(Session[FilterSessionID + "filterPanelsList"])))
            {
                if (p.ID == "filterPanel-" + panelID.ToString())
                {
                    foreach (System.Web.UI.Control c in p.Controls)
                    {
                        if (c.GetType() == typeof(DropDownList))
                        {
                            if (c.ID.StartsWith("ddlColumnName"))
                            {
                                columnName = Session[FilterSessionID + "filterOperator"].ToString() + ((DropDownList)c).SelectedValue;
                            }
                            else
                            {
                                operation = ((DropDownList)c).Text;
                            }
                        }
                        if (c.GetType() == typeof(TextBox))
                        {
                            Regex r = new Regex("^([0]?[1-9]|[1|2][0-9]|[3][0|1])[/]([0]?[1-9]|[1][0-2])[/]([0-9]{4}|[0-9]{2})$");

                            if (r.IsMatch(((TextBox)c).Text))
                            {
                                DateTime dt = new DateTime();
                                string day = (((TextBox)c).Text).Split(new char[] { '/' })[0].ToString();
                                string month = (((TextBox)c).Text).Split(new char[] { '/' })[1].ToString();
                                string year = (((TextBox)c).Text).Split(new char[] { '/' })[2].ToString();

                                string ds = month + "/" + day + "/" + year;


                                dt = DateTime.Parse(ds);
                                columnValue = "#" + dt.ToString() + "#";
                            }
                            else
                            {
                                if (operation != "LIKE")

                                    columnValue = "'" + ((TextBox)c).Text + "'";
                                else
                                    columnValue = "'%" + ((TextBox)c).Text + "%'";
                            }
                        }
                        if (c.GetType() == typeof(Button))
                        {
                            if (((Button)c).ID.StartsWith("btnS"))
                            {
                                ((Button)c).Visible = false;
                            }
                        }
                    }
                    filterPanelsDict = (Dictionary<string, Filter>)Session[FilterSessionID + "filterPanelsDict"];
                    filterPanelsDict.Add("filterPanel-" + panelID.ToString(), new Filter(columnName, operation, columnValue));
                    Session.Add(FilterSessionID + "filterPanelsDict", filterPanelsDict);
                    if (filterPanelsDict.Count > 0)
                    {
                        btnAddNewFilter.Visible = false;
                        btnAndNewFilter.Visible = true;
                        btnOrNewFilter.Visible = true;
                    }
                    //btnAddNewFilter.Visible = true;
                    OnFilterAdded();

                }
            }


        }

        protected void cancelFilterButtonHandler_Click(object sender, EventArgs e)
        {

            int panelID = Int32.Parse(((Button)sender).ID.Split('-')[1]);
            int nextPanelID = 0;
            bool isFirstPanel = false;
            bool isLastPanel = false;

            for (int i = 0; i < filterPanelsList.Count; i++)
            {
                Panel p = filterPanelsList[i];
                if (p.ID == "filterPanel-" + panelID.ToString())
                {

                    if (i == 0)
                    {
                        isFirstPanel = true;
                    }
                    else
                    {
                        isFirstPanel = false;
                    }

                    if (filterPanelsList.Count == 1)
                    {
                        isLastPanel = true;

                    }
                    else
                    {
                        isLastPanel = false;
                        if (isFirstPanel)
                            nextPanelID = Int32.Parse(filterPanelsList[i + 1].ID.Split('-')[1]);
                    }


                    break;
                }
            }

            filterPanelsDict = (Dictionary<string, Filter>)Session[FilterSessionID + "filterPanelsDict"];

            filterPanelsDict.Remove("filterPanel-" + panelID.ToString());

            if (isFirstPanel && (!isLastPanel))
            {
                try
                {
                    filterPanelsDict["filterPanel-" + nextPanelID.ToString()].ColumnName = filterPanelsDict["filterPanel-" + nextPanelID.ToString()].ColumnName.Trim().Split(' ')[1].ToString();
                }
                catch
                {
                    string x;
                }
            }

            Session.Add(FilterSessionID + "filterPanelsDict", filterPanelsDict);

            filterPanelsList = (List<Panel>)Session[FilterSessionID + "filterPanelsList"];
            foreach (Panel p in filterPanelsList)
            {
                if (p.ID == "filterPanel-" + panelID.ToString())
                {
                    filterPanelsList.Remove(p);
                    break;
                }
            }
            Session.Add(FilterSessionID + "filterPanelsList", filterPanelsList);

            try
            {
                foreach (System.Web.UI.Control c in pnlNewFilter.Controls)
                {
                    if (c.GetType() == typeof(Panel))
                    {
                        if (c.ID == "filterPanel-" + panelID.ToString())
                        {
                            pnlNewFilter.Controls.Remove(c);
                        }
                    }
                }
            }
            catch
            {
                string x;
            }
            if (filterPanelsDict.Count == 0)
            {
                btnAddNewFilter.Visible = true;
                btnAndNewFilter.Visible = false;
                btnOrNewFilter.Visible = false;
            }
            else
            {
                btnAndNewFilter.Visible = true;
                btnOrNewFilter.Visible = true;
            }
            //LoadPanels(filterPanelsList);


            OnFilterAdded();
        }

        public void DeleteAllFilters()
        {
            List<string> panelIDs = new List<string>();
            foreach (Panel pnlLL in filterPanelsList)
            {
                panelIDs.Add(pnlLL.ID.Split('-')[1]);
            }
            foreach (String panelID in panelIDs)
            {
                int nextPanelID = 0;
                bool isFirstPanel = false;
                bool isLastPanel = false;

                for (int i = 0; i < filterPanelsList.Count; i++)
                {
                    Panel p = filterPanelsList[i];
                    if (p.ID == "filterPanel-" + panelID.ToString())
                    {

                        if (i == 0)
                        {
                            isFirstPanel = true;
                        }
                        else
                        {
                            isFirstPanel = false;
                        }

                        if (filterPanelsList.Count == 1)
                        {
                            isLastPanel = true;

                        }
                        else
                        {
                            isLastPanel = false;
                            if (isFirstPanel)
                                nextPanelID = Int32.Parse(filterPanelsList[i + 1].ID.Split('-')[1]);
                        }


                        break;
                    }
                }

                filterPanelsDict = (Dictionary<string, Filter>)Session[FilterSessionID + "filterPanelsDict"];

                filterPanelsDict.Remove("filterPanel-" + panelID.ToString());

                if (isFirstPanel && (!isLastPanel))
                {
                    try
                    {
                        filterPanelsDict["filterPanel-" + nextPanelID.ToString()].ColumnName = filterPanelsDict["filterPanel-" + nextPanelID.ToString()].ColumnName.Trim().Split(' ')[1].ToString();
                    }
                    catch
                    {
                        string x;
                    }
                }

                Session.Add(FilterSessionID + "filterPanelsDict", filterPanelsDict);

                filterPanelsList = (List<Panel>)Session[FilterSessionID + "filterPanelsList"];
                foreach (Panel p in filterPanelsList)
                {
                    if (p.ID == "filterPanel-" + panelID.ToString())
                    {
                        filterPanelsList.Remove(p);
                        break;
                    }
                }
                Session.Add(FilterSessionID + "filterPanelsList", filterPanelsList);

                try
                {
                    foreach (System.Web.UI.Control c in pnlNewFilter.Controls)
                    {
                        if (c.GetType() == typeof(Panel))
                        {
                            if (c.ID == "filterPanel-" + panelID.ToString())
                            {
                                pnlNewFilter.Controls.Remove(c);
                            }
                        }
                    }
                }
                catch
                {
                    string x;
                }
                if (filterPanelsDict.Count == 0)
                {
                    btnAddNewFilter.Visible = true;
                    btnAndNewFilter.Visible = false;
                    btnOrNewFilter.Visible = false;
                }
                else
                {
                    btnAndNewFilter.Visible = true;
                    btnOrNewFilter.Visible = true;
                }
            }
            OnFilterAdded();


        }

        public bool Visible
        {
            get { return updatePanel.Visible; }
            set { updatePanel.Visible = value; }
        }

        private Dictionary<string, Filter> Clone(Dictionary<string, Filter> dict)
        {
            Dictionary<string, Filter> dictNew = new Dictionary<string, Filter>();
            foreach (KeyValuePair<string, Filter> kvp in dict)
            {
                dictNew.Add(kvp.Key, kvp.Value);
            }
            return dictNew;
        }

        protected void btnAddNewFilter_Click(object sender, EventArgs e)
        {
            CrearFilters();
            Session.Add(FilterSessionID + "filterOperator", " ");
            AddNewFilter();
            btnAddNewFilter.Visible = false;


        }

        public void CrearFilters()
        {
            filters = new List<Filter>();
            filterPanelsDict = new Dictionary<string, Filter>();
            filterPanelsList = new List<Panel>();

            Session.Remove(FilterSessionID + "filtersCounter");
            Session.Remove(FilterSessionID + "filterPanelsList");

            filtersCounter = 0;
            Session.Add(FilterSessionID + "filtersCounter", filtersCounter);

            filterPanelsDict = new Dictionary<string, Filter>();
            Session.Add(FilterSessionID + "filterPanelsDict", filterPanelsDict);

            filterPanelsList = new List<Panel>();
            Session.Add(FilterSessionID + "filterPanelsList", filterPanelsList);

            //DeleteAllFilters();
            //LoadPanels(filterPanelsList);



        }

        protected void btnAndNewFilter_Click(object sender, EventArgs e)
        {
            Session.Add(FilterSessionID + "filterOperator", " AND ");
            AddNewFilter();
            btnAndNewFilter.Visible = false;
            btnOrNewFilter.Visible = false;
        }
        public void BeginFilter()
        {
            Session.Add(FilterSessionID + "filterOperator", " ");
        }
        public void AndNewFilter()
        {
            Session.Add(FilterSessionID + "filterOperator", " AND ");
            //AddNewFilter();
            //btnAndNewFilter.Visible = false;
            //btnOrNewFilter.Visible = false;
        }
        protected void btnOrNewFilter_Click(object sender, EventArgs e)
        {
            Session.Add(FilterSessionID + "filterOperator", " OR ");
            AddNewFilter();
            btnAndNewFilter.Visible = false;
            btnOrNewFilter.Visible = false;

        }
    }

}