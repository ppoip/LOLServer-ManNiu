using System;
using System.Collections.Generic;
using System.Text;

namespace GameCommon
{
    public class SocketModel
    {
        /// <summary> 一级协议，区分所属模块 </summary>
        public byte type { get; set; }

        /// <summary> 二级协议，区分模块下所属子模块 </summary>
        public int area { get; set; }

        /// <summary> 三级协议，区分当前处理逻辑功能 </summary>
        public int command { get; set; }

        /// <summary> 消息体，当前需要处理的主题数据 </summary>
        public object message { get; set; }

        public SocketModel() { }
        public SocketModel(byte t,int a,int c,object m)
        {
            type = t;
            area = a;
            command = c;
            message = m;
        }

        public T GetMessage<T>()
        {
            return (T)this.message;
        }
    }
}
