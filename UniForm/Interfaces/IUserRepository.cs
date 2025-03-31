using UniForm.Entity;
using UniForm.Models;

namespace UniForm.Interfaces
{
    public interface IUserRepository {
        Task<ApiResponse<UserDto>> GetUserById(int userId);
        Task<ApiResponse<UserDto>> GetUserForLogin(UserInfo userInfo);
        Task<ApiResponse<User>> SetUser(User user);
        Task<ApiResponse<User>> UpdatePassByUser(User user);
        Task<ApiResponse<User>> GetUserByEmail(string email);
    }
}
