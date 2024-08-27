using CoreApiInNet.Model;
using CoreApiInNet.Users;
using Microsoft.AspNetCore.Identity;

namespace CoreApiInNet.Contracts
{
    public interface InterfaceAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto);
        Task<AuthResponse> login(ApiUserDto userDto);
        Task<string> CreateNewToken();
        Task<AuthResponse> VerifyToken(AuthResponse authResponse);
    }
}
