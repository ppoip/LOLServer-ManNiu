﻿using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.biz
{
    public interface IAccountBiz
    {
        /// <summary>
        /// 创建账号
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns>0成功，1账号重复，2账号不合法</returns>
        int Create(UserToken token, string account, string password);

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns>0成功，-1账号不存在，-2账号已经在线，-3密码错误，-4输入不合法</returns>
        int Login(UserToken token, string account, string password);

        /// <summary>
        /// 客户端断开连接（下线）
        /// </summary>
        /// <param name="token"></param>
        void Close(UserToken token);

        /// <summary>
        /// 获取连接对象登陆的账号ID
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        int GetID(UserToken token);
    }
}
