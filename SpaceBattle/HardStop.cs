using ShipNamespace;

public class HardStopCommand : IComand
{
    private readonly ServerThread _t;
    private readonly Action _onStoppedAction;

    public HardStopCommand(ServerThread t, Action onStoppedAction = null)
    {
        _t = t;
        _onStoppedAction = onStoppedAction;
    }

    public void Execute()
    {
        if (_t.GetThread() != Thread.CurrentThread)
        {
            throw new Exception("incorrect thread");
        }

        _t.SetBehaviour(() =>
        {
            _t.Stop();
            _onStoppedAction?.Invoke();
        });
    }
}
