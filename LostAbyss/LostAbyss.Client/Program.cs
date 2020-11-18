using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace LostAbyss.Client
{
    static class Program
    {
        [STAThread]
        static async Task Main()
        {
            await Task.Delay(2000);
            Console.WriteLine("Cnnecting to server...");
            var conn = new ServerConnection(IPAddress.Parse("127.0.0.1"), 2344);
            _ = Task.Run(conn.StartConnectionAsync);

            while(true)
            {
                await Task.Delay(5000);
                await conn.RequestServerStatusAsync();
            }

            //using (var game = new Game1())
            //    game.Run();
        }
    }
}
