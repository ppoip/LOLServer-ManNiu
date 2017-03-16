using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;
using GameCommon;

namespace LOLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerStart server = new ServerStart(5000);
            server.PE = MessageEncoding.Encode;
            server.PD = MessageEncoding.Decode;
            server.LE = LengthEncoding.encode;
            server.LD = LengthEncoding.decode;
            server.center = new HandlerCenter();
            server.Start(6550);
            Console.WriteLine("server is starting at 127.0.0.1:6568");
            while (true);
        }
    }
}
