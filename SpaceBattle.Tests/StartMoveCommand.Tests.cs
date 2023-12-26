using Hwdtech;
using Hwdtech.Ioc;
using ShipNamespace;

namespace spacebattletests.StepDefinitions
{
    public class StartMoveCommandTest
    {
        private readonly Mock<Queue<IComand>> queueMock;
        private readonly Mock<UniversalyObject> spaceship;

        public StartMoveCommandTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            queueMock = new Mock<Queue<IComand>>();
            spaceship = new Mock<UniversalyObject>(new Dictionary<string, object>());
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue", (object[] args) => { return queueMock.Object; }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Spaceship", (object[] args) => spaceship.Object).Execute();
        }

        [Fact]
        public void testingStartCommand()
        {
            spaceship.Object.properties.Add("Position", new Vector2d(0, 0));
            spaceship.Object.properties.Add("Velocity", new Vector2d(0, 0));

            var Order = new Mock<Order>();

            Order.Setup(o => o.Target).Returns(IoC.Resolve<UniversalyObject>("Spaceship"));
            Order.Setup(o => o.Command).Returns("MoveCommand");
            Order.Setup(o => o.Velocity).Returns(new Vector2d(1, 1));

            new StartCommand(Order.Object).Execute();

            var command = (InjectCommand)IoC.Resolve<Queue<IComand>>("Queue").Peek();
            Assert.IsType<MoveCommand>(command.InternalCommand);
        }
    }
}
