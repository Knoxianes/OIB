using CertificateManager;
using Common;
using SecurityManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Policy;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace Logger
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // Ako prvi put pokrecete kod vas na kompjuteru morate odkomentarisati tri linije ispod i pokrenuti logger kao admin
            // Nakon takoga iskljucite logger i onda ga pokrenuti kao user koji ima potrebne sertifikate za logger

           // EventLog.CreateEventSource(Audit.SourceName, Audit.LogInfo);
           // EventLog.CreateEventSource(Audit.SourceName, Audit.LogName);
            //Console.ReadLine();


            /// srvCertCN.SubjectName should be set to the service's username. .NET WindowsIdentity class provides information about Windows user running the given process
			string srvCertCN = CertificateManager.Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

           

            NetTcpBinding binding = new NetTcpBinding();

            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
            binding.MaxReceivedMessageSize = 10000;
            binding.MaxBufferPoolSize = 10000;
            binding.MaxBufferSize = 10000;

            string address = "net.tcp://localhost:4001/ILogger";
            ServiceHost host = new ServiceHost(typeof(LoggerServis));
            host.AddServiceEndpoint(typeof(ILogger), binding, address);

            ///Custom validation mode enables creation of a custom validator - CustomCertificateValidator
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            
            
            ///If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            ///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
            host.Credentials.ServiceCertificate.Certificate = Manager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
            //dodato
            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

            host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            host.Description.Behaviors.Add(newAudit);

            try
            {
                host.Open();
                Console.WriteLine("WCFService is started.\nPress <enter> to stop ...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.ReadLine();
            }
            finally
            {
                host.Close();
            }
            Console.ReadLine();
        }
    }
}
