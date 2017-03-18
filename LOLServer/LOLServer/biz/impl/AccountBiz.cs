using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using LOLServer.cache;

namespace LOLServer.biz.impl
{
    public class AccountBiz : IAccountBiz
    {
        public void Close(UserToken token)
        {
            //下线
            CacheFactory.accountCache.Offline(token);
        }

        public int Create(UserToken token, string account, string password)
        {
            //判断输入是否合法
            if (account == null || password == null)
                return 2;

            //判断账号是否已存在
            if (CacheFactory.accountCache.HasAccount(account))
                return 1;

            //创建账号
            CacheFactory.accountCache.Add(account, password);
            return 0;
        }

        public int GetID(UserToken token)
        {
            //返回当前token所登陆的账号的id
            return CacheFactory.accountCache.GetID(token);
        }

        public int Login(UserToken token, string account, string password)
        {
            //判断输入是否合法
            if (account == null || password == null)
                return -4;

            //判断账号是否不存在
            if (!CacheFactory.accountCache.HasAccount(account))
                return -1;

            //判断账号是否在线
            if (CacheFactory.accountCache.IsOnline(account))
                return -2;

            //判断密码是否正确
            if (!CacheFactory.accountCache.Match(account, password))
                return -3;

            CacheFactory.accountCache.Online(token, account);
            return 0;
        }
    }
}
