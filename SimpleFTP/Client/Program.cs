using System;
using System.IO;
using System.Threading.Tasks;

namespace Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var ftp = new FtpServer("localhost", 6666);
            //Console.WriteLine(await ftp.List("./test"));
            var file = File.CreateText("T.txt");
            await ftp.Get("./test/test.txt", file.BaseStream);
            file.Close();
        }
    }
}