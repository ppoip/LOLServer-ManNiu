using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
    public abstract class AbsHandlerCenter
    {
        /// <summary>
        /// 当有客户端连接时触发
        /// </summary>
        /// <param name="token">客户端token</param>
        public abstract void OnClientConnect(UserToken token);

        /// <summary>
        /// 当有客户端断开连接时触发
        /// </summary>
        /// <param name="token">断开连接的token</param>
        /// <param name="message">断开消息</param>
        public abstract void OnClientClose(UserToken token, string message);

        /// <summary>
        /// 当有客户端收到解包后的message时触发
        /// </summary>
        /// <param name="token">收到message包的token</param>
        /// <param name="message">包</param>
        public abstract void OnMessageReceive(UserToken token, object message);
    }
}
