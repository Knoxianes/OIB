using CertificateManager;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace Logger
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /// srvCertCN.SubjectName should be set to the service's username. .NET WindowsIdentity class provides information about Windows user running the given process
			string srvCertCN = "wcflogger";

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
