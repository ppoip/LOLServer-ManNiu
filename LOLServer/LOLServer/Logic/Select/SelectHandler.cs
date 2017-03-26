using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using LOLServer.tools;
using System.Collections.Concurrent;
using LOLServer.biz;

namespace LOLServer.Logic.Select
{
    public class SelectHandler : AbsOnceHandler, IHandler
    {
        /// <summary> 玩家所在匹配房间映射 (userId,房间Area) </summary>
        private ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();

        /// <summary> 房间id与模型映射 </summary>
        private ConcurrentDictionary<int, SelectRoom> roomMap = new ConcurrentDictionary<int, SelectRoom>();

        /// <summary> 回收利用过的房间对象 </summary>
        private ConcurrentStack<SelectRoom> cache = new ConcurrentStack<SelectRoom>();

        /// <summary> 房间area索引 </summary>
        ConcurrentInteger areaIndex = new ConcurrentInteger();




        public SelectHandler()
        {
            //传递给委托，跨模块调用用
            EventUtil.CreateSelect = Create;
            EventUtil.DestroySelect = Destroy;
        }

        /// <summary>
        /// 创建一个选人房间
        /// </summary>
        /// <param name="teamOne"></param>
        /// <param name="teamTwo"></param>
        private void Create(List<int> teamOne,List<int> teamTwo)
        {
            SelectRoom room = null;
            if (!cache.TryPop(out room))
            {
                room = new SelectRoom();
            }
            //设置房间号码
            room.SetAreaNumber(areaIndex.GetAndAdd());
            //初始化房间
            room.Init(teamOne, teamTwo);
            //roomMap
            roomMap.TryAdd(room.GetAreaNumber(), room);
            //userRoom
            foreach(int userId in room.teamOne)
            {
                userRoom.TryAdd(userId, room.GetAreaNumber());
            }
            foreach (int userId in room.teamTwo)
            {
                userRoom.TryAdd(userId, room.GetAreaNumber());
            }
        }

        /// <summary>
        /// 销毁一个选人房间
        /// </summary>
        /// <param name="roomArea"></param>
        private void Destroy(int roomArea)
        {
            //要销毁的房间
            SelectRoom room = null;
            roomMap.TryGetValue(roomArea, out room);
            if (room==null)
                return;

            //移除userRoom
            foreach(int userId in room.teamOne)
            {
                int temp;
                userRoom.TryRemove(userId, out temp);
            }
            foreach (int userId in room.teamTwo)
            {
                int temp;
                userRoom.TryRemove(userId, out temp);
            }
            //roomMap
            roomMap.TryRemove(roomArea, out room);
        }

        public void OnClientClose(UserToken token, string message)
        {
            int userId = BizFactory.userBiz.GetInfo(token).id;
            //remove userRoom
            int roomArea;
            if(userRoom.TryRemove(userId,out roomArea))
            {
                SelectRoom room = null;
                if(roomMap.TryGetValue(roomArea,out room))
                {
                    //remove team
                    room.teamOne.Remove(userId);
                    room.teamTwo.Remove(userId);

                    //leave 移除token
                    room.Leave(BizFactory.userBiz.GetUserToken(userId));
                }
            }
        }

        public void OnClientConnect(UserToken token)
        {
            throw new NotImplementedException();
        }

        public void OnMessageReceive(UserToken token, object message)
        {
            throw new NotImplementedException();
        }
    }
}
