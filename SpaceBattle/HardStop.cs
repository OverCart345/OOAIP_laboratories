using ShipNamespace;

public class HardStopCommand : IComand
{
    private readonly ServerThread _t;
    private readonly Action _afterStop;

    public HardStopCommand(ServerThread t, Action? afterStop = null)
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
                _t.Stop();
                _afterStop.Invoke();
            });
        }
        else
        {
            throw new Exception("incorrect thread");
        }
    }
}
