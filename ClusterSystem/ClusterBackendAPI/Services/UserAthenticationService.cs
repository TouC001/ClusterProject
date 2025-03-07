using ClusterAPILibrary.DTOs;
using ClusterBackendAPI.Models;
using ClusterBackendAPI.Services.Repo;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClusterBackendAPI.Services
{
    public class UserAthenticationService : BaseService
    {
        /// <summary>
        /// The secret key used to generate the JWT token.
        /// </summary>
        private readonly string _jwtSecret;

        /// <summary>
        /// The number of days the JWT token will expire.
        /// </summary>
        private readonly int _jwtLifespans;

        public UserAthenticationService(Repository repository, ILogger<UserAthenticationService> logger, IConfiguration configuration) : base(repository, logger)
        {
            // Getting the secret key from the appsettings.json file.
            _jwtSecret = configuration["Jwt:Secret"];

            // Getting the number of days the token will expire from the appsettings.json file.
            _jwtLifespans = int.Parse(configuration["Jwt:Lifespan"]);
        }

        /// <summary>
        /// Authenticates a user during registration.
        /// </summary>
        /// <param name="userRegistrationDTO">DTO that is holding the Users information.</param>
        /// <returns>The User Registraction DTO object.</returns>
        public async Task<UserRegistrationDTO> UserRegistration(UserRegistrationDTO userRegistrationDTO)
        {
            if (userRegistrationDTO == null)
                throw new ArgumentNullException(nameof(userRegistrationDTO));

            List<User> existingUser = await _repository.GetUsersAsync(userRegistrationDTO.Email, null);

            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already exits.");
            }

            userRegistrationDTO.UserName = await GenerateUniqueUserName("Enjoyer");

            _repository.UserRegister(userRegistrationDTO);

            return new UserRegistrationDTO()
            {
                UserName = userRegistrationDTO.UserName,
                Email = userRegistrationDTO.Email,
                Is_banned = userRegistrationDTO.Is_banned,
                userRoleDTO = userRegistrationDTO.userRoleDTO,
                
            };
        }

        /// <summary>
        /// Authenticates a user during login.
        /// </summary>
        /// <param name="userLoginDTO">DTO that contains the User Login details.</param>
        /// <returns>Returns a DTO or Null.</returns>
        public async Task<UserLoginDTO> UserLogin(UserLoginDTO userLoginDTO)
        {
            if (userLoginDTO == null)
            {
                _logger.LogError("UserLoginDTO is null.");
                throw new ArgumentNullException(nameof(userLoginDTO));
            }

            try
            {
                User user = await _repository.UserLoginAsync(userLoginDTO);

                if (user == null)
                {
                    _logger.LogWarning("User not found for provided Username or Email.");
                    return null;
                }

                // Generating a token for the user.
                string token = GenerateJwtToken(user);

                return new UserLoginDTO()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = token
                };
            }
            catch
            (Exception ex)
            {
                _logger.LogError($"Error logging in user: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Updates the current users password.
        /// </summary>
        /// <param name="passwordUpdateDTO">The new password information.</param>
        /// <param name="userId">The Id tied to the User.</param>
        public async Task UpdatePasswordAsync(PasswordUpdateDTO passwordUpdateDTO, int userId)
        {
            try
            {
                if (passwordUpdateDTO == null || userId == 0)
                {
                    throw new InvalidOperationException("Something is null;");
                }
                await _repository.PasswordUpdateAsync(passwordUpdateDTO, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error logging in UpdatePassword: {ex.Message}");
                throw;
            }
        }

        #region Helper Methods
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7142",
                audience: "https://localhost:7142",
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtLifespans),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> GenerateUniqueUserName(string enjoyer)
        {
            string userName = enjoyer;
            List<User> existingUsers;

            // Keep generating a new username if one already exists
            do
            {
                existingUsers = await _repository.GetUsersAsync(null, userName);

                if (existingUsers.Any())
                {
                    userName = $"{enjoyer}{new Random().Next(10000, 99999)}";
                }

            } while (existingUsers.Any()); // Keep going if there's already a user with that username

            return userName;
        }
        #endregion
    }
}
