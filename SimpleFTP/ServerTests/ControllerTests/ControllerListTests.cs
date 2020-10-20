using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using SimpleFTP;

namespace ServerTests.ControllerTests
{
    public class ControllerListTests : ControllerCommandTests
    {
        private bool ComparePaths(string path1, string path2)
        {
            return string.Compare(
                Path.GetFullPath(path1).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                Path.GetFullPath(path2).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                StringComparison.InvariantCultureIgnoreCase) == 0;
        }

        [Test]
        public async Task ListCommandShouldWorkCorrect()
        {
            Directory.CreateDirectory("./test/dir1");
            File.Create("./test/file1").Close();
            
            await Controller.ListCommand("./test", this.Writer);
            var result = await ReadResultAsync();

            var resultSplit = result.Split();
            
            Assert.AreEqual(5, resultSplit.Length);
            
            var count = resultSplit[0];
            var file1 = resultSplit[1];
            var isDir1 = resultSplit[2];
            var file2 = resultSplit[3];
            var isDir2 = resultSplit[4];

            Assert.AreEqual("2", count);

            Assert.That((ComparePaths("./test/dir1", file1) && isDir1 == "True") ||
                        (ComparePaths("./test/file1", file1) && isDir1 == "False"));
            Assert.That((ComparePaths("./test/dir1", file2) && isDir2 == "True") ||
                        (ComparePaths("./test/file1", file2) && isDir2 == "False"));
        }

        [Test]
        public async Task ListCommandShouldReturnZeroIfDirectoryIsEmpty()
        {
            await Controller.ListCommand("./test", this.Writer);
            var result = await ReadResultAsync();
            
            Assert.AreEqual("0 ", result);
        }

        [Test]
        public async Task ListCommandShouldReturnMinusOneIfFolderDoesNotExist()
        {
            await Controller.ListCommand("./testtest", this.Writer);
            var result = await ReadResultAsync();
            
            Assert.AreEqual("-1", result);
        }

        [Test]
        public async Task ListCommandShouldReturnMinusOneIfFolderIsFile()
        {
            File.Create("./test/file.txt").Close();
            await Controller.ListCommand("./test/file.txt", this.Writer);
            var result = await ReadResultAsync();
            
            Assert.AreEqual("-1", result);
        }
    }
}