using Microsoft.VisualStudio.TestTools.UnitTesting;
using TapoConnect;
using TapoConnect.Util;

namespace Test
{
    [TestClass]
    public class UtilTest
    {
        public const string LocalDeviceMacAddress = "<MacAddress>";
        public const string LocalDeviceIpAddress = "<IpAddress>";

        [TestMethod]
        public void GetIpAddressByMacAddress()
        {
            var ip = TapoUtils.GetIpAddressByMacAddress(LocalDeviceMacAddress);

            Assert.AreEqual(LocalDeviceIpAddress, ip);
        }
    }
}