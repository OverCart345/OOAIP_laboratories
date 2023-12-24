namespace ShipNamespace
{

    public class MovableAdapter : IMovable
{
    private readonly UniversalyObject movableObject;

    public MovableAdapter(UniversalyObject movableObject)
    {
        this.movableObject = movableObject;
    }

    public Vector2d Position
    {
        get => (Vector2d)movableObject.properties["Position"];
        set => movableObject.properties["Position"] = value;
    }

    public Vector2d Velocity => (Vector2d)movableObject.properties["Velocity"];
}

}

