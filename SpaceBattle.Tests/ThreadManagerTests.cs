using Hwdtech;
using Hwdtech.Ioc;

namespace ShipNamespace
{
    public class ThreadManagerTests
    {
        private readonly ThreadManager _threadManager = new ThreadManager();
        private Exception ExceptionHandler = new Exception();
        public ThreadManagerTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler", (object[] args) =>
            {
                return new ActionCommand(() =>
                {
                    ExceptionHandler = (Exception)args[0];
                });
            }).Execute();
        }

        [Fact]
        public void GetWrongId()
        {
            var id = Guid.NewGuid();
            var exception = Record.Exception(() => _threadManager.GetThread(id));
            IoC.Resolve<IComand>("ExceptionHandler", exception).Execute();
            Assert.Equal("Wrong thread id", ExceptionHandler.Message);
        }
    }
}
