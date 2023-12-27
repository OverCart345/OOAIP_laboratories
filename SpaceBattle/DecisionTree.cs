namespace ShipNamespace;
using Hwdtech;

public class DecisionTree : ICommand
{
    private readonly string path;
    public DecisionTree(string path)
    {
        this.path = path;
    }

    public void Execute()
    {
        var strat = IoC.Resolve<Dictionary<int, object>>("Game.ConstructDecisionTree");

        try
        {
            using (var reader = File.OpenText(path))
            {
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    var record = line.Split().Select(int.Parse).ToList();
                    AddInTree(record, strat);
                }
            }
        }
        catch (FileNotFoundException ex)
        {
            throw new FileNotFoundException(ex.ToString());
        }
    }

    private static void AddInTree(List<int> list1, IDictionary<int, object> root)
    {
        var tree = root;
        foreach (var item in list1)
        {
            tree.TryAdd(item, new Dictionary<int, object>());
            tree = (Dictionary<int, object>)tree[item];
        }
    }
}
