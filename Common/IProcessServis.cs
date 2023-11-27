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
        bool StartProcess(int pid);

        [OperationContract]
        bool StopProcess(int pid);

        [OperationContract]
        List<Proces> ShowActiveProcesses();

        [OperationContract]
        bool StopAllProcesses(); 

        [OperationContract]
        void ReadLogFile();

        [OperationContract]
        void ManagePermission(bool isAdd, string rolename, params string[] permissions);

        [OperationContract]
        void ManageRoles(bool isAdd, string rolename);

        [OperationContract]
        void TestCommunication();
    }
}
