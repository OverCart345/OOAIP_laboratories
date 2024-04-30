namespace ShipNamespace
{
    public class Message
    {
        public IDictionary<string, object> Fields { get; set; } = new Dictionary<string, object>();

        public Message(IDictionary<string, object> fields)
        {
            Fields = fields;
        }
    }
}
