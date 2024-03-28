namespace ShipNamespace
{
    public interface IMovable
    {
        Vector2d Position { get; set; }
        Vector2d Velocity { get; }
    }
}
