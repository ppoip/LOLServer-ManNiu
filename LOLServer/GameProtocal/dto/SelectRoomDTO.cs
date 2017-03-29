using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal.dto
{
    [Serializable]
    public class SelectRoomDTO
    {
        public SelectModel[] teamOne;
        public SelectModel[] teamTwo;
    }
}
