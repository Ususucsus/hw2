using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleFTP
{
    public static class RequestHandler
    {
        public static async Task Handle(NetworkStream stream)
        {
            using var reader = new StreamReader(stream);
            await using var writer = new StreamWriter(stream);

            var inputData = await reader.ReadLineAsync() ?? string.Empty;

            if (inputData.Length == 0)
            {
                await Controller.HandleInvalidCommand(writer);
                return;
            }

            switch (inputData[0])
            {
                case '1':
                {
                    try
                    {
                        var path = inputData.Split()[1];
                        await Controller.ListCommand(path, writer);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        await Controller.HandleInvalidCommand(writer);
                    }

                    break;
                }
                case '2':
                {
                    try
                    {
                        var path = inputData.Split()[1];
                        await Controller.GetCommand(path, writer);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        await Controller.HandleInvalidCommand(writer);
                    }

                    break;
                }
                default:
                {
                    await Controller.HandleInvalidCommand(writer);
                    break;
                }
            }
        }
    }
}