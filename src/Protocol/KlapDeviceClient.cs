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
    public class KlapDeviceClient : ITapoDeviceClient
    {
        protected const string TpSessionKey = "TP_SESSIONID";
        public TapoDeviceProtocol Protocol => TapoDeviceProtocol.Klap;

        protected sealed class KlapHandshakeKey
        {
            public KlapHandshakeKey(
                string sessionCookie,
                TimeSpan? timeout,
                DateTime issueTime,
                byte[] key,
                byte[] iv)
            {
                SessionCookie = sessionCookie;
                Timeout = timeout;
                IssueTime = issueTime;
                RemoteSeed = key;
                AuthHash = iv;
            }

            public string SessionCookie { get; } = null!;
            public TimeSpan? Timeout { get; }
            public DateTime IssueTime { get; }
            public byte[] RemoteSeed { get; } = null!;
            public byte[] AuthHash { get; } = null!;
        }

        protected virtual JsonSerializerOptions _jsonSerializerOptions { get; }

        public KlapDeviceClient(
            JsonSerializerOptions? jsonSerializerOptions = null)
        {
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
            string deviceIp,
            string username,
            string password)
        {
            if (deviceIp == null)
            {
                throw new ArgumentNullException(nameof(deviceIp));
            }

            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var localSeed = TapoCrypto.GenerateRandomBytes(16);

            var handshake1 = await KlapHandshake1Async(deviceIp, username, password, localSeed);
            var klapChiper = await KlapHandshake2Async(deviceIp, handshake1.SessionCookie, localSeed, handshake1.RemoteSeed, handshake1.AuthHash);

            return new KlapDeviceKey(deviceIp, handshake1.SessionCookie, handshake1.Timeout, handshake1.IssueTime, klapChiper);
        }

        public virtual async Task<DeviceGetEnergyUsageResult> GetEnergyUsageAsync(TapoDeviceKey deviceKey)
        {
            if (deviceKey == null)
            {
                throw new ArgumentNullException(nameof(deviceKey));
            }

            var protocol = deviceKey.ToProtocol<KlapDeviceKey>();

            var request = new TapoRequest
            {
                Method = "get_energy_usage",
            };

            var jsonRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);

            var response = await KlapRequestAsync<DeviceGetEnergyUsageResponse>(jsonRequest, deviceKey.DeviceIp, deviceKey.SessionCookie, protocol.KlapChiper);

            return response.Result;
        }

        public virtual async Task SetPowerAsync(TapoDeviceKey deviceKey, bool on)
        {
            if (deviceKey == null)
            {
                throw new ArgumentNullException(nameof(deviceKey));
            }

            var protocol = deviceKey.ToProtocol<KlapDeviceKey>();

            var request = new TapoRequest<TapoSetBulbState>
            {
                Method = "set_device_info",
                Params = new TapoSetBulbState(
                   on)
            };

            var jsonRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);

            await KlapRequestAsync<TapoResponse>(jsonRequest, deviceKey.DeviceIp, deviceKey.SessionCookie, protocol.KlapChiper);
        }

        public virtual async Task SetBrightnessAsync(TapoDeviceKey deviceKey, int brightness)
        {
            if (deviceKey == null)
            {
                throw new ArgumentNullException(nameof(deviceKey));
            }

            var protocol = deviceKey.ToProtocol<KlapDeviceKey>();

            var request = new TapoRequest<TapoSetBulbState>
            {
                Method = "set_device_info",
                Params = new TapoSetBulbState(
                    brightness: brightness)
            };

            var jsonRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);

            await KlapRequestAsync<TapoResponse>(jsonRequest, deviceKey.DeviceIp, deviceKey.SessionCookie, protocol.KlapChiper);
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

            var protocol = deviceKey.ToProtocol<KlapDeviceKey>();

            var request = new TapoRequest<TapoSetBulbState>
            {
                Method = "set_device_info",
                Params = new TapoSetBulbState(color),
            };

            var jsonRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);

            await KlapRequestAsync<TapoResponse>(jsonRequest, deviceKey.DeviceIp, deviceKey.SessionCookie, protocol.KlapChiper);
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

            var protocol = deviceKey.ToProtocol<KlapDeviceKey>();

            var request = new TapoRequest<TState>
            {
                Method = "set_device_info",
                Params = state,
            };

            var jsonRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);

            await KlapRequestAsync<TapoResponse>(jsonRequest, deviceKey.DeviceIp, deviceKey.SessionCookie, protocol.KlapChiper);
        }

        protected virtual async Task<KlapHandshakeKey> KlapHandshake1Async(string deviceIp, string username, string password, byte[] localSeed)
        {
            if (deviceIp == null)
            {
                throw new ArgumentNullException(nameof(deviceIp));
            }

            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (localSeed == null)
            {
                throw new ArgumentNullException(nameof(localSeed));
            }

            var requestContent = new ByteArrayContent(localSeed);

            using var httpClient = new HttpClient();

            var url = $"http://{deviceIp}/app/handshake1";

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

            //This is the response string I was getting from a TAPO device without KLAP auth
            var responseContentStr = await response.Content.ReadAsStringAsync();
            if (responseContentStr == "<html><body><center>200 OK</center></body></html>")
                throw new TapoProtocolDeprecatedException("Klap Authentication hash does not match server hash.");

            var responseContentBytes = await response.Content.ReadAsByteArrayAsync();

            if (responseContentBytes == null)
            {
                throw new TapoKlapException($"Payload missing bytes.");
            }
            else
            {
                var remoteSeed = responseContentBytes.Take(16).ToArray();
                var serverHash = responseContentBytes.Skip(16).ToArray();

                var usernameHash = TapoCrypto.Sha1Hash(Encoding.UTF8.GetBytes(username));
                var passwordHash = TapoCrypto.Sha1Hash(Encoding.UTF8.GetBytes(password));

                var authHash = TapoCrypto.Sha256Hash(usernameHash.Concat(passwordHash).ToArray());

                var localSeedAuthHash = TapoCrypto.Sha256Hash(localSeed.Concat(remoteSeed).Concat(authHash).ToArray());
                var localServerHashMatches = localSeedAuthHash.SequenceEqual(serverHash);

                if (!localServerHashMatches)
                    throw new TapoInvalidCredentialException($"Local hash does not match server hash (Confirm you are using the correct username and password).");

                if (response.Headers.TryGetValues("set-cookie", out var values))
                {
                    var s = values.First();

                    var keyValue = s
                        .Split(';')
                        .Select(x => x.Split('='))
                        .ToDictionary(x => x[0], x => x[1]);

                    if (!keyValue.ContainsKey(TpSessionKey))
                    {
                        throw new Exception("Tapo login did not recieve a session id.");
                    }

                    TimeSpan? timeout = null;
                    if (keyValue.ContainsKey("TIMEOUT"))
                    {
                        timeout = TimeSpan.FromSeconds(int.Parse(keyValue["TIMEOUT"]));
                    }

                    var sessionCookie = $"{TpSessionKey}={keyValue[TpSessionKey]}";

                    return new KlapHandshakeKey(sessionCookie, timeout, requestTime, remoteSeed, authHash);
                }
                else
                {
                    throw new TapoNoSetCookieHeaderException("Tapo login did not recieve a set-cookie header.");
                }
            }
        }

        protected virtual async Task<KlapChiper> KlapHandshake2Async(string deviceIp, string sessionCookie, byte[] localSeed, byte[] remoteSeed, byte[] authHash)
        {
            if (deviceIp == null)
            {
                throw new ArgumentNullException(nameof(deviceIp));
            }

            if (sessionCookie == null)
            {
                throw new ArgumentNullException(nameof(sessionCookie));
            }

            if (localSeed == null)
            {
                throw new ArgumentNullException(nameof(localSeed));
            }

            if (remoteSeed == null)
            {
                throw new ArgumentNullException(nameof(remoteSeed));
            }

            if (authHash == null)
            {
                throw new ArgumentNullException(nameof(authHash));
            }

            var payload = TapoCrypto.Sha256Hash(remoteSeed.Concat(localSeed).Concat(authHash).ToArray());

            var requestContent = new ByteArrayContent(payload);

            var baseUrl = $"http://{deviceIp}";
            var url = $"{baseUrl}/app/handshake2";

            using var httpClient = new HttpClient();
            using var message = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Method = HttpMethod.Post,
                Content = requestContent,
            };

            if (sessionCookie != null)
                message.Headers.Add("Cookie", sessionCookie);

            var requestTime = DateTime.Now;

            var response = await httpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpResponseException(response);
            }

            return new KlapChiper(localSeed, remoteSeed, authHash);
        }

        public virtual async Task<DeviceGetInfoResult> GetDeviceInfoAsync(TapoDeviceKey deviceKey)
        {
            if (deviceKey == null)
            {
                throw new ArgumentNullException(nameof(deviceKey));
            }

            var protocol = deviceKey.ToProtocol<KlapDeviceKey>();

            var request = new TapoRequest
            {
                Method = "get_device_info",
            };

            var jsonRequest = JsonSerializer.Serialize(request, _jsonSerializerOptions);

            var response = await KlapRequestAsync<DeviceGetInfoResponse>(jsonRequest, deviceKey.DeviceIp, deviceKey.SessionCookie, protocol.KlapChiper);

            response.Result.Ssid = TapoCrypto.Base64Decode(response.Result.Ssid);
            response.Result.Nickname = TapoCrypto.Base64Decode(response.Result.Nickname);

            return response.Result;
        }

        protected virtual async Task<TResponse> KlapRequestAsync<TResponse>(
            string deviceRequest,
            string deviceIp,
            string sessionCookie,
            KlapChiper klapChiper)
            where TResponse : TapoResponse
        {
            if (deviceIp == null)
            {
                throw new ArgumentNullException(nameof(deviceIp));
            }

            var payload = klapChiper.Encrypt(deviceRequest);

            var requestContent = new ByteArrayContent(payload);

            var baseUrl = $"http://{deviceIp}";
            var url = $"{baseUrl}/app/request?seq={klapChiper.Seq}";

            using var httpClient = new HttpClient();
            using var message = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Method = HttpMethod.Post,
                Content = requestContent,
            };

            if (sessionCookie != null)
                message.Headers.Add("Cookie", sessionCookie);

            var requestTime = DateTime.Now;

            var response = await httpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpResponseException(response);
            }

            var responseBytes = await response.Content.ReadAsByteArrayAsync();
            var decryptedBytes = klapChiper.Decrypt(responseBytes);
            var decryptedString = Encoding.UTF8.GetString(decryptedBytes);

            var responseJson = JsonSerializer.Deserialize<DeviceSecurePassthroughReponse>(decryptedString, _jsonSerializerOptions);
            if (responseJson == null)
            {
                throw new TapoJsonException($"Failed to deserialize {responseJson}.");
            }
            else
            {
                TapoException.ThrowFromErrorCode(responseJson.ErrorCode);

                var decryptedResponseJson = JsonSerializer.Deserialize<TResponse>(decryptedString, _jsonSerializerOptions);
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
