using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapoConnect.Exceptions
{
    public class TapoNoSetCookieHeaderException : TapoException
    {
        public TapoNoSetCookieHeaderException(string? message) : base(message)
        {
        }
    }
}
