using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;
using MainComponent;

namespace Client
{
    public class WCFClient: ChannelFactory<IProcessServis>, IDisposable
    {
        IProcessServis factory;

        public WCFClient(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            factory = this.CreateChannel();
        }

        public void TestComunication()
        {
            factory.TestCommunication();
        }

        public bool StartProcess(int key)
        {
            bool retValue = false;
            try
            {
                retValue = factory.StartProcess(key);
                Console.WriteLine("Start Process allowed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Start Process : {0}", e.Message);
            }
            return retValue;
        }

        public bool StopProcess(int key)
        {
            bool retValue = false;
            try
            {
                retValue = factory.StopProcess(key);
                Console.WriteLine("Stop Process allowed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Stop Process : {0}", e.Message);
            }
            return retValue;
        }

        public List<Proces> ShowActiveProcesses()
        {
            List<Proces> ps = new List<Proces>();
            try
            {
                ps = factory.ShowActiveProcesses();
                Console.WriteLine("Show Active Processes allowed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Show Active Processes : {0}", e.Message);
            }
            return ps;
        }

        public bool StopAllProcesses()
        {
            bool retValue = false;
            try
            {
                retValue = factory.StopAllProcesses();
                Console.WriteLine("Stop All Processes allowed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Stop All Processes : {0}", e.Message);
            }
            return retValue;
        }

        public void ManagePermission(bool isAdd, string rolename, params string[] permissions)
        {
            try
            {
                factory.ManagePermission(isAdd, rolename, permissions);
                Console.WriteLine("Manage allowed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Manage : {0}", e.Message);
            }
        }

        public void ManageRoles(bool isAdd, string rolename)
        {
            try
            {
                factory.ManageRoles(isAdd, rolename);
                Console.WriteLine("Manage allowed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Manage : {0}", e.Message);
            }
        }

        public void ReadLogFile()
        {
            throw new NotImplementedException();
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

