using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapoConnect.Exceptions
{
    public class TapoProtocolMismatchException : TapoException
    {
        public TapoProtocolMismatchException(string? message) : base(message)
        {
        }
    }
}
