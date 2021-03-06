﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using GameProtocal.dto;
using GameProtocal;
using System.Collections.Concurrent;
using GameProtocal.dto.fight;
using GameProtocal.constants;
using static GameProtocal.constants.HeroData;
using GameProtocol.constans;
using static GameProtocal.constants.BuildingData;
using GameCommon;
using LOLServer.tools;
using LOLServer.biz;

namespace LOLServer.Logic.Fight
{
    public class FightRoom : AbsMultiHandler, IHandler
    {
        /// <summary> 队伍1战斗模型 </summary>
        private ConcurrentDictionary<int, AbsFightModel> modelTeamOne = new ConcurrentDictionary<int, AbsFightModel>();

        /// <summary> 队伍2战斗模型 </summary>
        private ConcurrentDictionary<int, AbsFightModel> modelTeamTwo = new ConcurrentDictionary<int, AbsFightModel>();

        /// <summary> 已经加载资源完成的玩家数量 </summary>
        private ConcurrentInteger enterCount = new ConcurrentInteger();

        /// <summary> 已经离线的玩家 </summary>
        private ConcurrentDictionary<int, UserToken> offlinePlayer = new ConcurrentDictionary<int, UserToken>();

        /// <summary> 该房间玩家的个数 </summary>
        private ConcurrentInteger playerCount = new ConcurrentInteger();

        /// <summary>
        /// 初始化房间
        /// </summary>
        /// <param name="teamOne"></param>
        /// <param name="teamTwo"></param>
        public void Init(SelectModel[] teamOne, SelectModel[] teamTwo)
        {
            //设置房间人数
            playerCount.Set(teamOne.Length + teamTwo.Length);

            //初始化玩家战斗模型 玩家id [0,正无穷]
            foreach (var item in teamOne)
            {
                var model = CreatePlayerFightModel(item);
                modelTeamOne.TryAdd(item.userId, model);
            }
            foreach (var item in teamTwo)
            {
                var model = CreatePlayerFightModel(item);
                modelTeamTwo.TryAdd(item.userId, model);
            }
            //初始化建筑
            //队伍1 建筑id[-1,-10] ，队伍2 建筑id[-11,-20]
            for (int i = -1; i >= -3; i--) //初始化3个建筑，位置与id绑定
            {
                modelTeamOne.TryAdd(i, CreateBuildingFightModel(i, Math.Abs(i)));
            }
            for (int i = -11; i >= -13; i--) //初始化3个建筑，位置与id绑定
            {
                modelTeamTwo.TryAdd(i, CreateBuildingFightModel(i, Math.Abs(i)-10));
            }

        }

        /// <summary>
        /// 创建玩家战斗模型
        /// </summary>
        /// <param name="sModel">选人模型</param>
        /// <returns></returns>
        private PlayerFightModel CreatePlayerFightModel(SelectModel sModel)
        {
            HeroDataModel heroDataModel = null;
            //获取对应的英雄数据
            HeroData.heroMap.TryGetValue(sModel.hero, out heroDataModel);
            if (heroDataModel == null)
                return null;

            PlayerFightModel retModel = new PlayerFightModel()
            {
                id = sModel.userId,
                code = sModel.hero,
                name = sModel.name,
                level = 1,
                exp = 0,
                free = 1,
                money = 0,
                equs = new int[3],
                atk = heroDataModel.atkBase,
                atkRange = heroDataModel.range,
                atkSpeed = heroDataModel.atkSpeed,
                curHp = heroDataModel.hpBase,
                maxHp = heroDataModel.hpBase,
                curMp = heroDataModel.mpBase,
                maxMp = heroDataModel.mpBase,
                def = heroDataModel.defBase,
                speed = heroDataModel.speed,
                viewRange = heroDataModel.viewRange,
                skills = CreateFightSkills(heroDataModel.skills),  //创建技能模型
                modelType = ModelType.Human
            };

            return retModel;
        }

