using System;
using System.Linq;
using System.Threading.Tasks;

namespace TinyBus
{
    public interface ITinyBusControl
    {
        void Handle<T>(Func<T, T> handler);
        Task<T[]> Publish<T>(T message) where T : ITinyBusMessage<T>;
    }

    public class TinyBusControl : ITinyBusControl
    {
        readonly TinyHandlers _handlers = new TinyHandlers();

        public void Handle<T>(Func<T, T> handler)
        {
            _handlers.Add(handler);
        }

        public async Task<T[]> Publish<T>(T message) where T: ITinyBusMessage<T>
        {
            var handlers = _handlers.Get<T>().ToList();

            var taskList = handlers.Select(handler => Task.Run(() => handler(message.Clone())));

            return await Task.WhenAll(taskList);
        }

        public static Func<ITinyBusControl> Create = () => new TinyBusControl();
    }
}
