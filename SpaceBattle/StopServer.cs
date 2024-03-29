namespace ShipNamespace;
using System.Collections.Concurrent;
using Hwdtech;

public class StopServerCommand : IComand
{
    public void Execute()
    {
        IoC.Resolve<ConcurrentDictionary<int, object>>("ServerThreadMap").ToList().ForEach(
            pair => IoC.Resolve<IServerThread>(
                "ServerThreadCommandSend",
                pair.Key,
                IoC.Resolve<IServerThread>("ServerThreadStop", pair.Key)
            ).Stop()
        );
    }
}
