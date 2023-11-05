using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace CertificateManager
{
    public class ServiceCertValidator : X509CertificateValidator
    {
        
        // Implementation of a custom certificate validation on the service side.
        public override void Validate(X509Certificate2 certificate)
        {
            // Take service's certificate from storage
            X509Certificate2 srvCert = Manager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine,
                Formatter.ParseName(WindowsIdentity.GetCurrent().Name));

            if (!certificate.Issuer.Equals(srvCert.Issuer))
            {
                throw new Exception("Certificate is not from the valid issuer.");
            }
        }
    }
}
