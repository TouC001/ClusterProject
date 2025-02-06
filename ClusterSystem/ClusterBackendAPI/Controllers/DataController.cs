using ClusterAPILibrary;
using ClusterAPILibrary.DTOs;
using ClusterBackendAPI.DataContext;
using Microsoft.AspNetCore.Mvc;

namespace ClusterBackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly ClusterDbContext _context;
        public DataController(ClusterDbContext content) 
        {
            _context = content;
        }

        [HttpGet]
        public IActionResult Test()
        {
            //return Ok("It's working!");

            try
            {
                throw new Exception("Something went wrong while processing the request.");

                var role = new RoleDTO()
                {
                    Id = 1,
                    Name = "Admin"
                };

                return Ok(new ApiResponse<RoleDTO>(role));
            }
            catch(Exception ex)
            {
                return BadRequest(new ApiResponse<string>(ex.Message));
            }
        }

    }
}
