using ShipNamespace;
namespace SpaceBattle.Tests

{
    public class EqualsTestsVector
    {
        [Fact]
        public void SameVector_ShouldReturnTrue()
        {
            var v1 = new Vector2d(0, 0);

            var result = v1.Equals(v1);

            Assert.True(result);
        }

        [Fact]
        public void NullVector_ShouldReturnFalse()
        {
            var v1 = new Vector2d(1, 1);

            var result = v1.Equals(null);

            Assert.False(result);
        }

        [Fact]
        public void DifferentTypeObject_ShouldReturnFalse()
        {
            var v1 = new Vector2d(2, 2);
            var differentObject = new Mock<object>().Object;

            var result = v1.Equals(differentObject);

            Assert.False(result);
        }

        [Fact]
        public void EqualVectors_ShouldReturnTrue()
        {
            var v1 = new Vector2d(3, 5);
            var v2 = new Vector2d(3, 5);

            var result = v1.Equals(v2);

            Assert.True(result);
        }

        [Fact]
        public void Equals_DifferentVectors_ShouldReturnFalse()
        {
            var v1 = new Vector2d(0, 0);
            var v2 = new Vector2d(1, 1);

            var result = v1.Equals(v2);

            Assert.False(result);
        }
    }
}
