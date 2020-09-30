using System;
using System.Threading.Tasks;

namespace SimpleFTP
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var listener = new Listener(6666);
            await listener.Listen();
        }
    }
}