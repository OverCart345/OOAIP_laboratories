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
            Dictionary<string, object> objectOneProperties = objectOne.properties;
            Dictionary<string, object> objectTwoProperties = objectTwo.properties;

            Vector2d positionOne = (Vector2d)objectOneProperties["Position"] + (Vector2d)objectOneProperties["Velocity"];
            Vector2d positionTwo = (Vector2d)objectTwoProperties["Position"] + (Vector2d)objectTwoProperties["Velocity"];

            if(positionOne.GetHashCode() != positionTwo.GetHashCode())
                return;

            IoC.Resolve<ICommand>("CollisionCommand", objectOne, objectTwo).Execute();
        }
    }
}
