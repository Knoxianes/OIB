using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [ServiceContract]
    public interface IShowActiveProcess
    {
        [OperationContract]
        //List<Process>ShowActiveProcesses();
        int a();
    }
}
