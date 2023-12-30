using Hwdtech;

namespace ShipNamespace;

public class MacroCreate
{
    private readonly string[] commands;

    public MacroCreate(string[] commands)
    {
        this.commands = commands;

    }
    public List<IComand> Create()
    {
        var cmds = new List<IComand>();

        foreach (var command in commands)
        {
            cmds.Add(IoC.Resolve<IComand>("Config." + command));
        }

        return cmds;
    }
}
