using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SimpleFTP.Responses;

namespace SimpleFTP
{
    public class Controller
    {
        public async Task<string> GetResponse(string inputData)
        {
            if (inputData.Length == 0) return string.Empty;

            switch (inputData[0])
            {
                case '1':
                {
                    try
                    {
                        var path = inputData.Split()[1];
                        return ListCommand(path);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return string.Empty;
                    }
                }
                case '2':
                {
                    try
                    {
                        var path = inputData.Split()[1];
                        return await GetCommand(path);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        return string.Empty;
                    }
                }
                default:
                    return string.Empty;
            }
        }

        private string ListCommand(string path)
        {
            if (!Directory.Exists(path))
            {
                return "-1";
            }

            var files = Directory.GetFiles(path);
            var directories = Directory.GetDirectories(path);

            var response = new ListCommandResponse(
                files.Select(file => new FileResponse(file, false)).Concat(
                    directories.Select(directory => new FileResponse(directory, true))
                ).ToList()
            );

            return response.ToString();
        }

        private async Task<string> GetCommand(string path)
        {
            if (!File.Exists(path))
            {
                return "-1";
            }
            
            var content = await File.ReadAllBytesAsync(path);
            
            var response = new GetCommandResponse(content);

            return response.ToString();
        }
    }
}