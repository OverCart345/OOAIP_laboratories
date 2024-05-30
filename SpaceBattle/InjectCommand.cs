namespace ShipNamespace;
public class InjectCommand : IComand
{
    private readonly IComand _command;

    public InjectCommand(IComand command) => _command = command;

    public void Execute() => _command.Execute();

}
