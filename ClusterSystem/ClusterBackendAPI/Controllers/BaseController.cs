using ClusterBackendAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClusterBackendAPI.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly UserService _userServices;

        public BaseController(UserService userServices)
        {
            _userServices = userServices;
        }
    }
}
