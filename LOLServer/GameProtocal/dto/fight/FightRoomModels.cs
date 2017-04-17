using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal.dto.fight
{
    [Serializable]
    public class FightRoomModels
    {
        public AbsFightModel[] teamOne;
        public AbsFightModel[] teamTwo;
    }
}
