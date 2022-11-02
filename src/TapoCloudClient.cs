using System.Text;
using System.Text.Json;
using TapoConnect.Dto;
using TapoConnect.Exceptions;

namespace TapoConnect
{
    public class TapoCloudClient : ITapoCloudClient
    {
        public const string DefaultBaseUrl = "https://wap.tplinkcloud.com/";
        public const string DefaultAppType = "Tapo_Android";

        protected virtual string AppType { get; }
        protected virtual string BaseUrl { get; }
        protected virtual JsonSerializerOptions JsonSerializerOptions { get; }

        public TapoCloudClient(
            string appType = DefaultAppType,
            string baseUrl = DefaultBaseUrl,
            JsonSerializerOptions? jsonSerializerOptions = null)
        {
            AppType = appType;
            BaseUrl = baseUrl;

            if (jsonSerializerOptions != null)
            {
                JsonSerializerOptions = jsonSerializerOptions;
            }
            else
            {
                JsonSerializerOptions = new JsonSerializerOptions();
                JsonSerializerOptions.Converters.Add(new TapoJsonDateTimeConverter());
            }
        }


        public virtual async Task<CloudLoginResult> LoginAsync(
            string email,
            string password,
            bool refreshTokenNeeded = false)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            var request = new TapoRequest<object>
            {
                Method = "login",
                Params = new
                {
                    appType = AppType,
                    cloudUserName = email,
                    cloudPassword = password,
                    refreshTokenNeeded,
                    terminalUUID = TapoCrypto.UuidV4(),
                }
            };

            var requestJson = JsonSerializer.Serialize(request, JsonSerializerOptions);

            var requestContent = new StringContent(
                requestJson,
                Encoding.UTF8,
                "application/json");

            using var httpClient = new HttpClient();

            using var message = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
            {
                Method = HttpMethod.Post,
                Content = requestContent,
            };

            var response = await httpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpResponseException(response);
            }

            var responseContentString = await response.Content.ReadAsStringAsync();

            var responseJson = JsonSerializer.Deserialize<CloudLoginResponse>(responseContentString, JsonSerializerOptions);
            if (responseJson == null)
            {
                throw new TapoJsonException($"Failed to deserialize {responseJson}.");
            }
            else
            {
                TapoException.ThrowFromErrorCode(responseJson.ErrorCode);

                return responseJson.Result;
            }
        }


        public virtual async Task<CloudRefreshLoginResult> RefreshLoginAsync(
            string refreshToken)
        {
            if (refreshToken == null)
            {
                throw new ArgumentNullException(nameof(refreshToken));
            }

            var request = new TapoRequest<object>
            {
                Method = "refreshToken",
                Params = new
                {
                    appType = AppType,
                    refreshToken,
                    terminalUUID = TapoCrypto.UuidV4(),
                }
            };

            var requestJson = JsonSerializer.Serialize(request, JsonSerializerOptions);

            var requestContent = new StringContent(
                requestJson,
                Encoding.UTF8,
                "application/json");

            using var httpClient = new HttpClient();

            using var message = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
            {
                Method = HttpMethod.Post,
                Content = requestContent,
            };

            var response = await httpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpResponseException(response);
            }

            var responseContentString = await response.Content.ReadAsStringAsync();

            var responseJson = JsonSerializer.Deserialize<CloudRefreshLoginResponse>(responseContentString, JsonSerializerOptions);
            if (responseJson == null)
            {
                throw new TapoJsonException($"Failed to deserialize {responseJson}.");
            }
            else
            {
                TapoException.ThrowFromErrorCode(responseJson.ErrorCode);

                return responseJson.Result;
            }
        }

        public virtual async Task<CloudListDeviceResult> ListDevicesAsync(string cloudToken)
        {
            if (cloudToken == null)
            {
                throw new ArgumentNullException(nameof(cloudToken));
            }

            var request = new TapoRequest
            {
                Method = "getDeviceList",
            };

            var requestJson = JsonSerializer.Serialize(request, JsonSerializerOptions);

            var requestContent = new StringContent(
                requestJson,
                Encoding.UTF8,
                "application/json");

            using var httpClient = new HttpClient();

            var url = $"{BaseUrl}?token={cloudToken}";

            using var message = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Method = HttpMethod.Post,
                Content = requestContent,
            };

            var response = await httpClient.SendAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpResponseException(response);
            }

            var responseContentString = await response.Content.ReadAsStringAsync();

            var responseJson = JsonSerializer.Deserialize<CloudListDeviceReponse>(responseContentString, JsonSerializerOptions);

            if (responseJson == null)
            {
                throw new TapoJsonException($"Failed to deserialize {responseJson}.");
            }
            else
            {
                TapoException.ThrowFromErrorCode(responseJson.ErrorCode);

                foreach (var d in responseJson.Result.DeviceList)
                {
                    if (TapoUtils.IsTapoDevice(d.DeviceType))
                    {
                        d.Alias = TapoCrypto.Base64Decode(d.Alias);
                    }
                }

                TapoException.ThrowFromErrorCode(responseJson.ErrorCode);

                return responseJson.Result;
            }
        }
    }
}
