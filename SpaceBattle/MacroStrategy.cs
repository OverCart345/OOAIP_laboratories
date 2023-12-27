using Hwdtech;
namespace ShipNamespace;
public class MacroStrategy : IStrategy
{
    public object Strat(params object[] args)
    {
        var operationName = (string)args[0];
        var uObject = (UniversalyObject)args[1];

        var commands = new List<IComand> { };
        IoC.Resolve<List<string>>("Config." + operationName).ForEach(commandName =>
            commands.Add(IoC.Resolve<ShipNamespace.IComand>(commandName, uObject))
        );

        return IoC.Resolve<IComand>("Game.Command.Macro", commands);
    }
}
