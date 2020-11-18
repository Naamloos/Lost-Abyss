using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LostAbyss.Server
{
    public class Server
    {
        private TcpListener _tcp;
        private int _port;
        private CancellationTokenSource _cts;
        private List<Client> _clients;

        public Server(int port)
        {
            this._tcp = new TcpListener(IPAddress.Any, port);
            this._port = port;
            this._cts = new CancellationTokenSource();
            this._clients = new List<Client>();
        }

        public async Task StartServerAsync()
        {
            // Start tcp listener
            this._tcp.Start();

            // Run while not canceled
            while (!this._cts.IsCancellationRequested)
            {
                // if new client pending
                // accept new client, make client class, add it to connected client list.
                var tcpclient = await this._tcp.AcceptTcpClientAsync();

                var client = new Client(tcpclient);
                this._clients.Add(client);

                _ = Task.Run(client.StartConnectionAsync);

                // Accept new connection with 100ms delay
                await Task.Delay(100);
            }
        }

        public async Task StopServer()
        {
            this._cts.Cancel();
            foreach(var c in _clients)
            {
                await c.DisconnectAsync("Server shutting down.");
            }
        }
    }
}
