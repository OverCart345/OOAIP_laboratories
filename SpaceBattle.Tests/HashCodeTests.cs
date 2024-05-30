namespace SpaceBattle.Tests
{
    using ShipNamespace;
    using Xunit;

    public class HashTestsVector
    {
        [Fact]
        public void GetHashCode_SameVectors_ShouldReturnSameHashCode()
        {
            var v1 = new Vector2d(0, 0);
            var v2 = new Vector2d(0, 0);

            var hashCode1 = v1.GetHashCode();
            var hashCode2 = v2.GetHashCode();

            Assert.Equal(hashCode1, hashCode2);
        }

        [Fact]
        public void GetHashCode_DifferentVectors_ShouldReturnDifferentHashCodes()
        {
            var v1 = new Vector2d(0, 0);
            var v2 = new Vector2d(3, 6);

            var hashCode1 = v1.GetHashCode();
            var hashCode2 = v2.GetHashCode();

            Assert.NotEqual(hashCode1, hashCode2);
        }
    }
}
