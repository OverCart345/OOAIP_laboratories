namespace ShipNamespace
{
    public interface UniversalyObject
    {
        public object GetProperty(string key);
        public void SetProperty(string key, object value);
    }
}