        /// <summary>
        /// 创建英雄技能
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private FightSkill[] CreateFightSkills(int[] values)
        {
            FightSkill[] skills = new FightSkill[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                SkillDataModel skillModel = null;
                SkillLevelData skillLevelData = null;
                //获取对应的技能数据
                SkillData.skillMap.TryGetValue(values[i], out skillModel);
                //获取技能第1级的数据
                skillLevelData = skillModel.levels[0];
                //构造返回数据
                FightSkill retFightSkill = new FightSkill()
                {
                    code = skillModel.code,
                    info = skillModel.info,
                    level = 0,     //一开始是0级，也就是还没学习
                    nextLevel = skillLevelData.level, //学习等级为第1级的所需学习级别
                    name = skillModel.name,
                    range = skillLevelData.range,
                    skillType = skillModel.type,
                    targetType = skillModel.target,
                    time = skillLevelData.time,
                    mp = skillLevelData.mp
                };

                skills[i] = retFightSkill;
            }


            return skills;
        }

        /// <summary>
        /// 创建建筑
        /// </summary>
        /// <param name="id">唯一id</param>
        /// <param name="code">建筑code</param>
        /// <returns></returns>
        private BuildingFightModel CreateBuildingFightModel(int id, int code)
        {
            //获取相应建筑数据
            BuildingDataModel dataModel = BuildingData.buildingMap[code];
            //构造战斗实体
            BuildingFightModel retModel = new BuildingFightModel()
            {
                id = id,
                code = code,
                atk = dataModel.atk,
                born = dataModel.reborn,
                bornTime = dataModel.rebornTime,
                curHp = dataModel.hp,
                maxHp = dataModel.hp,
                def = dataModel.def,
                infrared = dataModel.infrared,
                initiative = dataModel.initiative,
                name = dataModel.name,
                modelType = ModelType.Building
            };

            return retModel;
        }



        public void OnClientClose(UserToken token, string message)
        {
            //玩家离开战场
            Leave(token);

            //添加到离线玩家字典
            if (!offlinePlayer.ContainsKey(BizFactory.userBiz.GetInfo(token).id))
            {
                offlinePlayer.TryAdd(BizFactory.userBiz.GetInfo(token).id, token);
            }

            //判断是否所有玩家都离线
            if(offlinePlayer.Count >= playerCount.Get())
            {
                //销毁战斗房间
                EventUtil.DestroyFight(GetAreaNumber());
            }
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
                case FightProtocal.LOADING_COMPLETED_CQEQ:
                    ProcessLoadingCompleted(token);
                    break;

                case FightProtocal.MOVE_CREQ:
                    ProcessHeroMoveReq(token,model.message as HeroMoveDto);
                    break;

                case FightProtocal.ATTACK_CREQ:
                    ProcessHeroAttack(token, (int)model.message);
                    break;

                case FightProtocal.DAMAGE_CREQ:
                    ProcessDamage(token, model.message as DamageDTO);
                    break;

                case FightProtocal.SKILL_UP_CREQ:
                    ProcessSkillUp(token, (int)model.message);
                    break;
            }
        }

        /// <summary>
        /// 处理玩家加载资源完成
        /// </summary>
        /// <param name="token"></param>
        private void ProcessLoadingCompleted(UserToken token)
        {
            if (IsExist(token))
                return;

            //加入到房间token数组
            Enter(token);

            //计数+1
            enterCount.GetAndAdd();

            //判断是否全部加载完成
            if (enterCount.Get() >= playerCount.Get()) 
            {
                //广播开始战斗
                FightRoomModels models = new FightRoomModels()
                {
                    teamOne = modelTeamOne.Values.ToArray(),
                    teamTwo = modelTeamTwo.Values.ToArray()
                };
                Broadcast(FightProtocal.START_BRO, models);
            }
        }

        /// <summary>
        /// 处理玩家英雄移动
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dto"></param>
        private void ProcessHeroMoveReq(UserToken token,HeroMoveDto dto)
        {
            dto.userId = BizFactory.userBiz.GetInfo(token).id;
            Broadcast(FightProtocal.MOVE_BRO, dto);
        }

        /// <summary>
        /// 处理玩家英雄普通攻击请求
        /// </summary>
        /// <param name="token"></param>
        /// <param name="targetId"></param>
        private void ProcessHeroAttack(UserToken token,int targetId)
        {
            HeroAttackDto dto = new HeroAttackDto()
            {
                srcId = BizFactory.userBiz.GetInfo(token).id,
                targetId = targetId
            };

            Broadcast(FightProtocal.ATTACK_BRO, dto);
            Console.WriteLine(string.Format("广播->UserName:{0} attack UserName:{1}", BizFactory.userBiz.GetInfo(token).name, BizFactory.userBiz.GetInfo(BizFactory.userBiz.GetUserToken(targetId)).name));
        }

