using ClusterAPILibrary.DTOs;
using ClusterBackendAPI.DataContext;
using ClusterBackendAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClusterBackendAPI.Services.Repo
{
    public class Repository
    {
        private readonly ClusterDbContext _context;

        private readonly IPasswordHasher<User> _passwordHasher;

        public Repository(ClusterDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Save changes to the database
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }

        #region Roles

        /// <summary>
        /// Gets a certain Role by Id
        /// </summary>
        /// <param name="id">The Id that reoresents the Role.</param>
        /// <returns>A Role from the database.</returns>
        public Role GetRoleById(int id)
        {
            return _context.Roles.FirstOrDefault(r => r.Id == id);
        }

        /// <summary>
        /// Gets all Roles from the database.
        /// </summary>
        /// <returns>A list of all Roles in the database.</returns>
        public IEnumerable<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }

        /// <summary>
        /// Adds a Role to the database.
        /// </summary>
        /// <param name="role">The new Role information being added.</param>
        public void AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
        }

        /// <summary>
        /// Removes a Role from the database.
        /// </summary>
        /// <param name="role">The Role information being removed.</param>
        public void RemoveRole(Role role)
        {
            _context.Roles.Remove(role);
        }

        /// <summary>
        /// Updates a Role in the database.
        /// </summary>
        /// <param name="role">The Role information being updated.</param>
        public Role UpdateRole(RoleDTO roleDTO)
        {
            Role role = _context.Roles.FirstOrDefault(r => r.Id == roleDTO.Id);

            if (role != null)
            {
                role.Name = roleDTO.Name;
                _context.SaveChanges();
            }
            return role;
        }

        #endregion

        #region Users

        /// <summary>
        /// Gets a User by Id
        /// </summary>
        /// <param name="id">The ID related to the User.</param>
        /// <returns>A user object.</returns>
        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                               .Include(u => u.userRoles)
                               .ThenInclude(ur => ur.Role)
                               .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            return await _context.Users
                               .Include(u => u.userRoles)
                               .ThenInclude(ur => ur.Role)
                               .FirstOrDefaultAsync(u => u.UserName == name);
        }

        /// <summary>
        /// Get Users from the database.
        /// </summary>
        /// <returns>A list of Users along with there Roles.</returns>
        public IEnumerable<User> GetUsers()
        {
            return _context.Users
                               .Include(u => u.userRoles)
                               .ThenInclude(ur => ur.Role)
                               .ToList();
        }

        /// <summary>
        /// Updates a User in the database.
        /// </summary>
        /// <param name="userUpdateDTO">The new user information.</param>
        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        #endregion

        #region UserAuth

        /// <summary>
        /// Register a new user to the database.
        /// </summary>
        /// <param name="userRegistrationDTO">New user information being used.</param>
        public void UserRegister(UserRegistrationDTO userRegistrationDTO)
        {
            User user = new User
            {
                UserName = userRegistrationDTO.UserName,
                Email = userRegistrationDTO.Email,
                is_banned = false,
                PasswordHash = _passwordHasher.HashPassword(null, userRegistrationDTO.Password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // Always assign "Enjoyer" role (RoleId = 3)
            const int enjoyerRoleId = 3;

            UserRole userRole = new UserRole()
            {
                UserId = user.Id,
                RoleId = enjoyerRoleId
            };

            _context.UserRoles.Add(userRole);
            _context.SaveChanges();
        }

        /// <summary>
        /// Login a user to the database.
        /// </summary>
        /// <param name="userLoginDTO">User Login Details.</param>
        public User UserLogin(UserLoginDTO userLoginDTO)
        {
            User user = _context.Users.FirstOrDefault(u => u.UserName == userLoginDTO.UserName || u.Email == userLoginDTO.Email);

            if (user != null)
            {
                PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLoginDTO.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    return user;
                }
            }

            return null;
        }

        /// <summary>
        /// Updating A Users password and saving it.
        /// </summary>
        /// <param name="passwordUpdateDTO">Contains the </param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="UnauthorizedAccessException"></exception>
        public async Task PasswordUpdateAsync(PasswordUpdateDTO passwordUpdateDTO, string name)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == name);

            if (user != null)
            {
                PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, passwordUpdateDTO.CurrentPassword);

                if (result != PasswordVerificationResult.Success)
                {
                    throw new UnauthorizedAccessException("Current password was incorrect.");
                }

                if (passwordUpdateDTO.NewPassword == passwordUpdateDTO.ConfirmNewPassword)
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, passwordUpdateDTO.NewPassword);

                    _context.Users.Update(user);
                    _context.SaveChangesAsync();
                }
            }
        }

        #endregion
    }
}
