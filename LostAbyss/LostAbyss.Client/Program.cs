using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace LostAbyss.Client
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Thread.Sleep(2000);
            Console.WriteLine("Cnnecting to server...");
            var conn = new ServerConnection(IPAddress.Parse("127.0.0.1"), 2344);
            Task.WaitAll(conn.StartConnectionAsync());

            //using (var game = new Game1())
            //    game.Run();
        }
    }
}
