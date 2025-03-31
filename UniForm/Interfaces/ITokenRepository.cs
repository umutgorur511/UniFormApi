using UniForm.Entity;
using UniForm.Models;

namespace UniForm.Interfaces
{
    public interface ITokenRepository {
        Task<ApiResponse<bool>> ValidateTokenAsync(string accessToken);
        Task<ApiResponse<string>> CreateTokenAsync(int userId);
    }
}
