using System;
using System.Threading;

namespace LostAbyss.Debug
{
    class Program
    {
        static Thread server;
        static Thread client;
        static void Main()
        {
            Console.WriteLine("Hello World!");
            server = new Thread(new ThreadStart(StartServer));
            client = new Thread(new ThreadStart(StartClient));
            server.Start();
            client.Start();
            while(true)
            { }
        }

        static void StartServer()
        {
            LostAbyss.Server.Program.Main();
        }

        static void StartClient()
        {
            LostAbyss.Client.Program.Main();
        }
    }
}
