using Hwdtech;
using ShipNamespace;

public class SoftStopCommand : IComand
{
    private readonly ServerThread _t;
    private readonly Action _afterStop;

    public SoftStopCommand(ServerThread t, Action? afterStop = null)
    {
        _t = t;
        if (afterStop == null)
        {
            _afterStop = new Action(() => { });
        }
        else
        {
            _afterStop = afterStop;
        }
    }

    public void Execute()
    {
        if (_t.GetThread() == Thread.CurrentThread)
        {
            _t.SetBehaviour(() =>
            {
                var queue = _t.GetQueue();

                if (queue.Count == 0)
                {
                    _t.Stop();
                    _afterStop.Invoke();
                }
                else
                {
                    var cmd = queue.Take();
                    try
                    {
                        cmd.Execute();
                    }
                    catch (Exception e)
                    {
                        IoC.Resolve<IComand>("ExceptionHandler", e).Execute();
                    }
                }
            });
        }
        else
        {
            throw new Exception("incorrect thread");
        }
    }
}
