namespace ShipNamespace
{
    public interface Order
    {
        UniversalyObject Target { get; }
        Dictionary<string, object> properties { get; }
    }
}
