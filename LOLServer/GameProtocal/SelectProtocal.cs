using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal
{
    public class SelectProtocal
    {
        //玩家进入选人房间
        public const int ENTER_CREQ = 0;
        public const int ENTER_SRES = 1;
        public const int ENTER_BRO = 2;

        //玩家选择了一个英雄
        public const int SELECT_CREQ = 3;
        public const int SELECT_SRES = 4;  //选择失败才会发送该响应，否则广播select
        public const int SELECT_BRO = 5;

        //玩家发送文字聊天
        public const int TALK_CREQ = 6;
        public const int TALK_BRO = 7;

        //玩家准备了（按下选择英雄时的确定按钮）
        public const int READY_CREQ = 8;
        public const int READY_BRO = 9;

        //房间被销毁，可能是有玩家退出了
        public const int DESTROY_BRO = 10;

        //所有人都准备好了，可以进入战斗场景
        public const int FIGHT_BRO = 11;
    }
}
