using ClusterAPILibrary.DTOs;
using ClusterAPILibrary;
using ClusterBackendAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClusterBackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAthenticationController : ControllerBase
    {
        private readonly UserAthenticationService _userAthenticationService;

        public UserAthenticationController(UserAthenticationService userAthenticationService)
        {
            _userAthenticationService = userAthenticationService;
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
    }
}
