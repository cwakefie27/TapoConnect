using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapoConnect.Exceptions
{
    public class TapoProtocolDeprecatedException : TapoException
    {
        public TapoProtocolDeprecatedException(string? message) : base(message)
        {
        }
    }
}
