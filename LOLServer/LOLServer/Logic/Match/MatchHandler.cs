using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using GameCommon;
using GameProtocal;
using System.Collections.Concurrent;

namespace LOLServer.Logic.Match
{
    public class MatchHandler : AbsOnceHandler, IHandler
    {
        /// <summary> 玩家所在匹配房间映射 </summary>
        private ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();

        /// <summary> 房间id与模型映射 </summary>
        private ConcurrentDictionary<int, MatchRoom> roomMap = new ConcurrentDictionary<int, MatchRoom>();

        /// <summary> 回收利用过的房间对象 </summary>
        private ConcurrentStack<MatchRoom> cache = new ConcurrentStack<MatchRoom>();




        public void OnClientClose(UserToken token, string message)
        {
            throw new NotImplementedException();
        }

        public void OnClientConnect(UserToken token)
        {
            throw new NotImplementedException();
        }

        public void OnMessageReceive(UserToken token, object message)
        {
            SocketModel model = message as SocketModel;
            switch (model.command)
            {
                case MatchProtocal.ENTER_CREQ:
                    break;

                case MatchProtocal.LEAVE_CREQ:
                    break;
            }
        }
    }
}
