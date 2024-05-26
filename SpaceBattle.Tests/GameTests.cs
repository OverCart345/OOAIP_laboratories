using Hwdtech;
using Hwdtech.Ioc;
using ShipNamespace;

namespace SpaceBattle.Tests
{
    public class GameTests
    {
        public GameTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
                IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
            ).Execute();
        }

        [Fact]
        public void GameCommandComplete()
        {
            var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

            var mockStrategy = new Mock<IStrategy>();
            mockStrategy.Setup(x => x.Invoke()).Returns(new TimeSpan(0, 0, 1));

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => mockStrategy.Object.Invoke(args)).Execute();

            var queue = new Queue<IComand>();
            queue.Enqueue(new Mock<IComand>().Object);
            queue.Enqueue(new Mock<IComand>().Object);

            var gameCommand = new GameCommand(scope, queue);
            gameCommand.Execute();

            Assert.True(queue.Count == 0);
        }

        [Fact]
        public void GameCommandExetion()
        {
            var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

            var mockStrategy = new Mock<IStrategy>();
            mockStrategy.Setup(x => x.Invoke()).Returns(new TimeSpan(0, 0, 1));

            var mockHandler = new Mock<IHandler>();
            mockHandler.Setup(x => x.Handle()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => mockStrategy.Object.Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler.Find", (object[] args) => mockHandler.Object).Execute();

            var queue = new Queue<IComand>();
            var mockCommand = new Mock<IComand>();
            mockCommand.Setup(x => x.Execute()).Throws(new Exception());
            queue.Enqueue(mockCommand.Object);

            var gameCommand = new GameCommand(scope, queue);
            gameCommand.Execute();

            mockHandler.Verify(x => x.Handle(), Times.Once);
        }

        [Fact]
        public void GameCommandDefault()
        {
            var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

            var mockStrategy = new Mock<IStrategy>();
            mockStrategy.Setup(x => x.Invoke()).Returns(new TimeSpan(0, 0, 1));

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => mockStrategy.Object.Invoke(args)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler.Find", (object[] args) => new DefaultHandler((Exception)args[1])).Execute();

            var queue = new Queue<IComand>();
            var mockCommand = new Mock<IComand>();
            mockCommand.Setup(x => x.Execute()).Throws(new Exception());
            queue.Enqueue(mockCommand.Object);

            var gameCommand = new GameCommand(scope, queue);
            Assert.Throws<Exception>(() => gameCommand.Execute());
        }

        [Fact]
        public void GameCommandExecution()
        {
            var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

            var mockStrategy = new Mock<IStrategy>();
            mockStrategy.Setup(x => x.Invoke()).Returns(new TimeSpan(0, 0, 2));

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => mockStrategy.Object.Invoke(args)).Execute();

            var mockCommand = new Mock<IComand>();
            mockCommand.Setup(x => x.Execute()).Callback(() => System.Threading.Thread.Sleep(500));

            var mockCommand2 = new Mock<IComand>();
            mockCommand2.Setup(x => x.Execute()).Callback(() => System.Threading.Thread.Sleep(500));

            var queue = new Queue<IComand>();
            queue.Enqueue(mockCommand.Object);
            queue.Enqueue(mockCommand2.Object);

            var gameCommand = new GameCommand(scope, queue);
            gameCommand.Execute();

            Assert.True(queue.Count == 0);
        }

        [Fact]
        public void GameCommandExecutionTwo()
        {
            var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

            var mockStrategy = new Mock<IStrategy>();
            mockStrategy.Setup(x => x.Invoke()).Returns(new TimeSpan(0, 0, 0));

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.Time.Quantum", (object[] args) => mockStrategy.Object.Invoke(args)).Execute();

            var mockCommand = new Mock<IComand>();
            mockCommand.Setup(x => x.Execute()).Callback(() => System.Threading.Thread.Sleep(500));

            var mockCommand2 = new Mock<IComand>();
            mockCommand2.Setup(x => x.Execute()).Callback(() => System.Threading.Thread.Sleep(1500));

            var queue = new Queue<IComand>();
            queue.Enqueue(mockCommand.Object);
            queue.Enqueue(mockCommand2.Object);

            var gameCommand = new GameCommand(scope, queue);
            gameCommand.Execute();

            Assert.True(queue.Count == 2);
        }
    }
}
