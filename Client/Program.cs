using Common;
using System;
using System.Collections.Generic;
using System.Linq;
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
            using (WCFClient proxy = new WCFClient(binding, address))
            {
                
                Console.WriteLine("Client Started");
                Console.ReadLine();
            }
        }
    }
}
