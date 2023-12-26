using Hwdtech;

namespace ShipNamespace
{
    public class CheckCollisionCommand : ICommand
    {
        private readonly UniversalyObject objectOne, objectTwo;

        public CheckCollisionCommand(UniversalyObject objectOne, UniversalyObject objectTwo)
        {
            this.objectOne = objectOne;
            this.objectTwo = objectTwo;
        }

        public void Execute()
        {
            var objectOneProperties = objectOne.properties;
            var objectTwoProperties = objectTwo.properties;

            var positionOne = (Vector2d)objectOneProperties["Position"] + (Vector2d)objectOneProperties["Velocity"];
            var positionTwo = (Vector2d)objectTwoProperties["Position"] + (Vector2d)objectTwoProperties["Velocity"];

            if (positionOne.GetHashCode() == positionTwo.GetHashCode())
            {
                IoC.Resolve<ICommand>("CollisionCommand", objectOne, objectTwo).Execute();
            }
        }
    }
}
