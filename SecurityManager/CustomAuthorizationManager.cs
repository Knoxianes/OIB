using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public class CustomAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            //Ovde trebamo da proveravamo koji user pripada kojij grupi i da po tome proveravamo da li ima taj role
            CustomPrincipal principal = operationContext.ServiceSecurityContext.
                 AuthorizationContext.Properties["Principal"] as CustomPrincipal;
          
            return principal.IsInRole("Show") || principal.IsInRole("Administrate") || principal.IsInRole("Basic");
        }
    }
}
