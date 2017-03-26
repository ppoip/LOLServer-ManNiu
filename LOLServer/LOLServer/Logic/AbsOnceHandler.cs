using GameCommon;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.Logic
{
    public abstract class AbsOnceHandler
    {
        /// <summary> type码 </summary>
        private byte type;

        /// <summary> area码 </summary>
        private int area;

        public virtual byte GetTypeNumber()
        {
            return type;
        }

        public void SetTypeNumber(byte type)
        {
            this.type = type;
        }

        public virtual int GetAreaNumber()
        {
            return area;
        }

        public void SetAreaNumber(int area)
        {
            this.area = area;
        }


        #region Write重载
        public void Write(UserToken token,int command)
        {
            Write(token, command, null);
        }

        public void Write(UserToken token, int command,object message)
        {
            Write(token, GetAreaNumber(), command, message);
        }

        public void Write(UserToken token,int area, int command, object message)
        {
            Write(token, GetTypeNumber(), area, command, message);
        }

        public void Write(UserToken token,byte type, int area, int command, object message)
        {
            SocketModel sm = CreateSocketModel(type, area, command, message);
            token.Write(LengthEncoding.encode(MessageEncoding.Encode(sm)));
        }
        #endregion



        protected SocketModel CreateSocketModel(byte type, int area, int command, object message)
        {
            return new SocketModel() { type = type, area = type, command = command, message = message };
        }

    }
}
