using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleFTP
{
    public class Listener
    {
        private readonly int port;
        
        public Listener(int port)
        {
            this.port = port;
        }

        public async Task Listen()
        {
            var listener = new TcpListener(IPAddress.Any, this.port);
            listener.Start();
            Console.WriteLine($"Listening on {this.port} port");

            while (true)
            {
                var socket = await listener.AcceptSocketAsync();

                Task.Run(async () =>
                {
                    try
                    {
                        await using var stream = new NetworkStream(socket);
                        await RequestHandler.Handle(stream);
                    }
                    finally
                    {
                        socket.Close();
                    }
                });
            }
        }
    }
}