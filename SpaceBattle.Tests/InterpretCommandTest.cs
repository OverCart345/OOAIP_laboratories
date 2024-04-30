namespace SpaceBattle.Tests;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using ShipNamespace;


public class TestInterpretCommand
{
    Dictionary<string, Queue<IComand>> games = new Dictionary<string, Queue<IComand>>();
    Dictionary<string, UniversalyObject> gameUObjectMap = new Dictionary<string, UniversalyObject>();

    public TestInterpretCommand()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        Mock<UniversalyObject> mockUObject = new Mock<UniversalyObject>();
        mockUObject.Setup(x => x.SetProperty(It.IsAny<string>(), It.IsAny<object>()));

        games.Add("1", new Queue<IComand>());
        games.Add("2", new Queue<IComand>());

        gameUObjectMap.Add("1", mockUObject.Object);
        gameUObjectMap.Add("2", mockUObject.Object);

        IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "QueueMap", (object[] args) => games).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "UObjectMap", (object[] args) => gameUObjectMap).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "GetQueue", (object[] args) => 
        {
            if (IoC.Resolve<IDictionary<string, Queue<IComand>>>("QueueMap").TryGetValue((string)args[0], out Queue<IComand>? queue))
            {
                return queue;
            }

            throw new Exception();
        }).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "GetObject", (object[] args) => 
        {
            if (IoC.Resolve<IDictionary<string, UniversalyObject>>("UObjectMap").TryGetValue((string)args[0], out UniversalyObject? uObject))
            {
                return uObject;
            }

            throw new Exception();
        }).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "CreateCommand", (object[] args) => new CreateCommand().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "PushQueue", (object[] args) => new QueuePush().Invoke(args)).Execute();

    }

     [Fact]
    public void PushIsSuccess()
    {
        Mock<IComand> mockCommand = new Mock<IComand>();

        Mock<IOrderMessage> mockMessage = new Mock<IOrderMessage>();
        mockMessage.SetupGet(x => x._gameId).Returns("1");
        mockMessage.SetupGet(x => x._typecommand).Returns("Move");
        mockMessage.SetupGet(x => x._parameters).Returns(new Dictionary<string, object> { { "X", 10 } ,{ "Y", 20 }});
        mockMessage.SetupGet(x => x._objectId).Returns("1");
        
        IoC.Resolve<ICommand>("IoC.Register", $"Command.{mockMessage.Object._typecommand}", (object[] args) => mockCommand.Object).Execute();

        var intepretcmd = new InterpretationCommand(mockMessage.Object);
        intepretcmd.Execute();
        
        mockMessage.SetupGet(x => x._gameId).Returns("2");
        mockMessage.SetupGet(x => x._typecommand).Returns("Rotate");
        mockMessage.SetupGet(x => x._parameters).Returns(new Dictionary<string, object> { { "Angle", 360 } });
        mockMessage.SetupGet(x => x._objectId).Returns("2");

        IoC.Resolve<ICommand>("IoC.Register", $"Command.{mockMessage.Object._typecommand}", (object[] args) => mockCommand.Object).Execute();
        intepretcmd = new InterpretationCommand(mockMessage.Object);

        intepretcmd.Execute();
        intepretcmd.Execute();
        intepretcmd.Execute();

        Assert.True(games["1"].Count() == 1);
        Assert.True(games["2"].Count() == 3);
    }

    [Fact]
    public void IncorrectObjectID()
    {
        Mock<IComand> mockCommand = new Mock<IComand>();

        Mock<IOrderMessage> mockMessage = new Mock<IOrderMessage>();
        mockMessage.SetupGet(x => x._gameId).Returns("1");
        mockMessage.SetupGet(x => x._typecommand).Returns("Move");
        mockMessage.SetupGet(x => x._parameters).Returns(new Dictionary<string, object> { { "X", 10 } ,{ "Y", 20 }});
        mockMessage.SetupGet(x => x._objectId).Returns("3");
        
        IoC.Resolve<ICommand>("IoC.Register", $"Command.{mockMessage.Object._typecommand}", (object[] args) => mockCommand.Object).Execute();
        var intepretcmd = new InterpretationCommand(mockMessage.Object);
        Assert.Throws<Exception>(() => { intepretcmd.Execute(); });
    }

    [Fact]
    public void IncorrectGameID()
    {
        Mock<IComand> mockCommand = new Mock<IComand>();

        Mock<IOrderMessage> mockMessage = new Mock<IOrderMessage>();
        mockMessage.SetupGet(x => x._gameId).Returns("3");
        mockMessage.SetupGet(x => x._typecommand).Returns("Move");
        mockMessage.SetupGet(x => x._parameters).Returns(new Dictionary<string, object> { { "X", 10 } ,{ "Y", 20 }});
        mockMessage.SetupGet(x => x._objectId).Returns("1");
        
        IoC.Resolve<ICommand>("IoC.Register", $"Command.{mockMessage.Object._typecommand}", (object[] args) => mockCommand.Object).Execute();
        var intepretcmd = new InterpretationCommand(mockMessage.Object);
        Assert.Throws<Exception>(() => { intepretcmd.Execute(); });
    }
 
}