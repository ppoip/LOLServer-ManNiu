using LOLServer.biz;
using LOLServer.biz.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.biz
{
    public class BizFactory
    {
        public static readonly IAccountBiz accountBiz;

        static BizFactory()
        {
            accountBiz = new AccountBiz();
        }
    }
}
