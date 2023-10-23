using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(Service)))
            {
                host.Open();
                Console.WriteLine("Listening");
                Console.ReadLine();
                host.Close();
            }
        }
    }
}
