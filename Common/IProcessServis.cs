using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IProcessServis
    {
        [OperationContract]
        void StartProcess();
        [OperationContract]
        void StopProcess();
        [OperationContract]
        void ShowActiveProcesses();
        [OperationContract]
        void StopAllProcesses(); 
        [OperationContract]
        void ReadLogFile();
    }
}
