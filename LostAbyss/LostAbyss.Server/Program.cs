using LostAbyss.Shared.Packets;
using System;
using System.IO;
using System.Threading.Tasks;

namespace LostAbyss.Server
{
    static class Program
    {
        static void Main()
        {
            Console.WriteLine("Starting server...");
            var server = new Server(2344);
            Task.WaitAll(server.StartServerAsync());
            Console.WriteLine("Killed server.");
        }
    }
}
