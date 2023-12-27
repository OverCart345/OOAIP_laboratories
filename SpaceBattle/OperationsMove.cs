namespace ShipNamespace;
public class OperationsMove : IComand
{
    private readonly UniversalyObject target;

    public OperationsMove(UniversalyObject target)
    {
        this.target = target;
    }
    public void Execute()
    {
        var queueInner = (Queue<IComand>)target.properties["CommandQueue"];
        queueInner.Peek();
    }
}
