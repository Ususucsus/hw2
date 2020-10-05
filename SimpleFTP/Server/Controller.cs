using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SimpleFTP.Responses;

namespace SimpleFTP
{
    public static class Controller
    {
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

        public static async Task GetCommand(string path, StreamWriter writer)
        {
            if (!File.Exists(path))
            {
                await writer.WriteLineAsync("-1");
                await writer.FlushAsync();
                return;
            }

            var stream = File.OpenRead(path);
            await stream.CopyToAsync(writer.BaseStream);
        }

        public static async Task HandleInvalidCommand(StreamWriter writer)
        {
            await writer.WriteLineAsync("invalid command");
            await writer.FlushAsync();
        }
    }
}