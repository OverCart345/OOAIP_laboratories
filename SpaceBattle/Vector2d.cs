namespace ShipNamespace
{

    public class Vector2d
    {
        public int? X { get; set; }
        public int? Y { get; set; }

        public Vector2d()
        {
            X = null;
            Y = null;
        }

        public Vector2d(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Vector2d operator +(Vector2d v1, Vector2d v2)
        {
            if (v1.X == null || v1.Y == null || v2.X == null || v2.Y == null)
            {
                throw new Exception("coordinates or speed vector was null");
            }

            return new Vector2d(v1.X.Value + v2.X.Value, v1.Y.Value + v2.Y.Value);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var otherVector = (Vector2d)obj;
            return X == otherVector.X && Y == otherVector.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
