using System;
using System.Collections.Concurrent;

namespace Kleu.Utility.Common
{
    public class NamedLocker
    {
        private readonly ConcurrentDictionary<string, object> _locks = new ConcurrentDictionary<string, object>();

        public object GetLock(string name)
        {
            lock (_locks.GetOrAdd(name, s => new object()))
            {
                return _locks.GetOrAdd(name, s => new object());
            }
        }

        public TResult RunWithLock<TResult>(string name, Func<TResult> body)
        {
            lock (_locks.GetOrAdd(name, s => new object()))
            {
                return body();
            }
        }

        public void RunWithLock(string name, Action body)
        {
            lock (_locks.GetOrAdd(name, s => new object()))
            {
                body();
            }
        }

        public void RemoveLock(string name)
        {
            lock (_locks.GetOrAdd(name, s => new object()))
            {
                _locks.TryRemove(name, out _);
            }
        }
    }
}
