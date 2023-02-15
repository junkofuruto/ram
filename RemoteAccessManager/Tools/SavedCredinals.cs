using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace RemoteAccessManager.Tools
{
    internal enum CredinalPlacement
    {
        Domain,
        Machine,
        Application,
        None
    }
    internal static class SavedCredinals
    {
        public static void Proceed(string[] command)
        {
            if (command.Length != 2)
            {
                AccessManagerShell.Post("     scred [-v:0|1]\n\tv: checks if credinal is valid (1 - show only valid, 0 - show all)", PostMessageType.None);
                return;
            }
        }
        private static CredinalPlacement CheckCredinal(string username, string password)
        {
            PrincipalContext domainContext = new PrincipalContext(ContextType.Domain);
            PrincipalContext machineContext = new PrincipalContext(ContextType.Machine);
            PrincipalContext appContext = new PrincipalContext(ContextType.ApplicationDirectory);
            if (domainContext.ValidateCredentials(username, password)) return CredinalPlacement.Domain;
            else if (machineContext.ValidateCredentials(username, password)) return CredinalPlacement.Machine;
            else if (appContext.ValidateCredentials(username, password)) return CredinalPlacement.Application;
            else return CredinalPlacement.None;
        }
    }
}
