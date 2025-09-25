namespace CryptoNETStandar
{
    public interface ICrypto
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}
