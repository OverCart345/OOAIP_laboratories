using Hwdtech;
using Hwdtech.Ioc;
using ShipNamespace;
namespace SpaceBattle.Tests
{
    public class TestStartServerCommand
    {
        public TestStartServerCommand()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<ICommand>("Scopes.Current.Set",
                IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
            ).Execute();
        }
        [Fact]
        public void StartServerSuccess()
        {
            var count = 228;
            var _thread = new Mock<IServerThread>();
            IoC.Resolve<ICommand>("IoC.Register", "ThreadCreate", (object[] args) => _thread.Object).Execute();
            IoC.Resolve<ICommand>("IoC.Register", "ServerStart", (object[] args) => new StartServerCommand((int)args[0])).Execute();
            _thread.Setup(c => c.Start()).Verifiable();

            _thread.Verify(c => c.Start(), Times.Never());

            IoC.Resolve<IComand>("ServerStart", count).Execute();
            _thread.Verify(c => c.Start(), Times.Exactly(count));
        }
    }
}
