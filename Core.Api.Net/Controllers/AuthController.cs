using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Core.Api.Net.Business.Services.Auth;
using Core.Api.Net.Business.Models.Auth;
using Core.Api.Net.Business.Models.Response;
using Core.Api.Net.Business.Models.Rbac;
using Core.Api.Net.Business.Models.Account;

namespace Core.Api.Net.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _IAuthService;

        public AuthController(IAuthService IAuthService)
        {
            _IAuthService = IAuthService;
        }

        [AllowAnonymous]
        [HttpGet("RefreshToken/{AccountId}")]
        public ActionResult RefreshToken(string AccountId)
        {
            if (string.IsNullOrEmpty(AccountId))
            {
                return BadRequest();
            }

            string newToken = _IAuthService.RefreshToken(AccountId);

            if (!string.IsNullOrEmpty(newToken))
            {
                RefreshToken resp = new RefreshToken()
                {
                    status = StatusResponse.Success,
                    message = "success.",
                    token = newToken,
                };
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("SignIn")]
        public IActionResult SignIn([FromBody] SignInModel userInfo)
        {
            SignInResponse resp = _IAuthService.SignIn(userInfo);

            if (resp != null)
            {
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [HttpPost("SignOut")]
        public IActionResult SignOut([FromBody] UserInfoResponse userInfo)
        {
            SignInResponse resp = _IAuthService.SignOut(userInfo);

            if (resp != null)
            {
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [HttpPost("ChangePassword")]
        public IActionResult ChangePassword([FromBody] ChangePasswordModel info)
        {
            ChangePasswordResponse resp = _IAuthService.ChangePassword(info);

            if (resp != null)
            {
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordModel email)
        {
            MessageResponse resp = _IAuthService.ForgotPassword(email);

            if (resp != null)
            {
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public IActionResult SignUp([FromBody] AccountModel account)
        {
            MessageResponse resp = _IAuthService.SignUp(account);

            if (resp != null)
            {
                return Ok(new { resp });
            }
            else return BadRequest();
        }


        [HttpGet("ClaimRegister/{Token}")]
        public ActionResult ClaimRegister(string Token)
        {
            var _data = _IAuthService.ClaimRegister(Token);

            if (_data != null)
            {
                ResponseRegister resp = new ResponseRegister()
                {
                    status = StatusResponse.Success,
                    message = "success.",
                    data = _data,
                };
                return Ok(new { resp });
            }
            else return BadRequest();
        }

        [HttpPost("OwnerRegistered")]
        public ActionResult OwnerRegistered([FromBody] RegisterModel register)
        {
            ResponseRegister resp = _IAuthService.OwnerRegistered(register);

            return Ok(new { resp });
        }


    }
}