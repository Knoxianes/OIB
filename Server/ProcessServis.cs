using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Server;

namespace MainComponent
{
    public class ProcessServis : IProcessServis
    {
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
