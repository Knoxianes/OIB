using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public class Audit : IDisposable
    {

        private static EventLog customLog = null;
        public const string SourceName = "SecurityManager.Audit";
   
        public const string LogInfo = "Info";

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogInfo);
                }
                customLog = new EventLog(LogInfo,
                    Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }


        public static void AuthenticationSuccess(string userName)
        {
            //TO DO

            if (customLog != null)
            {
                string UserAuthenticationSuccess =
                    AuditEvents.AuthenticationSuccess;
                string message = String.Format(UserAuthenticationSuccess, DateTime.Now,
                    userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthenticationSuccess));
            }
        }

        public static void AuthorizationSuccess(string userName, string serviceName)
        {
            //TO DO
            if (customLog != null)
            {
                string AuthorizationSuccess =
                    AuditEvents.AuthorizationFailed;
                string message = String.Format(AuthorizationSuccess, DateTime.Now,
                    userName, serviceName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthorizationSuccess));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="serviceName"> should be read from the OperationContext as follows: OperationContext.Current.IncomingMessageHeaders.Action</param>
        /// <param name="reason">permission name</param>
        public static void AuthorizationFailed(string userName, string serviceName, string reason)
        {
            if (customLog != null)
            {
                string AuthorizationFailed =
                    AuditEvents.AuthorizationFailed;
                string message = String.Format(AuthorizationFailed,DateTime.Now,
                    userName, serviceName, reason);
                customLog.WriteEntry(message);


            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthorizationFailed));
            }
        }

        public static void WriteInfo(string message)
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {

                    EventLog.CreateEventSource(SourceName, LogInfo);
                }
                customLog = new EventLog(LogInfo,
                    Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
            if (customLog != null)
            {


                customLog.WriteEntry(message);

            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthenticationSuccess));
            }
        }
        public static void WriteEvent(Alarm a, string action)
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {

                    EventLog.CreateEventSource(SourceName, LogInfo);
                }
                customLog = new EventLog(LogInfo,
                    Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
            if (customLog != null)
            {


                customLog.WriteEntry(a.ToString() + " - " + action);
                

            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthenticationSuccess));
            }
        }

        public static EventLogEntryCollection GetEventLogs()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {

                    EventLog.CreateEventSource(SourceName, LogInfo);
                }
                customLog = new EventLog(LogInfo,
                    Environment.MachineName, SourceName);
                if (customLog == null)
                {
                    return null;
                }
                return customLog.Entries;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
          
        }

        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}
