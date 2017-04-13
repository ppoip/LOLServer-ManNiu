using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal
{
    public class Protocal
    {
        /// <summary> 登陆，注册，账号上线模块 </summary>
        public const byte TYPE_LOGIN = 0;   

        /// <summary> 创建角色，获取角色，上线角色模块 </summary>
        public const byte TYPE_USER = 1;

        /// <summary> 战斗匹配 </summary>
        public const byte TYPE_MATCH = 2;

        /// <summary> 选人模块 </summary>
        public const byte TYPE_SELECT = 3;

        /// <summary> 战斗模块 </summary>
        public const byte TYPE_FIGHT = 4;

    }
}
