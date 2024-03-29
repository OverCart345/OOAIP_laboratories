namespace ShipNamespace;
using Hwdtech;
public class Program : IComand
{
    private readonly int _count;
    public Program(int count)
    {
        _count = count;
    }
    public void Execute()
    {
        Console.WriteLine("Starting server...");
        IoC.Resolve<IComand>("ServerStart", _count).Execute();
        Console.WriteLine("Server started successfully");
        Console.WriteLine("Stopping server...");
        IoC.Resolve<IComand>("ServerStop").Execute();
        Console.WriteLine("Server stopped successfully");
    }
}
