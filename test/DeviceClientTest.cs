using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TapoConnect;
using TapoConnect.Protocol;

namespace Test
{
    [TestClass]
    public class DeviceClientTest
    {

        public const string Username = "<Username>";
        public const string Password = "<Password>";
        public const string IpAddress = "<IpAddress>";

        private ITapoDeviceClient _client;
        private TapoDeviceKey _deviceKey = null!;

        [TestInitialize]
        public async Task TestInitialize()
        {
            _client = new TapoDeviceClient(new List<ITapoDeviceClient>
            {
                new KlapDeviceClient(),
                new SecurePassthroughDeviceClient(),
            });

            _deviceKey = await _client.LoginByIpAsync(IpAddress, Username, Password);
        }

        [TestMethod]
        public async Task GetDeviceInfoAsync()
        {
            var deviceInfo = await _client.GetDeviceInfoAsync(_deviceKey);
        }

        [TestMethod]
        public async Task SetPowerAsync()
        {
            await _client.SetPowerAsync(_deviceKey, true);
        }

        [TestMethod]
        public async Task SetBrightnessAsync()
        {
            await _client.SetBrightnessAsync(_deviceKey, 1);
        }

        [TestMethod]
        public async Task SetColorAsync()
        {
            await _client.SetColorAsync(_deviceKey, TapoColor.FromRgb(10, 0, 0));
        }

        [TestMethod]
        public async Task SetColorTempAsync()
        {
            await _client.SetColorAsync(_deviceKey, TapoColor.FromTemperature(4500, 10));
        }

        [TestMethod]
        public async Task SetStateAsync()
        {
            await _client.SetStateAsync(_deviceKey, new TapoSetBulbState(
                color: TapoColor.FromTemperature(4500),
                deviceOn: true));
        }
    }
}