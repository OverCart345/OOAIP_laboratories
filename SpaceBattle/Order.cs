namespace ShipNamespace
{
    public interface Order
{
    UniversalyObject Target { get; }
    string Command { get; }
    Vector2d Velocity { get; }
}

}