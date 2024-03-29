namespace ShipNamespace;
using Hwdtech;

public class StartServerCommand : IComand
{
    private readonly int _count;

    public StartServerCommand(int count)
    {
        _count = count;
    }

    public void Execute()
    {
        var serverThreads = CreateServerThreads();
        StartServerThreads(serverThreads);
    }

    private IEnumerable<IServerThread> CreateServerThreads()
    {
        var serverThreads = Enumerable.Range(0, _count)
            .Select(index => IoC.Resolve<IServerThread>("ThreadCreate", index))
            .ToList();
        return serverThreads;
    }

    private static void StartServerThreads(IEnumerable<IServerThread> serverThreads)
    {
        serverThreads.ToList().ForEach(thread => thread.Start());
    }
}
