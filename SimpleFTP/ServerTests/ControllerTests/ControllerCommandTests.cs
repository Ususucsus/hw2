using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ServerTests.ControllerTests
{
    public class ControllerCommandTests
    {
        private MemoryStream stream;
        protected StreamWriter Writer;
        
        [SetUp]
        public void Setup()
        {
            this.stream = new MemoryStream();
            this.Writer = new StreamWriter(this.stream);

            Directory.CreateDirectory("./test");
        }

        [TearDown]
        public void TearDown()
        {
            this.stream.Close();
            this.Writer.Close();
            
            Directory.Delete("./test", true);
        }

        protected async Task<string> ReadResultAsync()
        {
            using var reader = new StreamReader(this.stream);
            this.stream.Seek(0, SeekOrigin.Begin);
            var result = await reader.ReadLineAsync();
            return result;
        }
    }
}