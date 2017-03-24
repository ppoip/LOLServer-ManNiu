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

        //编包解包委托
        public LengthEncode LE;
        public LengthDecode LD;
        public PackEncode PE;
        public PackDecode PD;

        /// <summary> 应用层消息处理中心 </summary>
        public AbsHandlerCenter center;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="max"></param>
        public ServerStart(int max)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            maxClient = max;
            tokenPool = new UserTokenPool(max);
            acceptClientSem = new Semaphore(max, max);
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        /// <param name="port"></param>
        public void Start(int port)
        {
            //初始化tokenPool
            for (int i = 0; i < maxClient; i++)
            {
                UserToken token = new UserToken();
                //注册指针
                token.receiveSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                token.sendSAEA.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                token.sendProcess = ProcessSend;
                token.closeProcess = ClientClose;
                //设置Receive缓冲区
                token.receiveSAEA.SetBuffer(new byte[1024], 0, 1024);
                //赋值解码器
                token.LE = LE;
                token.LD = LD;
                token.PE = PE;
                token.PD = PD;
                //注入AbsHandlerCenter
                token.center = this.center;
                //加入到token池
                tokenPool.Push(token);
            }

            //banding IP
            server.Bind(new IPEndPoint(IPAddress.Any, port));
            //server.Bind(new IPEndPoint(new IPAddress(new byte[] { 127, 0, 0, 1 }),port));
            
            //start listen
            server.Listen(10);
            
            //start accept
            StartAccept(null);
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
            try
            {
                bool result = token.conn.ReceiveAsync(token.receiveSAEA);
                if (!result)
                {
                    ProcessReceive(token.receiveSAEA);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
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

            //通知应用层有客户端连接了
            center.OnClientConnect(token);

            //开启消息接收
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
            UserToken token = e.UserToken as UserToken;
            if(token.receiveSAEA.BytesTransferred>0 && token.receiveSAEA.SocketError == SocketError.Success)
            {
                //收到的数据
                byte[] message = new byte[token.receiveSAEA.BytesTransferred];
                Buffer.BlockCopy(token.receiveSAEA.Buffer, 0, message, 0, message.Length);

                //处理收到的数据
                token.Received(message);
                
                //继续接受，递归
                StartReceive(token);
            }
            else
            {
                if (token.receiveSAEA.SocketError != SocketError.Success)
                {
                    ClientClose(token, token.receiveSAEA.SocketError.ToString());
                }
                else
                {
                    ClientClose(token, "客户端主动断开连接");
                }
            }
        }

        /// <summary>
        /// 处理Send
        /// </summary>
        /// <param name="e"></param>
        public void ProcessSend(SocketAsyncEventArgs e)
        {
            UserToken token = e.UserToken as UserToken;
            if(token.sendSAEA.SocketError!= SocketError.Success)
            {
                ClientClose(token, token.sendSAEA.SocketError.ToString());
            }else
            {
                //消息发送成功，回掉成功
                token.Writted();
            }
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
            else if(e.LastOperation == SocketAsyncOperation.Send)
            {
                ProcessSend(e);
            }
        }

        /// <summary>
        /// 关闭客户端连接，并释放信号量
        /// </summary>
        /// <param name="token"></param>
        /// <param name="message"></param>
        public void ClientClose(UserToken token,string message)
        {
            if (token.conn != null)
            {
                lock (token)
                {
                    //通知应用层有客户端断开连接了
                    center.OnClientClose(token, message);
                    //reset token
                    token.Close();
                    //放回token池
                    tokenPool.Push(token);
                    //释放1个信号量
                    acceptClientSem.Release();
                }
            }

            //Console.WriteLine(message);
        }

    }
}
