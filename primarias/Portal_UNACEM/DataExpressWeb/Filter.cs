using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace DataExpressWeb
{

    /// <summary>
    /// Summary description for Filter
    /// </summary>
    [Serializable]
    public class Filter
    {
        string operation;
        public string Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        string columnName;
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        string columnValue;
        public string ColumnValue
        {
            get { return columnValue; }
            set { columnValue = value; }
        }

        public Filter()
        {

        }
        public Filter(string columnName, string operation, string columnValue)
        {
            this.columnName = columnName;
            this.operation = operation;
            this.columnValue = columnValue;

        }
        public override string ToString()
        {
            return columnName + " " + operation + " " + columnValue;
        }
    }
}