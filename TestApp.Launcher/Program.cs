using System;

namespace TestApp.Launcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var stop = false;
            using (var host = new AppDomainReloader.DomainHost("foo"))
            {
                do
                {
                    if (host.IsDomainLoaded)
                    {
                        var domainEntry = host.CreateDynamicEntryPoint("TestApp.Library", "TestApp.Library.EntryPoint");
                        object answer = domainEntry.Execute(new YoMessage());
                        Console.WriteLine("data from domain: {0}", answer);
                    }

                    Console.WriteLine("Press <q> to quit, <r> to reload, <u> to unload, or any other key to continue");

                    var key = Console.ReadKey();
                    switch (key.Key)
                    {
                        case ConsoleKey.Q: stop = true; break;
                        case ConsoleKey.R: host.ReloadDomain(); break;
                        case ConsoleKey.U: host.UnloadDomain(); break;
                    }
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
