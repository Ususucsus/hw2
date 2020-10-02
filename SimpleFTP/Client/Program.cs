using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            using var client = new TcpClient("localhost", 6666);
            var stream = client.GetStream();

            while (true)
            {
                Console.Write("> ");
                var command = Console.ReadLine();

                if (command == "exit")
                {
                    break;
                }
                
                await Send(command, stream);
                var response = await Read(stream);
                Console.WriteLine($"Server: {response}");
            }
            
            client.Close();
        }

        private static async Task Send(string message, NetworkStream stream)
        {
            await using var writer = new StreamWriter(stream, leaveOpen: true);
            await writer.WriteLineAsync(message);
            await writer.FlushAsync();
        }

        private static async Task<string> Read(NetworkStream stream)
        {
            using var reader = new StreamReader(stream, leaveOpen: true);
            return await reader.ReadLineAsync();
        }
    }
}