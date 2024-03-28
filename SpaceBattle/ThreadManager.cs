using System.Collections.Concurrent;

namespace ShipNamespace
{
    public class ThreadManager
    {
        private readonly ConcurrentDictionary<Guid, ServerThread> _threads = new ConcurrentDictionary<Guid, ServerThread>();

        public void AddThread(Guid id, ServerThread serverThread)
        {
            _threads.TryAdd(id, serverThread);
        }

        public ServerThread GetThread(Guid id)
        {
            if (_threads.TryGetValue(id, out var serverThread))
            {
                return serverThread;
            }
            else
            {
                throw new Exception("Wrong thread id");
            }
        }
    }
}
