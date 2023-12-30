using Hwdtech;
using Hwdtech.Ioc;
using ShipNamespace;

namespace SpaceBattle.Tests
{
    public class longOperationTests
    {

        private readonly Mock<Queue<IComand>> queueMock;
        private readonly Mock<UniversalyObject> spaceship;
        private readonly Mock<IComand> TurnCommand;
        private readonly Mock<IComand> MoveCommand;

        public longOperationTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            queueMock = new Mock<Queue<IComand>>();
            spaceship = new Mock<UniversalyObject>(new Dictionary<string, object>());

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Queue", (object[] args) => { return queueMock.Object; }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Spaceship", (object[] args) => spaceship.Object).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StartCommand", (object[] args) => new StartCommand((Order)args[0])).Execute();

            TurnCommand = new Mock<IComand>();
            TurnCommand.Setup(x => x.Execute()).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Config." + "TurnCommand", (object[] args) => TurnCommand.Object).Execute();

            MoveCommand = new Mock<IComand>();
            MoveCommand.Setup(x => x.Execute()).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Config." + "MoveCommand", (object[] args) => MoveCommand.Object).Execute();
        }

        [Fact]
        public void LongOperationWithOneCommand()
        {
            spaceship.Object.properties.Add("Position", new Vector2d(0, 0));
            spaceship.Object.properties.Add("Velocity", new Vector2d(0, 0));
            spaceship.Object.properties.Add("Angle", new Turn(0));
            spaceship.Object.properties.Add("AngleSpeed", new Turn(0));

            string[] commandNames = { "MoveCommand" };

            var order = new Mock<Order>();
            order.Setup(o => o.Target).Returns(IoC.Resolve<UniversalyObject>("Spaceship"));
            order.Setup(o => o.properties).Returns(new Dictionary<string, object> { { "AngleSpeed", new Turn(13) } });

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateMacro", (object[] args) => new MacroCommand(new MacroCreate((string[])args[0]).Create())).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "LongOperation", (object[] args) => { return new LongOperationStrategy().Strat(args[0], args[1]); }).Execute();

            IoC.Resolve<StartCommand>("LongOperation", commandNames, order.Object).Execute();

            IoC.Resolve<Queue<IComand>>("Queue").Dequeue().Execute();

            MoveCommand.Verify(m => m.Execute(), Times.Once());
            Assert.Equal(new Turn(13), spaceship.Object.properties["AngleSpeed"]);
        }

        [Fact]
        public void LongOperationWithTwoCommand()
        {
            spaceship.Object.properties.Add("Position", new Vector2d(0, 0));
            spaceship.Object.properties.Add("Velocity", new Vector2d(0, 0));
            spaceship.Object.properties.Add("Angle", new Turn(0));
            spaceship.Object.properties.Add("AngleSpeed", new Turn(0));

            string[] commandNames = { "MoveCommand", "TurnCommand" };

            var order = new Mock<Order>();
            order.Setup(o => o.Target).Returns(IoC.Resolve<UniversalyObject>("Spaceship"));
            order.Setup(o => o.properties).Returns(new Dictionary<string, object> { { "AngleSpeed", new Turn(13) }, { "Velocity", new Vector2d(3, 5) } });

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateMacro", (object[] args) => new MacroCommand(new MacroCreate((string[])args[0]).Create())).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "LongOperation", (object[] args) => { return new LongOperationStrategy().Strat(args[0], args[1]); }).Execute();

            IoC.Resolve<StartCommand>("LongOperation", commandNames, order.Object).Execute();

            IoC.Resolve<Queue<IComand>>("Queue").Dequeue().Execute();

            TurnCommand.Verify(t => t.Execute(), Times.Once());
            MoveCommand.Verify(m => m.Execute(), Times.Once());
            Assert.Equal(new Vector2d(3, 5), spaceship.Object.properties["Velocity"]);
            Assert.Equal(new Turn(13), spaceship.Object.properties["AngleSpeed"]);
        }

        [Fact]
        public void LongOperationWithZeroCommand()
        {
            spaceship.Object.properties.Add("Position", new Vector2d(0, 0));
            spaceship.Object.properties.Add("Velocity", new Vector2d(0, 0));
            spaceship.Object.properties.Add("Angle", new Turn(0));
            spaceship.Object.properties.Add("AngleSpeed", new Turn(0));

            string[] commandNames = { "UnknownCommand" };

            var order = new Mock<Order>();
            order.Setup(o => o.Target).Returns(IoC.Resolve<UniversalyObject>("Spaceship"));
            order.Setup(o => o.properties).Returns(new Dictionary<string, object> { { "AngleSpeed", new Turn(13) }, { "Velocity", new Vector2d(3, 5) } });

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateMacro", (object[] args) => new MacroCommand(new MacroCreate((string[])args[0]).Create())).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "LongOperation", (object[] args) => { return new LongOperationStrategy().Strat(args[0], args[1]); }).Execute();

            Assert.Throws<ArgumentException>(() => IoC.Resolve<StartCommand>("LongOperation", commandNames, order.Object).Execute());
        }
    }
}
