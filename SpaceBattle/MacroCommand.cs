namespace ShipNamespace
{
    public class MacroCommand : IComand
    {
        public List<IComand> commands { get; set; }
        public MacroCommand(List<IComand> commands)
        {
            this.commands = commands;
        }

        public void Execute()
        {
            foreach (var command in commands)
            {
                command.Execute();
            }
        }
    }
}
