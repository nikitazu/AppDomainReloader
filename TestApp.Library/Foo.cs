using System;

namespace TestApp.Library
{
    public class Foo
    {
        private static readonly string _loadTimeStamp = DateTime.UtcNow.ToString();

        public string LoadTimeStamp { get { return _loadTimeStamp; } }
    }
}
