using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapoConnect.Protocol
{
    public class KlapDeviceKey : TapoDeviceKey
    {
        public KlapChiper KlapChiper { get; }

        public KlapDeviceKey(
            string deviceIp,
            string sessionCookie,
            TimeSpan? timeout,
            DateTime issueTime,
            KlapChiper klapChiper)
            : base(TapoDeviceProtocol.Klap, deviceIp, sessionCookie, timeout, issueTime)
        {
            KlapChiper = klapChiper;
        }
    }
}
