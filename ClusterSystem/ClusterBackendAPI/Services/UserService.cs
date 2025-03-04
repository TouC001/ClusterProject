using ClusterAPILibrary;
using ClusterAPILibrary.DTOs;
using ClusterBackendAPI.Models;
using ClusterBackendAPI.Services.Repo;

namespace ClusterBackendAPI.Services
{
    public class UserService : BaseService
    {
        public UserService(Repository repository, ILogger<UserService> logger) : base(repository, logger) { }

        #region Roles

        /// <summary>
        /// Gets a Role from the database.
        /// </summary>
        /// <param name="id">The Id that is tied to the role.</param>
        /// <returns></returns>
        public Role GetRoleById(int id)
        {
            return _repository.GetRoleById(id);
        }

        /// <summary>
        /// Gets all Roles from the database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Role> GetRoles()
        {
            return _repository.GetRoles();
        }

        /// <summary>
        /// Updates a Role in the database.
        /// </summary>
        /// <param name="roleDTO">The Role being updated.</param>
        /// <returns>A RoleDTO with updated values.</returns>
        public RoleDTO UpdateRole(RoleDTO roleDTO)
        {
            _logger.LogInformation($"Updating role with Id: {roleDTO.Id}");

            RoleDTO result = null;

            try
            {
                Role updatedRole = _repository.UpdateRole(roleDTO);

                if (updatedRole != null)
                {
                    result = new RoleDTO
                    {
                        Id = updatedRole.Id,
                        Name = updatedRole.Name
                    };

                    _logger.LogInformation($"Role with Id: {roleDTO.Id} updated successfully.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating role with Id: {roleDTO.Id}. Error: {ex.Message}");
            }
            

            return result;
        }

        /// <summary>
        /// Add a Role to the database.
        /// </summary>
        /// <param name="roleDTO">The new role information.</param>
        public void AddRole(RoleDTO roleDTO)
        {
            Role role = new Role
            {
                Name = roleDTO.Name
            };

            _repository.AddRole(role);
        }
        #endregion

        #region Users

        /// <summary>
        /// Gets a User by its ID with its User Roles.
        /// </summary>
        /// <param name="userId">The Id tied to the User.</param>
        /// <returns>The User Object or Null.</returns>
        public UserResponseDTO GetUserById(int userId)
        {
            User user = _repository.GetUserById(userId);

            if (user != null)
            {
                return new UserResponseDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Is_banned = user.is_banned,
                    userRoleDTOs = user.userRoles.Select(ur => new UserRoleDTO
                    {
                        Id = ur.Id,
                        RoleId = ur.RoleId,
                        UserId = ur.UserId,
                        RoleDTO = new RoleDTO
                        {
                            Id = ur.Role.Id,
                            Name = ur.Role.Name
                        }
                    }).ToList()
                };
            }

            return null;
        }

        /// <summary>
        /// Gets all users and their User Role.
        /// </summary>
        /// <returns></returns>
        public List<UserResponseDTO> GetUsers()
        {
            List<User> users = _repository.GetUsers().ToList();

            if (!users.Any())
            {
                return new List<UserResponseDTO>(); // Just going to return an emtpy list.
            }

            return users.Select(user => new UserResponseDTO()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                userRoleDTOs = user.userRoles.Select(ur => new UserRoleDTO
                {
                    Id = ur.Id,
                    RoleId = ur.RoleId,
                    UserId = ur.UserId,
                    RoleDTO = new RoleDTO
                    {
                        Id = ur.Role.Id,
                        Name = ur.Role.Name
                    }
                }).ToList()
            }).ToList();
        }

        /// <summary>
        /// Updates a User in the Database.
        /// </summary>
        /// <param name="userResponseDTO"> DTO that has the updated user information. </param>
        public void UpdateUser(UserResponseDTO userResponseDTO)
        {
            if (userResponseDTO == null)
            {
                throw new ArgumentNullException(nameof(userResponseDTO), "User data must not be null.");
            }

            User user = _repository.GetUserById(userResponseDTO.Id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userResponseDTO.Id} could not be found.");
            }

            user.UserName = userResponseDTO.UserName;
            user.Email = userResponseDTO.Email;

            _repository.UpdateUser(user);
        }

        #endregion
    }
}
