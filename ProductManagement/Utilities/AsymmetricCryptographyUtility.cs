using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class AsymmetricCryptographyUtility
    {
        private readonly RSACryptoServiceProvider _rsa = new RSACryptoServiceProvider(2048);
        private readonly IConfiguration _configuration;
        public AsymmetricCryptographyUtility(IConfiguration configuration)
        {
            // _logginUtlity= logginUtlity;
            _configuration = configuration;
        }
        //public byte[] EncryptData(string data, RSAParameters publicKey)
        public byte[] EncryptData(string data)
        {
            try
            {
                RSAHelper rSAHelper = new RSAHelper();
                using (var rsa = new RSACryptoServiceProvider(2048))
                {
                    rsa.ImportParameters(rSAHelper.GetPublicKey());
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    byte[] encryptedData = rsa.Encrypt(dataBytes, RSAEncryptionPadding.OaepSHA1);
                    return encryptedData;
                }
            }
            catch (CryptographicException ex)
            {
                //Console.WriteLine("CryptographicException during decryption: " + ex.Message);
                LoggingUtility.LogTxt(ex.Message, _configuration);
                throw;
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Exception during decryption: " + ex.Message);
                LoggingUtility.LogTxt(ex.Message, _configuration);
                throw;
            }

        }

        //public string DecryptData(byte[] encryptedData, RSAParameters privateKey)
        public string DecryptData(byte[] encryptedData)
        {
            try
            {
                RSAHelper rSAHelper = new RSAHelper();
                using (var rsa = new RSACryptoServiceProvider(2048))
                {
                    rsa.ImportParameters(rSAHelper.GetPrivateKey());
                    byte[] decryptedData = rsa.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA1);
                    return Encoding.UTF8.GetString(decryptedData);
                }
            }
            catch (CryptographicException ex)
            {

                //Console.WriteLine("CryptographicException during decryption: " + ex.Message);
                LoggingUtility.LogTxt(ex.Message, _configuration);
                throw;
            }
            catch (Exception ex)
            {

                //Console.WriteLine("Exception during decryption: " + ex.Message);
                LoggingUtility.LogTxt(ex.Message, _configuration);
                throw;
            }
        }
    }
}
