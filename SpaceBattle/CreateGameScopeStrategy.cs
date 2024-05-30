namespace ShipNamespace;
using Hwdtech;

public class CreateGameScopeStrategy : IStrategy
{
    public object Invoke(params object[] args)
    {
        var gameId = (string)args[0];
        var parentScope = (object)args[1];
        var quantum = (double)args[2];

        var gameScope = IoC.Resolve<object>("Scopes.New", parentScope);

        var gameScopeMap = IoC.Resolve<IDictionary<string, object>>("ScopeMap");
        gameScopeMap.Add(gameId, gameScope);

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", gameScope).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "TimeQuantum", (object[] args) => (object)quantum).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "QueuePush", (object[] args) => new QueuePush().Invoke(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "QueuePop", (object[] args) => new QueuePop().Invoke(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeleteUObject", (object[] args) => new DeleteGameUObject((int)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", parentScope).Execute();

        return gameScope;
    }
}
