using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using GameProtocal;
using GameCommon;
using LOLServer.tools;
using LOLServer.biz;

namespace LOLServer.Logic.User
{
    public class UserHandler : AbsOnceHandler, IHandler
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
            SocketModel model = message as SocketModel;
            switch (model.command)
            {
                case UserProtocal.CREATE_CREQ:
                    ProcessCreate(token, model.GetMessage<string>());
                    break;

                case UserProtocal.GET_INFO_CREQ:
                    ProcessGetInfo(token);
                    break;

                case UserProtocal.ONLINE_CREQ:
                    ProcessOnline(token);
                    break;
            }
        }

        /// <summary>
        /// 处理创建角色
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        private void ProcessCreate(UserToken token,string name)
        {
            ExecutorPool.Instance.Execute(() => 
            {
                Write(token, UserProtocal.CREATE_SRES, 
                    BizFactory.userBiz.Create(token, name));
            });
        }

        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <param name="token"></param>
        private void ProcessGetInfo(UserToken token)
        {
            ExecutorPool.Instance.Execute(() =>
            {
                Write(token, UserProtocal.GET_INFO_SRES, 
                    BizFactory.userBiz.GetInfo(token));
            });
        }

        /// <summary>
        /// 角色上线
        /// </summary>
        /// <param name="token"></param>
        private void ProcessOnline(UserToken token)
        {
            ExecutorPool.Instance.Execute(() =>
            {
                Write(token, UserProtocal.ONLINE_SRES,
                    BizFactory.userBiz.Online(token));
            });
        }

        public override byte GetTypeNumber()
        {
            //模块码
            return Protocal.TYPE_USER;
        }
    }
}
