namespace ShipNamespace;
using Hwdtech;

public class CreateCommand : IStrategy  
{
    public object Invoke(params object[] args)
    {
        var message = (IOrderMessage)args[0];

        var obj = IoC.Resolve<UniversalyObject>("GetObject", message._objectId);

        message._parameters.ToList().ForEach(x => obj.SetProperty(x.Key, x.Value));

        return IoC.Resolve<IComand>($"Command.{message._typecommand}", obj);
    }
}