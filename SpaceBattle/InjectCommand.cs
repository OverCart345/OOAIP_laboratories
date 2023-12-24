using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShipNamespace
{
    public class InjectCommand : IComand
    {
        private IComand internalCommand;

        public IComand InternalCommand
        {
            get { return internalCommand; }
            private set { internalCommand = value; }
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