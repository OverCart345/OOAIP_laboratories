namespace ShipNamespace
{
    public class TurnComand : IComand
    {
        private readonly ITurn turn;

        public TurnComand(ITurn turn)
        {
            this.turn = turn;
        }

        public void Execute()
        {
            turn.Angle = turn.Angle + turn.AngleSpeed;
        }
    }
}
