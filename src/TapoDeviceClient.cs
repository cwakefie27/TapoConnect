using Org.BouncyCastle.Math.EC.Rfc7748;
using Org.BouncyCastle.Ocsp;
using System.Drawing;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TapoConnect.Dto;
using TapoConnect.Exceptions;
using TapoConnect.Protocol;

namespace TapoConnect
{
    public class TapoDeviceClient : ITapoDeviceClient
    {
        private readonly List<ITapoDeviceClient> _deviceClients;

        public TapoDeviceProtocol Protocol => TapoDeviceProtocol.Multi;

        public TapoDeviceClient(
            List<ITapoDeviceClient> deviceClients = null)
        {
            _deviceClients = deviceClients ?? new List<ITapoDeviceClient>{
                new KlapDeviceClient(),
                new SecurePassthroughDeviceClient(),
            };
        }

        public virtual async Task<TapoDeviceKey> LoginByIpAsync(
            string ipAddress,
            string username,
            string password)
        {
            if (ipAddress == null)
            {
                throw new ArgumentNullException(nameof(ipAddress));
            }

            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            foreach (var client in _deviceClients)
            {
                try
                {
                    return await client.LoginByIpAsync(ipAddress, username, password);
                }
                catch (TapoProtocolDeprecatedException)
                {
                }
            }

            throw new TapoUnknownDeviceKeyProtocolException($"No protocol worked for logging into device.");
        }

        public virtual Task<DeviceGetInfoResult> GetDeviceInfoAsync(TapoDeviceKey deviceKey)
        {
            if (deviceKey == null)
            {
                throw new ArgumentNullException(nameof(deviceKey));
            }

            var client = _deviceClients.FirstOrDefault(c => c.Protocol == deviceKey.DeviceProtocol);

            if (client != null)
            {
                return client.GetDeviceInfoAsync(deviceKey);
            }
            else
            {
                throw new TapoUnknownDeviceKeyProtocolException($"Uhandled device key protocol: {deviceKey.GetType().FullName}.");
            }
        }

        public virtual Task<DeviceGetInfoResult> GetEnergyUsageAsync(TapoDeviceKey deviceKey)
        {
            if (deviceKey == null)
            {
                throw new ArgumentNullException(nameof(deviceKey));
            }

            var client = _deviceClients.FirstOrDefault(c => c.Protocol == deviceKey.DeviceProtocol);

            if (client != null)
            {
                return client.GetEnergyUsageAsync(deviceKey);
            }
            else
            {
                throw new TapoUnknownDeviceKeyProtocolException($"Uhandled device key protocol: {deviceKey.GetType().FullName}.");
            }
        }

        public virtual Task SetPowerAsync(TapoDeviceKey deviceKey, bool on)
        {
            if (deviceKey == null)
            {
                throw new ArgumentNullException(nameof(deviceKey));
            }

            var client = _deviceClients.FirstOrDefault(c => c.Protocol == deviceKey.DeviceProtocol);

            if (client != null)
            {
                return client.SetPowerAsync(deviceKey, on);
            }
            else
            {
                throw new TapoUnknownDeviceKeyProtocolException($"Uhandled device key protocol: {deviceKey.GetType().FullName}.");
            }
        }

        public virtual Task SetBrightnessAsync(TapoDeviceKey deviceKey, int brightness)
        {
            if (deviceKey == null)
            {
                throw new ArgumentNullException(nameof(deviceKey));
            }

            var client = _deviceClients.FirstOrDefault(c => c.Protocol == deviceKey.DeviceProtocol);

            if (client != null)
            {
                return client.SetBrightnessAsync(deviceKey, brightness);
            }
            else
            {
                throw new TapoUnknownDeviceKeyProtocolException($"Uhandled device key protocol: {deviceKey.GetType().FullName}.");
            }
        }

        public virtual Task SetColorAsync(TapoDeviceKey deviceKey, TapoColor color)
        {
            if (deviceKey == null)
            {
                throw new ArgumentNullException(nameof(deviceKey));
            }

            if (color == null)
            {
                throw new ArgumentNullException(nameof(color));
            }

            var client = _deviceClients.FirstOrDefault(c => c.Protocol == deviceKey.DeviceProtocol);

            if (client != null)
            {
                return client.SetColorAsync(deviceKey, color);
            }
            else
            {
                throw new TapoUnknownDeviceKeyProtocolException($"Uhandled device key protocol: {deviceKey.GetType().FullName}.");
            }
        }

        public virtual Task SetStateAsync<TState>(
            TapoDeviceKey deviceKey,
            TState state)
           where TState : TapoSetDeviceState
        {
            if (deviceKey == null)
            {
                throw new ArgumentNullException(nameof(deviceKey));
            }

            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            var client = _deviceClients.FirstOrDefault(c => c.Protocol == deviceKey.DeviceProtocol);

            if (client != null)
            {
                return client.SetStateAsync(deviceKey, state);
            }
            else
            {
                throw new TapoUnknownDeviceKeyProtocolException($"Uhandled device key protocol: {deviceKey.GetType().FullName}.");
            }
        }
    }
}
