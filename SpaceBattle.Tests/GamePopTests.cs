using Hwdtech;
using Hwdtech.Ioc;
using ShipNamespace;
namespace SpaceBattle.Tests;
public class TestGameQueuePopStrategy
{
    public TestGameQueuePopStrategy()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulGameQueuePopping()
    {
        var gameQueueMap = new Dictionary<int, Queue<IComand>>();
        IoC.Resolve<ICommand>("IoC.Register", "QueuePop", (object[] args) => new QueuePop().Invoke(args)).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "QueueMap", (object[] args) => gameQueueMap).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "GetQueue", (object[] args) =>
        {
            if (IoC.Resolve<IDictionary<int, Queue<IComand>>>("QueueMap").TryGetValue((int)args[0], out var queue))
            {
                return queue;
            }

            throw new Exception();
        }).Execute();

        var mockCommand = new Mock<IComand>();
        var queue = new Queue<IComand>();
        queue.Enqueue(mockCommand.Object);
        gameQueueMap.Add(0, queue);

        Assert.Single(queue);
        IoC.Resolve<Action>("QueuePop", 0).Invoke();
        Assert.Empty(queue);
    }
}
