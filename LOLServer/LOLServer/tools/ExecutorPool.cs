using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LOLServer.tools
{
    public class ExecutorPool
    {
        private static ExecutorPool _instance;
        public static ExecutorPool Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ExecutorPool();

                return _instance;
            }
        }

        private ExecutorPool() { }

        /// <summary> 互斥对象 </summary>
        private Mutex mutex = new Mutex();

        public void Execute(Action func)
        {
            lock (this)
            {
                //获取信号量
                mutex.WaitOne();
                //执行委托
                func();
                //释放信号量
                mutex.ReleaseMutex();
            }
        }
    }
}
