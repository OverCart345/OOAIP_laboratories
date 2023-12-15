namespace SpaceBattle.Tests
{
    using ShipNamespace;
    using Xunit;

    public class HashTests
    {
        [Fact]
        public void GetHashCode_SameAngles_ShouldReturnSameHashCode()
        {
            var turn1 = new Turn(45);
            var turn2 = new Turn(45);

            var hashCode1 = turn1.GetHashCode();
            var hashCode2 = turn2.GetHashCode();

            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void GetHashCode_DifferentAngles_ShouldReturnDifferentHashCodes()
        {
            var turn1 = new Turn(45);
            var turn2 = new Turn(90);

            var hashCode1 = turn1.GetHashCode();
            var hashCode2 = turn2.GetHashCode();

            Assert.NotEqual(hashCode1, hashCode2);
        }
        [Fact]
        public void GetHashCode_Null_Angle()
        {
            var turn1 = new Turn(45);
            var turn2 = new Turn();

            var hashCode1 = turn1.GetHashCode();
            var hashCode2 = turn2.GetHashCode();

            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}
