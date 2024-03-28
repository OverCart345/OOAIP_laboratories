using Hwdtech;
using ShipNamespace;

public class SoftStopCommand : IComand
{
    private readonly ServerThread _t;
    private readonly Action _onStoppedAction;

    public SoftStopCommand(ServerThread t, Action onStoppedAction)
    {
        _t = t;
        _onStoppedAction = onStoppedAction;
    }

    public void Execute()
    {
        if (_t.GetThread() == Thread.CurrentThread)
        {
            _t.SetBehaviour(() =>
            {
                //var queue = _t.GetQueue();

                if (_t.GetQueue().Count == 0)
                {
                    _t.Stop();
                    _onStoppedAction.Invoke();
                    //throw new Exception("fff thread");

                }
                else 
                {
                    var cmd = _t.GetQueue().Take();
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
