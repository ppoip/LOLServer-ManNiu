using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal.constants
{
    /// <summary>
    /// 英雄属性表
    /// </summary>
    public class HeroData
    {
        /// <summary> 所有英雄 map(heroId,HeroDataModel) </summary>
        public static readonly Dictionary<int, HeroDataModel> heroMap = new Dictionary<int, HeroDataModel>();

        static HeroData()
        {
            Create(1, "阿狸", 100, 20, 500, 300, 5, 2, 30, 10, 1, 0.5f, 6, 1, 2, 3, 4);
            Create(2, "阿木木", 100, 20, 500, 300, 5, 2, 30, 10, 1, 0.5f, 3, 1, 2, 3, 4);
            Create(3, "埃希", 100, 20, 500, 300, 5, 2, 30, 10, 1, 0.5f, 1, 6, 2, 3, 4);
            Create(4, "盲僧", 100, 20, 500, 300, 5, 2, 30, 10, 1, 0.5f, 1, 3, 2, 3, 4);
        }

        static void Create(
            int code, 
            string name, 
            int atkBase, 
            int defBase, 
            int hpBase, 
            int mpBase, 
            int atkArr, 
            int defArr, 
            int hpArr, 
            int mpArr, 
            float speed, 
            float atkSpeed, 
            float range, 
            float viewRange,
            params int[] skills)
        {
            HeroDataModel model = new HeroDataModel();
            model.code = code;
            model.name = name;
            model.atkBase = atkBase;
            model.defBase = defBase;
            model.hpBase = hpBase;
            model.mpBase = mpBase;
            model.atkArr = atkArr;
            model.defArr = defArr;
            model.hpArr = hpArr;
            model.mpArr = mpArr;
            model.speed = speed;
            model.atkSpeed = atkSpeed;
            model.range = range;
            model.viewRange = viewRange;
            model.skills = skills;
            heroMap.Add(code, model);
        }


        public class HeroDataModel
        {
            /// <summary> 策划定义的唯一编号 </summary>
            public int code;

            /// <summary> 英雄名称 </summary>
            public string name;

            /// <summary> 初始（基础）攻击力 </summary>
            public int atkBase;

            /// <summary> 防御基础 </summary>
            public int defBase;

            /// <summary> 初始血量 </summary>
            public int hpBase;

            /// <summary> 初始蓝 </summary>
            public int mpBase;

            /// <summary> 攻击成长 </summary>
            public int atkArr;

            /// <summary> 防御成长 </summary>
            public int defArr;

            /// <summary> 血量成长 </summary>
            public int hpArr;

            /// <summary> 蓝成长 </summary>
            public int mpArr;

            /// <summary> 移动速度 </summary>
            public float speed;

            /// <summary> 攻击速度 </summary>
            public float atkSpeed;

            /// <summary> 攻击距离 </summary>
            public float range;

            /// <summary> 视野范围 </summary>
            public float viewRange;

            /// <summary> 拥有技能 </summary>
            public int[] skills;
        }
    }
}