        /// <summary>
        /// 处理伤害请求
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dto"></param>
        private void ProcessDamage(UserToken token,DamageDTO dto)
        {
            if(dto.damageSrcId >= 0)
            {
                //只能请求自己的伤害
                if (dto.damageSrcId != BizFactory.userBiz.GetInfo(token).id)
                    return;

                //获取攻击源战斗模型
                AbsFightModel srcModel = GetRoomFightModel(dto.damageSrcId);

                //ret result
                int[][] retResult = new int[][] { };

                foreach (var item in dto.parameters)
                {
                    //被攻击者模型
                    AbsFightModel targetModel = GetRoomFightModel(item[0]);

                    //获取该技能当前等级
                    int skillLevel = GetSkillLevel(dto.damageSrcId, dto.skillId);

                    //获取技能伤害处理Map
                    ISkillProc attackProc = SkillProcessMap.hasSkillProc(dto.skillId) ? SkillProcessMap.GetSkillProc(dto.skillId) : null;
                    if (attackProc == null)
                    {
                        Console.WriteLine("找不到技能处理程序:" + dto.skillId.ToString());
                        return;
                    }

                    //伤害处理
                    attackProc.DamageTarget(skillLevel, ref srcModel, ref targetModel, ref retResult);

                    if (targetModel.curHp == 0)
                    {
                        switch (targetModel.modelType)
                        {
                            case ModelType.Building:
                                    //击杀了建筑
                                break;
                            case ModelType.Human:
                                if (targetModel.id >= 0)
                                {
                                    //击杀玩家
                                }
                                else
                                {
                                    //击杀小兵
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                //广播伤害处理
                dto.parameters = retResult;
                Broadcast(FightProtocal.DAMAGE_BRO, dto);
            }
            else
            {
                //小兵及建筑伤害请求
            }
        }

        /// <summary>
        /// 处理技能升级请求
        /// </summary>
        /// <param name="token"></param>
        /// <param name="skillCode"></param>
        private void ProcessSkillUp(UserToken token,int skillCode)
        {
            int selfId = BizFactory.userBiz.GetInfo(token).id;
            PlayerFightModel selfModel = GetRoomFightModel(selfId) as PlayerFightModel;
            foreach (var item in selfModel.skills)
            {
                if(item.code == skillCode)
                {
                    //找到了要升级的技能

                    if (selfModel.free > 0 && item.nextLevel!=-1)
                    {
                        //减少技能点
                        selfModel.free--;

                        //获取下一级
                        int nextLevel = item.level + 1;

                        //获取下一级的技能数据
                        SkillLevelData nextLevelData = SkillData.skillMap[skillCode].levels[nextLevel];

                        //赋值
                        item.nextLevel = nextLevelData.level;
                        item.time = nextLevelData.time;
                        item.mp = nextLevelData.mp;
                        item.range = nextLevelData.range;

                        //响应给客户端
                        Write(token, FightProtocal.SKILL_UP_SRES, item);
                    }
                }
            }

        }


        public void OnDestroy()
        {
            modelTeamOne.Clear();
            modelTeamTwo.Clear();
            enterCount = new ConcurrentInteger();
            offlinePlayer.Clear();
            playerCount = new ConcurrentInteger();
            ClearToken();
        }

        public override byte GetTypeNumber()
        {
            return Protocal.TYPE_FIGHT;
        }

        public List<int> GetAllUserId()
        {
            List<int> userIds = new List<int>();
            foreach (var kv in modelTeamOne)
            {
                userIds.Add(kv.Key);
            }
            foreach (var kv in modelTeamTwo)
            {
                userIds.Add(kv.Key);
            }

            return userIds;
        }

        private AbsFightModel GetRoomFightModel(int id)
        {
            if (modelTeamOne.ContainsKey(id))
                return modelTeamOne[id];

            return modelTeamTwo[id];
        }

        private int GetSkillLevel(int modelId,int skillId)
        {
            AbsFightModel model = GetRoomFightModel(modelId);
            if (model.modelType == ModelType.Human)
            {
                //英雄则返回技能等级
                PlayerFightModel playerModel = model as PlayerFightModel;
                foreach (var item in playerModel.skills)
                {
                    if(item.code == skillId)
                    {
                        return item.level;
                    }
                }
                
            }
            else
            {
                //非英雄直接返回-1
                return -1;
            }

            return -1;
        }

    }
}
