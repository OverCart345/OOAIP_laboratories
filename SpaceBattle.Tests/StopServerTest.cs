namespace SpaceBattle.Tests;
using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;
using ShipNamespace;
public class TestStopServerCommand
{
    public TestStopServerCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        var map = new ConcurrentDictionary<int, object>();

        IoC.Resolve<ICommand>("IoC.Register", "ServerThreadMap", (object[] args) => map).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "ServerStop", (object[] args) => new StopServerCommand()).Execute();
    }

    [Fact]
    public void SuccessfulSoftStoppingServer()
    {
        var map = IoC.Resolve<ConcurrentDictionary<int, object>>("ServerThreadMap");
        var mockCommand = new Mock<IServerThread>();
        mockCommand.Setup(c => c.Stop()).Verifiable();

        IoC.Resolve<ICommand>("IoC.Register", "ServerThreadCommandSend", (object[] args) => mockCommand.Object).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "ServerThreadStop", (object[] args) => mockCommand.Object).Execute();

        map[1] = 12;
        map[2] = 27;
        map[4] = 52;
        map[5] = 10;

        var barrier = new Barrier(map.Count);
        mockCommand.Verify(c => c.Stop(), Times.Never());
        IoC.Resolve<IComand>("ServerStop").Execute();

        Parallel.ForEach(map.Values, item =>
        {
            barrier.SignalAndWait();
        });
        mockCommand.Verify(c => c.Stop(), Times.Exactly(4));
    }
}
