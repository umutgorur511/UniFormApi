using Microsoft.EntityFrameworkCore;
using UniForm.Data;
using UniForm.Entity;
using UniForm.Interfaces;
using UniForm.Models;

namespace UniForm.Repository
{
    public class UserRepository : IUserRepository
    {

        public readonly DataContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(DataContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<UserDto>> GetUserById(int userId)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                {
                    _logger.LogWarning("Kullanıcı bulunamadı. UserId: {UserId}", userId);
                    return new ApiResponse<UserDto>
                    {
                        Data = null,
                        Succes = false,
                    };
                }

                var response = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreateDate = user.CreateDate,
                    RecordStatus = user.RecordStatus,
                    UpdateDate = user.UpdateDate
                };

                _logger.LogInformation("Kullanıcı başarıyla getirildi. UserId: {UserId}", userId);

                return new ApiResponse<UserDto>
                {
                    Data = response,
                    Succes = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı getirilirken hata oluştu. UserId: {UserId}", userId);
                return new ApiResponse<UserDto>
                {
                    Data = null,
                    Succes = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<UserDto>> GetUserForLogin(UserInfo userInfo)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.Email == userInfo.Mail && x.Password == userInfo.Password);

                if (user == null)
                {
                    _logger.LogWarning("Giriş başarısız. Kullanıcı bulunamadı. Email: {Email}", userInfo.Mail);
                    return new ApiResponse<UserDto>
                    {
                        Data = null,
                        Succes = false,
                    };
                }

                var userDto = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreateDate = user.CreateDate,
                    RecordStatus = user.RecordStatus,
                    UpdateDate = user.UpdateDate
                };

                _logger.LogInformation("Kullanıcı giriş yaptı. UserId: {UserId}, Email: {Email}", user.Id, user.Email);

                return new ApiResponse<UserDto>
                {
                    Data = userDto,
                    Succes = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı giriş işlemi sırasında hata oluştu. Email: {Email}", userInfo.Mail);
                return new ApiResponse<UserDto>
                {
                    Data = null,
                    Succes = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<User>> GetUserByEmail(string email)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.Email == email);

                if (user == null)
                {
                    _logger.LogWarning("Kullanıcı bulunamadı. Email: {Email}", email);
                    return new ApiResponse<User>
                    {
                        Data = null,
                        Succes = false,
                    };
                }

                var userInfo = new User
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    CreateDate = user.CreateDate,
                    RecordStatus = user.RecordStatus,
                    UpdateDate = user.UpdateDate
                };

                _logger.LogInformation("Kullanıcı başarıyla getirildi. UserId: {UserId}, Email: {Email}", user.Id, user.Email);

                return new ApiResponse<User>
                {
                    Data = userInfo,
                    Succes = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı getirilirken hata oluştu. Email: {Email}", email);
                return new ApiResponse<User>
                {
                    Data = null,
                    Succes = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<User>> SetUser(User user)
        {
            try
            {
                // Validate input
                if (user == null || user.Email == null || user.Name == null)
                {
                    return new ApiResponse<User>
                    {
                        Data = null,
                        Succes = false,
                        Message = "Hata oluştu."
                    };
                }

                var defaultUser = await GetUserByEmail(user.Email);

                if (defaultUser != null && defaultUser.Data != null && defaultUser.Data.Email == user.Email)
                {
                    return new ApiResponse<User>
                    {
                        Data = null,
                        Succes = false,
                        Message = "Bu kullanıcı bulunmaktadır."
                    };
                }

                var userRow = new User
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    UpdateDate = DateTime.UtcNow,
                    CreateDate = DateTime.UtcNow,
                    RecordStatus = 'A'
                };

                await _context.Users.AddAsync(userRow);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Yeni kullanıcı eklendi. UserId: {UserId}, Email: {Email}", userRow.Id, userRow.Email);

                return new ApiResponse<User>
                {
                    Data = userRow,
                    Succes = true,
                    Message = "Başarılı işlem."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı eklenirken hata oluştu. Email: {Email}", user.Email);
                return new ApiResponse<User>
                {
                    Data = null,
                    Succes = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }


        public async Task<ApiResponse<User>> UpdatePassByUser(User user)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(user.Id);

                if (existingUser == null)
                {
                    return new ApiResponse<User>
                    {
                        Data = null,
                        Succes = false,
                    };
                }

                if (existingUser.Password != user.Password)
                {
                    existingUser.Password = user.Password;
                    existingUser.UpdateDate = DateTime.UtcNow;

                    _context.Users.Update(existingUser);
                    await _context.SaveChangesAsync();
                }

                return new ApiResponse<User>
                {
                    Data = existingUser,
                    Succes = true,
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<User>
                {
                    Data = null,
                    Succes = false,
                    Message = $"Error: {ex.Message}"
                };
            }

        }
    }
}