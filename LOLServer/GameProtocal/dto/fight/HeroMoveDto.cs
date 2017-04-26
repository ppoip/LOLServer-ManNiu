using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal.dto.fight
{
    [Serializable]
    public class HeroMoveDto
    {
        public int userId;
        public float x;
        public float y;
        public float z;
    }
}
