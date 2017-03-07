using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
    /// <summary>
    /// 用户连接信息
    /// </summary>
    public class UserToken
    {
        /// <summary> 与客户端通信的Socket </summary>
        public Socket conn;

        /// <summary> 消息接收SocketAsyncEventArgs </summary>
        public SocketAsyncEventArgs receiveSAEA;

        /// <summary> 消息发送SocketAsyncEventArgs </summary>
        public SocketAsyncEventArgs sendSAEA;
    }
}
