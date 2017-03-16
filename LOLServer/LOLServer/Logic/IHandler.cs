using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;

namespace LOLServer.Logic
{
    interface IHandler
    {
        void OnClientConnect(UserToken token);

        void OnClientClose(UserToken token, string message);

        void OnMessageReceive(UserToken token, object message);
    }
}
