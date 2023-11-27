using Common;
using MainComponent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class DataBase
    {
        internal static Dictionary<int, Proces> procesi = new Dictionary<int, Proces>();

        static DataBase()
        {
            Proces p1 = new Proces(1, "Chrome", State.Started);
            Proces p2 = new Proces(2, "Notepad", State.Started);
            Proces p3 = new Proces(3, "Paint", State.Stopped);
            Proces p4 = new Proces(4, "Canva", State.Started);

            procesi.Add(1,p1);
            procesi.Add(2,p2);
            procesi.Add(3,p3);
            procesi.Add(4,p4);
        }
    }
}
