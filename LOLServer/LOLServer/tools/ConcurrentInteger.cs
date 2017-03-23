using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOLServer.tools
{
    public class ConcurrentInteger
    {
        private int value = 0;
        private Mutex mutex = new Mutex();

        public int GetAndAdd()
        {
            lock (this)
            {
                mutex.WaitOne();
                value++;
                mutex.ReleaseMutex();
                return value;
            }
        }
    }
}
