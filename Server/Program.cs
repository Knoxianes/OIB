using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using CertificateManager;
using Common;
using SecurityManager;
using System.Configuration;
using System.IO;

namespace MainComponent
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Define the expected service certificate. It is required to establish cmmunication using certificates.
            string srvCertCN = "wcflogger";

            NetTcpBinding bindingServerMainComponenet = new NetTcpBinding(); //Binding za main component kada je main componenet server
            string addressServerMainComponent = "net.tcp://localhost:4000/IProcessServis"; // Adresa za main component kada je main component server
            NetTcpBinding bindingClientMainComponent = new NetTcpBinding(); // Binding za main component kada je main componenet client

            // Use Manager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = Manager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

            EndpointAddress addressClientMainComponent = new EndpointAddress(new Uri("net.tcp://localhost:4001/ILogger"),
                                      new X509CertificateEndpointIdentity(srvCert)); // Adresa za konekciju maincomponenta kao clienta sa logerom koji je server

            
            bindingServerMainComponenet.Security.Mode = SecurityMode.Transport;
            bindingServerMainComponenet.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingServerMainComponenet.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            bindingClientMainComponent.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            ServiceHost host = new ServiceHost(typeof(ProcessServis));

            // podesavamo da se koristi MyAuthorizationManager umesto ugradjenog
            host.Authorization.ServiceAuthorizationManager = new CustomAuthorizationManager();

            // podesavamo custom polisu, odnosno nas objekat principala
            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();
            //Audit
            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

            host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            host.Description.Behaviors.Add(newAudit);
            host.AddServiceEndpoint(typeof(IProcessServis), bindingServerMainComponenet, addressServerMainComponent);
            try
            {
                host.Open();
                Console.WriteLine("WCFService is started.\nPress <enter> to stop ...");
                using (WCFServis proxy = new WCFServis(bindingClientMainComponent, addressClientMainComponent))
                {
                    proxy.TestCommunication();
                    Task.Factory.StartNew(async () =>
                    {
                        int count = 0;
                        int timer = int.Parse(ConfigurationSettings.AppSettings["timer"]);
                        while (proxy.State == CommunicationState.Opened)
                        {
                            
                            CheckProcesses(proxy,ref count);
                            await Task.Delay(timer);
                        }
                        
                    });
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
                Console.ReadLine();
            }
            finally
            {
                host.Close();
            }

        } 
        static void CheckProcesses(WCFServis proxy,ref int count)
        {
            var listProcesses = Process.GetProcesses();
            var badProcesses = ReadBadProcesses();
            foreach (Process process in  listProcesses)
            {
                if (badProcesses.Contains(process.ProcessName))
                {
                    try
                    {
                        process.Kill();
                        count++;
                        proxy.WriteLog(process, count);
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                   
                }
            }
            
        }

        static string file = "badprocesses.txt";
        static List<string> ReadBadProcesses()
        {
            var list = new List<string>();
            if (File.Exists(file))
            {
                // Reads file line by line 
                StreamReader Textfile = new StreamReader(file);
                string line;

                while ((line = Textfile.ReadLine()) != null)
                {
                    
                    list.Add(line);
                }

                Textfile.Close();

            }
            return list;
        }
    }
}
