using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
    public class HandlerCenter : AbsHandlerCenter
    {
        public override void OnClientClose(UserToken token, string message)
        {
            Console.WriteLine("有客户端断开连接，Message:"+message);
        }

        public override void OnClientConnect(UserToken token)
        {
            Console.WriteLine("有客户端连接了。");
        }

        public override void OnMessageReceive(UserToken token, object message)
        {
            Console.WriteLine(""); 
        }
    }
}
