using CryptoNETStandar;
using System.Text;

namespace DataExpressWeb
{
    public class Generic : System.Web.UI.Page
    {
        private readonly ICrypto _crypto;

        public Generic()
        {
            _crypto = new Crypto();
        }
        protected string Encrypt(string value)
        {
            return _crypto.Encrypt(value);
        }
        protected string Decrypt(string value)
        {
            return _crypto.Decrypt(value);
        }
    }
}