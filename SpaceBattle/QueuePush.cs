namespace ShipNamespace;
using Hwdtech;

public class QueuePush : IStrategy
{  
    public object Invoke(params object[] args)
    {
        string id = (string)args[0];

        IComand cmd = (IComand)args[1];

        var queue = IoC.Resolve<Queue<IComand>>("GetQueue", id);

        return new Action(() => { queue.Enqueue(cmd); });
    }
}