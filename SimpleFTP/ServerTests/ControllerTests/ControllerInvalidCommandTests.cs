using System.Threading.Tasks;
using NUnit.Framework;
using SimpleFTP;

namespace ServerTests.ControllerTests
{
    public class ControllerInvalidCommandTests : ControllerCommandTests
    {
        [Test]
        public async Task HandleInvalidCommandShouldReturnErrorString()
        {
            await Controller.HandleInvalidCommand(Writer);
            var result = await ReadResultAsync();
            Assert.AreEqual("invalid command", result);
        }
    }
}