using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace MainComponent
{
    internal class Program
    {
        static void Main(string[] args)
        {

            NetTcpBinding bindingServerMainComponenet = new NetTcpBinding(); //Binding za main component kada je main componenet server
            string addressServerMainComponent = "net.tcp://localhost:4000/IProcessServis"; // Adresa za main component kada je main component server
            NetTcpBinding bindingClientMainComponent = new NetTcpBinding(); // Binding za main component kada je main componenet client
            EndpointAddress addressClientMainComponent = new EndpointAddress(new Uri("net.tcp://localhost:4001/ILogger")); // Adresa za konekciju maincomponenta kao clienta sa logerom koji je server

            bindingServerMainComponenet.Security.Mode = SecurityMode.Transport;
            bindingServerMainComponenet.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            bindingServerMainComponenet.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            ServiceHost host = new ServiceHost(typeof(ProcessServis));
            host.AddServiceEndpoint(typeof(IProcessServis), bindingServerMainComponenet, addressServerMainComponent);
            try
            {
                host.Open();
                Console.WriteLine("WCFService is started.\nPress <enter> to stop ...");
                using (WCFClient proxy = new WCFClient(bindingClientMainComponent, addressClientMainComponent))
                {

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
