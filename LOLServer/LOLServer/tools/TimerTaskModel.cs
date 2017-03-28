using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.tools
{
    public class TimerTaskModel
    {
        public int id;            //任务id
        public long executeTime;  //该任务何时执行
        public Action execute;

        public TimerTaskModel() { }

        public TimerTaskModel(int id, long executeTime, Action execute)
        {
            this.id = id;
            this.executeTime = executeTime;
            this.execute = execute;
        }
    }
}
