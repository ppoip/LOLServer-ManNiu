using GameProtocal.dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.tools
{
    public class EventUtil
    {
        /// <summary> 指向SelectHandler的Create </summary>
        public static Action<List<int>, List<int>> CreateSelect;

        /// <summary> 指向SelectHandler的Destroy </summary>
        public static Action<int> DestroySelect;

        /// <summary> 指向FightHandler的Create </summary>
        public static Action<SelectModel[], SelectModel[]> CreateFight;

        /// <summary> 指向FightHandler的Destroy </summary>
        public static Action<int> DestroyFight;
    }
}
