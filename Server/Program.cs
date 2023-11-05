using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using CertificateManager;
using Common;

namespace MainComponent
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Define the expected service certificate. It is required to establish cmmunication using certificates.
            string srvCertCN = "wcfservice";

            NetTcpBinding bindingServerMainComponenet = new NetTcpBinding(); //Binding za main component kada je main componenet server
            string addressServerMainComponent = "net.tcp://localhost:4000/IProcessServis"; // Adresa za main component kada je main component server
            NetTcpBinding bindingClientMainComponent = new NetTcpBinding(); // Binding za main component kada je main componenet client

            // Use Manager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = Manager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);


            EndpointAddress addressClientMainComponent = new EndpointAddress(new Uri("net.tcp://localhost:4001/ILogger"),
                                      new X509CertificateEndpointIdentity(srvCert)); // Adresa za konekciju maincomponenta kao clienta sa logerom koji je server

            
            bindingServerMainComponenet.Security.Mode = SecurityMode.Transport;
            bindingServerMainComponenet.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingServerMainComponenet.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            bindingServerMainComponenet.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            ServiceHost host = new ServiceHost(typeof(ProcessServis));
            host.AddServiceEndpoint(typeof(IProcessServis), bindingServerMainComponenet, addressServerMainComponent);
            try
            {
                host.Open();
                Console.WriteLine("WCFService is started.\nPress <enter> to stop ...");
                using (WCFClient proxy = new WCFClient(bindingClientMainComponent, addressClientMainComponent))
                {
                    proxy.TestCommunication();
                    Console.WriteLine("Server Started > " + WindowsIdentity.GetCurrent().Name);
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }
            finally
            {
                host.Close();
            }

        } 
    }
}
