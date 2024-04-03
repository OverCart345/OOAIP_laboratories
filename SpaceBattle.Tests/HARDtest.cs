using System.Collections.Concurrent;
using Hwdtech;
using Hwdtech.Ioc;

namespace ShipNamespace
{
    public class HARDtest
    {
        private Exception ExceptionHandler = new Exception();
        public HARDtest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Create And Start Thread", (object[] args) =>
            {
                return new ActionCommand(() =>
                {
                    var serverThread = new ServerThread((BlockingCollection<IComand>)args[1], (Action)args[2]);

                    IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"Thread.{(Guid)args[0]}", (object[] args) =>
                    {
                        return serverThread;
                    }).Execute();

                    serverThread.Start();
                });
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Send Command", (object[] args) =>
            {
                return new ActionCommand(() =>
                {
                    IoC.Resolve<ServerThread>($"Thread.{(Guid)args[0]}").GetQueue().Add((IComand)args[1]);
                });
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Hard Stop The Thread", (object[] args) =>
            {
                return new ActionCommand(() =>
                {
                    var thread = IoC.Resolve<ServerThread>($"Thread.{(Guid)args[0]}");
                    Action? action = null;

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
        public void HardStopThread()
        {
            var id = Guid.NewGuid();
            var mre = new ManualResetEvent(false);
            var queue = new BlockingCollection<IComand>(100);
            var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));

            IoC.Resolve<IComand>("Create And Start Thread", id, queue, new Action(() => { IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute(); })).Execute();

            var hardstop = IoC.Resolve<IComand>("Hard Stop The Thread", id, new Action(() => { mre.Set(); }));
            IoC.Resolve<IComand>("Send Command", id, new ActionCommand(() => { })).Execute();
            IoC.Resolve<IComand>("Send Command", id, hardstop).Execute();
            IoC.Resolve<IComand>("Send Command", id, new ActionCommand(() => { })).Execute();
            mre.WaitOne();
            IoC.Resolve<ServerThread>($"Thread.{id}").Wait();
            Assert.Single(queue);
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
            var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));

            IoC.Resolve<IComand>("Create And Start Thread", id, queue, new Action(() =>
            { IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute(); })).Execute();
            IoC.Resolve<IComand>("Create And Start Thread", id2, queue2, new Action(() =>
            { IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute(); })).Execute();

            var hardStop = IoC.Resolve<IComand>("Hard Stop The Thread", id, new Action(() => { mre.Set(); }));
            var hardStop2 = IoC.Resolve<IComand>("Hard Stop The Thread", id2, new Action(() => { mre2.Set(); }));

            IoC.Resolve<IComand>("Send Command", id, hardStop2).Execute();
            IoC.Resolve<IComand>("Send Command", id, hardStop).Execute();
            IoC.Resolve<IComand>("Send Command", id2, hardStop).Execute();
            IoC.Resolve<IComand>("Send Command", id2, hardStop2).Execute();

            mre.WaitOne();
            mre2.WaitOne();
            IoC.Resolve<ServerThread>($"Thread.{id}").Wait();
            IoC.Resolve<ServerThread>($"Thread.{id2}").Wait();

            Assert.Equal("incorrect thread", ExceptionHandler.Message);
        }

        [Fact]
        public void HardStopThreadWithoutLambda()
        {
            var id = Guid.NewGuid();

            var queue = new BlockingCollection<IComand>(100);
            var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));

            IoC.Resolve<IComand>("Create And Start Thread", id, queue, new Action(() => { IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute(); })).Execute();

            var hardstop = IoC.Resolve<IComand>("Hard Stop The Thread", id);
            IoC.Resolve<IComand>("Send Command", id, new ActionCommand(() => { })).Execute();
            IoC.Resolve<IComand>("Send Command", id, hardstop).Execute();
            IoC.Resolve<ServerThread>($"Thread.{id}").Wait();
            Assert.Empty(queue);
        }
    }
}
