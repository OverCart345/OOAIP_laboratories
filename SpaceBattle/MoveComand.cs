using System.Reflection.Metadata.Ecma335;

namespace ShipNamespace
{

	public class MoveCommand : IComand
	{
		private IMovable movable;

		public MoveCommand(IMovable movable)
		{
			this.movable = movable;
		}

		public void Execute()
		{
			if(movable.isCantMoving)
				throw new Exception("SpaceShip can't moving");

			movable.Position = movable.Position + movable.Velocity;
			
		}
		
	}
}