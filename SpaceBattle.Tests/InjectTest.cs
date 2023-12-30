using ShipNamespace;

public class InjectCommandTests
{
    [Fact]
    public void Constructor_InitializesInternalCommand()
    {
        var mockCommand = new Mock<IComand>();

        var injectCommand = new InjectCommand(mockCommand.Object);

        Assert.Equal(mockCommand.Object, injectCommand.InternalCommand);
    }

    [Fact]
    public void Inject_UpdatesInternalCommand()
    {
        var initialCommand = new Mock<IComand>();
        var newCommand = new Mock<IComand>();
        var injectCommand = new InjectCommand(initialCommand.Object);

        injectCommand.Inject(newCommand.Object);

        Assert.Equal(newCommand.Object, injectCommand.InternalCommand);
    }

    [Fact]
    public void Execute_ExecutesInternalCommand()
    {
        var mockCommand = new Mock<IComand>();
        mockCommand.Setup(m => m.Execute());
        var injectCommand = new InjectCommand(mockCommand.Object);

        injectCommand.Execute();

        mockCommand.Verify(m => m.Execute(), Times.Once());
    }
}
