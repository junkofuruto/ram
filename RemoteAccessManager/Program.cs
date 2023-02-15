using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteAccessManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AccessManagerShell.Start();
            AccessManagerShell.Exit();
        }
    }
}
