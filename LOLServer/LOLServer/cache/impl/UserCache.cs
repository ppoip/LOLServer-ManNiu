using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProtocal.dto;
using NetFrame;
using LOLServer.dao.model;
using LOLServer.biz;

namespace LOLServer.cache.impl
{
    public class UserCache : IUserCache
    {
        /// <summary> 在线角色表，map(连接对象，角色ID) </summary>
        private Dictionary<UserToken, int> onlineUsers = new Dictionary<UserToken, int>();

        /// <summary> user表所有行映射 map(账号id,角色行数据) </summary>
        private Dictionary<int, UserModel> userMap = new Dictionary<int, UserModel>();

        /// <summary> 主键索引 </summary>
        int idIndex = 0;

        public bool Add(UserToken token, string name,int accountId)
        {
            //是否已经有角色
            if (Exist(token))
                return false;

            userMap.Add(idIndex, new UserModel()
            {
                name = name,
                id = idIndex,
                exp = 0,
                level = 0,
                loseCount = 0,
                ranCount = 0,
                winCount = 0,
                accountId = accountId,
                ownHeroList = new List<int>()
            });
            idIndex++;
            return true;
        }

        public bool Exist(UserToken token)
        {
            return userMap.ContainsKey(BizFactory.accountBiz.GetID(token));
        }

        public UserDTO GetInfo(UserToken token)
        {
            //如果不存在角色
            /*
            if (!Exist(token))
                return null;    //error : null无法序列化
            */
            if (!Exist(token))
                return new UserDTO(); //返回空信息

            UserModel model = userMap[BizFactory.accountBiz.GetID(token)];
            UserDTO dto = new UserDTO()
            {
                exp = model.exp,
                id = model.id,
                level = model.level,
                loseCount = model.loseCount,
                name = model.name,
                ranCount = model.ranCount,
                winCount = model.winCount,
                accountId = model.accountId,
                ownHeroList = model.ownHeroList
            };
            return dto;
        }

        public UserDTO GetInfoByAccountID(int accountID)
        {
            //如果不存在角色
            if (!userMap.ContainsKey(accountID))
                return null;

            UserModel model = userMap[accountID];
            UserDTO dto = new UserDTO()
            {
                exp = model.exp,
                id = model.id,
                level = model.level,
                loseCount = model.loseCount,
                name = model.name,
                ranCount = model.ranCount,
                winCount = model.winCount,
                ownHeroList = model.ownHeroList
            };
            return dto;
        }

        public UserToken GetUserToken(int userId)
        {
            foreach (var kv in onlineUsers)
            {
                if (kv.Value == userId) 
                {
                    return kv.Key;
                }
            }
            return null;
        }

        public bool IsOnline(UserToken token)
        {
            return onlineUsers.ContainsKey(token);
        }

        public void Offline(UserToken token)
        {
            //是否在线
            if (IsOnline(token))
            {
                onlineUsers.Remove(token);
            }
        }

        public bool Online(UserToken token)
        {
            //是否已经在线
            if (!IsOnline(token))
            {
                onlineUsers.Add(token, userMap[BizFactory.accountBiz.GetID(token)].id);
                return true;
            }

            return false;
        }
    }
}
