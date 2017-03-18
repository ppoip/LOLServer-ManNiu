using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.cache
{
    public interface IAccountCache
    {
        /// <summary>
        /// 账号是否存在
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        bool HasAccount(string account);

        /// <summary>
        /// 账号密码是否匹配
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Match(string account, string password);

        /// <summary>
        /// 账号是否在线
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        bool IsOnline(string account);

        /// <summary>
        /// 获取连接对象登陆的id
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        int GetID(UserToken token);

        /// <summary>
        /// 账号上线
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        void Online(UserToken token, string account);

        /// <summary>
        /// 用户下线
        /// </summary>
        /// <param name="token"></param>
        void Offline(UserToken token);

        /// <summary>
        /// 添加账号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        void Add(string account, string password);
    }
}
