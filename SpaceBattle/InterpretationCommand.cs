using Hwdtech;

namespace ShipNamespace;

public class InterpretationCommand : IComand
{
    private readonly IOrderMessage _msg;

    public InterpretationCommand(IOrderMessage msg)
    {
        _msg = msg;
    }

    public void Execute()
    {
        var cmd = IoC.Resolve<IComand>("CreateCommand", _msg);

        IoC.Resolve<Action>("PushQueue", _msg._gameId, cmd).Invoke();
    }
}
