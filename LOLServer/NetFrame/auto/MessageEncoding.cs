using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame.auto
{
    public class MessageEncoding
    {
        /// <summary>
        /// 把SocketModel编码成byte[]
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static byte[] Encode(object message)
        {
            SocketModel socketModel = message as SocketModel;
            ByteArray ba = new ByteArray();
            ba.Write(socketModel.type);
            ba.Write(socketModel.area);
            ba.Write(socketModel.command);
            ba.Write(SerializeUtil.Encode(socketModel.message));
            byte[] data = ba.GetBuffer();
            ba.Close();
            return data;
        }

        /// <summary>
        /// 把byte[]解码成socketModel
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static object Decode(byte[] data)
        {
            SocketModel socketModel = new SocketModel();
            ByteArray ba = new ByteArray(data);
            byte type;
            int area;
            int command;
            ba.Read(out type);
            ba.Read(out area);
            ba.Read(out command);
            if (ba.Readnable)
            {
                byte[] message;
                ba.Read(out message, ba.Length - ba.Position);
                socketModel.message = SerializeUtil.Decode(message);
            }
            socketModel.type = type;
            socketModel.area = area;
            socketModel.command = command;
            ba.Close();
            return socketModel;
        }
    }
}
