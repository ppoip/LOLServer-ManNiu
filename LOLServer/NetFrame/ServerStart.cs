using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetFrame
{
    public class ServerStart
    {
        /// <summary> 服务器socket监听对象 </summary>
        Socket server;

        /// <summary> 客户端最大连接数 </summary>
        int maxClient;

        /// <summary> 身份pool </summary>
        UserTokenPool tokenPool;

        /// <summary> 信号量 </summary>
        Semaphore acceptClientSem;

        public ServerStart(int max,int port)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            maxClient = max;
            tokenPool = new UserTokenPool(max);
            acceptClientSem = new Semaphore(max, max);

            for(int i = 0; i < max; i++)
            {
                UserToken token = new UserToken();
                token.receiveSAEA = new SocketAsyncEventArgs();
                token.receiveSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                token.sendSAEA = new SocketAsyncEventArgs();
                token.sendSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                tokenPool.Push(token);
            }
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="port"></param>
        public void StartListen(int port)
        {
            server.Bind(new IPEndPoint(IPAddress.Any, port));
            server.Listen(10);
        }

        /// <summary>
        /// 开始Accept
        /// </summary>
        /// <param name="e"></param>
        public void StartAccept(SocketAsyncEventArgs e)
        {
            if (e == null)
            {
                e = new SocketAsyncEventArgs();
                e.Completed += new EventHandler<SocketAsyncEventArgs>(Accept_Completed);
            }
            else
            {
                e.AcceptSocket = null;
            }

            //信号量-1
            acceptClientSem.WaitOne();
            bool result = server.AcceptAsync(e);
            if (!result)
            {
                ProcessAccept(e);
            }


        }

        public void StartReceive(UserToken token)
        {
            token.conn.ReceiveAsync(token.receiveSAEA);
        }

        /// <summary>
        /// 处理Accept
        /// </summary>
        /// <param name="e"></param>
        public void ProcessAccept(SocketAsyncEventArgs e)
        {
            //取出连接对象
            UserToken token = tokenPool.Pop();
            token.conn = e.AcceptSocket;

            //开启消息监听
            StartReceive(token);

            //释放当前异步对象，递归
            StartAccept(e);
        }

        /// <summary>
        /// 处理Receive
        /// </summary>
        /// <param name="e"></param>
        public void ProcessReceive(SocketAsyncEventArgs e)
        {

        }

        /// <summary>
        /// 处理Send
        /// </summary>
        /// <param name="e"></param>
        public void ProcessSend(SocketAsyncEventArgs e)
        {

        }

        public void Accept_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        /// <summary>
        /// 不管是Receive还是Send，都会触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            if(e.LastOperation == SocketAsyncOperation.Receive)
            {
                ProcessReceive(e);
            }
            else
            {
                ProcessSend(e);
            }
        }

    }
}
