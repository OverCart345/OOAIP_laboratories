using System.Collections.Concurrent;
using System.Net.Http.Json;
using Hwdtech;
using Hwdtech.Ioc;

namespace ShipNamespace
{
    public class ENDtests
    {
        public ENDtests()
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

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread", (object[] args) =>
            {
                return new ActionCommand(() =>
                {
                    var thread = IoC.Resolve<ServerThread>($"Thread.{(Guid)args[0]}");
                    Action? action = null;

                    if (args.Length > 1)
                    {
                        action = (Action)args[1];
                    }

                    new SoftStopCommand(thread, action).Execute();
                });
            }).Execute();
        }

        [Fact]
        public void SuccessEndPoint()
        {
            var fakeCommand = new Mock<IComand>();
            fakeCommand.Setup(m => m.Execute()).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "executeFakeCommand", (object[] args) =>
            {
                return fakeCommand.Object;
            }).Execute();

            var id = Guid.NewGuid();
            var mre = new ManualResetEvent(false);
            var queue = new BlockingCollection<IComand>(100);
            var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));
            var scopeAction = new Action(() => { IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute(); });

            IoC.Resolve<IComand>("Create And Start Thread", id, queue, scopeAction).Execute();

            var clientHandler = new HttpClientHandler();
            var client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri("http://localhost:5050");

            var messageData = new Dictionary<string, object>()
            {
                { "game id", id},
                { "type", "Fire"},
                { "game item id", 548}
            };
            var messageCommand = new Message(messageData);

            Endpoint.InitScope(scopeAction);
            Endpoint.InitAndStart();

            var jsonMessageCommand = JsonContent.Create(messageCommand);
            var response = client.PostAsync("/message", jsonMessageCommand);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.Result.StatusCode);

            var softStop = IoC.Resolve<IComand>("Soft Stop The Thread", id, new Action(() => { mre.Set(); }));
            IoC.Resolve<IComand>("Send Command", id, softStop).Execute();
            mre.WaitOne();
            IoC.Resolve<ServerThread>($"Thread.{id}").Wait();

            fakeCommand.Verify(m => m.Execute(), Times.Once());
        }

        [Fact]
        public void inSuccessEndPoint()
        {
            // Создаём пустую командку cmd для проверки её отработки где-то
            var fakeCommand = new Mock<IComand>();
            fakeCommand.Setup(m => m.Execute()).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "executeFakeCommand", (object[] args) =>
            {
                return fakeCommand.Object;
            }).Execute();

            var id = Guid.NewGuid();
            var mre = new ManualResetEvent(false);
            var queue = new BlockingCollection<IComand>(100);
            var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));
            var scopeAction = new Action(() => { IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute(); });

            IoC.Resolve<IComand>("Create And Start Thread", id, queue, scopeAction).Execute();

            // Настраиваем HTTP-клиента для взаимодействия с локальным сервером.
            var clientHandler = new HttpClientHandler();
            var client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri("http://localhost:5050");

            // Создаём сообщение с командой и параметрами для отправки на сервер.
            var fakeId = Guid.NewGuid();
            var messageData = new Dictionary<string, object>()
            {
                { "game id", fakeId},
                { "type", "Fire"},
                { "game item id", 548}
            };
            var messageCommand = new Message(messageData);

            // Запускаем сервер, чтобы обработать запрос.
            Endpoint.InitScope(scopeAction);
            Endpoint.InitAndStart();

            // Отправляем сообщение на сервер и проверяем статус-код ответа.
            var jsonMessageCommand = JsonContent.Create(messageCommand);
            var response = client.PostAsync("/message", jsonMessageCommand);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.Result.StatusCode);

            var softStop = IoC.Resolve<IComand>("Soft Stop The Thread", id, new Action(() => { mre.Set(); }));
            IoC.Resolve<IComand>("Send Command", id, softStop).Execute();
            mre.WaitOne();
            IoC.Resolve<ServerThread>($"Thread.{id}").Wait();

            fakeCommand.Verify(m => m.Execute(), Times.Never());
        }
    }
}
