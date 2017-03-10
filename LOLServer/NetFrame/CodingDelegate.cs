using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFrame
{
    public delegate byte[] LengthEncode(byte[] buffer);
    public delegate byte[] LengthDecode(ref List<byte> buffer);
    public delegate byte[] PackEncode(object value);
    public delegate object PackDecode(byte[] buffer);
}
