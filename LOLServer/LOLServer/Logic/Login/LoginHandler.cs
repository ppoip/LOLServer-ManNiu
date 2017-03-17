using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using GameCommon;
using GameProtocal;
using GameProtocal.dto;
using LOLServer.tools;
using LOLServer.biz;

namespace LOLServer.Logic.Login
{
    public class LoginHandler : AbsOnceHandler,IHandler
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
            //获取model
            SocketModel sm = message as SocketModel;
            //判断三级协议
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

        /// <summary>
        /// 处理登陆请求
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dto"></param>
        private void OnLoginRequest(UserToken token,AccountInfoDTO dto)
        {
            ExecutorPool.Instance.Execute(()=> 
            {
                //处理登陆
                int result = BizFactory.accountBiz.Login(token, dto.account, dto.password);
                //返回结果给客户端
                Write(token, LoginProtocal.LOGIN_SRES, result);
            });
        }

        /// <summary>
        /// 处理注册请求
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dto"></param>
        private void OnRegRequest(UserToken token, AccountInfoDTO dto)
        {
            ExecutorPool.Instance.Execute(() => 
            {
                //处理注册请求
                int result = BizFactory.accountBiz.Create(token, dto.account, dto.password);
                //返回结果给客户端
                Write(token, LoginProtocal.REG_SRES, result);
            });
        }

        /// <summary>
        /// 重写返回的type码
        /// </summary>
        /// <returns></returns>
        public override byte GetTypeNumber()
        {
            return Protocal.TYPE_LOGIN;
        }
    }
}
