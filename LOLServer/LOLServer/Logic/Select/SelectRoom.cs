using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using GameProtocal;
using LOLServer.biz;

namespace LOLServer.Logic.Select
{
    public class SelectRoom : AbsMultiHandler, IHandler
    {
        public List<int> teamOne = new List<int>(); //队伍1
        public List<int> teamTwo = new List<int>(); //队伍2

        public void Init(List<int> teamOne, List<int> teamTwo)
        {
            //Copy teamOne,teamTwo
            foreach (int userId in teamOne)
            {
                this.teamOne.Add(userId);
            }
            foreach (int userId in teamTwo)
            {
                this.teamTwo.Add(userId);
            }
            //token enter
            foreach(int userId in teamOne)
            {
                UserToken token = BizFactory.userBiz.GetUserToken(userId);
                Enter(token);
            }
            foreach (int userId in teamTwo)
            {
                UserToken token = BizFactory.userBiz.GetUserToken(userId);
                Enter(token);
            }
        }


        /// <summary>
        /// 设置Type码
        /// </summary>
        /// <returns></returns>
        public override byte GetTypeNumber()
        {
            return Protocal.TYPE_SELECT;
        }


        public void OnClientClose(UserToken token, string message)
        {
            throw new NotImplementedException();
        }

        public void OnClientConnect(UserToken token)
        {
            throw new NotImplementedException();
        }

        public void OnMessageReceive(UserToken token, object message)
        {
            throw new NotImplementedException();
        }
    }
}
