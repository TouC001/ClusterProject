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
        public UserRegistrationDTO UserRegistration(UserRegistrationDTO userRegistrationDTO)
        {
            if (userRegistrationDTO == null)
                throw new ArgumentNullException(nameof(userRegistrationDTO));

            var existingUser = _repository.GetUsers().FirstOrDefault(u => u.Email == userRegistrationDTO.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already exits.");
            }

            userRegistrationDTO.UserName = GenerateUniqueUserName("Enjoyer");

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
        public UserLoginDTO UserLogin(UserLoginDTO userLoginDTO)
        {
            if (userLoginDTO == null)
            {
                _logger.LogError("UserLoginDTO is null.");
                throw new ArgumentNullException(nameof(userLoginDTO));
            }

            try
            {
                User user = _repository.UserLogin(userLoginDTO);

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

        public async Task UpdatePasswordAsync(PasswordUpdateDTO passwordUpdateDTO, string name)
        {
            try
            {
                if (passwordUpdateDTO == null || name == "")
                {
                    throw new InvalidOperationException("Something is null;");
                }
                await _repository.PasswordUpdateAsync(passwordUpdateDTO, name);
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
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
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

        private string GenerateUniqueUserName(string enjoyer)
        {
            string userName;
            Random random = new Random();
            do
            {
                userName = $"{enjoyer}{random.Next(100000, 999999)}";
            } while (_repository.GetUsers().Any(u => u.UserName == userName));

            return userName;
        }
        #endregion
    }
}
