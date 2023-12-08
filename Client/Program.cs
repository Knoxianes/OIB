using CertificateManager;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:4000/IProcessServis"));

            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            using (WCFClient proxy = new WCFClient(binding, address))
            {
                var run = true;
                do
                {
                    switch (Program.PrintMenu())
                    {
                        case '1':
                            Program.StartProcess(proxy);
                            break;
                        case '2':
                            Program.StopProcess(proxy);
                            break;
                        case '3':
                            proxy.ShowActiveProcesses();
                            break;
                        case '4':
                            proxy.StopAllProcesses();
                            break;
                        case '0':
                            run = false;
                            break;
                        default:
                            break;
                    }

                } while (run);
            }
        }

        public static char PrintMenu()
        {
            Console.WriteLine("\n\t-----------------------");
            Console.WriteLine("\t\tMenu");
            Console.WriteLine("\t-----------------------\n");
            Console.WriteLine("\t1. Start process");
            Console.WriteLine("\t2. Stop process");
            Console.WriteLine("\t3. Show active processes");
            Console.WriteLine("\t4. Stop all processes\n");
            Console.WriteLine("\t0. Exit");
            return Console.ReadKey().KeyChar;
        }
        public static void StartProcess(WCFClient proxy)
        {
            
            Console.WriteLine("\n\n");
            var pidCorrect = false;
            int pid;
            
                Console.Write("Please enter pid of process to start: ");
                var readLine = Console.ReadLine();

                if (!int.TryParse(readLine, out pid))
                {
                    Console.WriteLine("Wrong format of input try again");
                }
                pidCorrect = proxy.StartProcess(pid);
                
            
            
            
        }
        public static void StopProcess(WCFClient proxy)
        {

            Console.WriteLine("\n\n");
            var pidCorrect = false;
            int pid;
            
                Console.Write("Please enter pid of process to stop: ");
                var readLine = Console.ReadLine();

                if (!int.TryParse(readLine, out pid))
                {
                    Console.WriteLine("Wrong format of input try again");
                }
                pidCorrect = proxy.StartProcess(pid);

            


        }
    }
}
