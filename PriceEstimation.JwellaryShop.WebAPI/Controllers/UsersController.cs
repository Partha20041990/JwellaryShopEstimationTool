using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PriceEstimation.JwellaryShop.WebApi.Entities;
using PriceEstimation.JwellaryShop.WebApi.Services;
using PriceEstimation.JwellaryShop.WebApi.Services.Contracts;

namespace PriceEstimation.JwellaryShop.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            if(userParam==null)
                return BadRequest(new { message = "Parameter can not be null" });

            var user = _userService.Authenticate(userParam.Username, userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [Authorize(Roles = Role.Privileged)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }
    }
}
