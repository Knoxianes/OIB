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

        public static void AddPermissions(string rolename, string[] permissions)
        {
            string permissionString = string.Empty;
            permissionString = (string)RolesConfigFile.ResourceManager.GetObject(rolename);

            if (permissionString != null) // dodaju se nove permisije
            {
                var reader = new ResXResourceReader(path);
                var node = reader.GetEnumerator();
                var writer = new ResXResourceWriter(path);
                while (node.MoveNext())
                {
                    if (node.Key.ToString().Equals(rolename))
                    {
                        string value = node.Value.ToString();
                        foreach (string prms in permissions)
                        {
                            value += "," + prms;
                        }
                        writer.AddResource(node.Key.ToString(), value);
                    }
                    else
                    {
                        writer.AddResource(node.Key.ToString(), node.Value.ToString());
                    }
                }
                writer.Generate();
                writer.Close();
            }

        }

        
    }
}
