using System.Diagnostics;
using Hwdtech;

namespace ShipNamespace
{

    public class GameCommand : ICommand
    {
        private readonly object _scope;
        private readonly Queue<IComand> _queue;

        public GameCommand(object scope, Queue<IComand> queue)
        {
            _scope = scope;
            _queue = queue;
        }

        public void Execute()
        {
            IoC.Resolve<ICommand>("Scopes.Current.Set", _scope).Execute();
            var timeout = IoC.Resolve<TimeSpan>("Game.Get.Time.Quantum");
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            while (_queue.Count > 0 && stopwatch.Elapsed < timeout)
            {
                var cmd = _queue.Dequeue();
                try
                {
                    cmd.Execute();
                }
                catch (Exception ex)
                {
                    var handler = IoC.Resolve<IHandler>("Exception.Handler.Find", cmd, ex);
                    handler.Handle();
                }
            }

            stopwatch.Stop();
        }
    }
}
