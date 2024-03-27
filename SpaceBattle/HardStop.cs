using ShipNamespace;

public class HardStopCommand : IComand
{
    private ServerThread _t;
    private Action _onStoppedAction;

    public HardStopCommand(ServerThread t, Action onStoppedAction = null)
    {
        _t = t;
        _onStoppedAction = onStoppedAction;
    }

    public void Execute()
    {
        if(_t.GetThread() != Thread.CurrentThread)
            throw new Exception("incorrect thread");

        _t.SetBehaviour(() =>
        {
            _t.Stop();
            _onStoppedAction?.Invoke();
        });
    }
}
