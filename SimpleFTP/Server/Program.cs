using System.Threading.Tasks;

namespace SimpleFTP
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var listener = new Listener(6666);
            await listener.Listen();
        }
    }
}