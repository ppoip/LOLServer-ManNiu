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

        /// <summary> 房间Area与模型映射 </summary>
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
            //add into roomMap
            roomMap.TryAdd(room.GetAreaNumber(), room);
            //add into userRoom
            foreach(int userId in room.GetAllUserId())
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
            foreach(int userId in room.GetAllUserId())
            {
                int temp;
                userRoom.TryRemove(userId, out temp);
            }
            //Clear
            room.OnDestroy();
            //roomMap
            roomMap.TryRemove(roomArea, out room);
            //放回缓存
            cache.Push(room);
        }

        public void OnClientClose(UserToken token, string message)
        {
            /*  
            int userId = BizFactory.userBiz.GetInfo(token).id;
            //remove userRoom
            int roomArea;
            if(userRoom.TryRemove(userId,out roomArea))
            {
                SelectRoom room = null;
                if(roomMap.TryGetValue(roomArea,out room))
                {
                    room.OnClientClose(token, message);
                }
            }
            */
        }

        public void OnClientConnect(UserToken token)
        {
            throw new NotImplementedException();
        }

        public void OnMessageReceive(UserToken token, object message)
        {
            int userId;
            //获取对应的userId
            userId = BizFactory.userBiz.GetInfo(token).id;
            int roomArea;
            //获取对应的roomArea
            if(userRoom.TryGetValue(userId,out roomArea))
            {
                SelectRoom room = null;
                //获取对应的room
                if(roomMap.TryGetValue(roomArea,out room))
                {
                    //通知相应房间
                    room.OnMessageReceive(token, message);
                }
            }
        }
    }
}
