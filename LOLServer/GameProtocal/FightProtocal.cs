using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal
{
    public class FightProtocal
    {
        public const int LOADING_COMPLETED_CQEQ = 0; //player资源加载完成请求
        public const int START_BRO = 1;  //所有player均完成资源加载，开始战斗

        public const int MOVE_CREQ = 2;  //player移动请求
        public const int MOVE_BRO = 3;   //player移动广播

        public const int SKILL_CREQ = 4; //技能使用请求
        public const int SKILL_BRO = 5;  //技能使用广播，通知player的英雄播放动画

        public const int SKILL_UP_CREQ = 6; //技能升级请求
        public const int SKILL_UP_SRES = 7; //技能升级响应

        public const int ATTACK_CREQ = 8;  //平A请求
        public const int ATTACK_BRO = 9;   //平A广播

        public const int DAMAGE_CREQ = 10; //造成伤害请求，在播放英雄攻击动画时得到该请求
        public const int DAMAGE_BRO = 11;  //造成伤害广播

    }
}
