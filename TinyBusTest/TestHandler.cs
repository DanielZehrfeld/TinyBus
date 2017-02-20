using System.Threading;
using TinyBus;

namespace TinyBusTest
{
    internal class TestHandler<T> where T: ITinyBusMessage<T>
    {
        public T ReceivedValue { get; private set; }

        public TestHandler(ITinyBusControl control, T returnMessage, int sleepMs = 0)
        {
            control.Handle((T inMessage) =>
            {
                Thread.Sleep(sleepMs);

                ReceivedValue = inMessage;
                return returnMessage;
            });
        }
    }
}
