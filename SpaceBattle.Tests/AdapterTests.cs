using ShipNamespace;
using Hwdtech;
using Hwdtech.Ioc;
using Xunit.Abstractions;
namespace SpaceBattle.Tests;
public class TestAdapterBulider
{
    private readonly ITestOutputHelper output;
    public TestAdapterBulider(ITestOutputHelper output)
    {
        this.output = output;
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfullTest()
    {

        var typeOld = typeof(UniversalyObject);
        var typeNew = typeof(IMovable);

        var result = new Adapter(typeOld, typeNew).Build();

        var expected = @"public class IMovableAdapter : IMovable {
        UniversalyObject _obj;
    
        public IMovableAdapter(UniversalyObject obj) => _obj = obj;
    
        public Vector2d Position
        {
        
            get
            {
                return IoC.Resolve<Vector2d>(""Game.Get.Property"", ""Position"", _obj);
            }
        
            set
            {
                return IoC.Resolve<ICommand>(""Game.Set.Property"", ""Position"", _obj, value).Execute();
            }
        }
        
        public Vector2d Velocity
        {
        
            get
            {
                return IoC.Resolve<Vector2d>(""Game.Get.Property"", ""Velocity"", _obj);
            }
        
        }
        
    }";

        Assert.Equal(expected, result);
    }
}