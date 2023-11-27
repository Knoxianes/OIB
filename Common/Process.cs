using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common
{
    [DataContract]
    public enum State { [EnumMember] Started, [EnumMember] Stopped}

    [DataContract]
    public class Proces
    {
        int pid;
        string pname;
        State pstate;

        public Proces(int pid, string pname, State pstate)
        {
            this.Pid = pid;
            this.Pname = pname;
            this.Pstate = pstate;
        }

        [DataMember]
        public int Pid { get => pid; set => pid = value; }

        [DataMember]
        public string Pname { get => pname; set => pname = value; }

        [DataMember]
        public State Pstate { get => pstate; set => pstate = value; }

        public override string ToString()
        {
            return "[" + pid + "]: " + pname + ", STATE: " + pstate.ToString();
        }
    }
}
