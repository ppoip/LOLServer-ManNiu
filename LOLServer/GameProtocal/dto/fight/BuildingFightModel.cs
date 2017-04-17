using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal.dto.fight
{
    [Serializable]
    public class BuildingFightModel:AbsFightModel
    {
        /// <summary> 是否重生 </summary>
        public bool born;

        /// <summary> 重生时间 </summary>
        public int bornTime;

        /// <summary> 是否攻击型建筑 </summary>
        public bool initiative;

        /// <summary> 红外线（是否反隐） </summary>
        public bool infrared;

    }
}
