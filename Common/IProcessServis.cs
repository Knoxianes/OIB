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
    public interface IProcessServis
    {
        [OperationContract]
        bool StartProcess(string path);

        [OperationContract]
        bool StopProcess(int pid);

        [OperationContract]
        List<Proces> ShowActiveProcesses();

        [OperationContract]
        void StopAllProcesses(); 

        [OperationContract]
        string ReadLogFile();

        [OperationContract]
        void ManagePermission(bool isAdd, string rolename, params string[] permissions);

        [OperationContract]
        void ManageRoles(bool isAdd, string rolename);

    }
}
