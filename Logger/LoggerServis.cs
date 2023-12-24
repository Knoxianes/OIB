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


        public string Read()
        {
            var ret = "\n";
            try
            {

                var logs = Audit.GetEventLogs();
                if (logs == null)
                {
                    return "Nema logova";
                }
                foreach(EventLogEntry log in logs)
                {
                    
                    ret += log.Message + "\n";
                    
                }
                return ret;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return ret;
           
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
        public void WriteEvent(Alarm a, string action)
        {
            try
            {
                Audit.WriteEvent(a, action);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }



    }
}
