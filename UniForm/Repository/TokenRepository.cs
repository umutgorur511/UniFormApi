using Microsoft.EntityFrameworkCore;
using UniForm.Data;
using UniForm.Entity;
using UniForm.Interfaces;
using UniForm.Models;

namespace UniForm.Repository
{
    public class TokenRepository : ITokenRepository
    {
        public readonly DataContext _context;
        private readonly ILogger<TokenRepository> _logger;

        public TokenRepository(DataContext context, ILogger<TokenRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<bool>> ValidateTokenAsync(string accessToken)
        {
            try
            {
                var tokenObject = await _context.Tokens.FirstOrDefaultAsync(t => t.Token == accessToken);

                if (tokenObject != null && !string.IsNullOrEmpty(tokenObject.Token)) //TokenObject.CreateDate kontrolü
                {
                    var elapsedMinutes = (DateTime.UtcNow - tokenObject.LastLoginDate).TotalMinutes;
                    if (elapsedMinutes <= 15)
                    {
                        return new ApiResponse<bool>
                        {
                            IsSuccessful = true,
                            Message = "Token validation successful",
                            Data = true
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
            }

            return new ApiResponse<bool>
            {
                IsSuccessful = false,
                Message = "Token validation failed",
                Data = false
            };
        }

        public async Task<ApiResponse<string>> CreateTokenAsync(int userId)
        {
            var uniqueToken = Guid.NewGuid().ToString();

            await _context.AddAsync(new Tokens
            {
                Token = uniqueToken,
                LastLoginDate = DateTime.UtcNow,
                UserId = userId
            });

            return new ApiResponse<string>
            {
                IsSuccessful = true,
                Message = "Token created successfully",
                Data = uniqueToken
            };
        }
    }
}