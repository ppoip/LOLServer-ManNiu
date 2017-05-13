using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal.dto.fight
{
    [Serializable]
    public class DamageDTO
    {
        /// <summary> 攻击发起者 </summary>
        public int damageSrcId;

        /// <summary> 所使用的技能 </summary>
        public int skillId;

        /// <summary> 参数 </summary>
        public int[][] parameters;
    }
}
