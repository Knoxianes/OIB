using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;

namespace CertificateManager
{
    public class ClientCertValidator : X509CertificateValidator
    {
        // Implementation of a custom certificate validation on the client side.
        public override void Validate(X509Certificate2 certificate)
        {
            if (certificate.Subject.Equals(certificate.Issuer))
            {
                throw new Exception("Certificate is self-issued.");
            }
        }
    }
}
