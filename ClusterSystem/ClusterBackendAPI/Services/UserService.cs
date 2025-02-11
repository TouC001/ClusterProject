using ClusterAPILibrary.DTOs;
using ClusterBackendAPI.Models;
using ClusterBackendAPI.Services.Repo;

namespace ClusterBackendAPI.Services
{
    public class UserService : BaseService
    {
        public UserService(Repository repository) : base(repository) { }

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
            RoleDTO result = null;

            Role updatedRole = _repository.UpdateRole(roleDTO);

            if (updatedRole != null)
            {
                result = new RoleDTO
                {
                    Id = updatedRole.Id,
                    Name = updatedRole.Name
                };
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
    }
}
