using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using SecurityManager;
using Server;

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
        public void ReadLogFile()
        {
            throw new NotImplementedException();
        }

        public List<Proces> ShowActiveProcesses()
        {
            var procesi = new List<Proces>();
            foreach(Proces proc in DataBase.procesi.Values)
            {
                if(proc.Pstate == State.Started)
                {
                    procesi.Add(proc);
                }
            }
            return procesi;
        }

        public bool StartProcess(int pid)
        {
            if(DataBase.procesi.ContainsKey(pid))
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

        public bool StopAllProcesses()
        {
            foreach(Proces proces in DataBase.procesi.Values)
            {
                if(proces.Pstate == State.Started)
                {
                    proces.Pstate = State.Stopped;
                }
            }
            return true;
        }

        public bool StopProcess(int pid)
        {
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

        public void TestCommunication()
        {
            Console.WriteLine("Communication established.");
        }

    }
}
