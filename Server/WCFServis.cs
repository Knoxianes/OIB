using CertificateManager;
using Common;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;

namespace MainComponent
{
    public class WCFServis : ChannelFactory<ILogger>, IDisposable
    {
        ILogger factory;
        ProcessServis ps = new ProcessServis();

        public WCFServis(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
			string cltCertCN = CertificateManager.Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = Manager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }
        public void TestCommunication()
        {
            try
            {
                factory.TestCommunication();
            }
            catch (Exception e)
            {
                Console.WriteLine("[TestCommunication] ERROR = {0}", e.Message);
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Basic")]
        public bool StartProcess(int pid)
        {
            return ps.StartProcess(pid);
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Basic")]
        public void StopProcess()
        {


        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Show")]
        public void ShowActiveProcesses()
        {
            
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void StopAllProcesses()
        {

        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void ReadLogFile()
        {

        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }
    }
}
