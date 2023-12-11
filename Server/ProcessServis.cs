using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CertificateManager;
using Common;
using SecurityManager;
using Server;

namespace MainComponent
{
    public class ProcessServis : IProcessServis
    {
        public Alarm a = new Alarm();
        public string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Greske.txt");
        public static int count = 0;
        UtilityLVL ut = UtilityLVL.Information;
        

        [PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void ManagePermission(bool isAdd, string rolename, params string[] permissions)
        {
            if (isAdd) // u pitanju je dodavanje
            {
                RolesConfig.AddPermissions(rolename, permissions);
            }
            else // u pitanju je brisanje
            {
                RolesConfig.RemovePermissions(rolename, permissions);
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void ManageRoles(bool isAdd, string rolename)
        {
            if (isAdd) // u pitanju je dodavanje
            {
                RolesConfig.AddRole(rolename);
            }
            else // u pitanju je brisanje
            {
                RolesConfig.RemoveRole(rolename);
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void ReadLogFile()
        {
            throw new NotImplementedException();
        }

        //[PrincipalPermission(SecurityAction.Demand, Role = "Show")]
        public List<Proces> ShowActiveProcesses()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = SecurityManager.Formatter.ParseName(principal.Identity.Name);
            var procesi = new List<Proces>();
            if (Thread.CurrentPrincipal.IsInRole("Show"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                foreach (Proces proc in DataBase.procesi.Values)
                {
                    if (proc.Pstate == State.Started)
                    {
                        procesi.Add(proc);
                    }
                }
            }
            else
            {
                count++;
                string message = "Show method need Show permission.";
                if (count <= 3)
                {
                    ut = UtilityLVL.Information;
                }
                if (count > 3 && count < 5)
                {
                    ut = UtilityLVL.Warning;
                }
                if (count > 5)
                {
                    ut = UtilityLVL.Error;
                }
                a = new Alarm(DateTime.Now, OperationContext.Current.IncomingMessageHeaders.Action, ut);
                try
                {
                    
                    File.AppendAllText(filePath, $"{DateTime.Now}: {message}\n");
                    WCFServis.factory.WriteEvent(a);
                    Audit.AuthorizationFailed(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action, message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                throw new FaultException("User " + userName +
                    " try to call Show method. Show method need  Show permission.");
            }
            return procesi;
        }

        //[PrincipalPermission(SecurityAction.Demand, Role = "Basic")]
        public bool StartProcess(int pid)
        {
            WCFServis.factory.TestCommunication();
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = SecurityManager.Formatter.ParseName(principal.Identity.Name);
            if (Thread.CurrentPrincipal.IsInRole("Basic"))
            {
                try
                {
                   
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                if (DataBase.procesi.ContainsKey(pid))
                {
                    if (DataBase.procesi[pid].Pstate == State.Started)
                    {
                        return false;
                    }
                    else
                    {
                        DataBase.procesi[pid].Pstate = State.Started;
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                count++;
                string message = "Basic method need Basic permission.";
                if (count <= 3)
                {
                    ut = UtilityLVL.Information;
                }
                if(count==4)
                {
                    ut = UtilityLVL.Warning;
                }
                if (count >= 5)
                {
                    ut = UtilityLVL.Error;
                }
                a = new Alarm(DateTime.Now, OperationContext.Current.IncomingMessageHeaders.Action, ut);
                try
                {
                    File.AppendAllText(filePath, $"{DateTime.Now}: {message}\n");
                    WCFServis.factory.WriteEvent(a);
                    
                    Audit.AuthorizationFailed(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action, message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                throw new FaultException("User " + userName +
                    " try to call Basic method. Basic method need  Basic permission.");
            }
        }
        //[PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public bool StopAllProcesses()
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = SecurityManager.Formatter.ParseName(principal.Identity.Name);
            var ret = false;
            if (Thread.CurrentPrincipal.IsInRole("Administrate"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                foreach (Proces proces in DataBase.procesi.Values)
                {
                    if (proces.Pstate == State.Started)
                    {
                        proces.Pstate = State.Stopped;
                        ret = true;
                    }
                }
            }
            else
            {
                

                string message = "Administrate method need Administrate permission.";
                count++;
                if (count <= 3)
                {
                    ut = UtilityLVL.Information;
                }
                if (count == 4)
                {
                    ut = UtilityLVL.Warning;
                }
                if (count >= 5)
                {
                    ut = UtilityLVL.Error;
                }
                a = new Alarm(DateTime.Now, OperationContext.Current.IncomingMessageHeaders.Action, ut);
                try
                {
                    File.AppendAllText(filePath, $"{DateTime.Now}: {message}\n");
                    WCFServis.factory.WriteEvent(a);
                    Audit.AuthorizationFailed(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action, message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                throw new FaultException("User " + userName +
                    " try to call Administrate method. Administrate method need  Administrate permission.");
            }
            return ret;
        }

        //[PrincipalPermission(SecurityAction.Demand, Role = "Basic")]
        public bool StopProcess(int pid)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = SecurityManager.Formatter.ParseName(principal.Identity.Name);
            if (Thread.CurrentPrincipal.IsInRole("Basic"))
            {
                try
                {
                    Audit.AuthorizationSuccess(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                if (DataBase.procesi.ContainsKey(pid))
                {
                    if (DataBase.procesi[pid].Pstate == State.Stopped)
                    {
                        return false;
                    }
                    else
                    {
                        DataBase.procesi[pid].Pstate = State.Stopped;
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                string message = "Basic method need Basic permission.";
                count++;

                if (count <= 3)
                {
                    ut = UtilityLVL.Information;
                }
                if (count == 4)
                {
                    ut = UtilityLVL.Warning;
                }
                if (count >= 5)
                {
                    ut = UtilityLVL.Error;
                }
                a = new Alarm(DateTime.Now, OperationContext.Current.IncomingMessageHeaders.Action, ut);
                try
                {
                    File.AppendAllText(filePath, $"{DateTime.Now}: {message}\n");
                    WCFServis.factory.WriteEvent(a);
                    Audit.AuthorizationFailed(userName,
                        OperationContext.Current.IncomingMessageHeaders.Action, message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                throw new FaultException("User " + userName +
                    " try to call Basic method. Basic method need  Basic permission.");
            }
        }

        public void TestCommunication()
        {
            Console.WriteLine("Communication established.");
        }

    }
}
