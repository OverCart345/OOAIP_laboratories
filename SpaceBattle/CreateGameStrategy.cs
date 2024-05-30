namespace ShipNamespace;
using Hwdtech;

public class CreateGameStrategy : IStrategy
{
    public object Invoke(params object[] args)
    {
        var gameId = (string)args[0];
        var parentScope = (object)args[1];
        var quantum = (double)args[2];

        var gameScope = IoC.Resolve<object>("ScopeGame", gameId, parentScope, quantum);
        var gameQueue = IoC.Resolve<object>("QueueGame");
        var gameCommand = IoC.Resolve<IComand>("GameCommand", gameQueue, gameScope);

        var commandsList = new List<IComand> { gameCommand };
        var macroCommand = IoC.Resolve<IComand>("MacroCommandGame", commandsList);
        var injectCommand = IoC.Resolve<IComand>("InjectMacroGame", macroCommand);
        var repeatConcurrentCommand = IoC.Resolve<IComand>("RepeatCommand", injectCommand);
        commandsList.Add(repeatConcurrentCommand);

        var gameMap = IoC.Resolve<IDictionary<string, IComand>>("GameMap");
        gameMap.Add(gameId, injectCommand);

        return injectCommand;
    }
}
