using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TapoConnect;

namespace Test
{
    [TestClass]
    public class CloudClientTest
    {
        public const string Username = "<Username>";
        public const string Password = "<Password>";

        [TestMethod]
        public async Task CloudLoginAsync()
        {
            var client = new TapoCloudClient();

            var login = await client.LoginAsync(Username, Password);

            Assert.IsNotNull(login.Token);
        }

        [TestMethod]
        public async Task CloudRefreshLoginAsync()
        {
            var client = new TapoCloudClient();

            var login = await client.LoginAsync(Username, Password, true);

            Assert.IsNotNull(login.Token);
            Assert.IsNotNull(login.RefreshToken);

            var refreshLogin = await client.RefreshLoginAsync(login.RefreshToken);

            Assert.IsNotNull(refreshLogin.Token);
        }

        [TestMethod]
        public async Task ListDevicesAsync()
        {
            var client = new TapoCloudClient();

            var login = await client.LoginAsync(Username, Password);

            var response = await client.ListDevicesAsync(login.Token);

            Assert.IsNotNull(response.DeviceList);
        }
    }
}