using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleFTP
{
    /// <summary>
    /// Represents tcp server listener.
    /// </summary>
    public class Listener
    {
        /// <summary>
        /// Port to listen.
        /// </summary>
        private readonly int port;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Listen"/> class.
        /// </summary>
        /// <param name="port">Port to listen.</param>
        public Listener(int port)
        {
            this.port = port;
        }

        /// <summary>
        /// Listen tcp connections on any ip address.
        /// </summary>
        /// <returns>Task.</returns>
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