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

        }

        [Fact]
        public void testingEndMoveCommandExecute()
        {

            var queue = new Queue<IComand>();

            var TurnCommand = new Mock<IComand>();
            TurnCommand.Setup(x => x.Execute()).Verifiable();
            var commandToInject = new InjectCommand(TurnCommand.Object);

            target.Object.properties.Add("Position", new Vector2d(1, 1));
            target.Object.properties.Add("Velocity", new Vector2d(2, 2));
            target.Object.properties.Add("Command", commandToInject);

            IoC.Resolve<Queue<IComand>>("Queue").Enqueue(commandToInject);

            var stopOrder = new Mock<IMoveStopOrder>();
            stopOrder.Setup(t => t.target).Returns(IoC.Resolve<UniversalyObject>("target"));

            new StopMoveCommand(stopOrder.Object).Execute();

            var command = (InjectCommand)IoC.Resolve<Queue<IComand>>("Queue").Peek();
            command.Execute();
            Assert.IsType<EmptyCommand>(command.InternalCommand);
        }
    }
}
