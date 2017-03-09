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

        public UserToken()
        {
            receiveSAEA = new SocketAsyncEventArgs();
            receiveSAEA.UserToken = this;

            sendSAEA = new SocketAsyncEventArgs();
            sendSAEA.UserToken = this;
        }

        public void Close()
        {
            try
            {
                conn.Shutdown(SocketShutdown.Both);
                conn.Close();
                conn = null;
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 接受到客户端的数据
        /// </summary>
        /// <param name="buffer"></param>
        public void received(byte[] buffer)
        {

        }

        /// <summary>
        /// Send成功回调
        /// </summary>
        public void writted()
        {

        }

    }
}
