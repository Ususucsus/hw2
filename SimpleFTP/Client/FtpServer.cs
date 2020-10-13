using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Client
{
    public class FtpServer
    {
        private readonly string ip;
        private readonly int port;
        
        public FtpServer(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        /// <summary>
        /// Sends list command to ftp server.
        /// </summary>
        /// <param name="path">Path to folder.</param>
        /// <returns>List command response.</returns>
        /// <exception cref="ArgumentNullException">Path is null.</exception>
        /// <exception cref="InvalidOperationException">Invalid path to folder.</exception>
        public async Task<ListResponse> List(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            
            using var client = new TcpClient(ip, port);
            var stream = client.GetStream();
            
            await Send($"1 {path}", stream);
            
            var response = await Read(stream);

            if (response == "-1")
            {
                throw new InvalidOperationException();
            }

            var split = response.Split();
            var filesSplit = split[1..];
            var numberOfFiles = int.Parse(split[0]);
            
            var files = new List<FileResponse>();
            for (var i = 0; i < filesSplit.Length; i+=2)
            {
                files.Add(new FileResponse(filesSplit[i], filesSplit[i + 1] == "True"));
            }

            return new ListResponse(files, numberOfFiles);
        }

        public async Task Get(string path, Stream fileStream)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (fileStream == null)
            {
                throw new ArgumentNullException(nameof(fileStream));
            }
            
            using var client = new TcpClient(ip, port);
            var networkStream = client.GetStream();

            await Send($"2 {path}", networkStream);
            
            // using var reader = new StreamReader(networkStream, leaveOpen: true);
            // var data = await reader.ReadToEndAsync();
            
            // var buffer = new char[32];
            // await reader.ReadAsync(buffer, 0, 1);
            // var length = int.Parse(string.Join("", buffer).Split(" ")[0]);
            // Console.WriteLine(length);

            await CopyToFile(fileStream, networkStream, 32);
        }

        private async Task CopyToFile(Stream destinationStream, Stream originalStream, int characters, int bufferSize = 1024)
        {
            var buffer = new char[bufferSize];

            using var reader = new StreamReader(originalStream, leaveOpen: true);
            await using var writer = new StreamWriter(destinationStream, leaveOpen: true);

            while (characters > 0)
            {
                var read = await reader.ReadAsync(buffer, 0, Math.Min(bufferSize, characters));
                await writer.WriteAsync(buffer, 0, read);
                await writer.FlushAsync();
                characters -= read;
            }
        }
        
        private async Task Send(string message, Stream stream)
        {
            await using var writer = new StreamWriter(stream, leaveOpen: true);
            await writer.WriteLineAsync(message);
            await writer.FlushAsync();
        }

        private async Task<string> Read(Stream stream)
        {
            using var reader = new StreamReader(stream, leaveOpen: true);
            return await reader.ReadLineAsync();
        }
    }
}