namespace ShipNamespace;
using Hwdtech;

public class QueuePop : IStrategy
{
    public object Invoke(params object[] args)
    {
        var gameQueueId = (int)args[0];

        var queue = IoC.Resolve<Queue<IComand>>("GetQueue", gameQueueId);

        return new Action(() => { queue.Dequeue(); });
    }
}
