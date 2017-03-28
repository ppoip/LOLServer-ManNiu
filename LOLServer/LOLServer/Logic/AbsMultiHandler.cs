using GameCommon;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.Logic
{
    public class AbsMultiHandler:AbsOnceHandler
    {
        /// <summary> 该handler里所有用户 </summary>
        private List<UserToken> tokenList = new List<UserToken>();

        /// <summary>
        /// 用户进入
        /// </summary>
        /// <param name="token"></param>
        public void Enter(UserToken token)
        {
            if (!IsExist(token))
            {
                tokenList.Add(token);
            }
        }

        /// <summary>
        /// 用户离开
        /// </summary>
        /// <param name="token"></param>
        public void Leave(UserToken token)
        {
            if (IsExist(token))
            {
                tokenList.Remove(token);
            }
        }

        /// <summary>
        /// 用户是否已存在
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool IsExist(UserToken token)
        {
            return tokenList.Contains(token);
        }

        /// <summary>
        /// 清空token
        /// </summary>
        protected void ClearToken()
        {
            tokenList.Clear();
        }

        public void Broadcast(int command, object message, UserToken exToken)
        {
            Broadcast(GetAreaNumber(), command, message, exToken);
        }

        public void Broadcast(int area, int command, object message, UserToken exToken)
        {
            Broadcast(GetTypeNumber(), area, command, message, exToken);
        }

        /// <summary>
        /// 向当前房间广播响应
        /// </summary>
        /// <param name="type"></param>
        /// <param name="area"></param>
        /// <param name="command"></param>
        /// <param name="message"></param>
        /// <param name="exToken"></param>
        public void Broadcast(byte type, int area, int command, object message, UserToken exToken)
        {
            //model
            SocketModel model = CreateSocketModel(type, area, command, message);
            //二进制model
            byte[] data = LengthEncoding.encode(MessageEncoding.Encode(model));
            //Send
            foreach(UserToken token in tokenList)
            {
                //排除exToken
                if (token != exToken)
                {
                    //Copy一个出来发送，防止data被改变
                    byte[] buffer = new byte[data.Length];
                    Array.Copy(data, 0, buffer, 0, data.Length);
                    token.Write(buffer);
                }
            }
        }
    }
}
