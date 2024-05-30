using ShipNamespace;
using TechTalk.SpecFlow;

namespace spacebattletests.StepDefinitions
{
    [Binding]
    public class StepDefinitions
    {
        private Exception exception = new Exception();
        private readonly Mock<ITurn> turnableMock = new Mock<ITurn>();

        [Given(@"космический корабль, угол наклона которого невозможно определить")]
        public void GivenImpossibleAngle()
        {
            turnableMock.Setup(m => m.Angle).Returns(new Turn());
        }

        [Given(@"мгновенную угловую скорость невозможно определить")]
        public void GivenImpossibleAngleSpeed()
        {
            turnableMock.Setup(m => m.AngleSpeed).Returns(new Turn());
        }

        [Given(@"невозможно изменить угол наклона к оси OX космического корабля")]
        public void GivenImpossibleAngleAction()
        {
            turnableMock.Setup(m => m.Angle).Throws<Exception>();
        }

        [Given(@"космический корабль имеет угол наклона (.*) град к оси OX")]
        public void GivenAngle(int angle)
        {
            turnableMock.SetupProperty(m => m.Angle);
            turnableMock.Object.Angle = new Turn(angle);
        }

        [Given(@"имеет мгновенную угловую скорость (.*) град")]
        public void GivenAngleSpeed(int angle_sp)
        {
            turnableMock.Setup(m => m.AngleSpeed).Returns(new Turn(angle_sp));
        }

        [When(@"происходит вращение вокруг собственной оси")]
        public void WhenMovingActionAngleCheckOnle()
        {
            try
            {
                var turnComand = new TurnComand(turnableMock.Object);
                turnComand.Execute();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }

        [Then(@"угол наклона космического корабля к оси OX составляет (.*) град")]
        public void ThenSpaceshipNowThisAngle(int angle)
        {
            turnableMock.VerifySet(m => m.Angle = new Turn(angle), Times.Once);
        }

        [Then(@"возникает ошибка Exception")]
        public void ThenThrowsException()
        {
            Assert.IsType<Exception>(exception);
        }
    }
}
