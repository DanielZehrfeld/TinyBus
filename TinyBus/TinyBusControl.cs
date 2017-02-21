using System;
using System.Linq;
using System.Threading.Tasks;

namespace TinyBus
{
    /// <summary>
    /// Used to bradcast messages to subscribers and collect returned messages.
    /// This class is thread safe
    /// </summary>
    public interface ITinyBusControl
    {
        /// <summary>
        /// Register a message recipient
        /// </summary>
        /// <typeparam name="T">The type of message that the recipient shell be registered for</typeparam>
        /// <param name="handler">The handler that is called when a message is broadcasted</param>
        void Handle<T>(Func<T, T> handler);

        /// <summary>
        /// Publish a message
        /// </summary>
        /// <typeparam name="T">The type of message to publish</typeparam>
        /// <param name="message">The message to publish. Each recipients will get a copy of the message</param>
        /// <returns>Collection of result messages returned by the recipients</returns>
        Task<T[]> Publish<T>(T message) where T : ITinyBusMessage<T>;
    }

    public class TinyBusControl : ITinyBusControl
    {
        private readonly TinyHandlers _handlers = new TinyHandlers();

        public void Handle<T>(Func<T, T> handler)
        {
            _handlers.Add(handler);
        }

        public async Task<T[]> Publish<T>(T message) where T: ITinyBusMessage<T>
        {
            var handlers = _handlers.Get<T>();

            var taskList = handlers.Select(handler => Task.Run(() => handler(message.Clone())));

            return await Task.WhenAll(taskList);
        }

        public static Func<ITinyBusControl> Create = () => new TinyBusControl();
    }
}
