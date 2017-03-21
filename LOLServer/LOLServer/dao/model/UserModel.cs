using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.dao.model
{
    public class UserModel
    {
        public int id;
        public string name;
        public int level;
        public int exp;
        public int winCount;
        public int loseCount;
        public int ranCount;    //逃跑场次
    }
}
