using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using NetFrame.auto;

namespace LOLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerStart server = new ServerStart(5000);
            server.PE = MessageEncoding.Encode;
            server.PD = MessageEncoding.Decode;
            server.Start(6568);
            while (true);
        }
    }
}
