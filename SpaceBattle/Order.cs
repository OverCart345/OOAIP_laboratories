namespace ShipNamespace
{
    public interface Order
    {
        UniversalyObject Target { get; }
        Vector2d Velocity { get; }
    }
}
