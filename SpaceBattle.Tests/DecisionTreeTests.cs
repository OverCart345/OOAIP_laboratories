using Hwdtech;
using Hwdtech.Ioc;
namespace ShipNamespace.Tests;
using TechTalk.SpecFlow;

[Binding]
public class StepDefinitionsTree
{
    private string way = @"../../../test.txt";
    private readonly Mock<IStrategy> getTreeStrategy = new Mock<IStrategy>();
    private DecisionTree? bdt;

    public static void InitScopes()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Given(@"Файл с векторами")]
    public void GivenFile()
    {
        InitScopes();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ConstructDecisionTree", (object[] args) => getTreeStrategy.Object.Strat(args)).Execute();
        getTreeStrategy.Setup(t => t.Strat(It.IsAny<object[]>())).Returns(new Dictionary<int, object>()).Verifiable();
    }

    [When(@"Выполняется операция построения дерева")]
    public void BuildingTree()
    {
        var bdt = new DecisionTree(way);
        bdt.Execute();
    }

    [Then(@"Дерево успешно строится")]
    public void SuccessBuilding()
    {
        getTreeStrategy.Verify();
    }

    [Given(@"Несуществующий файл")]
    public void DontGivenFile()
    {
        InitScopes();
        way = "./.txt";
        var getTreeStrategy = new Mock<IStrategy>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ConstructDecisionTree", (object[] args) => getTreeStrategy.Object.Strat(args)).Execute();
        getTreeStrategy.Setup(t => t.Strat(It.IsAny<object[]>())).Returns(new Dictionary<int, object>()).Verifiable();
    }

    [When(@"Попытка выполнения операции построения дерева")]
    public void TryBuildingTree()
    {
        bdt = new DecisionTree(way);
    }

    [Then(@"Дерево не строится")]
    public void FailBuilding()
    {
        Assert.NotNull(bdt);
        Assert.Throws<FileNotFoundException>(() => bdt.Execute());
        getTreeStrategy.Verify();
    }
}
