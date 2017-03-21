﻿using LOLServer.cache.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.cache
{
    public class CacheFactory
    {
        public static IAccountCache accountCache;
        public static IUserCache userCache;

        static CacheFactory()
        {
            accountCache = new AccountCache();
            userCache = new UserCache();
        }
    }
}
