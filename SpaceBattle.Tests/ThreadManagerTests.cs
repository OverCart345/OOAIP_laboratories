using System;
using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;


namespace ShipNamespace
{
    public class ThreadManagerTests
    {
        private readonly ThreadManager _threadManager = new ThreadManager();
        Exception ExceptionHandler = new Exception();
        public ThreadManagerTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Create And Start Thread", (object[] args) =>
            {
                return new ActionCommand(() =>
                {
                    Action action = null;

                    if (args.Length > 2)
                    {
                        action = (Action)args[2];
                    }

                    var serverThread = new ServerThread((BlockingCollection<IComand>)args[1], action);
                    _threadManager.AddThread((Guid)args[0], serverThread);
                    serverThread.Start();
                });
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Send Command", (object[] args) =>
            {
                return new ActionCommand(() =>
                {
                    _threadManager.GetThread((Guid)args[0]).GetQueue().Add((IComand)args[1]);
                });
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Hard Stop The Thread", (object[] args) =>
            {
                return new ActionCommand(() =>
                {
                    var thread = _threadManager.GetThread((Guid)args[0]);
                    Action action = null;

                    if (args.Length > 1)
                    {
                        action = (Action)args[1];
                    }

                    new HardStopCommand(thread, action).Execute();
                });
            }).Execute();


            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler", (object[] args) =>
            {
                return new ActionCommand(() =>
                {
                    ExceptionHandler = (Exception)args[0];
                });
            }).Execute();
        }

        [Fact]
        public void HardStopThreadd()
        {
            var f = Guid.NewGuid();
            _threadManager.GetThread(f);
            Assert.Equal("thread", ExceptionHandler.Message);
        }
    }
}
