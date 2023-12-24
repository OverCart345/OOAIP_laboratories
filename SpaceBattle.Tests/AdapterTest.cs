using ShipNamespace;
using Moq;

namespace SpaceBattle.Tests

{
    public class AdapterTest
    {
        [Fact]
    public void TestMovableAdapter_Position()
    {
        var mockProperties = new Dictionary<string, object>
        {
            { "Position", new Vector2d(1, 2) },
            { "Velocity", new Vector2d(3, 4) }
        };

        var mockUObject = new Mock<UniversalyObject>(mockProperties);
        var adapter = new MovableAdapter(mockUObject.Object);

        var position = adapter.Position;
        var velocity = adapter.Velocity;

        Assert.Equal(new Vector2d(1, 2), position);
        Assert.Equal(new Vector2d(3, 4), velocity);

        var newPosition = new Vector2d(5, 6);
        adapter.Position = newPosition;
        Assert.Equal(newPosition, mockProperties["Position"]);
    }

        
    }
}
