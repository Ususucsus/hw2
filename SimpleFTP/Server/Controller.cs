using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SimpleFTP.Responses;

namespace SimpleFTP
{
    /// <summary>
    /// Represents static class with server commands implementations.
    /// </summary>
    public static class Controller
    {
        /// <summary>
        /// List command -- writes directory files listing to writer stream.
        /// </summary>
        /// <remarks>
        /// Output format: 
        ///     {number of files} ({file name} {is file dir})...
        /// If directory does not exist writes -1.
        /// </remarks>
        /// <param name="path">Path to directory.</param>
        /// <param name="writer">Request network stream writer.</param>
        /// <returns>Task.</returns>
        public static async Task ListCommand(string path, StreamWriter writer)
        {
            if (!Directory.Exists(path))
            {
                await writer.WriteLineAsync("-1");
                await writer.FlushAsync();
                return;
            }

            var files = Directory.GetFiles(path);
            var directories = Directory.GetDirectories(path);

            var response = new ListCommandResponse(
                files.Select(file => new FileResponse(file, false))
                    .Concat(directories.Select(directory => new FileResponse(directory, true)))
                    .ToList()
            );

            await writer.WriteLineAsync(response.ToString());
            await writer.FlushAsync();
        }
        
        /// <summary>
        /// Get command -- writes content of specified file to writer stream.
        /// </summary>
        /// <remarks>
        /// Output format:
        ///     {file length} {file content}
        /// If file does not exist writes -1.
        /// </remarks>
        /// <param name="path">Path to specified file.</param>
        /// <param name="writer">Request network stream writer.</param>
        /// <returns>Task.</returns>
        public static async Task GetCommand(string path, StreamWriter writer)
        {
            if (!File.Exists(path))
            {
                await writer.WriteLineAsync("-1");
                await writer.FlushAsync();
                return;
            }

            await using var stream = File.OpenRead(path);
            await writer.WriteAsync(stream.Length.ToString() + " ");
            await writer.FlushAsync();
            await stream.CopyToAsync(writer.BaseStream);
        }

        /// <summary>
        /// Make response for invalid commands. Writes "invalid command".
        /// </summary>
        /// <param name="writer">Request network stream writer.</param>
        /// <returns>Task.</returns>
        public static async Task HandleInvalidCommand(StreamWriter writer)
        {
            await writer.WriteLineAsync("invalid command");
            await writer.FlushAsync();
        }
    }
}