using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.Logic.Match
{
    public class MatchRoom
    {
        public int id;           //房间唯一id
        public int teamMax=1;    //每只队伍最大人数，1人表示1v1
        public List<int> teamOne = new List<int>(); //队伍1
        public List<int> teamTwo = new List<int>(); //队伍2
    }
}
