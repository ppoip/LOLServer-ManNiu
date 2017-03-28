using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal
{
    public class MatchProtocal
    {
        public const int ENTER_CREQ = 0;  //进入匹配队列
        public const int ENTER_SRES = 1;

        public const int LEAVE_CREQ = 2;  //离开队列
        public const int LEAVE_SRES = 3;

        public const int ENTER_SELECT_BRO = 4; //广播，匹配完毕，通知进入选人界面
    }
}
