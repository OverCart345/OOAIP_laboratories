using Hwdtech;
using Hwdtech.Ioc;
using ShipNamespace;

namespace SpaceBattle.Tests
{
    public class longOperationTests
    {

        private readonly Mock<Queue<IComand>> queueMock;
        private readonly Mock<UniversalyObject> spaceship;

        public longOperationTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            queueMock = new Mock<Queue<IComand>>();
            spaceship = new Mock<UniversalyObject>(new Dictionary<string, object>());

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue", (object[] args) => { return queueMock.Object; }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Spaceship", (object[] args) => spaceship.Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartCommand", (object[] args) => new StartCommand((Order)args[0])).Execute();
        }

        [Fact]
        public void Strat_CreatesLongRunningOperation_WithMovementAndTurnCommands()
        {
            var moveCommand = new Mock<IComand>();
            moveCommand.Setup(x => x.Execute()).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Config." + "MoveCommand", (object[] args) => moveCommand.Object).Execute();

            var queue = new Queue<IComand>();

            spaceship.Object.properties.Add("Position", new Vector2d(0, 0));
            spaceship.Object.properties.Add("Velocity", new Vector2d(0, 0));
            spaceship.Object.properties.Add("CommandQueue", queue);

            var commandName = "MoveCommand";

            var order = new Mock<Order>();
            order.Setup(o => o.Target).Returns(IoC.Resolve<UniversalyObject>("Spaceship"));
            order.Setup(o => o.properties).Returns(new Dictionary<string, object> { { "Velocity", new Vector2d(2, 2) } });

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "LongOperation" + commandName, (object[] args) => { return new LongOperationStrategy(commandName, (Order)args[0]).Strat(); }).Execute();
            IoC.Resolve<IComand>("LongOperation" + commandName, order.Object).Execute();

            queueMock.Object.Dequeue().Execute();
            moveCommand.Verify(x => x.Execute(), Times.Exactly(1));
        }

        [Fact]
        public void turn()
        {
            var TurnCommand = new Mock<IComand>();
            TurnCommand.Setup(x => x.Execute()).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Config." + "TurnCommand", (object[] args) => TurnCommand.Object).Execute();

            var queue = new Queue<IComand>();

            spaceship.Object.properties.Add("Position", new Vector2d(0, 0));
            spaceship.Object.properties.Add("Velocity", new Vector2d(0, 0));
            spaceship.Object.properties.Add("Angle", new Turn(0));
            spaceship.Object.properties.Add("AngleSpeed", new Turn(0));
            spaceship.Object.properties.Add("CommandQueue", queue);

            var commandName = "TurnCommand";

            var order = new Mock<Order>();
            order.Setup(o => o.Target).Returns(IoC.Resolve<UniversalyObject>("Spaceship"));
            order.Setup(o => o.properties).Returns(new Dictionary<string, object> { { "AngleSpeed", new Turn(13) } });

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "LongOperation" + commandName, (object[] args) => { return new LongOperationStrategy(commandName, (Order)args[0]).Strat(); }).Execute();
            IoC.Resolve<IComand>("LongOperation" + commandName, order.Object).Execute();

            queueMock.Object.Dequeue().Execute();
            TurnCommand.Verify(x => x.Execute(), Times.Exactly(1));
        }
    }
}
