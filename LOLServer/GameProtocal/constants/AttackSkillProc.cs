using System;
using System.Collections.Generic;
using System.Text;
using GameProtocal.dto.fight;

namespace GameProtocal.constants
{
    /// <summary>
    /// 普通攻击伤害处理程序
    /// </summary>
    public class AttackSkillProc : ISkillProc
    {
        public void DamageTarget(int skillLevel, ref AbsFightModel srcModel, ref AbsFightModel targetModel, ref int[][] outputParameters)
        {
            //最小伤害为1
            int damage = srcModel.atk - targetModel.def > 0 ? srcModel.atk - targetModel.def : 1;
            int remainHP = targetModel.curHp - damage > 0 ? targetModel.curHp -= damage : targetModel.curHp = 0;

            List<int[]> results = new List<int[]>(outputParameters);
            results.Add(new int[] { targetModel.id, damage, remainHP });  //被攻击的目标的id | 造成的伤害 | 目标剩余血量
            outputParameters = results.ToArray();
        }
    }
}
