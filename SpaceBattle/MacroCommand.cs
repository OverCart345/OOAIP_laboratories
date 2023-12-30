namespace ShipNamespace
{
    public class MacroCommand : IComand
    {
        public readonly List<IComand> commands;

        public MacroCommand(List<IComand> commands)
        {
            this.commands = commands;
        }

        public void Execute()
        {
            commands.ForEach(c => c.Execute());
        }
    }
}
