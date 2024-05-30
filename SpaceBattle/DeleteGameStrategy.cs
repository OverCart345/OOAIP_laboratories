namespace ShipNamespace;
using Hwdtech;

public class DeleteGame : IComand
{
    private readonly string _gameId;

    public DeleteGame(string gameId) => _gameId = gameId;

    public void Execute()
    {
        var gameMap = IoC.Resolve<IDictionary<string, IInjectable>>("GameMap");

        IoC.Resolve<Action>("SwapCommand", gameMap, _gameId).Invoke();
        var game_scope_map = IoC.Resolve<IDictionary<string, object>>("ScopeMap");
        game_scope_map.Remove(_gameId);
    }
}
