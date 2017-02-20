using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyBus
{
    internal class TinyHandlers
    {
        private readonly Dictionary<Type, List<Delegate>> _handlers = new Dictionary<Type, List<Delegate>>();

        public void Add<T>(Func<T, T> handler)
        {
            lock (_handlers)
            {
                List<Delegate> handlerList;
                var type = typeof(T);

                if (!_handlers.TryGetValue(type, out handlerList))
                {
                    handlerList = new List<Delegate>();
                    _handlers[type] = handlerList;
                }

                handlerList.Add(handler);
            }
        }

        public IEnumerable<Func<T, T>> Get<T>()
        {
            IEnumerable<Func<T, T>> ret = null;

            lock (_handlers)
            {
                List<Delegate> handlerList;

                if (_handlers.TryGetValue(typeof(T), out handlerList))
                {
                    ret = handlerList.Cast<Func<T, T>>().ToList();
                }
            }

            return ret ?? new List<Func<T, T>>();
        }
    }
}
