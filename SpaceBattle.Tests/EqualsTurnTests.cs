using ShipNamespace;

namespace SpaceBattle.Tests
{

    public class EqualsTests
    {
        [Fact]
        public void SameAngle_ShouldReturnTrue()
        {
            var turn = new Turn(45);

            var result = turn.Equals(turn);

            Assert.True(result);
        }

        [Fact]
        public void NullObject_ShouldReturnFalse()
        {
            var turn = new Turn(45);

            var result = turn.Equals(null);

            Assert.False(result);
        }

        [Fact]
        public void DifferentTypeObject_ShouldReturnFalse()
        {
            var turn = new Turn(45);
            var differentObject = new Mock<object>().Object;

            var result = turn.Equals(differentObject);

            Assert.False(result);
        }

        [Fact]
        public void EqualAngles_ShouldReturnTrue()
        {
            var turn1 = new Turn(45);
            var turn2 = new Turn(45);

            var result = turn1.Equals(turn2);

            Assert.True(result);
        }

        [Fact]
        public void Equals_DifferentAngles_ShouldReturnFalse()
        {
            var turn1 = new Turn(45);
            var turn2 = new Turn(90);

            var result = turn1.Equals(turn2);

            Assert.False(result);
        }
    }
}
