using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal.constants
{
    public class BuildingData
    {
        public static Dictionary<int, BuildingDataModel> buildingMap = new Dictionary<int, BuildingDataModel>();

        static BuildingData()
        {
            Create(1,"主基地", 5000, 0, 50, false, true, false, 0);
            Create(2,"高级箭塔", 3000, 200, 50, false, true, true, 30);
            Create(3,"中级箭塔", 2000, 150, 30, true, true, false, 0);
            Create(4,"初级箭塔", 1000, 100, 20, true, true, false, 0);
        }

        static void Create(int code, string name, int hp, int atk, int def, bool initiative, bool infrared, bool reborn, int rebornTime)
        {
            BuildingDataModel model = new BuildingDataModel();
            model.atk = atk;
            model.code = code;
            model.def = def;
            model.hp = hp;
            model.infrared = infrared;
            model.initiative = initiative;
            model.name = name;
            model.reborn = reborn;
            model.rebornTime = rebornTime;
            buildingMap.Add(code, model);
        }


        public class BuildingDataModel
        {
            /// <summary> 箭塔id </summary>
            public int code;

            /// <summary> 箭塔血量 </summary>
            public int hp;

            /// <summary> 箭塔攻击 </summary>
            public int atk;

            /// <summary> 箭塔防御 </summary>
            public int def;

            /// <summary> 是否攻击形建筑 </summary>
            public bool initiative;

            /// <summary> 是否反隐 </summary>
            public bool infrared;

            /// <summary> 名字 </summary>
            public string name;

            /// <summary> 是否复活 </summary>
            public bool reborn;

            /// <summary> 复活时间 </summary>
            public int rebornTime;
        }
    }
}
