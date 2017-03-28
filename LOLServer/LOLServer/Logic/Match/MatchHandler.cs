using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using GameCommon;
using GameProtocal;
using System.Collections.Concurrent;
using LOLServer.biz;
using LOLServer.tools;

namespace LOLServer.Logic.Match
{
    public class MatchHandler : AbsOnceHandler, IHandler
    {
        /// <summary> 玩家所在匹配房间映射 (userId,房间id) </summary>
        private ConcurrentDictionary<int, int> userRoom = new ConcurrentDictionary<int, int>();

        /// <summary> 房间id与模型映射 </summary>
        private ConcurrentDictionary<int, MatchRoom> roomMap = new ConcurrentDictionary<int, MatchRoom>();

        /// <summary> 回收利用过的房间对象 </summary>
        private ConcurrentStack<MatchRoom> cache = new ConcurrentStack<MatchRoom>();

        /// <summary> 房间id索引 </summary>
        ConcurrentInteger idIndex = new ConcurrentInteger();


        public void OnClientClose(UserToken token, string message)
        {
            ProcessLeave(token);
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
                    ProcessEnterRoom(token);
                    break;

                case MatchProtocal.LEAVE_CREQ:
                    ProcessLeave(token);
                    break;
            }
        }

        /// <summary>
        /// 加入队列（房间）请求处理
        /// </summary>
        /// <param name="token"></param>
        private void ProcessEnterRoom(UserToken token)
        {
            //当前连接对象登陆的角色id
            int userId = BizFactory.userBiz.GetInfo(token).id;

            //是否加入了房间
            bool isJoined = false;

            //加入的房间
            MatchRoom joinedRoom = null;

            //有房间，尝试加入房间
            if (roomMap.Count > 0)
            {
                foreach(MatchRoom room in roomMap.Values)
                {
                    //判断房间是否满员
                    if( (room.teamOne.Count + room.teamTwo.Count) < room.teamMax * 2)
                    {
                        //加入房间队伍
                        if (room.teamOne.Count < room.teamMax)
                        {
                            room.teamOne.Add(userId);
                        }
                        else
                        {
                            room.teamTwo.Add(userId);
                        }
                        //记录字典
                        userRoom.TryAdd(userId, room.id);
                        //flag
                        isJoined = true;
                        joinedRoom = room;
                        break;
                    }
                }
            }

            //是否成功加入房间，没有成功加入则创建一个房间
            if (!isJoined)
            {
                //获取一个房间
                if (cache.Count > 0)
                {
                    MatchRoom room=null;
                    cache.TryPop(out room);
                    room.teamOne.Add(userId);
                    userRoom.TryAdd(userId, room.id);
                    roomMap.TryAdd(room.id, room);
                    joinedRoom = room;
                }
                else
                {
                    MatchRoom nRoom = new MatchRoom();
                    nRoom.id = idIndex.GetAndAdd();
                    nRoom.teamOne.Add(userId);
                    userRoom.TryAdd(userId, nRoom.id);
                    roomMap.TryAdd(nRoom.id, nRoom);
                    joinedRoom = nRoom;
                }
            }


            //加入的房间满员就开始游戏
            if(joinedRoom.teamMax*2 == (joinedRoom.teamOne.Count + joinedRoom.teamTwo.Count))
            {
                //房间满人
                //创建一个选人房间
                EventUtil.CreateSelect(joinedRoom.teamOne, joinedRoom.teamTwo);
                //通知进入选角模块
                WriteToUsers(joinedRoom.teamOne, GetTypeNumber(), 0, MatchProtocal.ENTER_SELECT_BRO, 0);
                WriteToUsers(joinedRoom.teamTwo, GetTypeNumber(), 0, MatchProtocal.ENTER_SELECT_BRO, 0);

                //移除房间
                //remove userRoom
                foreach (int uid in joinedRoom.teamOne)
                {
                    int value;
                    userRoom.TryRemove(uid,out value);
                }
                foreach (int uid in joinedRoom.teamTwo)
                {
                    int value;
                    userRoom.TryRemove(uid, out value);
                }
                //remove roomMap
                MatchRoom tempRoom;
                roomMap.TryRemove(joinedRoom.id, out tempRoom);
                //clear
                joinedRoom.teamOne.Clear();
                joinedRoom.teamTwo.Clear();
                //放回缓存
                cache.Push(joinedRoom);
            }
            
        }

        /// <summary>
        /// 离开队列（房间）请求处理
        /// </summary>
        /// <param name="token"></param>
        private void ProcessLeave(UserToken token)
        {
            int userId = BizFactory.userBiz.GetInfo(token).id;
            if (!userRoom.ContainsKey(userId))
                return;

            //清理roomMap
            MatchRoom room = roomMap[userRoom[userId]];
            if (room.teamOne.Contains(userId))
            {
                room.teamOne.Remove(userId);
            }
            else
            {
                room.teamTwo.Remove(userId);
            }
            //是否是房间最后一个玩家
            if (room.teamOne.Count + room.teamTwo.Count == 0)
            {
                MatchRoom tempRoom = null;
                roomMap.TryRemove(room.id, out tempRoom);
                cache.Push(room);
            }
            //清理userRoom
            int temp;
            userRoom.TryRemove(userId, out temp);

            //Console.WriteLine("有玩家离开了房间");
        }

    }
}
