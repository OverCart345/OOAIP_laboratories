using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;

namespace ShipNamespace
{
    public class softtest
    {
        private readonly ThreadManager _threadManager = new ThreadManager();
        private Exception ExceptionHandler = new Exception();
        public softtest()
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

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread", (object[] args) =>
            {
                return new ActionCommand(() =>
                {
                    var thread = _threadManager.GetThread((Guid)args[0]);
                    Action action = null;

                    if (args.Length > 1)
                    {
                        action = (Action)args[1];
                    }

                    new SoftStopCommand(thread, action).Execute();
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
        public void SoftStopThread()
        {
            var id = Guid.NewGuid();
            var mre = new ManualResetEvent(false);
            var queue = new BlockingCollection<IComand>(100);

            var mreSoft = new ManualResetEvent(false);
            var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));

            IoC.Resolve<IComand>("Create And Start Thread", id, queue, new Action(() => { IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute(); })).Execute();

            var hs = IoC.Resolve<IComand>("Soft Stop The Thread", id, new Action(() =>
            {
                mre.Set();
                mreSoft.WaitOne();
            }));
            IoC.Resolve<IComand>("Send Command", id, new ActionCommand(() => { })).Execute();
            IoC.Resolve<IComand>("Send Command", id, hs).Execute();
            IoC.Resolve<IComand>("Send Command", id, new ActionCommand(() => { })).Execute();
            mreSoft.Set();
            mre.WaitOne();
            _threadManager.GetThread(id).Wait();
            Assert.Empty(queue);
        }

         [Fact]
         public void IncorrectHardStopId()
         {
             var id = Guid.NewGuid();
             var mre = new ManualResetEvent(false);
             var queue = new BlockingCollection<IComand>(100);

             var id2 = Guid.NewGuid();
             var mre2 = new ManualResetEvent(false);
             var queue2 = new BlockingCollection<IComand>(100);

             var mreSoft = new ManualResetEvent(false);
             var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));

             IoC.Resolve<IComand>("Create And Start Thread", id, queue, new Action(() =>
             { IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute(); })).Execute();
             IoC.Resolve<IComand>("Create And Start Thread", id2, queue2, new Action(() =>
             { IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute(); })).Execute();

             var softStop = IoC.Resolve<IComand>("Soft Stop The Thread", id, new Action(() => { mre.Set(); mreSoft.WaitOne(); }));
             var softStop2 = IoC.Resolve<IComand>("Soft Stop The Thread", id2, new Action(() => { mre2.Set(); mreSoft.WaitOne(); }));

             IoC.Resolve<IComand>("Send Command", id, softStop2).Execute();
             IoC.Resolve<IComand>("Send Command", id, softStop).Execute();
             IoC.Resolve<IComand>("Send Command", id2, softStop).Execute();
             IoC.Resolve<IComand>("Send Command", id2, softStop2).Execute();

             mre.WaitOne();
             mre2.WaitOne();
             mreSoft.Set();

             _threadManager.GetThread(id).Wait();
             _threadManager.GetThread(id2).Wait();

             Assert.Equal("incorrect thread", ExceptionHandler.Message);
         }

         [Fact]
         public void AnExceptionSholdNotStopServerThread()
         {
             var id = Guid.NewGuid();
             var mre = new ManualResetEvent(false);
             var queue = new BlockingCollection<IComand>(100);

             var mreSoft = new ManualResetEvent(false);
             var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));

             IoC.Resolve<IComand>("Create And Start Thread", id, queue, new Action(() => { IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute(); })).Execute();
             var exceptionCommand = new Mock<IComand>();
             exceptionCommand.Setup(m => m.Execute()).Throws<Exception>().Verifiable();
             var softStop = IoC.Resolve<IComand>("Soft Stop The Thread", id, new Action(() =>
             {
                 mre.Set();
                 mreSoft.WaitOne();
             }));
             IoC.Resolve<IComand>("Send Command", id, new ActionCommand(() => { })).Execute();
             IoC.Resolve<IComand>("Send Command", id, softStop).Execute();
             IoC.Resolve<IComand>("Send Command", id, exceptionCommand.Object).Execute();
             IoC.Resolve<IComand>("Send Command", id, new ActionCommand(() => { })).Execute();
             IoC.Resolve<IComand>("Send Command", id, new ActionCommand(() => { })).Execute();
             IoC.Resolve<IComand>("Send Command", id, new ActionCommand(() => { })).Execute();
             mreSoft.Set();
             mre.WaitOne();
             _threadManager.GetThread(id).Wait();
             Assert.Empty(queue);
         }
    }
}
