using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal.dto.fight
{
    [Serializable]
    public class FightSkill
    {
        /// <summary> 技能代码 </summary>
        public int code;

        /// <summary> 技能等级 </summary>
        public int level;

        /// <summary> 下一级学习需要的英雄等级 </summary>
        public int nextLevel;

        /// <summary> 冷却时间--ms </summary>
        public int time;

        /// <summary> 技能名称 </summary>
        public string name;

        /// <summary> 技能描述 </summary>
        public string info;

        /// <summary> 释放距离 </summary>
        public float range;

        /// <summary> 技能伤害目标类型 </summary>
        public SkillTargetType targetType;

        /// <summary> 技能释放类型 </summary>
        public SkillType skillType;
    }

    /// <summary>
    /// 能够造成效果的单位类型
    /// </summary>
    public enum SkillTargetType
    {
        SELF,  //自身释放
        F_H,   //友方英雄
        F_N_B, //友方非建筑单位
        F_ALL, //友方全体
        E_H,   //敌方英雄
        E_N_B, //敌方非建筑
        E_S_N, //敌方和中立单位
        N_F_ALL//非友方单位
    }

    /// <summary>
    /// 技能释放类型
    /// </summary>
    public enum SkillType
    {
        SELF,     //以自身为中心释放
        TARGET,   //以目标为中心释放
        POSITION, //以鼠标点击位置释放
        PASSIVE   //被动技能
    }
}
