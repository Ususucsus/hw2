using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SimpleFTP;

namespace ServerTests
{
    public class RequestHandlerTests
    {
        private MemoryStream stream;

        [SetUp]
        public void Setup()
        {
            stream = new MemoryStream();

            Directory.CreateDirectory("./test");
        }

        [TearDown]
        public void TearDown()
        {
            stream.Close();
            
            Directory.Delete("./test", true);
        }

        [Test]
        public async Task RouteToListCommandTest()
        {
            File.Create("./test/file1.txt").Close();
            File.Create("./test/file2.txt").Close();
            File.Create("./test/file3.txt").Close();
            
            await using var writer = new StreamWriter(stream);
            await writer.WriteLineAsync("1 ./test");
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            await RequestHandler.Handle(stream);

            stream.Seek("1 ./test".Length + Environment.NewLine.Length, SeekOrigin.Begin);
            using var reader = new StreamReader(stream);
            var result = await reader.ReadLineAsync();
            
            Assert.NotNull(result);
            Assert.AreEqual('3', result[0]);
        }

        [Test]
        public async Task RouteToGetCommandTest()
        {
            var file = File.CreateText("./test/hello.txt");
            await file.WriteAsync("hello");
            await file.FlushAsync();
            file.Close();

            await using var writer = new StreamWriter(stream);
            await writer.WriteLineAsync("2 ./test/hello.txt");
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            await RequestHandler.Handle(stream);

            stream.Seek("2 ./test/hello.txt".Length + Environment.NewLine.Length, SeekOrigin.Begin);
            using var reader = new StreamReader(stream);
            var buffer = new char[256];
            await reader.ReadAsync(buffer, 0, 256);
            
            Assert.AreEqual('5', buffer[0]);
            Assert.AreEqual("hello", string.Join("", buffer.Skip(2).Take(5)));
        }

        [Test]
        public async Task RouteToUnknownCommandShouldReturnErrorMessage()
        {
            await using var writer = new StreamWriter(stream);
            await writer.WriteLineAsync("3 ./test");
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            await RequestHandler.Handle(stream);

            stream.Seek("3 ./test".Length + Environment.NewLine.Length, SeekOrigin.Begin);
            using var reader = new StreamReader(stream);
            var result = await reader.ReadLineAsync();
            
            Assert.NotNull(result);
            Assert.AreEqual("invalid command", result);
        }

        [Test]
        public async Task RouteToGetCommandWithoutPathShouldReturnErrorMessage()
        {
            await using var writer = new StreamWriter(stream);
            await writer.WriteLineAsync("1");
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            await RequestHandler.Handle(stream);

            stream.Seek("1".Length + Environment.NewLine.Length, SeekOrigin.Begin);
            using var reader = new StreamReader(stream);
            var result = await reader.ReadLineAsync();
            
            Assert.NotNull(result);
            Assert.AreEqual("invalid command", result);
        }
        
        [Test]
        public async Task RouteToListCommandWithoutPathShouldReturnErrorMessage()
        {
            await using var writer = new StreamWriter(stream);
            await writer.WriteLineAsync("2");
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            await RequestHandler.Handle(stream);

            stream.Seek("2".Length + Environment.NewLine.Length, SeekOrigin.Begin);
            using var reader = new StreamReader(stream);
            var result = await reader.ReadLineAsync();
            
            Assert.NotNull(result);
            Assert.AreEqual("invalid command", result);
        }
        
        [Test]
        public async Task EmptyCommandShouldReturnErrorMessage()
        {
            await using var writer = new StreamWriter(stream);
            await writer.WriteLineAsync(string.Empty);
            await writer.FlushAsync();
            stream.Seek(0, SeekOrigin.Begin);

            await RequestHandler.Handle(stream);

            stream.Seek(Environment.NewLine.Length, SeekOrigin.Begin);
            using var reader = new StreamReader(stream);
            var result = await reader.ReadLineAsync();
            
            Assert.NotNull(result);
            Assert.AreEqual("invalid command", result);
        }
    }
}