using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Client;
using Microsoft.VisualBasic.FileIO;
using NUnit.Framework;
using SimpleFTP;

namespace ClientTests
{
    public class FtpServerTests
    {
        private FtpServer ftp;
        
        [SetUp]
        public void Setup()
        {
            var listener = new Listener(6666);
            listener.Listen();
            
            ftp = new FtpServer("localhost", 6666);

            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "test"));
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(Path.Combine(Directory.GetCurrentDirectory(), "test"), true);
        }

        [Test]
        public async Task FtpServerListCommandShouldWork()
        {
            var expectedResult = new ListResponse(new List<FileResponse>()
            {
                new FileResponse(Path.Combine(Directory.GetCurrentDirectory(), "test", "file.txt"), false),
                new FileResponse(Path.Combine(Directory.GetCurrentDirectory(), "test", "dir"), true)
            }, 2);
            
            File.Create(Path.Combine(Directory.GetCurrentDirectory(), "test", "file.txt")).Close();
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "test", "dir"));
            
            var result = await ftp.List(Path.Combine(Directory.GetCurrentDirectory(), "test"));
            
            Assert.NotNull(result);
            Assert.AreEqual(expectedResult.NumberOfFiles, result.NumberOfFiles);
            Assert.That(
                (result.Files[0].FileName == Path.Combine(Directory.GetCurrentDirectory(), "test", "file.txt") &&
                 result.Files[0].IsDirectory == false) ||
                (result.Files[0].FileName == Path.Combine(Directory.GetCurrentDirectory(), "test", "dir") &&
                 result.Files[0].IsDirectory == true));
            Assert.That(
                (result.Files[1].FileName == Path.Combine(Directory.GetCurrentDirectory(), "test", "file.txt") &&
                 result.Files[1].IsDirectory == false) ||
                (result.Files[1].FileName == Path.Combine(Directory.GetCurrentDirectory(), "test", "dir") &&
                 result.Files[1].IsDirectory == true));
            Assert.That(result.Files[0].FileName != result.Files[1].FileName);
        }
    }
}