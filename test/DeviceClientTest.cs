using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using System.Transactions;
using TapoConnect;

namespace Test
{
    [TestClass]
    public class DeviceClientTest
    {
        public const string Username = "<Username>";
        public const string Password = "<Password>";
        public const string IpAddress = "<IpAddress>";

        private TapoDeviceKey _deviceKey = null!;

        [TestInitialize]
        public async Task TestInitialize()
        {
            var client = new TapoDeviceClient();
            var deviceKey = await client.LoginByIpAsync(IpAddress, Username, Password);

            _deviceKey = deviceKey;
        }

        [TestMethod]
        public async Task GetDeviceInfoAsync()
        {
            var client = new TapoDeviceClient();

            await client.GetDeviceInfoAsync(_deviceKey);
        }

        [TestMethod]
        public async Task SetPowerAsync()
        {
            var client = new TapoDeviceClient();

            await client.SetPowerAsync(_deviceKey, false);
        }

        [TestMethod]
        public async Task SetBrightnessAsync()
        {
            var client = new TapoDeviceClient();

            await client.SetBrightnessAsync(_deviceKey, 1);
        }

        [TestMethod]
        public async Task SetColorAsync()
        {
            var client = new TapoDeviceClient();

            await client.SetColorAsync(_deviceKey, TapoColor.FromRgb(10, 0, 0));
        }

        [TestMethod]
        public async Task SetColorTempAsync()
        {
            var client = new TapoDeviceClient();

            await client.SetColorAsync(_deviceKey, TapoColor.FromTemperature(4500, 10));
        }

        [TestMethod]
        public async Task SetStateAsync()
        {
            var client = new TapoDeviceClient();

            await client.SetStateAsync(_deviceKey, new TapoSetBulbState(
                color: TapoColor.FromTemperature(4500, brightness: 10),
                deviceOn: true));
        }
    }
}