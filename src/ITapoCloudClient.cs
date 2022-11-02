using TapoConnect.Dto;

namespace TapoConnect
{
    public interface ITapoCloudClient
    {
        Task<CloudLoginResult> LoginAsync(string email, string password, bool refreshTokenNeeded = false);
        Task<CloudRefreshLoginResult> RefreshLoginAsync(string refreshToken);
        Task<CloudListDeviceResult> ListDevicesAsync(string cloudToken);
    }
}
