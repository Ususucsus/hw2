using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;

namespace SimpleFTP
{
    /// <summary>
    /// Represents class for handle requests.
    /// </summary>
    public static class RequestHandler
    {
        /// <summary>
        /// Handles request.
        /// </summary>
        /// <remarks>
        /// This method route to specific controller by digit at first position in input line.
        /// If there is no controller for specified digit or digit is not specified calls HandleInvalidCommand controller method.
        /// </remarks>
        /// <param name="stream">Request network stream.</param>
        /// <returns>Task.</returns>
        public static async Task Handle(Stream stream)
        {
            using var reader = new StreamReader(stream, leaveOpen: true);
            await using var writer = new StreamWriter(stream, leaveOpen: true);

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