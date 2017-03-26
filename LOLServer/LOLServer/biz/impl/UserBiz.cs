using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProtocal.dto;
using NetFrame;
using LOLServer.cache;

namespace LOLServer.biz.impl
{
    public class UserBiz : IUserBiz
    {
        public bool Create(UserToken token, string name)
        {
            //登陆后才可以创建
            if (BizFactory.accountBiz.GetID(token) == -1)
                return false;

            return CacheFactory.userCache.Add(token, name, BizFactory.accountBiz.GetID(token));
        }

        public UserDTO GetInfo(UserToken token)
        {
            return CacheFactory.userCache.GetInfo(token);
        }

        public UserDTO GetInfoByAccountID(int accountID)
        {
            return CacheFactory.userCache.GetInfoByAccountID(accountID);
        }

        public UserToken GetUserToken(int userId)
        {
            return CacheFactory.userCache.GetUserToken(userId);
        }

        public bool IsOnline(UserToken token)
        {
            return CacheFactory.userCache.IsOnline(token);
        }

        public void Offline(UserToken token)
        {
            CacheFactory.userCache.Offline(token);
        }

        public bool Online(UserToken token)
        {
            return CacheFactory.userCache.Online(token);
        }
    }
}
