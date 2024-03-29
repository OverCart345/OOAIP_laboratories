namespace ShipNamespace
{
    public class ExceptionHandler : IComand
    {
        private readonly string _path;
        private readonly string _exception;

        public ExceptionHandler(string path, string exception)
        {
            _path = path;
            _exception = exception;
        }

        public void Execute()
        {
            using (var writer = new StreamWriter(_path, true))
            {
                writer.WriteLine($"{DateTime.Now}: {_exception}");
                writer.WriteLine();
            }
        }
    }
}
