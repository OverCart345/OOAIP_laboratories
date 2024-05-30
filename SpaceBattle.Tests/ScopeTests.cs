using ShipNamespace;
using Hwdtech;
using Hwdtech.Ioc;
namespace SpaceBattle.Tests;
public class TestNewGameScopeStrategy
{
    public TestNewGameScopeStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulCreatingNewGameScope()
    {
        var gameScopeMap = new Dictionary<string, object>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ScopeNew",
            (object[] args) => new CreateGameScopeStrategy().Invoke(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ScopeMap",
            (object[] args) => gameScopeMap).Execute();

        var parentScope = IoC.Resolve<object>("Scopes.Current");
        var scope = IoC.Resolve<object>("ScopeNew", "gameId", parentScope, 400d);

        Assert.Throws<ArgumentException>(() => IoC.Resolve<object>("TimeQuantum"));

        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();
        try
        {
            var quantum = IoC.Resolve<object>("TimeQuantum");
            Assert.Equal(400d, quantum);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }

        IoC.Resolve<ICommand>("Scopes.Current.Set", parentScope).Execute();
        Assert.Throws<ArgumentException>(() => IoC.Resolve<object>("TimeQuantum"));
    }
}
