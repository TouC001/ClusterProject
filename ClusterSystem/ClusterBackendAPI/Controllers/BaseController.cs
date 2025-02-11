using ClusterBackendAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClusterBackendAPI.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly UserService _userService;

        public BaseController(UserService userService)
        {
            _userService = userService;
        }
    }
}
