using System;

namespace TestApp.Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var quit = new ConsoleKeyInfo('q', ConsoleKey.Q, false, false, false);
            var reload = new ConsoleKeyInfo('r', ConsoleKey.R, false, false, false);
            var stop = false;

            using (var starter = new AppDomainReloader.DomainHost("foo"))
            {
                do
                {
                    var domainEntry = starter.CreateEntryPoint("TestApp.Library", "TestApp.Library.EntryPoint");
                    object answer = domainEntry.Execute(new YoMessage());
                    Console.WriteLine("data from domain: {0}", answer);

                    var key = Console.ReadKey();

                    if (key == quit)
                    {
                        stop = true;
                    }
                    else if (key == reload)
                    {
                        starter.RestartDomain();
                    }

                    Console.WriteLine("Press <q> to quit, <r> to reload, or any other key to continue");
                }
                while (!stop);
            }
        }
    }

    [Serializable]
    class YoMessage
    {
        private static readonly string _typeInitTime = DateTime.UtcNow.ToString();
        private readonly string _instanceInitTime = DateTime.UtcNow.ToString();

        public override string ToString()
        {
            return string.Format("Yo\n[type init {0}]\n[instance init {1}]\n", _typeInitTime, _instanceInitTime);
        }
    }
}
