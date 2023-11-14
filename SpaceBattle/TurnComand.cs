using ShipNamespace;

namespace ShipNamespace
{
    public class TurnComand : IComand
    {
        private ITurn turn;

        public TurnComand(ITurn turn)
        {
            this.turn = turn;
        }

        public void Execute()
        {
            if(turn.isCantTurn)
                throw new Exception("SpaceShip can't turn");

            turn.Angle = turn.Angle + turn.AngleSpeed;
        }

        //public Turn getAngle() => turn.Angle;
    }
}