using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface ILogger
    {
        [OperationContract]
        string Read();

        [OperationContract]
        void TestCommunication();

        [OperationContract]
        void WriteEvent(Alarm a,string action);
        [OperationContract]
        void WriteInfo(string message);
    }
}
