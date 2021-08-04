using ELibrary.Common.Helpers;
using ELibrary.Core.Abstractions;
using ELibrary.Dtos;
using ELibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ELibrary.MVC.Controllers.ApiControllers
{
    [AllowAnonymous]
    public class AuthController : BaseApiController
    {
        private readonly IAuthServices _authservices;
     

        public AuthController(IAuthServices authServices)
        {
            _authservices = authServices;
          
        }



        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegistrationDto model)
        {
            if (ModelState.IsValid)
            {
                var BASE_URL = UrlHelper.BaseAddress(HttpContext);
                var result = await _authservices.RegisterUserAsync(model,BASE_URL);
                return Ok(result);
            }
            return BadRequest("not successful!");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDetailDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authservices.LoginUserAsync(model);
                return Ok(result);
            }
            return BadRequest("Some properties are not valid");

        }

        [HttpPost("Logout")]
        public IActionResult LogOut()
        {
            var result = _authservices.Logout();
            return Ok(result);

        }


        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmation model)
        {
            if (string.IsNullOrWhiteSpace(model.userid) || string.IsNullOrWhiteSpace(model.Token))
                return NotFound();

            var result = await _authservices.ConfirmEmailAsync(model.userid, model.Token);

            return Ok(result);
        }
       
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody]ForgotPwdDto model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return NotFound();

            var result = await _authservices.ForgetPasswordAsync(model.Email, Url, Request.Scheme);

            if (result.Success)
                return Ok(result);
            
            return BadRequest(result); 
        }




        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authservices.ResetPasswordAsync(model);

                if (result.Success)
                    return Ok(result);

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }
    }
}
