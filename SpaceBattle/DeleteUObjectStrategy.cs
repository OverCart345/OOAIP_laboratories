namespace ShipNamespace;
using Hwdtech;

public class DeleteGameUObject : ICommand
{
    private readonly int _uObjectId;

    public DeleteGameUObject(int uObjectId) => _uObjectId = uObjectId;

    public void Execute()
    {
        IoC.Resolve<IDictionary<int, UniversalyObject>>("UObjectMap").Remove(_uObjectId);
    }
}
