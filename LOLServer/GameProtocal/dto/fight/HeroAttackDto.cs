using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal.dto.fight
{
    [Serializable]
    public class HeroAttackDto
    {
        public int srcId;
        public int targetId;
    }
}
