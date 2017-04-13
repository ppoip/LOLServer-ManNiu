using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using GameProtocal.dto;
using GameProtocal;

namespace LOLServer.Logic.Fight
{
    public class FightRoom : AbsMultiHandler, IHandler
    {
        public void Init(SelectModel[] teamOne, SelectModel[] teamTwo)
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

        public void OnDestroy()
        {
            //TODO
        }

        public override byte GetTypeNumber()
        {
            return Protocal.TYPE_FIGHT;
        }

        public List<int> GetAllUserId()
        {
            //TODO
            return null;
        }
    }
}
