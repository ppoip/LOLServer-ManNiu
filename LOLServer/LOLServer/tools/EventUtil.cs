using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOLServer.tools
{
    public class EventUtil
    {
        public static Action<List<int>, List<int>> CreateSelect;
        public static Action<int> DestroySelect;
    }
}
