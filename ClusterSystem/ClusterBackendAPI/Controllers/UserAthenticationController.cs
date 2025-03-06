using ClusterAPILibrary.DTOs;
using ClusterAPILibrary;
using ClusterBackendAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ClusterBackendAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace ClusterBackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAthenticationController : BaseController
    {
        private readonly UserAthenticationService _userAthenticationService;
        private readonly UserService _userService;

        public UserAthenticationController(UserAthenticationService userAthenticationService, UserService userService) :base(userService)
        {
            _userAthenticationService = userAthenticationService;
            _userService = userService;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult UserRegistration(UserRegistrationDTO userRegistrationDTO)
        {
            try
            {
                if (userRegistrationDTO == null)
                {
                    return BadRequest(new ApiResponse<string>("User registration information Email already exist."));
                }

                _userAthenticationService.UserRegistration(userRegistrationDTO);

                return Ok(new ApiResponse<string>("User registration successful."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult UserLogin(UserLoginDTO userLoginDTO)
        {
            try
            {
                if (userLoginDTO == null)
                {
                    return BadRequest(new ApiResponse<string>("Username or Email does not exit."));
                }

                UserLoginDTO userlogin =  _userAthenticationService.UserLogin(userLoginDTO);

                return Ok(new ApiResponse<UserLoginDTO>(userlogin, "User login was successful."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
        }

        [Authorize]
        [HttpPut]
        [Route("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(PasswordUpdateDTO passwordUpdateDTO)
        {
            try
            {
                string userName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("Invalid username.");
                }
                UserResponseDTO userExists = await _userService.GetUserByNameAsync(userName);

                //UserResponseDTO userExists = await _userService.GetUserByIdAsync(userId);

                if (userExists == null)
                {
                    return BadRequest($"User with user name: {userName} not found.");
                }

                await _userAthenticationService.UpdatePasswordAsync(passwordUpdateDTO, userName);

                return Ok(new ApiResponse<string>("Password Successfully Updated."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
        }
    }
}
