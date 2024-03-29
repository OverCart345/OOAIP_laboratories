using ShipNamespace;
namespace SpaceBattle.Tests
{
    public class ExceptionHandlerTest
    {
        [Fact]
        public void Execute_LogsExceptionToFile()
        {
            var path = "../../../CaughtErrors.txt";
            var exceptionMessage = "StopCommandException";

            var command = new ExceptionHandler(path, exceptionMessage);
            command.Execute();

            Assert.Contains(exceptionMessage, File.ReadAllText(path));
        }
    }
}
