using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    internal class RolesConfig
    {
        static string path = @"~\..\..\..\..\SecurityManager\RolesConfigFile.resx";
        public static bool GetPermissions(string rolename, out string[] permissions)
        {
            permissions = new string[10];
            string permissionString = string.Empty;

            permissionString = (string)RolesConfigFile.ResourceManager.GetObject(rolename);
            if (permissionString != null)
            {
                permissions = permissionString.Split(',');
                return true;
            }
            return false;

        }
    }
}
