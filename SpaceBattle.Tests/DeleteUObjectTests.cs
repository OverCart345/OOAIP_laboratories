using Hwdtech;
using Hwdtech.Ioc;
using ShipNamespace;
namespace SpaceBattle.Tests;

public class TestDeleteGameUObjectStrategy
{
    public TestDeleteGameUObjectStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulDeletingGameUObject()
    {
        var gameUObjectMap = new Dictionary<int, UniversalyObject>();
        IoC.Resolve<ICommand>("IoC.Register", "DeleteUObject", (object[] args) => new DeleteGameUObject((int)args[0])).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "UObjectMap", (object[] args) => gameUObjectMap).Execute();

        var mockUObject = new Mock<UniversalyObject>();

        Assert.Empty(gameUObjectMap);
        gameUObjectMap.Add(0, mockUObject.Object);
        Assert.Single(gameUObjectMap);
        IoC.Resolve<ICommand>("DeleteUObject", 0).Execute();
        Assert.Empty(gameUObjectMap);
    }
}
