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

            var coords = new List<int?>
            {
                ((Vector2d)objectOneProperties["Position"]).X - ((Vector2d)objectTwoProperties["Position"]).X,
                ((Vector2d)objectOneProperties["Position"]).Y - ((Vector2d)objectTwoProperties["Position"]).Y,
                ((Vector2d)objectOneProperties["Velocity"]).X - ((Vector2d)objectTwoProperties["Velocity"]).X,
                ((Vector2d)objectOneProperties["Velocity"]).Y - ((Vector2d)objectTwoProperties["Velocity"]).Y
            };

            var myTree = IoC.Resolve<IDictionary<int?, object>>("CollisionTree");

            foreach (var i in coords)
            {
                myTree = (IDictionary<int?, object>)myTree[i];
            }

            IoC.Resolve<ICommand>("CollisionCommand", objectOne, objectTwo).Execute();
        }
    }
}
