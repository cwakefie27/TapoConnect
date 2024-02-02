using Org.BouncyCastle.Ocsp;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TapoConnect.Dto;
using TapoConnect.Exceptions;
using TapoConnect.Util;

namespace TapoConnect.Protocol
{
    public class SecurePassthroughDeviceClient : ITapoDeviceClient
    {
        protected const string TpSessionKey = "TP_SESSIONID";
        public TapoDeviceProtocol Protocol => TapoDeviceProtocol.SecurePassThrough;
        protected virtual JsonSerializerOptions _jsonSerializerOptions { get; }
        protected readonly string privateKeyPassword;

        protected sealed class TapoHandshakeKey
        {
            public TapoHandshakeKey(
                string sessionCookie,
                TimeSpan? timeout,
                DateTime issueTime,
                byte[] key,
                byte[] iv)
            {
                SessionCookie = sessionCookie;
                Timeout = timeout;
                IssueTime = issueTime;
                Key = key;
                Iv = iv;
            }

            public string SessionCookie { get; } = null!;
            public TimeSpan? Timeout { get; }
            public DateTime IssueTime { get; }
            public byte[] Key { get; } = null!;
            public byte[] Iv { get; } = null!;
        }

       
        public SecurePassthroughDeviceClient(
            string? privateKeyPassword = null,
            JsonSerializerOptions? jsonSerializerOptions = null)
        {
            this.privateKeyPassword = privateKeyPassword ?? Guid.NewGuid().ToString();

            if (jsonSerializerOptions != null)
            {
                _jsonSerializerOptions = jsonSerializerOptions;
            }
            else
            {
                _jsonSerializerOptions = new JsonSerializerOptions();
                _jsonSerializerOptions.Converters.Add(new TapoJsonDateTimeConverter());
            }
        }

        public virtual async Task<TapoDeviceKey> LoginByIpAsync(
            string ipAddress,
            string email,
            string password)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (ipAddress == null)
            {
                throw new ArgumentNullException(nameof(ipAddress));
            }

            var result = await HandshakeAsync(ipAddress);

            var emailBytes = Encoding.UTF8.GetBytes(email);
            var emailHash = TapoCrypto.Sha1Hash(emailBytes);
            var emailHexString = Convert.ToHexString(emailHash).ToLower();

            var request = new TapoRequest<object>
            {
                Method = "login_device",
                Params = new
                {
                    username = TapoCrypto.Base64Encode(emailHexString),
                    password = TapoCrypto.Base64Encode(password),
                },
            };

            var jsonRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);

            var response = await SecurePassthroughAsync<DeviceLoginResponse>(
                jsonRequest,
                ipAddress,
                null,
                result.SessionCookie,
                result.Key,
                result.Iv);

