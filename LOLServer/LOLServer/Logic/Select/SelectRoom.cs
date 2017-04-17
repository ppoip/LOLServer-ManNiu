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
using GameCommon;

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

        /// <summary> 已经准备的玩家 </summary>
        private List<int> readyList = new List<int>();

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
            readyList.Clear();
            ScheduleUtil.Instance.RemoveTask(missionId);
        }

        /// <summary>
        /// 移除某个客户端对应的房间信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="message"></param>
        public void OnClientClose(UserToken token, string message)
        {
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
            SocketModel model = message as SocketModel;
            switch (model.command)
            {
                case SelectProtocal.ENTER_CREQ:
                    ProcessEnter(token);
                    break;

                case SelectProtocal.READY_CREQ:
                    ProcessReady(token);
                    break;

                case SelectProtocal.SELECT_CREQ:
                    ProcessSelect(token, model.GetMessage<int>());
                    break;

                case SelectProtocal.TALK_CREQ:
                    ProcessTalk(token, model.GetMessage<string>());
                    break;
            }
        }

        /// <summary>
        /// 处理进入房间
        /// </summary>
        /// <param name="token"></param>
        /// <param name="message"></param>
        private void ProcessEnter(UserToken token)
        {
            //用户已经进入房间则返回
            if (IsExist(token))
                return;

            //userId
            int userId = BizFactory.userBiz.GetInfo(token).id;
            //加入token到房间
            Enter(token);
            enterCount++;
            //修改房间model
            if (modelTeamOne.ContainsKey(userId))
            {
                modelTeamOne[userId].isEnter = true;
            }
            if (modelTeamTwo.ContainsKey(userId))
            {
                modelTeamTwo[userId].isEnter = true;
            }
            //响应进入房间请求，返回房间所有玩家数据
            SelectRoomDTO roomDto = new SelectRoomDTO();
            roomDto.teamOne = modelTeamOne.Values.ToArray<SelectModel>();
            roomDto.teamTwo = modelTeamTwo.Values.ToArray<SelectModel>();
            Write(token, SelectProtocal.ENTER_SRES, roomDto);
            //广播玩家进入房间事件给其他玩家，传递userId给房间内其他玩家
            Broadcast(SelectProtocal.ENTER_BRO, userId, token);
        }

        /// <summary>
        /// 处理准备
        /// </summary>
        /// <param name="token"></param>
        private void ProcessReady(UserToken token)
        {
            if (!IsExist(token))
                return;

            //判断是否已经准备
            int userId = BizFactory.userBiz.GetInfo(token).id;
            if (readyList.Contains(userId))
                return;

            //获取玩家数据模型
            SelectModel model = null;
            if (modelTeamOne.ContainsKey(userId))
            {
                model = modelTeamOne[userId];
            }
            if (modelTeamTwo.ContainsKey(userId))
            {
                model = modelTeamTwo[userId];
            }

            //玩家准备
            if (model.hero == -1)
            {
                //玩家还没有选择英雄，不可以准备
                return;
            }
            else
            {
                //准备
                model.isReady = true;
                readyList.Add(userId);
                //广播玩家准备
                Broadcast(SelectProtocal.READY_BRO, model);
            }

            //判断房间所有玩家是否都已经准备好，是就开始战斗
            if (readyList.Count >= (modelTeamOne.Count + modelTeamTwo.Count))
            {
                //创建战斗房间
                EventUtil.CreateFight(modelTeamOne.Values.ToArray(), modelTeamTwo.Values.ToArray());

                //开始战斗
                Broadcast(SelectProtocal.FIGHT_BRO, 0);

                //销毁当前房间
                EventUtil.DestroySelect(GetAreaNumber());
            }
        }

        /// <summary>
        /// 处理选择英雄
        /// </summary>
        /// <param name="token"></param>
        /// <param name="selectedHeroId"></param>
        private void ProcessSelect(UserToken token,int selectedHeroId)
        {
            //玩家不存在房间则返回
            if (!IsExist(token))
                return;

            //判断玩家是否拥有需要选择的英雄
            var userInfo = BizFactory.userBiz.GetInfo(token);
            if (!userInfo.ownHeroList.Contains(selectedHeroId))
            {
                //响应选择失败
                Write(token, SelectProtocal.SELECT_SRES, 0);
                return;
            }

            //判断选择的英雄是否已被友军选择，被选择就直接返回
            int userId = userInfo.id;
            if (modelTeamOne.ContainsKey(userId))
            {
                foreach(SelectModel model in modelTeamOne.Values)
                {
                    if (model.userId != userId && model.hero == selectedHeroId) 
                    {
                        return;
                    }
                }
            }
            if (modelTeamTwo.ContainsKey(userId))
            {
                foreach (SelectModel model in modelTeamTwo.Values)
                {
                    if (model.userId != userId && model.hero == selectedHeroId)
                    {
                        return;
                    }
                }
            }

            //选择英雄
            SelectModel ownModel = null;
            if (modelTeamOne.ContainsKey(userId))
            {
                ownModel = modelTeamOne[userId];
            }else if (modelTeamTwo.ContainsKey(userId))
            {
                ownModel = modelTeamTwo[userId];
            }
            //select
            ownModel.hero = selectedHeroId;
            //向房间广播选择了英雄
            Broadcast(SelectProtocal.SELECT_BRO, ownModel);
        }

        /// <summary>
        /// 处理聊天
        /// </summary>
        /// <param name="token"></param>
        /// <param name="words"></param>
        private void ProcessTalk(UserToken token,string words)
        {
            if (!IsExist(token))
                return;

            var userInfo = BizFactory.userBiz.GetInfo(token);
            //向房间广播聊天信息
            Broadcast(SelectProtocal.TALK_BRO, userInfo.name + ":" + words);
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
