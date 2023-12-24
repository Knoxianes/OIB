using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Common
{

    [DataContract]
    public class Proces
    {
        int pid;
        string pname;

        public Proces(int pid, string pname)
        {
            this.Pid = pid;
            this.Pname = pname;
        }

        [DataMember]
        public int Pid { get => pid; set => pid = value; }

        [DataMember]
        public string Pname { get => pname; set => pname = value; }

        public override string ToString()
        {
            return "\t[" + pid + "]: " + pname+"\n";
        }
    }
}
