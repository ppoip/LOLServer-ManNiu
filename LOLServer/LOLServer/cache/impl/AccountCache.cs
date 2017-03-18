using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using LOLServer.dao.model;

namespace LOLServer.cache.impl
{
    public class AccountCache : IAccountCache
    {
        /// <summary> 当前在线用户 map(连接对象-登陆账号) </summary>
        private Dictionary<UserToken, string> onlineAccMap = new Dictionary<UserToken, string>();

        /// <summary> account表中所有行映射 map(账号，行) </summary>
        private Dictionary<string, AccountModel> accMap = new Dictionary<string, AccountModel>();

        /// <summary> 主键索引 </summary>
        private int idIndex = 0;

        public void Add(string account, string password)
        {
            AccountModel am = new AccountModel();
            am.account = account;
            am.password = password;
            am.id = idIndex++;
            accMap.Add(account, am);
        }

        public int GetID(UserToken token)
        {
            if (!onlineAccMap.ContainsKey(token))
            {
                return -1;
            }

            return accMap[onlineAccMap[token]].id;
        }

        public bool HasAccount(string account)
        {
            return accMap.ContainsKey(account);
        }

        public bool IsOnline(string account)
        {
            return onlineAccMap.ContainsValue(account);
        }

        public bool Match(string account, string password)
        {
            if (!accMap.ContainsKey(account))
                return false;

            return accMap[account].password.Equals(password);
        }

        public void Offline(UserToken token)
        {
            onlineAccMap.Remove(token);
        }

        public void Online(UserToken token, string account)
        {
            onlineAccMap.Add(token, account);
        }
    }
}
