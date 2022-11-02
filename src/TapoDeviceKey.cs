using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TapoConnect
{
    public class TapoDeviceKey
    {
        public TapoDeviceKey(
            string deviceIp,
            string sessionCookie,
            TimeSpan? timeout,
            DateTime issueTime,
            byte[] key,
            byte[] iv,
            string token)
        {
            DeviceIp = deviceIp;
            SessionCookie = sessionCookie;
            IssueTime = issueTime;
            Timeout = timeout;
            Key = key;
            Iv = iv;
            Token = token;
        }

        public string DeviceIp { get; }
        public string SessionCookie { get; }
        public TimeSpan? Timeout { get; }
        public DateTime IssueTime { get; }
        public byte[] Key { get; }
        public byte[] Iv { get; }

        public string Token { get; }
    }
}
