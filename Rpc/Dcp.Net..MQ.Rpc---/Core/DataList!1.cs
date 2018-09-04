namespace Dcp.Net.MQ.Rpc.Core
{
    using Dynamic.Core.Excuter;
    using Dynamic.Core.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    public class DataList<T> : IDataList<T>, IDataQueue<T>
    {
        private Func<DataItem<T>, bool> currentPredicate;
        private IList<DataItem<T>> dataQueue;
        private readonly object syncObject;
        private ManualResetEvent waitEvent;

        public DataList()
        {
            this.syncObject = new object();
            this.dataQueue = (IList<DataItem<T>>) new List<DataItem<T>>();
            this.waitEvent = new ManualResetEvent(false);
        }

        public int Count() => 
            this.dataQueue.get_Count();

        public DataItem<T> Get(TimeSpan timeout)
        {
            DataItem<T> item = null;
            TimeSpan span = timeout;
            if (timeout == TimeSpan.MaxValue)
            {
                span = TimeSpan.FromMilliseconds(-1.0);
            }
            object syncObject = this.syncObject;
            lock (syncObject)
            {
                if (this.dataQueue.get_Count() > 0)
                {
                    item = Enumerable.LastOrDefault<DataItem<T>>((IEnumerable<DataItem<T>>) this.dataQueue);
                }
            }
            if (item == null)
            {
                this.waitEvent.Reset();
                this.waitEvent.WaitOne(span);
                object obj3 = this.syncObject;
                lock (obj3)
                {
                    if (this.dataQueue.get_Count() > 0)
                    {
                        item = Enumerable.LastOrDefault<DataItem<T>>((IEnumerable<DataItem<T>>) this.dataQueue);
                    }
                }
            }
            return item;
        }

        [Obsolete("通过循环抓取消息，存在空转浪费，建议改用pull方法替代！")]
        public DataItem<T> Get(Func<DataItem<T>, bool> predicate, TimeSpan timeout)
        {
            DataItem<T> item = null;
            TimeSpan span = timeout;
            if (timeout == TimeSpan.MaxValue)
            {
                span = TimeSpan.FromMilliseconds(-1.0);
            }
            object syncObject = this.syncObject;
            lock (syncObject)
            {
                if (this.dataQueue.get_Count() > 0)
                {
                    item = Enumerable.FirstOrDefault<DataItem<T>>((IEnumerable<DataItem<T>>) this.dataQueue, predicate);
                }
            }
            if (item == null)
            {
                Stopwatch stopwatch;
                for (long i = 0L; (item == null) && (i <= span.TotalMilliseconds); i += stopwatch.ElapsedMilliseconds)
                {
                    stopwatch = new Stopwatch();
                    stopwatch.Start();
                    object obj3 = this.syncObject;
                    lock (obj3)
                    {
                        item = Enumerable.FirstOrDefault<DataItem<T>>((IEnumerable<DataItem<T>>) this.dataQueue, predicate);
                    }
                    stopwatch.Stop();
                }
            }
            return item;
        }

        public DataItem<T> Pull(Func<DataItem<T>, bool> predicate, TimeSpan timeout)
        {
            object syncObject = this.syncObject;
            lock (syncObject)
            {
                this.currentPredicate = predicate;
            }
            DataItem<T> item = null;
            TimeSpan span = timeout;
            if (timeout == TimeSpan.MaxValue)
            {
                span = TimeSpan.FromMilliseconds(-1.0);
            }
            object obj3 = this.syncObject;
            lock (obj3)
            {
                if (this.dataQueue.get_Count() > 0)
                {
                    item = Enumerable.LastOrDefault<DataItem<T>>((IEnumerable<DataItem<T>>) this.dataQueue, this.currentPredicate);
                }
            }
            if (item == null)
            {
                this.waitEvent.Reset();
                this.waitEvent.WaitOne(span);
                object obj4 = this.syncObject;
                lock (obj4)
                {
                    if (this.dataQueue.get_Count() > 0)
                    {
                        item = Enumerable.LastOrDefault<DataItem<T>>((IEnumerable<DataItem<T>>) this.dataQueue, this.currentPredicate);
                    }
                }
            }
            return item;
        }

        public bool Push(DataItem<T> command)
        {
            if ((command == null) || (command.get_Data() == null))
            {
                return false;
            }
            command.set_ReceivedTime(DateTime.get_Now());
            object syncObject = this.syncObject;
            lock (syncObject)
            {
                this.dataQueue.Add(command);
                if (this.waitEvent > null)
                {
                    if (this.currentPredicate == null)
                    {
                        this.waitEvent.Set();
                    }
                    else if (Enumerable.Any<DataItem<T>>((IEnumerable<DataItem<T>>) this.dataQueue, this.currentPredicate))
                    {
                        this.waitEvent.Set();
                    }
                }
            }
            return true;
        }
    }
}

