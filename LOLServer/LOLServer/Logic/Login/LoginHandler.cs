using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using GameCommon;
using GameProtocal;
using GameProtocal.dto;

namespace LOLServer.Logic.Login
{
    public class LoginHandler : IHandler
    {
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
            SocketModel sm = message as SocketModel;
            switch (sm.command)
            {
                case LoginProtocal.LOGIN_CREQ:
                    OnLoginRequest(token, sm.GetMessage<AccountInfoDTO>());
                    break;

                case LoginProtocal.REG_CREQ:
                    OnRegRequest(token, sm.GetMessage<AccountInfoDTO>());
                    break;
            }
        }

        private void OnLoginRequest(UserToken token,AccountInfoDTO dto)
        {

        }

        private void OnRegRequest(UserToken token, AccountInfoDTO dto)
        {

        }
    }
}
