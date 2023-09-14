using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TapoConnect.Exceptions;
using TapoConnect.Protocol;

namespace TapoConnect
{
    public abstract class TapoDeviceKey
    {
        public TapoDeviceKey(
            TapoDeviceProtocol deviceProtocol,
            string deviceIp,
            string sessionCookie,
            TimeSpan? timeout,
            DateTime issueTime)
        {
            DeviceProtocol = deviceProtocol;
            DeviceIp = deviceIp;
            SessionCookie = sessionCookie;
            IssueTime = issueTime;
            Timeout = timeout;
        }

        public TapoDeviceProtocol DeviceProtocol { get; }
        public string DeviceIp { get; }
        public string SessionCookie { get; }
        public TimeSpan? Timeout { get; }
        public DateTime IssueTime { get; }

        public TProtocol ToProtocol<TProtocol>()
            where TProtocol : TapoDeviceKey
        {
            if (this is TProtocol)
            {
                return (TProtocol)this;
            }
            else
            {
                throw new TapoProtocolMismatchException($"Protocol {GetType().FullName} cannot be converted to {typeof(TProtocol).FullName}.");
            }
        }
    }
}
