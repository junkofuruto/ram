using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;

namespace RemoteAccessManager.Tools
{
    internal static class Ping
    {
        public static void Proceed(string[] command)
        {
            List<string> computers = new List<string>();
            if (command.Length != 2)
            {
                AccessManagerShell.Post("     ping [computername|all]", PostMessageType.None);
                return;
            }
            if (command.Contains("all"))
            {
                SqlCommand cmd = ConnectionManager.SqlConnection.CreateCommand();
                cmd.CommandText = @"SELECT [name] FROM [dbo].[computers]";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    computers.Add(reader.GetString(0));
                reader.Close();
            }
            else
                computers.Add(command[1]);

            foreach (string el in computers)
            {
                if (PingHost(el))
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\t" + (PingHost(el) ? "V" : "X"));
                Console.ResetColor();
                Console.WriteLine($"\t{el}");
            }
        }
        private static bool PingHost(string name)
        {
            bool pingable = false;
            System.Net.NetworkInformation.Ping pinger = null;

            try
            {
                pinger = new System.Net.NetworkInformation.Ping();
                PingReply reply = pinger.Send(name);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                    pinger.Dispose();
            }

            return pingable;
        }
    }
}
