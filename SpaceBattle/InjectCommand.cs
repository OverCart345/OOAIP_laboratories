namespace ShipNamespace
{
    public class InjectCommand : IComand
    {
        private IComand internalCommand;

        public IComand InternalCommand
        {
            get => internalCommand;
            private set => internalCommand = value;
        }

        public InjectCommand(IComand internalCommand)
        {
            this.internalCommand = internalCommand;
        }

        public void Inject(IComand InternalCommand)
        {
            this.InternalCommand = InternalCommand;
        }

        public void Execute()
        {
            InternalCommand.Execute();
        }
    }
}
