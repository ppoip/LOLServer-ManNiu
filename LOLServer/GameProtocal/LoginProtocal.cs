using System;
using System.Collections.Generic;
using System.Text;

namespace GameProtocal
{
    public class LoginProtocal
    {
        public const int LOGIN_CREQ = 0;   //登陆，客户端请求
        public const int LOGIN_SRES = 1;   //登陆，服务端响应

        public const int REG_CREQ = 2;     //注册，客户端请求
        public const int REG_SRES = 3;     //注册，服务端响应
    }
}
