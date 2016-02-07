using System;

namespace TestApp.Library
{
    [Serializable]
    public class EntryPoint : MarshalByRefObject
    {
        public object Execute(object message)
        {
            Console.WriteLine("got message: {0}", message);
            var foo = new Foo();
            return foo.LoadTimeStamp;
        }
    }
}
