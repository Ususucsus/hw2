using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Client
{
    /// <summary>
    /// Represents class to communicate with ftp server.
    /// </summary>
    public class FtpServer
    {
        private readonly string ip;
        private readonly int port;
        
        /// <summary>
        /// Initalizes a new instance of the <see cref="FtpServer"/> class.
        /// </summary>
        /// <param name="ip">Ftp server ip.</param>
        /// <param name="port">Ftp server port.</param>
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
            await using var stream = client.GetStream();
            
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

        /// <summary>
        /// Sends get command to ftp server.
        /// </summary>
        /// <param name="path">Path to file on server.</param>
        /// <param name="fileStream">File stream to copy.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Path or FileStream is null.</exception>
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
            await using var networkStream = client.GetStream();
            
            await Send($"2 {path}", networkStream);
            
            using var reader = new StreamReader(networkStream);

            var length = await ReadLong(reader);
            Console.WriteLine(length);

            await CopyToFile(fileStream, reader, length);
        }

        /// <summary>
        /// Copies file from one stream to another.
        /// </summary>
        /// <param name="destinationStream">Stream to copy to.</param>
        /// <param name="reader">Reader of stream to copy from.</param>
        /// <param name="characters">Number of characters in file.</param>
        /// <param name="bufferSize">Buffer size in characters.</param>
        /// <returns></returns>
        private async Task CopyToFile(Stream destinationStream, StreamReader reader, long characters, int bufferSize = 1024)
        {
            var buffer = new char[bufferSize];

            await using var writer = new StreamWriter(destinationStream, leaveOpen: true);

            while (characters > 0)
            {
                var count = Math.Min(bufferSize, (int) Math.Min(characters, int.MaxValue));
                var read = await reader.ReadAsync(buffer, 0, count);
                await writer.WriteAsync(buffer, 0, read);
                await writer.FlushAsync();
                characters -= read;
            }
        }

        /// <summary>
        /// Reads long number from stream.
        /// </summary>
        /// <param name="reader">Reader of stream.</param>
        /// <returns>Long number.</returns>
        private async Task<long> ReadLong(StreamReader reader)
        {
            long number = 0;
            long e = 1;

            var buffer = new char[1];

            while (true)
            {
                await reader.ReadAsync(buffer, 0, 1);
                if (buffer[0] == ' ')
                {
                    break;
                }

                number *= e;
                number += (buffer[0] - '0');
                e *= 10;
            }

            return number;
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