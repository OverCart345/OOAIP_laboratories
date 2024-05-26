using Hwdtech;

namespace ShipNamespace
{
    public interface IHandler
    {
        void Handle();
    }

    public class DefaultHandler : IHandler
    {
        private readonly Exception _exception;

        public DefaultHandler(Exception e)
        {
            _exception = e;
        }

        public void Handle()
        {
            throw _exception;
        }
    }
}
