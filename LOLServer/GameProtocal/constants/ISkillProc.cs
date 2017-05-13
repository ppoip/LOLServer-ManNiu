using GameProtocal.dto.fight;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal.constants
{
    public interface ISkillProc
    {
        void DamageTarget(int skillLevel,ref AbsFightModel srcModel,ref AbsFightModel targetModel,ref int[][] outputParameters);
    }
}
