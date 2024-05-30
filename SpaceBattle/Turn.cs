namespace ShipNamespace
{
    public class Turn
    {
        public float? angle { get; set; }

        public Turn()
        {
            angle = null;
        }

        public Turn(float angle)
        {
            this.angle = (angle % 360 + 360) % 360;
        }

        public static Turn operator +(Turn t1, Turn t2)
        {
            if (t1.angle == null || t2.angle == null)
            {
                throw new Exception("angle was null");
            }

            var newAngle = t1.angle.Value + t2.angle.Value;

            newAngle = (newAngle % 360 + 360) % 360;

            return new Turn(newAngle);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherTurn = (Turn)obj;
            return angle == otherTurn.angle;
        }

        public override int GetHashCode()
        {
            return angle.GetHashCode();
        }
    }
}
