using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Core.Queues
{
    public class BlockingQueue<T>
    {
        private readonly ConcurrentQueue<T> _collection = new ConcurrentQueue<T>();
        private readonly AutoResetEvent _resetEvent = new AutoResetEvent(false);

        public void Enqueue(T message)
        {
            _collection.Enqueue(message);
            _resetEvent.Set();
        }

        public bool TryDequeue(out T? message)
        {
            var dequeued = _collection.TryDequeue(out message);
            _resetEvent.Set();
            return dequeued;
        }

        public bool TryPeek(out T? message)
        {
            return _collection.TryPeek(out message);
        }

        public int Count()
        {
            return _collection.Count;
        }

        public void WaitNewMessage()
        {
            _resetEvent.WaitOne();
        }

        public void WaitNewMessage(TimeSpan timeout)
        {
            _resetEvent.WaitOne(timeout);
        }
    }
}