            return new SecurePassThroughDeviceKey(
                ipAddress,
                result.SessionCookie,
                result.Timeout,
                result.IssueTime,
                result.Key,
                result.Iv,
                response.Result.Token);
        }

        public virtual async Task<DeviceGetInfoResult> GetDeviceInfoAsync(TapoDeviceKey deviceKey)
        {
            if (deviceKey == null)
            {
                throw new ArgumentNullException(nameof(deviceKey));
            }

            var protocol = deviceKey.ToProtocol<SecurePassThroughDeviceKey>();

            var request = new TapoRequest
            {
                Method = "get_device_info",
            };

            var jsonRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);

            var response = await SecurePassthroughAsync<DeviceGetInfoResponse>(jsonRequest, deviceKey.DeviceIp, protocol.Token, deviceKey.SessionCookie, protocol.Key, protocol.Iv);

            response.Result.Ssid = TapoCrypto.Base64Decode(response.Result.Ssid);
            response.Result.Nickname = TapoCrypto.Base64Decode(response.Result.Nickname);

            return response.Result;
        }

        public virtual async Task<DeviceGetEnergyUsageResult> GetEnergyUsageAsync(TapoDeviceKey deviceKey)
        {
            if (deviceKey == null)
            {
                throw new ArgumentNullException(nameof(deviceKey));
            }

            var protocol = deviceKey.ToProtocol<SecurePassThroughDeviceKey>();

            var request = new TapoRequest
            {
                Method = "get_energy_usage",
            };

            var jsonRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);

            var response = await SecurePassthroughAsync<DeviceGetEnergyUsageResponse>(jsonRequest, deviceKey.DeviceIp, protocol.Token, deviceKey.SessionCookie, protocol.Key, protocol.Iv);

            return response.Result;
        }

        public virtual async Task SetPowerAsync(TapoDeviceKey deviceKey, bool on)
        {
            if (deviceKey == null)
            {
                throw new ArgumentNullException(nameof(deviceKey));
            }

            var protocol = deviceKey.ToProtocol<SecurePassThroughDeviceKey>();

            var request = new TapoRequest<TapoSetBulbState>
            {
                Method = "set_device_info",
                Params = new TapoSetBulbState(
                   on)
            };

            var jsonRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);

            await SecurePassthroughAsync<TapoResponse>(jsonRequest, deviceKey.DeviceIp, protocol.Token, deviceKey.SessionCookie, protocol.Key, protocol.Iv);
        }

        public virtual async Task SetBrightnessAsync(TapoDeviceKey deviceKey, int brightness)
        {
            if (deviceKey == null)
            {
                throw new ArgumentNullException(nameof(deviceKey));
            }

            var protocol = deviceKey.ToProtocol<SecurePassThroughDeviceKey>();

            var request = new TapoRequest<TapoSetBulbState>
            {
                Method = "set_device_info",
                Params = new TapoSetBulbState(
                    brightness: brightness)
            };

            var jsonRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);

            await SecurePassthroughAsync<TapoResponse>(jsonRequest, deviceKey.DeviceIp, protocol.Token, deviceKey.SessionCookie, protocol.Key, protocol.Iv);
        }

        public virtual async Task SetColorAsync(TapoDeviceKey deviceKey, TapoColor color)
        {
            if (deviceKey == null)
            {
                throw new ArgumentNullException(nameof(deviceKey));
            }

            if (color == null)
            {
                throw new ArgumentNullException(nameof(color));
            }

            var protocol = deviceKey.ToProtocol<SecurePassThroughDeviceKey>();

            var request = new TapoRequest<TapoSetBulbState>
            {
                Method = "set_device_info",
                Params = new TapoSetBulbState(color),
            };

            var jsonRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);

            await SecurePassthroughAsync<TapoResponse>(jsonRequest, deviceKey.DeviceIp, protocol.Token, deviceKey.SessionCookie, protocol.Key, protocol.Iv);
        }

        public virtual async Task SetStateAsync<TState>(
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

            var protocol = deviceKey.ToProtocol<SecurePassThroughDeviceKey>();

            var request = new TapoRequest<TState>
            {
                Method = "set_device_info",
                Params = state,
            };

            var jsonRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);

            await SecurePassthroughAsync<TapoResponse>(jsonRequest, deviceKey.DeviceIp, protocol.Token, deviceKey.SessionCookie, protocol.Key, protocol.Iv);
        }

        protected virtual async Task<TapoHandshakeKey> HandshakeAsync(string deviceIp)
        {
            if (deviceIp == null)
            {
                throw new ArgumentNullException(nameof(deviceIp));
            }

            var keyPair = TapoCrypto.GenerateKeyPair(privateKeyPassword, 1024);

            var request = new TapoRequest<object>
            {
                Method = "handshake",
                Params = new
                {
                    key = keyPair.PublicKey,
                }
            };

            var jsonRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);

            var requestContent = new StringContent(
                jsonRequest,
                Encoding.UTF8,
                "application/json");

            using var httpClient = new HttpClient();

            var url = $"http://{deviceIp}?app";


            using var message = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Method = HttpMethod.Post,
                Content = requestContent,
            };

            var requestTime = DateTime.Now;

            var response = await httpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpResponseException(response);
            }

            var responseContentString = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<DeviceHandshakeReponse>(responseContentString, _jsonSerializerOptions);
            if (responseJson == null)
            {
                throw new TapoJsonException($"Failed to deserialize {responseJson}.");
            }
            else
            {
                TapoException.ThrowFromErrorCode(responseJson.ErrorCode);

                string sessionCookie;
                TimeSpan? timeout = null;
                if (response.Headers.TryGetValues("set-cookie", out var values))
                {
                    var s = values.First();

                    var keyValue = s
                        .Split(';')
                        .Select(x => x.Split('='))
                        .ToDictionary(x => x[0], x => x[1]);

                    if (keyValue.ContainsKey(TpSessionKey))
                    {
                        sessionCookie = $"{TpSessionKey}={keyValue[TpSessionKey]}";
                    }
                    else
                    {
                        throw new Exception("Tapo login did not recieve a session id.");
                    }

                    if (keyValue.ContainsKey("TIMEOUT"))
                    {
                        timeout = TimeSpan.FromSeconds(int.Parse(keyValue["TIMEOUT"]));
                    }
                }
                else
                {
                    throw new TapoNoSetCookieHeaderException("Tapo login did not recieve a set-cookie header.");
                }

                var deviceKey = TapoCrypto.DecryptWithPrivateKeyAndPassword(responseJson.Result.Key, keyPair.PrivateKey, privateKeyPassword);

                var key = deviceKey.Take(16).ToArray();
                var iv = deviceKey.Skip(16).Take(16).ToArray();

                return new TapoHandshakeKey(
                    sessionCookie,
                    timeout,
                    requestTime,
                    key,
                    iv);
            }
        }

        protected virtual async Task<Response> SecurePassthroughAsync<Response>(
            string deviceRequest,
            string deviceIp,
            string? token,
            string cookie,
            byte[] key,
            byte[] iv)
            where Response : TapoResponse
        {
            if (deviceRequest == null)
            {
                throw new ArgumentNullException(nameof(deviceRequest));
            }

            if (deviceIp == null)
            {
                throw new ArgumentNullException(nameof(deviceIp));
            }

            if (cookie == null)
            {
                throw new ArgumentNullException(nameof(cookie));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (iv == null)
            {
                throw new ArgumentNullException(nameof(iv));
            }


            var encryptedBytes = TapoCrypto.Encrypt(deviceRequest, key, iv);

            var encryptedString = Convert.ToBase64String(encryptedBytes);

            var request = new TapoRequest<object>
            {
                Method = "securePassthrough",
                Params = new
                {
                    request = encryptedString
                }
            };
            var jsonRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);

            var requestContent = new StringContent(
                jsonRequest,
                encoding: Encoding.UTF8,
                mediaType: "application/json");

            var url = $"http://{deviceIp}/app?token={token ?? "undefined"}";

            using var message = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Method = HttpMethod.Post,
                Content = requestContent,
            };

            if (cookie != null)
                message.Headers.Add("Cookie", cookie);

            using var httpClient = new HttpClient();

            var response = await httpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpResponseException(response);
            }

            var responseContentString = await response.Content.ReadAsStringAsync();
            var responseJson = JsonSerializer.Deserialize<DeviceSecurePassthroughReponse>(responseContentString, _jsonSerializerOptions);
            if (responseJson == null)
            {
                throw new TapoJsonException($"Failed to deserialize {responseJson}.");
            }
            else
            {
                TapoException.ThrowFromErrorCode(responseJson.ErrorCode);

                var bytes = Convert.FromBase64String(responseJson.Result.Response);
                var decryptedBytes = TapoCrypto.Decrypt(bytes, key, iv);
                var decryptedString = Encoding.UTF8.GetString(decryptedBytes);

                var decryptedResponseJson = JsonSerializer.Deserialize<Response>(decryptedString, _jsonSerializerOptions);
                if (decryptedResponseJson == null)
                {
                    throw new TapoJsonException($"Failed to deserialize {decryptedString}.");
                }
                else
                {
                    TapoException.ThrowFromErrorCode(decryptedResponseJson.ErrorCode);
                    return decryptedResponseJson;
                }
            }
        }
    }
}
