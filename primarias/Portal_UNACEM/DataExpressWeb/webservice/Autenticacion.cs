using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;

namespace DataExpressWeb.webservice
{
    public class Autenticacion : SoapHeader
    {
         private string sUserPass; 
        private string sUserName;

        /// <summary> 
        /// Lee o escribe la clave del usuario 
        /// </summary> 
        public string UsuarioClave
        {
            get
            {
                return sUserPass;
            }
            set
            {
                sUserPass = value;
            }
        }

         /// <summary> 
        /// Lee o escribe el nombre del usuario 
        /// </summary> 
        public string UsuarioNombre
        {
            get
            {
                return sUserName;
            }
            set
            {
                sUserName = value;
            }
        }
    }
}