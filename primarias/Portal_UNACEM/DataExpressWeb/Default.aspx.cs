using System;
using Datos;
using clibLogger;

namespace DataExpressWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var DB = new BasesDatos();
            try
            {
                if (Session["rfcUser"] != null)
                {
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
    }
}
