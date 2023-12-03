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
                if (!retValue)
                {
                    Console.WriteLine("Process with this pid is already started");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Start Process : {0}", e.Message);
                return true;
              
            }
            return retValue;
        }

        public bool StopProcess(int key)
        {
            bool retValue = false;
            try
            {
                retValue = factory.StopProcess(key);
                if (!retValue)
                {
                   Console.WriteLine("Process with this pid is already stopped");
                }
              
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Stop Process : {0}", e.Message);
                return true;
            
            }
            return retValue;
        }

        public void ShowActiveProcesses()
        {
            List<Proces> ps = new List<Proces>();
            try
            {
                Console.WriteLine("\n\n");
                ps = factory.ShowActiveProcesses();
                
                if (ps.Count <= 0)
                {
                    Console.WriteLine("There is no active processes right now");
                }
                else
                {
                    Console.WriteLine("\n\tActive processes:");
                    foreach (var process in ps)
                    {
                        Console.WriteLine(process);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Show Active Processes : {0}", e.Message);
              
            }
        }

        public void StopAllProcesses()
        {
           
            try
            {
                Console.WriteLine("\n\n");
                var error = factory.StopAllProcesses();
                if (!error)
                {
                    Console.WriteLine("All processes already stopped");
                    return;
                }
                Console.WriteLine("All processes stopped");

            }
            catch (Exception e)
            {
                Console.WriteLine("Error while trying to Stop All Processes : {0}", e.Message);
              
            }
        }

        public void ManagePermission(bool isAdd, string rolename, params string[] permissions)
        {
            try
            {
                factory.ManagePermission(isAdd, rolename, permissions);
        
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

