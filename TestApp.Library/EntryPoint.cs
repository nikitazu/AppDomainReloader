using System;
using AppDomainReloader;

namespace TestApp.Library
{
    [Serializable]
    public class EntryPoint : MarshalByRefObject, IEntryPoint
    {
        public object Execute(object message)
        {
            Console.WriteLine("got message: {0}", message);
            var foo = new Foo();
            return foo.LoadTimeStamp;
        }
    }
}
