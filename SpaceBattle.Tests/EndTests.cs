using Hwdtech;
using Hwdtech.Ioc;
using ShipNamespace;

namespace spacebattletests.StepDefinitions2
{
    public class EndMoveCommandTest
    {
        private readonly Mock<Queue<IComand>> queueMock;
        private readonly Mock<UniversalyObject> target;

        public EndMoveCommandTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            queueMock = new Mock<Queue<IComand>>();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue", (object[] args) => { return queueMock.Object; }).Execute();

            target = new Mock<UniversalyObject>(new Dictionary<string, object>());
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "target", (object[] args) => { return target.Object; }).Execute();

            var TurnCommand = new Mock<IComand>();
            TurnCommand.Setup(x => x.Execute()).Verifiable();
            var commandToInject = new InjectCommand(TurnCommand.Object);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "commandToInject", (object[] args) => { return commandToInject; }).Execute();

        }

        [Fact]
        public void testingEndMoveCommandExecute()
        {

            var queue = new Queue<IComand>();

            queue.Enqueue(IoC.Resolve<IComand>("commandToInject"));

            target.Object.properties.Add("Position", new Vector2d(1, 1));
            target.Object.properties.Add("Velocity", new Vector2d(2, 2));
            target.Object.properties.Add("CommandQueue", queue);

            IoC.Resolve<Queue<IComand>>("Queue").Enqueue(IoC.Resolve<InjectCommand>("commandToInject"));

            var stopOrder = new Mock<IMoveStopOrder>();
            stopOrder.Setup(t => t.target).Returns(IoC.Resolve<UniversalyObject>("target"));
            //stopOrder.Object.target = IoC.Resolve<UniversalyObject>("target");

            new StopMoveCommand(stopOrder.Object).Execute();

            //var movableMock = new Mock<IMovable>();

            //movableMock.SetupProperty(m => m.Position);
            //movableMock.Object.Position = new Vector2d(0, 0);
            //movableMock.SetupGet(m => m.Velocity).Returns(new Vector2d(0, 0));

            //IoC.Resolve<Queue<IComand>>("Queue").Enqueue(new InjectCommand(new MoveCommand(movableMock.Object)));

            //new StopMoveCommand().Execute();

            var command = (InjectCommand)IoC.Resolve<Queue<IComand>>("Queue").Peek();
            command.Execute();
            Assert.IsType<EmptyCommand>(command.InternalCommand);
        }
    }
}
