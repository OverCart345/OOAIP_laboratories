using Hwdtech;
using Hwdtech.Ioc;
using ShipNamespace;

namespace spacebattletests.StepDefinitions2
{
    public class EndMoveCommandTest
    {
        private readonly Mock<Queue<IComand>> queueMock;

        public EndMoveCommandTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            queueMock = new Mock<Queue<IComand>>();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue", (object[] args) => { return queueMock.Object; }).Execute();
        }

        [Fact]
        public void testingEndMoveCommandExecute()
        {
            var movableMock = new Mock<IMovable>();

            movableMock.SetupProperty(m => m.Position);
            movableMock.Object.Position = new Vector2d(0, 0);
            movableMock.SetupGet(m => m.Velocity).Returns(new Vector2d(0, 0));

            IoC.Resolve<Queue<IComand>>("Queue").Enqueue(new InjectCommand(new MoveCommand(movableMock.Object)));

            new StopMoveCommand().Execute();

            var command = (InjectCommand)IoC.Resolve<Queue<IComand>>("Queue").Peek();
            command.Execute();
            Assert.IsType<EmptyCommand>(command.InternalCommand);
        }
    }
}
