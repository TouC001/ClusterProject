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
            return Ok("It's working!");
        }
    }
}
