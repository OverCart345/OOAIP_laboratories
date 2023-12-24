namespace ShipNamespace
{
    public class UniversalyObject
    {
        public Dictionary<string, object> properties { get; }

        public UniversalyObject(Dictionary<string, object> properties)
        {
            this.properties = properties;
        }
    }
}
