using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapoConnect.Exceptions
{
    public class TapoUnknownDeviceKeyProtocolException : TapoException
    {
        public TapoUnknownDeviceKeyProtocolException(string? message) : base(message)
        {
        }
    }
}
