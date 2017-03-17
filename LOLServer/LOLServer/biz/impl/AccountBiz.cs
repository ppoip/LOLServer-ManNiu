using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;

namespace LOLServer.biz.account.impl
{
    public class AccountBiz : IAccountBiz
    {
        public void Close(UserToken token)
        {
            throw new NotImplementedException();
        }

        public int Create(UserToken token, string account, string password)
        {
            throw new NotImplementedException();
        }

        public int GetID(UserToken token)
        {
            throw new NotImplementedException();
        }

        public int Login(UserToken token, string account, string password)
        {
            throw new NotImplementedException();
        }
    }
}
