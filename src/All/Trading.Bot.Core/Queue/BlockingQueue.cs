using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trading.Bot.Core.Queue
{
    public class BlockingQueue<T> : ConcurrentQueue<T>
    {
        private readonly AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        
        public new void Enqueue(T obj)
        {
            base.Enqueue(obj);
            autoResetEvent.Set();
        }

        public new bool TryDequeue(out T? obj)
        {
            return base.TryDequeue(out obj);
        }

        public new bool TryPeek(out T? obj)
        {
            autoResetEvent.WaitOne();
            return base.TryPeek(out obj);
        }
    }
}
