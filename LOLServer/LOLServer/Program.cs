using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetFrame;

namespace LOLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerStart server = new ServerStart(5000);
            server.Start(6568);
            //while (true) ;
        }
    }
}
