using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;
using ShipNamespace;
namespace SpaceBattle.Tests

{
    public class ConsoleTest
    {
        public ConsoleTest()
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
        public void Main_StartsAndStopsServer()
        {
            var map = IoC.Resolve<ConcurrentDictionary<int, object>>("ServerThreadMap");
            var mockStartCommand = new Mock<IServerThread>();
            var mockStopCommand = new Mock<IServerThread>();
            mockStartCommand.Setup(c => c.Start());
            IoC.Resolve<ICommand>("IoC.Register", "ThreadCreate", (object[] args) => mockStartCommand.Object).Execute();
            IoC.Resolve<ICommand>("IoC.Register", "ServerStart", (object[] args) => new StartServerCommand((int)args[0])).Execute();

            map[1] = 1;
            map[2] = 3;
            map[3] = 5;
            map[4] = 7;
            map[5] = 8;

            mockStartCommand.Setup(c => c.Stop());
            IoC.Resolve<ICommand>("IoC.Register", "ServerThreadCommandSend", (object[] args) => mockStopCommand.Object).Execute();
            IoC.Resolve<ICommand>("IoC.Register", "ServerThreadStop", (object[] args) => mockStopCommand.Object).Execute();

            var prog = new Program(5);
            prog.Execute();
        }
    }
}
