using GameProtocal.dto;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.cache
{
    public interface IUserCache
    {
        /// <summary>
        /// 添加一个角色到表映射
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool Add(UserToken token, string name);

        /// <summary>
        /// 该连接对象登陆的账号是否已存在角色
        /// 必须在登陆后才可调用
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool Exist(UserToken token);

        /// <summary>
        /// 获取该连接对象对应的角色信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserDTO GetInfo(UserToken token);

        /// <summary>
        /// 通过账号id获取角色信息
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        UserDTO GetInfoByAccountID(int accountID);

        /// <summary>
        /// 是否在线
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool IsOnline(UserToken token);

        /// <summary>
        /// 上线
        /// </summary>
        /// <param name="token"></param>
        bool Online(UserToken token);

        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="token"></param>
        void Offline(UserToken token);
    }
}
