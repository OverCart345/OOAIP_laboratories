using Hwdtech;
using Hwdtech.Ioc;
using ShipNamespace;

namespace spacebattletests.StepDefinitions
{
    public class CheckCollisionTest
    {
        public CheckCollisionTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }

        [Fact]

        public void CollisionTestPositive()
        {
            var spaceship = new Mock<UniversalyObject>(new Dictionary<string, object>());
            var spaceship2 = new Mock<UniversalyObject>(new Dictionary<string, object>());

            spaceship.Object.properties.Add("Position", new Vector2d(1, 1));
            spaceship.Object.properties.Add("Velocity", new Vector2d(0, 0));
            spaceship2.Object.properties.Add("Position", new Vector2d(1, 1));
            spaceship2.Object.properties.Add("Velocity", new Vector2d(0, 0));

            var CollisionCommand = new Mock<ICommand>();
            CollisionCommand.Setup(c => c.Execute()).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CollisionCommand", (object[] args) => CollisionCommand.Object).Execute();

            new CheckCollisionCommand(spaceship.Object, spaceship2.Object).Execute();

            CollisionCommand.Verify(command => command.Execute(), Times.Once());
        }

        [Fact]
        public void TryGetNewTreeThrowsException()
        {
            var spaceship = new Mock<UniversalyObject>(new Dictionary<string, object>());
            var spaceship2 = new Mock<UniversalyObject>(new Dictionary<string, object>());

            spaceship.Object.properties.Add("Position", new Vector2d(2, 4));
            spaceship.Object.properties.Add("Velocity", new Vector2d(1, 3));
            spaceship2.Object.properties.Add("Position", new Vector2d(1, 4));
            spaceship2.Object.properties.Add("Velocity", new Vector2d(1, 8));

            var CollisionCommand = new Mock<ICommand>();
            CollisionCommand.Setup(c => c.Execute()).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CollisionCommand", (object[] args) => CollisionCommand.Object).Execute();

            new CheckCollisionCommand(spaceship.Object, spaceship2.Object).Execute();

            CollisionCommand.Verify(command => command.Execute(), Times.Never());
        }
    }
}
