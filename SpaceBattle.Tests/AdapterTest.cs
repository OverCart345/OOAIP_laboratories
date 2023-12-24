using ShipNamespace;

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

        [Fact]
        public void Inject_UpdatesInternalCommand()
        {
            // Arrange
            var mockCommand1 = new Mock<IComand>();
            var mockCommand2 = new Mock<IComand>();
            var injectCommand = new InjectCommand(mockCommand1.Object);

            // Act
            injectCommand.Inject(mockCommand2.Object);

            // Assert
            Assert.Equal(mockCommand2.Object, injectCommand.InternalCommand);
        }
        [Fact]
        public void Execute_InvokesExecuteOfInternalCommand()
        {
            // Arrange
            var mockCommand = new Mock<IComand>();
            var injectCommand = new InjectCommand(mockCommand.Object);

            // Act
            injectCommand.Execute();

            // Assert
            mockCommand.Verify(m => m.Execute(), Times.Once());
        }
        [Fact]
        public void Constructor_SetsInternalCommand()
        {
            // Arrange
            var mockCommand = new Mock<IComand>();

            // Act
            var injectCommand = new InjectCommand(mockCommand.Object);

            // Assert
            Assert.Equal(mockCommand.Object, injectCommand.InternalCommand);
        }
    }
}
