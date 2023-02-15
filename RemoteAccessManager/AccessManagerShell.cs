using System;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using RemoteAccessManager.Tools;

namespace RemoteAccessManager
{
    internal enum PostMessageType
    {
        None,
        Info,
        Debug,
        Warning,
        Error
    }

    internal static class AccessManagerShell
    {
        private static bool _parceLoop;

        public static void Start()
        {
            if (ConnectionManager.SqlConnection.State != ConnectionState.Open)
                Post("no connection", PostMessageType.Error);

            Post($"{ConnectionManager.SqlConnection.DataSource}:{ConnectionManager.SqlConnection.ServerVersion} connected", PostMessageType.Info);

            _parceLoop = true;
            while(_parceLoop)
                Parce();
        }
        public static void Parce()
        {
            Console.Write("\n ~>  ");
            string[] command = Console.ReadLine().Split(' ');
            switch (command.ElementAt(0).ToLower())
            {
                case "ping": Ping.Proceed(command); break;
                case "scred": SavedCredinals.Proceed(command); break;
                default: Post("нет такого", PostMessageType.Warning); break;
            }
        }
        public static void Exit()
        {
            _parceLoop = false;
            ConnectionManager.SqlConnection.Close();
            ConnectionManager.SqlConnection.Dispose();

            Console.ReadKey();
            Environment.Exit(0);
        }
        public static void Post(string message, PostMessageType type)
        {
            if (type == PostMessageType.None)
            {
                Console.WriteLine(message);
                return;
            }
            Console.Write($"[{DateTime.Now.Hour.ToString().PadLeft(2, '0')}:" +
                          $"{DateTime.Now.Minute.ToString().PadLeft(2, '0')}:" +
                          $"{DateTime.Now.Second.ToString().PadLeft(2, '0')}." +
                          $"{DateTime.Now.Millisecond.ToString().PadLeft(3, '0')}]");
            Console.Write("[");
            switch (type)
            {
                case PostMessageType.Info: Console.ForegroundColor = ConsoleColor.Green; break;
                case PostMessageType.Debug: Console.ForegroundColor = ConsoleColor.Blue; break;
                case PostMessageType.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
                case PostMessageType.Error: Console.ForegroundColor = ConsoleColor.Red; break;
            }
            Console.Write($"{type.ToString().ToUpper()[0]}");
            Console.ResetColor();
            Console.WriteLine($"] {message}");

            if (type == PostMessageType.Error)
                Exit();
        }
    }
}