using System;
using System.Threading;

namespace Invoke
{
    class Test
    {
        public event EventHandler SomeEvent;

        protected virtual void OnSomeEvent(EventArgs e)
        {
            SomeEvent?.Invoke(this, e);
        }

        public void MyMethod()
        {
            Console.WriteLine("Some work");
            Thread.Sleep(1000);

            OnSomeEvent(new EventArgs());

            Thread.Sleep(1000);
            Console.WriteLine("Additional work");
            Thread.Sleep(1000);
        }
    }
}
