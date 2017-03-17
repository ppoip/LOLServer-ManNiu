﻿using LOLServer.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProtocal;
using LOLServer.Logic.Login;
using NetFrame;
using GameCommon;

namespace LOLServer
{
    public class HandlerCenter : AbsHandlerCenter
    {
        /// <summary> handler字典 </summary>
        private Dictionary<byte, IHandler> handlers = new Dictionary<byte, IHandler>();

        public HandlerCenter()
        {
            //初始化所有handler
            handlers.Add(Protocal.TYPE_LOGIN, new LoginHandler());
        }



        public override void OnClientClose(UserToken token, string message)
        {
            Console.WriteLine("有客户端断开连接，Message:"+message);
        }

        public override void OnClientConnect(UserToken token)
        {
            Console.WriteLine("有客户端连接了。");
        }

        public override void OnMessageReceive(UserToken token, object message)
        {
            SocketModel sm = message as SocketModel;
            IHandler handler = null;
            if (handlers.ContainsKey(sm.type))
            {
                //获取对应的handler
                handler = handlers[sm.type];
                //handler处理
                handler.OnMessageReceive(token, message);
            }
        }
    }
}