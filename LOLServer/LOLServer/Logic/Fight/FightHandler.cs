using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using System.Collections.Concurrent;
using LOLServer.tools;
using GameProtocal.dto;
using LOLServer.biz;

namespace LOLServer.Logic.Fight
{
    public class FightHandler : AbsOnceHandler, IHandler
    {
        /// <summary> 玩家所在匹配房间映射 (userId,房间Area) </summary>
        private ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();

        /// <summary> 房间Area与模型映射 </summary>
        private ConcurrentDictionary<int, FightRoom> roomMap = new ConcurrentDictionary<int, FightRoom>();

        /// <summary> 回收利用过的房间对象 </summary>
        private ConcurrentStack<FightRoom> cache = new ConcurrentStack<FightRoom>();

        /// <summary> 房间area索引 </summary>
        ConcurrentInteger areaIndex = new ConcurrentInteger();

        public FightHandler()
        {
            EventUtil.CreateFight = Create;
            EventUtil.DestroyFight = Destroy;
        }


        /// <summary>
        /// 创建一个战斗房间
        /// </summary>
        /// <param name="teamOne"></param>
        /// <param name="teamTwo"></param>
        private void Create(SelectModel[] teamOne, SelectModel[] teamTwo)
        {
            FightRoom room = null;
            if (!cache.TryPop(out room))
            {
                room = new FightRoom();
            }
            //设置房间号码
            room.SetAreaNumber(areaIndex.GetAndAdd());
            //初始化房间
            room.Init(teamOne, teamTwo);
            //add into roomMap
            roomMap.TryAdd(room.GetAreaNumber(), room);
            //add into userRoom
            foreach (int userId in room.GetAllUserId())
            {
                userRoom.TryAdd(userId, room.GetAreaNumber());
            }
        }

        /// <summary>
        /// 销毁一个战斗房间
        /// </summary>
        /// <param name="userId"></param>
        private void Destroy(int roomArea)
        {
            //要销毁的房间
            FightRoom room = null;
            roomMap.TryGetValue(roomArea, out room);
            if (room == null)
                return;

            //移除userRoom映射
            foreach (int userId in room.GetAllUserId())
            {
                int temp;
                userRoom.TryRemove(userId, out temp);
            }
            //Clear
            room.OnDestroy();
            //移除roomMap映射
            roomMap.TryRemove(roomArea, out room);
            //放回缓存
            cache.Push(room);
        }


        public void OnClientClose(UserToken token, string message)
        {
            int roomId;
            if (userRoom.TryGetValue(BizFactory.userBiz.GetInfo(token).id, out roomId))
            {
                //传递给玩家所在房间处理
                roomMap[roomId].OnClientClose(token, message);
            }
        }

        public void OnClientConnect(UserToken token)
        {
            throw new NotImplementedException();
        }

        public void OnMessageReceive(UserToken token, object message)
        {
            int roomId;
            if (userRoom.TryGetValue(BizFactory.userBiz.GetInfo(token).id, out roomId))
            {
                //传递给玩家所在房间处理
                roomMap[roomId].OnMessageReceive(token, message);
            }
        }
    }
}
