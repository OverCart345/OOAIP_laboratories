using Hwdtech;

namespace SpaceBattle;

public class Tree : ICommand
{
    private readonly string _path;
    public Tree(string path)
    {
        _path = path;
    }
    public void Execute()
    {
        var parameters = new List<List<int>>();
        var lines = File.ReadAllLines(_path);

        foreach (var line in lines)
        {
            var values = line.Split().Select(int.Parse).ToList();
            parameters.Add(values);
        }

        var builddecisiontree = IoC.Resolve<IDictionary<int, object>>("Game.CheckCollision");

        parameters.ForEach(list =>
        {
            var decisiontree = builddecisiontree;
            list.ForEach(coordinate =>
            {
                decisiontree.TryAdd(coordinate, new Dictionary<int, object>());
                decisiontree = (Dictionary<int, object>)decisiontree[coordinate];
            }
            );
        }
        );
    }
}
