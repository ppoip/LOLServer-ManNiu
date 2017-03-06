using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
    public  class UserTokenPool
    {
        private Stack<UserToken> pool;

        public UserTokenPool(int max)
        {
            pool = new Stack<UserToken>(max);
        }

        /// <summary>
        /// 插入一个连接对象--释放连接
        /// </summary>
        /// <param name="token"></param>
        public void Push(UserToken token)
        {
            pool.Push(token);
        }

        /// <summary>
        /// 取出一个连接对象--创建连接
        /// </summary>
        /// <returns></returns>
        public UserToken Pop()
        {
            return pool.Pop();
        }

        public int Size
        {
            get
            {
                return pool.Count;
            }
        }
    }
}
