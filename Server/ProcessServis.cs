using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace MainComponent
{
    public class ProcessServis : IProcessServis
    {
        

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

        [PrincipalPermission(SecurityAction.Demand, Role = "Show")]
        public List<Proces> ShowActiveProcesses()
        {
            var processes  = Process.GetProcesses();
            var ret =new List<Proces>();
            foreach(var proces in processes)
            {
                var tmp = new Proces(proces.Id, proces.ProcessName);
                ret.Add(tmp);
            }
            return ret;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Basic")]
        public bool StartProcess(string path)
        {
            try
            {
                var process = Process.Start(path);
                Alarm newAlarm = new Alarm
                {
                    UtLVL = UtilityLVL.Information,
                    DateTime = DateTime.Now,
                    Pname = process.ProcessName
                };
                WCFServis.factory.WriteEvent(newAlarm);
                return true;
            }catch
            {
                return false;
            }
        }
        [PrincipalPermission(SecurityAction.Demand, Role = "Administrate")]
        public void StopAllProcesses()
        {
            var processes = Process.GetProcesses();
            try
            {
                foreach (var process in processes)
                {
                    process.Kill();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Basic")]
        public bool StopProcess(int pid)
        {
            try
            {
               var process = Process.GetProcessById(pid);
                process.Kill();
                Alarm newAlarm = new Alarm
                {
                    UtLVL = UtilityLVL.Information,
                    DateTime = DateTime.Now,
                    Pname = process.ProcessName
                };
                WCFServis.factory.WriteEvent(newAlarm);
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
