using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using LOLServer.tools;

namespace LOLServer.Logic.Select
{
    public class SelectHandler : AbsOnceHandler, IHandler
    {

        public SelectHandler()
        {
            EventUtil.CreateSelect = Create;
            EventUtil.DestroySelect = Destroy;
        }

        private void Create(List<int> teamOne,List<int> teamTwo)
        {

        }

        private void Destroy(int roomId)
        {

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
