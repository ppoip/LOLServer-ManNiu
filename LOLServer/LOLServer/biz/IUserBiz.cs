using GameProtocal.dto;
using NetFrame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.biz
{
    public interface IUserBiz
    {
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        bool Create(UserToken token,string name);

        /// <summary>
        /// 获取该连接对象所登陆的账号下的第一个角色的信息
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
        /// 判断该连接对象对应的角色是否在线
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool IsOnline(UserToken token);

        /// <summary>
        /// 上线该连接对象对应的角色
        /// </summary>
        /// <param name="token"></param>
        bool Online(UserToken token);

        /// <summary>
        /// 下线该连接对象对应的角色
        /// </summary>
        /// <param name="token"></param>
        void Offline(UserToken token);

        /// <summary>
        /// 通过userId获取UserToken
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserToken GetUserToken(int userId);
    }
}
