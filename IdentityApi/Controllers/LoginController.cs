using IdentityApi.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sql.Data.Model;

namespace IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public LoginController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet("login")]
        public async Task<ActionResult> LoginSystem([FromQuery] LoginRequestDto input)
        {
            User user = await _userManager.FindByNameAsync(input.UserName);
            if (user == null)
            {
                return BadRequest("当前账号不存在~");
            }

            bool exists = await _userManager.CheckPasswordAsync(user, input.Password);

            if (exists)
            {
                return new ObjectResult(new { Code = 200, Message = "登陆成功" });
            }
            else
            {
                return BadRequest("密码错误~");
            }
        }

        [HttpPost("RegisterUser")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterRequestDto input)
        {
            Role role = await _roleManager.FindByNameAsync(input.RoleName);
            if (role == null)
            {
                var resultRole = await _roleManager.CreateAsync(new Role { Name = input.RoleName });
                if (resultRole.Succeeded)
                {
                    User user = new User { UserName = input.UserMessage.UserName };
                    var resultUser = await _userManager.CreateAsync(user, input.UserMessage.Password);
                    if (resultUser.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, input.RoleName);
                        return new ObjectResult(new { Code = 200, Message = "注册成功~" });
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                User user = new User { UserName = input.UserMessage.UserName };
                var resultUser = await _userManager.CreateAsync(user, input.UserMessage.Password);
                if (resultUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, input.RoleName);
                    return new ObjectResult(new { Code = 200, Message = "注册成功~" });
                }
                else
                {
                    return BadRequest();
                }

            }

        }
    }
}
