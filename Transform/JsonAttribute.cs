using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPath2Json.Transform
{
    enum JsonAttribute
    {
        None = 0,
        Type = 0x1,
        Array = 0x2,
        Null = 0x4,
        Empty = 0x8,
        EmptyArray = 0x10
    }
}
