using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal.dto.fight
{
    [Serializable]
    public class AbsFightModel
    {
        /// <summary> 战斗区域中 唯一识别码 </summary>
        public int id;

        /// <summary> 模型唯一识别码，但是战斗中会有多个相同兵种出现，所以这里只用于标志形象获取对应数据 </summary>
        public int code;

        /// <summary> 当前hp </summary>
        public int curHp;

        /// <summary> 最大hp </summary>
        public int maxHp;

        /// <summary> 攻击 </summary>
        public int atk;

        /// <summary> 防御 </summary>
        public int def;

        /// <summary> 名称 </summary>
        public string name;

        /// <summary> 移动速度 </summary>
        public float speed;

        /// <summary> 攻击速度 </summary>
        public float atkSpeed;

        /// <summary> 攻击范围 </summary>
        public float atkRange;

        /// <summary> 视野范围 </summary>
        public float viewRange;

        /// <summary> 模型类型 </summary>
        public ModelType modelType;
    }

    public enum ModelType
    {
        Building,   //建筑
        Human       //生命体
    }


}
