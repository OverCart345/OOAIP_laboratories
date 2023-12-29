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

            var myTree = new Mock<IDictionary<int?, object>>();
            myTree.SetupGet(d => d[It.IsAny<int?>()]).Returns(myTree.Object);
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CollisionTree", (object[] args) => myTree.Object).Execute();

            var collisionCommand = new Mock<ICommand>();
            collisionCommand.Setup(c => c.Execute()).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CollisionCommand", (object[] args) => collisionCommand.Object).Execute();

            new CheckCollisionCommand(spaceship.Object, spaceship2.Object).Execute();

            collisionCommand.Verify(command => command.Execute(), Times.Once());
        }

        [Fact]
        public void TryGetNewTreeThrowsException()
        {
            var spaceship = new Mock<UniversalyObject>(new Dictionary<string, object>());
            var spaceship2 = new Mock<UniversalyObject>(new Dictionary<string, object>());

            spaceship.Object.properties.Add("Position", new Vector2d());
            spaceship.Object.properties.Add("Velocity", new Vector2d());
            spaceship2.Object.properties.Add("Position", new Vector2d(1, 4));
            spaceship2.Object.properties.Add("Velocity", new Vector2d(1, 8));

            var myTree = new Mock<IDictionary<int?, object>>();
            myTree.SetupGet(d => d[It.IsAny<int?>()]).Throws(new Exception()).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CollisionTree", (object[] args) => myTree.Object).Execute();

            var collisionCommand = new Mock<ICommand>();
            collisionCommand.Setup(c => c.Execute()).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CollisionCommand", (object[] args) => collisionCommand.Object).Execute();

            Assert.Throws<Exception>(() =>  new CheckCollisionCommand(spaceship.Object, spaceship2.Object).Execute());

            myTree.Verify(d => d[It.IsAny<int?>()], Times.Once());
            collisionCommand.Verify(command => command.Execute(), Times.Never());
        }
    }
}