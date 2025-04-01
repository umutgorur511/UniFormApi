using Azure.Core;
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
        private readonly ITokenRepository _tokenRepository;

        public UserRepository(DataContext context, ILogger<UserRepository> logger, ITokenRepository tokenRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
        }

        public async Task<ApiResponse<User>> GetUserById(int userId)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                {
                    _logger.LogWarning("Kullanıcı bulunamadı. UserId: {UserId}", userId);
                    return new ApiResponse<User>
                    {
                        Data = null,
                        IsSuccessful = false,
                    };
                }

                _logger.LogInformation("Kullanıcı başarıyla getirildi. UserId: {UserId}", userId);

                return new ApiResponse<User>
                {
                    Data = user,
                    IsSuccessful = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı getirilirken hata oluştu. UserId: {UserId}", userId);
                return new ApiResponse<User>
                {
                    Data = null,
                    IsSuccessful = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<User>> GetUserForLogin(UserInfo userInfo)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.Email == userInfo.Mail && x.Password == userInfo.Password);

                if (user == null)
                {
                    _logger.LogWarning("Giriş başarısız. Kullanıcı bulunamadı. Email: {Email}", userInfo.Mail);
                    return new ApiResponse<User>
                    {
                        Data = null,
                        IsSuccessful = false,
                    };
                }

                var token = await _tokenRepository.CreateTokenAsync(user.Id);

                if (token == null || string.IsNullOrEmpty(token.Data))
                {
                    throw new Exception("Could not create a valid token.");
                }

                user.AccessToken = token.Data;
                _context.Users.Update(user); // Kullanıcı nesnesini güncelle
                await _context.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet

                _logger.LogInformation("Kullanıcı giriş yaptı. UserId: {UserId}, Email: {Email}", user.Id, user.Email);

                return new ApiResponse<User>
                {
                    Data = user,
                    IsSuccessful = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı giriş işlemi sırasında hata oluştu. Email: {Email}", userInfo.Mail);
                return new ApiResponse<User>
                {
                    Data = null,
                    IsSuccessful = false,
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
                        IsSuccessful = false,
                    };
                }

                _logger.LogInformation("Kullanıcı başarıyla getirildi. UserId: {UserId}, Email: {Email}", user.Id, user.Email);

                return new ApiResponse<User>
                {
                    Data = user,
                    IsSuccessful = true,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı getirilirken hata oluştu. Email: {Email}", email);
                return new ApiResponse<User>
                {
                    Data = null,
                    IsSuccessful = false,
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
                        IsSuccessful = false,
                        Message = "Hata oluştu."
                    };
                }

                var defaultUser = await GetUserByEmail(user.Email);

                if (defaultUser != null && defaultUser.Data != null && defaultUser.Data.Email == user.Email)
                {
                    return new ApiResponse<User>
                    {
                        Data = null,
                        IsSuccessful = false,
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
                    IsSuccessful = true,
                    Message = "Başarılı işlem."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı eklenirken hata oluştu. Email: {Email}", user.Email);
                return new ApiResponse<User>
                {
                    Data = null,
                    IsSuccessful = false,
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
                        IsSuccessful = false,
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
                    IsSuccessful = true,
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<User>
                {
                    Data = null,
                    IsSuccessful = false,
                    Message = $"Error: {ex.Message}"
                };
            }

        }
    }
}