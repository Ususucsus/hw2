using System;
using System.IO;
using System.Threading.Tasks;

namespace Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("enter command");
            var command = Console.ReadLine();

            switch (command)
            {
                case "list":
                {
                    Console.WriteLine("enter path");
                    var path = Console.ReadLine();
                    var ftp = new FtpServer("localhost", 6666);
                    Console.WriteLine(await ftp.List(path));
                    break;
                }
                case "get":
                {
                    Console.WriteLine("enter server path");
                    var path = Console.ReadLine();
                    Console.WriteLine("enter file path");
                    var filePath = Console.ReadLine();
                    var ftp = new FtpServer("localhost", 6666);
                    var file = File.Create(filePath);
                    await ftp.Get(path, file);
                    file.Close();
                    break;
                }
                default:
                    Console.WriteLine("invalid command");
                    break;
            }
        }
    }
}