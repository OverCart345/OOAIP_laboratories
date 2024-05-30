namespace SpaceBattle.Tests;
using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using ShipNamespace;
public class TestCreateGameStrategy
{
    public TestCreateGameStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulCreatingNewGame()
    {
        var mockCommand = new Mock<IComand>();
        mockCommand.Setup(x => x.Execute()).Verifiable();
        var gameMap = new Dictionary<string, IComand>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateGame",
            (object[] args) => new CreateGameStrategy().Invoke(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ScopeGame", (object[] args) => (object)0).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "QueueGame", (object[] args) => new Queue<IComand>()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameCommand", (object[] args) => mockCommand.Object).Execute();

        var concurrentQueue = new BlockingCollection<IComand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ThreadQueue", (object[] args) => concurrentQueue).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "MacroCommandGame",
            (object[] args) => new MacroCommand((List<IComand>)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "InjectMacroGame",
            (object[] args) => new InjectCommand((IComand)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "RepeatCommand",
            (object[] args) => new RepeatConcurrentCommand((IComand)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GameMap", (object[] args) => gameMap).Execute();

        Assert.Empty(gameMap);

        var gameId = "gameId";
        var game = IoC.Resolve<IComand>(
            "CreateGame",
            gameId,
            IoC.Resolve<object>("Scopes.Current"),
            400d
        );

        Assert.Single(gameMap);
        Assert.Equal(typeof(InjectCommand), gameMap[gameId].GetType());
        Assert.Equal(typeof(InjectCommand), game.GetType());
        Assert.Equal(game, gameMap[gameId]);

        Assert.Empty(concurrentQueue);
        concurrentQueue.Add(game);
        Assert.Single(concurrentQueue);

        concurrentQueue.Take().Execute();
        concurrentQueue.Take().Execute();
        concurrentQueue.Take().Execute();
        mockCommand.Verify(x => x.Execute(), Times.Exactly(3));

        Assert.Single(concurrentQueue);
        concurrentQueue.Take();
        Assert.Empty(concurrentQueue);
    }
}
