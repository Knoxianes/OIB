﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecurityManager
{
    public class Formatter
    {
        
        // Returns username based on the Windows Logon Name. 
        public static string ParseName(string winLogonName)
        {
            string[] parts = new string[] { };

            if (winLogonName.Contains("@"))
            {
                ///UPN format
                parts = winLogonName.Split('@');
                return parts[0];
            }
            else if (winLogonName.Contains("\\"))
            {
                /// SPN format
                parts = winLogonName.Split('\\');
                return parts[1];
            }
            else
            {
                return winLogonName;
            }
        }
    }
}
