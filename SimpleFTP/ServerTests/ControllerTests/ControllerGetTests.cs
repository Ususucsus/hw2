using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using SimpleFTP;

namespace ServerTests.ControllerTests
{
    public class ControllerGetTests : ControllerCommandTests
    {
        [Test]
        public async Task GetCommandShouldReturnFileContent()
        {
            var file = File.CreateText("./test/hello.txt");
            await file.WriteAsync("hello");
            await file.FlushAsync();
            file.Close();
            await Controller.GetCommand("./test/hello.txt", Writer);
            var result = await ReadResultAsync();
            Assert.AreEqual("5 hello", result);
        }
        
        [Test]
        public async Task GetCommandShouldReturnEmptyFileContent()
        {
            File.Create("./test/empty.txt").Close();
            await Controller.GetCommand("./test/empty.txt", Writer);
            var result = await ReadResultAsync();
            Assert.AreEqual("0 ", result);
        }

        [Test]
        public async Task GetCommandShouldReturnMinusOneIfFileIsDirectory()
        {
            var file = Directory.CreateDirectory("./test/dir");
            await Controller.GetCommand("./test/dir", Writer);
            var result = await ReadResultAsync();
            Assert.AreEqual("-1", result);
        }
        
        [Test]
        public async Task GetCommandShouldReturnMinusOneIfFileDoesNotExist()
        {
            await Controller.GetCommand("./test/notExist.txt", Writer);
            var result = await ReadResultAsync();
            Assert.AreEqual("-1", result);
        }
    }
}