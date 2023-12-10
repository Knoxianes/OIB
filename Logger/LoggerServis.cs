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
        private static EventLog customLog = null;
        const string SourceName = "Logger.LoggerServis";
        const string LogName = "Greske";

        
        
        public void Read()
        {
            throw new NotImplementedException();
        }

        public void TestCommunication()
        {
            Console.WriteLine("Communication established.");
        }

        public void WriteEvent(Alarm a)
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                customLog = new EventLog(LogName,
                    Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
            if (customLog != null)
            {
                // Koristite vrednosti iz objekta Alarm
                DateTime alarmDateTime = a.DateTime;  // Pretpostavka da želite koristiti StartDateTime
                EventLogEntryType logEntryType = ConvertToEventLogEntryType(a.UtLVL);  // Koristite utLvl direktno

                // Prilagodite kako vam odgovara ostatak vaših podataka iz objekta Alarm
                object[] values = new object[] { alarmDateTime, a.Pname };
                string message = "useo si";
                customLog.WriteEntry(message);
                
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthenticationSuccess));
            }
        }
        private static EventLogEntryType ConvertToEventLogEntryType(UtilityLVL utLvl)
        {
            // Mapiranje enumeracije UtilityLVL na EventLogEntryType
            switch (utLvl)
            {
                case UtilityLVL.Information:
                    return EventLogEntryType.Information;
                case UtilityLVL.Warning:
                    return EventLogEntryType.Warning;
                case UtilityLVL.Error:
                    return EventLogEntryType.Error;
                default:
                    throw new ArgumentException("Unsupported UtilityLVL value");
            }
        }
    }
}
