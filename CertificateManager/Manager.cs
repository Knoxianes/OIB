using System.Security.Cryptography.X509Certificates;
using System.Security;

namespace CertificateManager
{
    public class Manager
    {
		// Get a certificate with the specified subject name from the predefined certificate storage
		public static X509Certificate2 GetCertificateFromStorage(StoreName storeName, StoreLocation storeLocation, string subjectName)
        {
            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);

            //X509Certificate2Collection certCollection = store.Certificates.Find(X509FindType.FindBySubjectName, subjectName, true);

            // Check whether the subjectName of the certificate is exactly the same as the given "subjectName"
            foreach (X509Certificate2 c in store.Certificates)
            {
                if (c.SubjectName.Name.Equals(string.Format("CN={0}", subjectName)))
                {
                    return c;
                }
            }

            return null;
        }

        // Get a certificate from file.		
        public static X509Certificate2 GetCertificateFromFile(string fileName)
        {
            X509Certificate2 certificate = null;


            return certificate;
        }

        // Get a certificate from file.
  		public static X509Certificate2 GetCertificateFromFile(string fileName, SecureString pwd)
        {
            X509Certificate2 certificate = null;


            return certificate;
        }
    }
}
