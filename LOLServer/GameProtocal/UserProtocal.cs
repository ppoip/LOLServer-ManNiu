using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal
{
    public class UserProtocal
    {
        public const int CREATE_CREQ = 0;      //创建角色
        public const int CREATE_SRES = 1;

        public const int GET_INFO_CREQ = 2;    //获取角色信息
        public const int GET_INFO_SRES = 3;

        public const int ONLINE_CREQ = 4;      //角色上线
        public const int ONLINE_SRES = 5;
    }
}
