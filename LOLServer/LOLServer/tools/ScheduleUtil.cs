using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace LOLServer.tools
{
    public class ScheduleUtil
    {
        private static ScheduleUtil _instance;
        public static ScheduleUtil Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ScheduleUtil();
                }
                return _instance;
            }
        }

        /// <summary> 计时器 </summary>
        private Timer timer;

        /// <summary> id生成器 </summary>
        private ConcurrentInteger idGetter = new ConcurrentInteger();

        /// <summary> 任务容器 </summary>
        private Dictionary<int, TimerTaskModel> taskDict = new Dictionary<int, TimerTaskModel>();

        /// <summary> 待删除列表 </summary>
        private List<int> removeList = new List<int>();

        private ScheduleUtil()
        {
            timer = new Timer(200);
            timer.Elapsed += Elapsed_Completed;
            timer.Start();
        }

        private void Elapsed_Completed(object sender, ElapsedEventArgs e)
        {
            lock(removeList)
            {
                lock (taskDict)
                {
                    //执行之前先移除
                    foreach (int taskId in removeList)
                    {
                        if(taskDict.ContainsKey(taskId))
                        {
                            taskDict.Remove(taskId);
                        }
                    }
                    removeList.Clear();

                    //执行
                    foreach(var kv in taskDict)
                    {
                        if (kv.Value.executeTime <= DateTime.Now.Millisecond)
                        {
                            //执行任务委托
                            kv.Value.execute();
                            //添加到待删除列表
                            removeList.Add(kv.Key);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 添加一个任务
        /// </summary>
        /// <param name="execute">延迟后执行的函数</param>
        /// <param name="delay">延迟，单位毫秒</param>
        public int AddTask(Action execute,long delay)
        {
            lock (taskDict)
            {
                //任务模型
                TimerTaskModel model = new TimerTaskModel();
                model.id = idGetter.GetAndAdd();
                model.execute = execute;
                model.executeTime = DateTime.Now.Millisecond + delay;
                taskDict.Add(model.id, model);
                return model.id;
            }
        }

        /// <summary>
        /// 移除一个任务
        /// </summary>
        /// <param name="taskId"></param>
        public void RemoveTask(int taskId)
        {
            lock (removeList)
            {
                //加入到待一处列表
                removeList.Add(taskId);
            }
        }

    }
}
