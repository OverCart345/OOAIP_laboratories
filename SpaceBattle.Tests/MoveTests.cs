using ShipNamespace;
using TechTalk.SpecFlow;

namespace spacebattletests.StepDefinitions
{
    [Binding]
    public class StepDefinitionsVector
    {
        private Exception exception = new Exception();
        private readonly Mock<IMovable> movableMock = new Mock<IMovable>();

        [Given(@"космический корабль находится в точке пространства с координатами \((.*), (.*)\)")]
        public void GivenCoordinates(int x, int y)
        {
            movableMock.SetupProperty(m => m.Position);
            movableMock.Object.Position = new Vector2d(x, y);
        }

        [Given(@"имеет мгновенную скорость \((.*), (.*)\)")]
        public void GivenVelocity(int x, int y)
        {
            movableMock.SetupGet(m => m.Velocity).Returns(new Vector2d(x, y));
        }

        [Given(@"космический корабль, положение в пространстве которого невозможно определить")]
        public void GivenImpossibleCoordinates()
        {
            movableMock.Setup(m => m.Position).Returns(new Vector2d());
        }

        [Given(@"скорость корабля определить невозможно")]
        public void GivenImpossibleSpeedVector()
        {
            movableMock.Setup(m => m.Velocity).Returns(new Vector2d());
        }

        [Given(@"изменить положение в пространстве космического корабля невозможно")]
        public void GivenImpossibleAction()
        {
            movableMock.Setup(m => m.Position).Throws<Exception>();
        }

        [When(@"происходит прямолинейное равномерное движение без деформации")]
        public void WhenMovingAction()
        {
            try
            {
                var moveCommand = new MoveCommand(movableMock.Object);
                moveCommand.Execute();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }

        [Then(@"космический корабль перемещается в точку пространства с координатами \((.*), (.*)\)")]
        public void ThenSpaceshipMovingToCoordinates(int p0, int p1)
        {
            movableMock.VerifySet(m => m.Position = new Vector2d(p0, p1), Times.Once);
        }

        [Then(@"возникает ошибка")]
        public void ThenThrowsException()
        {
            Assert.IsType<Exception>(exception);
        }
    }
}
