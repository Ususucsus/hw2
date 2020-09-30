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
                    var stream = new NetworkStream(socket);
                    var reader = new StreamReader(stream);
                    var inputData = await reader.ReadLineAsync() ?? string.Empty;

                    var controller = new Controller();
                    var result = await controller.GetResponse(inputData);
                    
                    var writer = new StreamWriter(stream);
                    await writer.WriteAsync(result);
                    await writer.FlushAsync();
                    
                    socket.Close();
                });
            }
        }
    }
}