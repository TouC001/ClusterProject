using ClusterAPILibrary;
using ClusterAPILibrary.DTOs;
using ClusterBackendAPI.DataContext;
using ClusterBackendAPI.Models;
using ClusterBackendAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClusterBackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : BaseController
    {
        private readonly UserService _userServices;
        public DataController(UserService userService) : base(userService)
        {
            _userServices = userService;
        }

        [HttpGet]
        public IActionResult Test()
        {
            //return Ok("It's working!");

            try
            {
                //throw new Exception("Something went wrong while processing the request.");

                var role = new RoleDTO()
                {
                    Id = 1,
                    Name = "Admin"
                };

                return Ok(new ApiResponse<RoleDTO>(role));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
        }

        #region Roles

        /// <summary>
        /// Gets a single Role based on the Id.
        /// </summary>
        /// <param name="id">The Id Related to the role.</param>
        /// <returns>A success or failed response.</returns>
        [HttpGet]
        [Route("GetRole")]
        public IActionResult GetRole(int id)
        {
            try
            {
                Role role = _userService.GetRoleById(id);
                if (role == null)
                {
                    throw new Exception("Role not found.");
                }
                RoleDTO roleDTO = new RoleDTO
                {
                    Id = role.Id,
                    Name = role.Name
                };
                return Ok(new ApiResponse<RoleDTO>(roleDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
        }

        /// <summary>
        /// Adds a new role to the database.
        /// </summary>
        /// <param name="roleDTO">DTO containing the new role.</param>
        /// <returns>A success or failed response.</returns>
        [HttpPost]
        [Route("AddRole")]
        public IActionResult AddRole(RoleDTO roleDTO)
        {
            try
            {
               if (roleDTO == null)
                {
                    throw new Exception("RoleDTO information is empty.");
                }

               Role existingRole = _userService.GetRoles().FirstOrDefault(r => r.Name == roleDTO.Name);

                if (existingRole != null)
                {
                    throw new Exception("Role with that name already exists.");
                }

                _userService.AddRole(roleDTO);

                return Ok(new ApiResponse<string>("Role added successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
        }

        #endregion

    }
}
