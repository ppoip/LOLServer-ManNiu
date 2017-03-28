using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using GameProtocal;
using LOLServer.biz;
using System.Collections.Concurrent;
using GameProtocal.dto;
using System.Timers;
using LOLServer.tools;

namespace LOLServer.Logic.Select
{
    public class SelectRoom : AbsMultiHandler, IHandler
    {
        /// <summary> 选人模型1 map(userId,Model) </summary>
        private ConcurrentDictionary<int, SelectModel> modelTeamOne = new ConcurrentDictionary<int, SelectModel>();
        
        /// <summary> 选人模型2 map(userId,Model) </summary>
        private ConcurrentDictionary<int, SelectModel> modelTeamTwo = new ConcurrentDictionary<int, SelectModel>();

        /// <summary> 已经进入房间的人数 </summary>
        private int enterCount = 0;

        /// <summary> ScheduleUtil的任务id </summary>
        private int missionId = 0;

        public void Init(List<int> teamOne, List<int> teamTwo)
        {
            //Init teamOne,teamTwo model
            foreach (int userId in teamOne)
            {
                SelectModel model = new SelectModel()
                {
                    userId = userId,
                    hero = -1,
                    name = BizFactory.userBiz.GetInfo(BizFactory.userBiz.GetUserToken(userId)).name,
                    isEnter = false,
                    isReady = false
                };
                modelTeamOne.TryAdd(userId, model);
            }
            foreach (int userId in teamTwo)
            {
                SelectModel model = new SelectModel()
                {
                    userId = userId,
                    hero = -1,
                    name = BizFactory.userBiz.GetInfo(BizFactory.userBiz.GetUserToken(userId)).name,
                    isEnter = false,
                    isReady = false
                };
                modelTeamTwo.TryAdd(userId, model);
            }
            //token enter
            foreach(int userId in teamOne)
            {
                UserToken token = BizFactory.userBiz.GetUserToken(userId);
                Enter(token);
            }
            foreach (int userId in teamTwo)
            {
                UserToken token = BizFactory.userBiz.GetUserToken(userId);
                Enter(token);
            }

            //创建房间30秒后如果玩家没有全部进入，则解散掉房间
            missionId = ScheduleUtil.Instance.AddTask(() => 
            {
                if (enterCount < (modelTeamOne.Count + modelTeamTwo.Count))
                {
                    //广播销毁房间响应
                    Broadcast(SelectProtocal.DESTROY_BRO, 0, null);
                    //销毁房间
                    EventUtil.DestroySelect(GetAreaNumber());
                }
            }, 30 * 1000); //30秒后执行
            
        }


        /// <summary>
        /// 设置Type码
        /// </summary>
        /// <returns></returns>
        public override byte GetTypeNumber()
        {
            return Protocal.TYPE_SELECT;
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void OnDestroy()
        {
            modelTeamOne.Clear();
            modelTeamTwo.Clear();
            ClearToken();
            enterCount = 0;
            missionId = 0;
        }

        /// <summary>
        /// 移除某个客户端对应的房间信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="message"></param>
        public void OnClientClose(UserToken token, string message)
        {
            /*
            int userId = BizFactory.userBiz.GetInfo(token).id;

            //移除队伍
            SelectModel temp_model;
            modelTeamOne.TryRemove(userId,out temp_model);
            modelTeamTwo.TryRemove(userId, out temp_model);
            //移除token
            Leave(token);
            */

            //广播销毁房间响应
            Broadcast(SelectProtocal.DESTROY_BRO, 0, null);
            //销毁房间
            EventUtil.DestroySelect(GetAreaNumber());
        }

        public void OnClientConnect(UserToken token)
        {
            throw new NotImplementedException();
        }

        public void OnMessageReceive(UserToken token, object message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取该房间里所有user的id
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllUserId()
        {
            List<int> ids = new List<int>();
            foreach(var kv in modelTeamOne)
            {
                ids.Add(kv.Key);
            }
            foreach (var kv in modelTeamTwo)
            {
                ids.Add(kv.Key);
            }
            return ids;
        }
    }
}
