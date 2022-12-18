using Guden.Lib.Core;
using Guden.Lib.Bll; 

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    public class AuthController : Controller
    { 
            private IAuthService _authService; private ICore_MenuService _menuService;
            public AuthController(IAuthService authService, ICore_MenuService core_MenuService)
            {
                _authService = authService;
                _menuService = core_MenuService;
            }

            [AllowAnonymous]
            [HttpPost("Login")]
            public IActionResult Login([FromBody] UserForLoginDto userForLoginDto)
            {
                var userToLogin = _authService.Login(userForLoginDto);
                if (!userToLogin.Success)
                {
                    return Ok(userToLogin);
                }

                var result = _authService.CreateAccessToken(userToLogin.Data);
                if (result.Success)
                {

                    return Ok(result);
                }

                return Ok(result);

            }
            [HttpPost("Register")]
            public ActionResult Register([FromBody] UserForRegisterDto userForRegisterDto)
            {
                //userExists
                var userExists = _authService.UserExists(userForRegisterDto.Email, null);
                if (!userExists.Success)
                {
                    return Ok(userExists);
                }
                //user added
                var registerResult = _authService.Register(userForRegisterDto);

                if (registerResult.Success)
                {
                    return Ok(registerResult);
                }

                return Ok(registerResult);
            }
            [HttpPost("UpdateUser")]
            public ActionResult UpdateUser([FromBody] UserForRegisterDto userForRegisterDto)
            {
                //user Exists
                var userExists = _authService.UserExists(userForRegisterDto.Email, userForRegisterDto.Id);
                if (!userExists.Success)
                {
                    return Ok(userExists);
                }
                //user added
                var registerResult = _authService.UpdateUser(userForRegisterDto);

                if (registerResult.Success)
                {
                    return Ok(registerResult);
                }

                return Ok(registerResult);
            }
            [HttpGet("GetCore_MenuListByUserId")]
            [Authorize]
            public IActionResult GetCore_MenuListByUserId()
            {
                int userId = Token.GetUserId(User);

                var result = _menuService.GetMenusByUser(userId);
                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return Ok(result);
                }
            }
            [HttpPost("RefreshToken")]
            public ActionResult RefreshToken([FromBody] TokenDto token)
            {
                int userId = Token.GetUserId(User);
                return Ok(_authService.CreateRefreshToken(token.RefreshToken));
            }
        }
    }
