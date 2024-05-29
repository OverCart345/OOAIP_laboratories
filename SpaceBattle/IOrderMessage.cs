namespace ShipNamespace;
public interface IOrderMessage
{
    public string _gameId { get; set; }
    public string _objectId { get; set; }
    public string _typecommand { get; set; }
    public IDictionary<string, object> _parameters { get; set; }
}
