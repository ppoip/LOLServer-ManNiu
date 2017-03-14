using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
    public class SerializeUtil
    {
        /// <summary>
        /// 序列化SocketModel的消息体(message)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static byte[] Encode(object message)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, message);
            byte[] data = new byte[ms.Length];
            Buffer.BlockCopy(ms.GetBuffer(), 0, data, 0, (int)ms.Length);
            ms.Close();
            return data;
        }

        /// <summary>
        /// 反序列化SocketModel的消息体(message)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static object Decode(byte[] message)
        {
            MemoryStream ms = new MemoryStream(message);
            BinaryFormatter bf = new BinaryFormatter();
            object result = bf.Deserialize(ms);
            ms.Close();
            return result;
        }
    }
}
