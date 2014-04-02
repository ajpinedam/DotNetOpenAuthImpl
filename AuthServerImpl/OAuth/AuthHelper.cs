using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace AuthServerImpl.OAuth
{
    public static class AuthHelper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thumbprint"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static X509Certificate2 LoadCert(string thumbprint)
        {
            var xstore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            xstore.Open(OpenFlags.ReadOnly);

            var allcert = (X509Certificate2Collection) xstore.Certificates;

            var certificate = allcert.Cast<X509Certificate2>().FirstOrDefault(certification => String.Equals(certification.Thumbprint, thumbprint, StringComparison.InvariantCulture));

            if (null == certificate)
                throw new Exception("Could not find Certificate");

            /*
              Find Method is not working properly
              var certificates = allcert.Find(X509FindType.FindByThumbprint, thumbprint, false);

              if (certificates.Count == 0)
                throw new Exception("Could not find Certificate");
             */

            return certificate;
        }

    }
}