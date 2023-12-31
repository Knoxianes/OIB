﻿using CertificateManager;
using Common;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using SecurityManager;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Diagnostics;

namespace MainComponent
{
    public class WCFServis : ChannelFactory<ILogger>, IDisposable
    {
       public static ILogger factory;

        public WCFServis(NetTcpBinding binding, EndpointAddress address)
            : base(binding, address)
        {
            /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
			string cltCertCN = CertificateManager.Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate = Manager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);

            factory = this.CreateChannel();
        }
        public void TestCommunication()
        {
            try
            {
                factory.TestCommunication();
            }
            catch (Exception e)
            {
                Console.WriteLine("[TestCommunication] ERROR = {0}", e.Message);
            }
        }

        public void WriteLog(Process badProcess, int count)
        {
            try
            {
                if(count >= 5)
                {
                    Alarm newAlaram = new Alarm()
                    {
                        Pname = badProcess.ProcessName,
                        UtLVL = UtilityLVL.Critical,
                        DateTime = DateTime.Now
                    };
                    factory.WriteEvent(newAlaram, "Bad process");

                }else if (count >= 4)
                {
                    Alarm newAlaram = new Alarm()
                    {
                        Pname = badProcess.ProcessName,
                        UtLVL = UtilityLVL.Warning,
                        DateTime = DateTime.Now
                    };
                    factory.WriteEvent(newAlaram, "Bad process");

                }
                else if(count >= 2)
                {
                    Alarm newAlaram = new Alarm()
                    {
                        Pname = badProcess.ProcessName,
                        UtLVL = UtilityLVL.Information,
                        DateTime = DateTime.Now
                    };
                    factory.WriteEvent(newAlaram, "Bad process");
                }

            }catch(Exception ex)
            {
                Console.WriteLine("Error while writting log for bad process! ", ex.Message);
            }
        }
        
        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }

            this.Close();
        }
    }
}
