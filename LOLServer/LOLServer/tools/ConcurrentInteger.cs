﻿using System;
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

        public void Reset()
        {
            lock (this)
            {
                mutex.WaitOne();
                value = 0;
                mutex.ReleaseMutex();
            }
        }

        public int Get()
        {
            return value;
        }

        public void Set(int value)
        {
            lock (this)
            {
                mutex.WaitOne();
                this.value = value;
                mutex.ReleaseMutex();
            }
        }
    }
}
