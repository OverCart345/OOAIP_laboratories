namespace TreeTests;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using SpaceBattle;

public class TreeTests
{
    public TreeTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        var mockDelegate = new Mock<Func<object[], object>>();
        mockDelegate.Setup(x => x.Invoke(It.IsAny<object[]>())).Returns(new Dictionary<int, object>());

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.CheckCollision", (object[] args) => mockDelegate.Object.Invoke(args)).Execute();
    }
    public static IDictionary<int, object> GetNestedDictionary(IDictionary<int, object> dict, int key)
    {
        return (IDictionary<int, object>)dict[key];
    }
    [Fact]
    public void SuccessfulBuildSolutionTree()
    {

        var buildCommand = new Tree(@"../../../vector_tree_test.txt");

        buildCommand.Execute();

        var solutionTree = IoC.Resolve<IDictionary<int, object>>("Game.CheckCollision");

        Assert.NotNull(solutionTree);
        AssertContainsKey(solutionTree, 2);

        var nestedDict = GetNestedDictionary(solutionTree, 2);
        AssertContainsKey(nestedDict, 3);

        var nestedDict2 = GetNestedDictionary(nestedDict, 3);
        AssertContainsKey(nestedDict2, 5);

        AssertContainsKey(solutionTree, 5);

        var nestedDict3 = GetNestedDictionary(solutionTree, 5);
        AssertContainsKey(nestedDict3, 7);

        void AssertContainsKey(IDictionary<int, object> dict, int key)
        {
            Assert.True(dict.ContainsKey(key));
        }
    }
}
