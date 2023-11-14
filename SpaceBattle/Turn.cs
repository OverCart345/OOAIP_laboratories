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

        private static bool isNull(Turn t1, Turn t2)
        {
            if(t1.angle == null || t2.angle == null)
                throw new Exception("angle was null");

            return false;
        }


        public static Turn operator +(Turn t1, Turn t2)
        {
            isNull(t1, t2);

            float newAngle = t1.angle.Value + t2.angle.Value;
            
            newAngle = (newAngle % 360 + 360) % 360;

            return new Turn(newAngle);
        }

        public override bool Equals(object obj)
        {
                Turn otherTurn = (Turn)obj;
                return angle == otherTurn.angle;
        }

       
    }
}