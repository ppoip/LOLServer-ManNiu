using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal.dto.fight
{
    [Serializable]
    public class PlayerFightModel:AbsFightModel
    {
        /// <summary> 等级 </summary>
        public int level;

        /// <summary> 经验 </summary>
        public int exp;

        /// <summary> 剩余的技能点 </summary>
        public int free;

        /// <summary> 金钱 </summary>
        public int money;

        /// <summary> 玩家装备 </summary>
        public int[] equs;

        /// <summary> 当前mp </summary>
        public int curMp;

        /// <summary> 最大mp </summary>
        public int maxMp;

        /// <summary> 玩家拥有的技能 </summary>
        public FightSkill[] skills;
        
    }
}
