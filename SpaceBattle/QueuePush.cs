﻿namespace ShipNamespace;
using Hwdtech;

public class QueuePush : IStrategy
{
    public object Invoke(params object[] args)
    {
        var id = (string)args[0];

        var cmd = (IComand)args[1];

        var queue = IoC.Resolve<Queue<IComand>>("GetQueue", id);

        return new Action(() => { queue.Enqueue(cmd); });
    }
}
