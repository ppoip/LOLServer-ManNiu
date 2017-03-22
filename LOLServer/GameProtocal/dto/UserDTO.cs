using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal.dto
{
    [Serializable]
    public class UserDTO
    {
        public int id;
        public int accountId;  //所属账号id
        public string name;
        public int level;
        public int exp;
        public int winCount;
        public int loseCount;
        public int ranCount;    //逃跑场次

        public UserDTO() { }
    }
}
