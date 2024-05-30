namespace ShipNamespace;
using System.Collections.Concurrent;
using Hwdtech;

public class RepeatConcurrentCommand : IComand
{
    private readonly IComand _command;

    public RepeatConcurrentCommand(IComand command) => _command = command;

    public void Execute() => IoC.Resolve<BlockingCollection<IComand>>("ThreadQueue").Add(_command);
}
