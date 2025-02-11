using ClusterAPILibrary.DTOs;
using ClusterBackendAPI.DataContext;
using ClusterBackendAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClusterBackendAPI.Services.Repo
{
    public class Repository
    {
        private readonly ClusterDbContext _context;

        public Repository(ClusterDbContext context)
        {
            _context = context;
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
            Role role = _context.Roles
                                     .Where(a => a.Id == roleDTO.Id)
                                     .Include(a => a.Name)
                                     .FirstOrDefault();

            if (role != null)
            {
                role.Name = roleDTO.Name;
                _context.SaveChanges();
            }

            return role;
        }

        #endregion
    }
}
