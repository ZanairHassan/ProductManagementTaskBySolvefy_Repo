using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Utilities
{
    public class RSAHelper
    {
        public RSAParameters GetPublicKey()
        {
            string public_key_path = "Data\\public.pub.pem";
            string key = ReadKeyFromPEMFile(public_key_path);
            return StringToRSAParameters(key, true);
        }

        public RSAParameters GetPrivateKey()
        {
            string private_key_path = "Data\\private.key.pem";
            string key = ReadKeyFromPEMFile(private_key_path);
            return StringToRSAParameters(key, false);
        }

        private RSAParameters StringToRSAParameters(string parametersString, bool isPublic)
        {
            RSAParameters parameters = new RSAParameters();

            try
            {
                var doc = new XmlDocument();
                doc.LoadXml(parametersString);

                parameters.Modulus = Convert.FromBase64String(doc.DocumentElement.SelectSingleNode("Modulus").InnerText);
                parameters.Exponent = Convert.FromBase64String(doc.DocumentElement.SelectSingleNode("Exponent").InnerText);
                if (!isPublic)
                    parameters.P = Convert.FromBase64String(doc.DocumentElement.SelectSingleNode("P").InnerText);
                if (!isPublic)
                    parameters.Q = Convert.FromBase64String(doc.DocumentElement.SelectSingleNode("Q").InnerText);
                if (!isPublic)
                    parameters.DP = Convert.FromBase64String(doc.DocumentElement.SelectSingleNode("DP").InnerText);
                if (!isPublic)
                    parameters.DQ = Convert.FromBase64String(doc.DocumentElement.SelectSingleNode("DQ").InnerText);
                if (!isPublic)
                    parameters.InverseQ = Convert.FromBase64String(doc.DocumentElement.SelectSingleNode("InverseQ").InnerText);
                if (!isPublic)
                    parameters.D = Convert.FromBase64String(doc.DocumentElement.SelectSingleNode("D").InnerText);
            }
            catch (Exception ex)
            {
                // Handle parsing errors
                Console.WriteLine("Error parsing RSAParameters: " + ex.Message);
            }

            return parameters;
        }

        private string ReadKeyFromPEMFile(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
