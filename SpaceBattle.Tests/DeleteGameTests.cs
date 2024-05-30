using Hwdtech;
using Hwdtech.Ioc;
using ShipNamespace;
namespace SpaceBattle.Tests;
public class TestDeleteGameStrategy
{
    public TestDeleteGameStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulDeletingGame()
    {
        var gameMap = new Dictionary<string, IInjectable>();
        var gameScopeMap = new Dictionary<string, object>();
        var emptyCommand = new Mock<IComand>();
        emptyCommand.Setup(x => x.Execute()).Verifiable();
        IoC.Resolve<ICommand>("IoC.Register", "DeleteGame", (object[] args) => new DeleteGame((string)args[0])).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "GameMap", (object[] args) => gameMap).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "EmptyCommand", (object[] args) => emptyCommand.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "SwapCommand", (object[] args) =>
        {
            var gameMap = (IDictionary<string, IInjectable>)args[0];
            var gameId = (string)args[1];
            var action = new Action(() =>
            {
                gameMap[gameId].Inject(IoC.Resolve<IComand>("EmptyCommand"));
            });
            return action;
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "ScopeMap", (object[] args) => gameScopeMap).Execute();

        var mockGame = new Mock<IInjectable>();
        var gameId = "gameId";
        var scope = "scope";

        Assert.Empty(gameMap);
        Assert.Empty(gameScopeMap);

        gameMap.Add(gameId, mockGame.Object);
        gameScopeMap.Add(gameId, scope);
        Assert.Single(gameMap);
        Assert.Single(gameScopeMap);

        IoC.Resolve<IComand>("DeleteGame", gameId).Execute();
        IoC.Resolve<IComand>("EmptyCommand").Execute();
        Assert.Single(gameMap);
        Assert.Empty(gameScopeMap);
    }
}
