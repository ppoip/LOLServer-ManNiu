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

        //编包解包解码器委托
        public LengthEncode LE;
        public LengthDecode LD;
        public PackEncode PE;
        public PackDecode PD;

        /// <summary> SendProcess委托 </summary>
        public Action<SocketAsyncEventArgs> sendProcess;

        /// <summary> reveive缓存 </summary>
        private List<byte> cache = new List<byte>();

        /// <summary> write缓存 </summary>
        private Queue<byte[]> writeQueue = new Queue<byte[]>();

        /// <summary> 是否正在读取cache </summary>
        private bool isReading = false;

        /// <summary> 是否正在发送writeQueue </summary>
        private bool isWriting = false;

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
                //重置
                cache.Clear();
                writeQueue.Clear();
                isReading = false;
                isWriting = false;
                //断开连接
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
        public void Received(byte[] buffer)
        {
            //添加到读取缓存
            cache.AddRange(buffer);
            if (!isReading)
            {
                isReading = true;
                OnData();
            }
        }

        /// <summary>
        /// cache中有数据
        /// </summary>
        void OnData()
        {
            byte[] data=null;

            //判断是否有粘包解码器
            if (LD != null)
            {
                data = LD(ref cache);
                if (data == null)
                {
                    //消息未接收完全，退出
                    isReading = false;
                    return;
                }
            }
            else
            {
                //缓存中没有数据，直接退出
                if (cache.Count == 0)
                {
                    isReading = false;
                    return;
                }
            }

            //判断是否有反序列化方法
            if (PD == null)
            {
                throw new Exception("packdecode is null");
            }

            //反序列化data
            object message = PD(data);

            //递归
            OnData();
        }

        /// <summary>
        /// 写入data到Send队列
        /// </summary>
        /// <param name="data"></param>
        public void Write(byte[] data)
        {
            //入队
            writeQueue.Enqueue(data);
            if (!isWriting)
            {
                isWriting = true;
                OnWrite();
            }
        }

        /// <summary>
        /// writeQueue有数据时
        /// </summary>
        public void OnWrite()
        {
            //队列空时退出
            if (writeQueue.Count == 0)
            {
                isWriting = false;
                return;
            }

            byte[] send_data = writeQueue.Dequeue();
            //设置Send数据
            sendSAEA.SetBuffer(send_data, 0, send_data.Length);
            //是否立即完成
            bool result = conn.SendAsync(sendSAEA);
            if (!result)
            {
                sendProcess(sendSAEA);
            }
        }

        /// <summary>
        /// Send成功回调
        /// </summary>
        public void Writted()
        {

            //递归
            OnWrite();
        }

    }
}
