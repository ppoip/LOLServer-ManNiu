using System;
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

namespace LOLServer.Logic.Fight
{
    public class FightRoom : AbsMultiHandler, IHandler
    {
        /// <summary> 队伍1战斗模型 </summary>
        private ConcurrentDictionary<int, AbsFightModel> modelTeamOne = new ConcurrentDictionary<int, AbsFightModel>();

        /// <summary> 队伍2战斗模型 </summary>
        private ConcurrentDictionary<int, AbsFightModel> modelTeamTwo = new ConcurrentDictionary<int, AbsFightModel>();


        public void Init(SelectModel[] teamOne, SelectModel[] teamTwo)
        {
            //初始化玩家战斗模型
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
            //TODO



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
                skills = CreateFightSkills(heroDataModel.skills)  //创建技能模型
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
                    time = skillLevelData.time
                };

                skills[i] = retFightSkill;
            }


            return skills;
        }

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
            throw new NotImplementedException();
        }

        public void OnDestroy()
        {
            //TODO
        }

        public override byte GetTypeNumber()
        {
            return Protocal.TYPE_FIGHT;
        }

        public List<int> GetAllUserId()
        {
            //TODO
            return null;
        }
    }
}
