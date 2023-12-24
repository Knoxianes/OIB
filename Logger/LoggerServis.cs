using Common;
using SecurityManager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class LoggerServis : ILogger
    {


        public EventLogEntryCollection Read()
        {
            return Audit.GetEventLogs();
        }

        public void TestCommunication()
        {
            Console.WriteLine("Communication established.");
        }
        public void WriteInfo(string message)
        {
            try
            {
                Audit.WriteInfo(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void WriteEvent(Alarm a)
        {
            try
            {
                Audit.WriteEvent(a);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



    }
}
