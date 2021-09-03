using Data.ViewModels;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;
        public AuthenticateController(IJWTAuthenticationManager jWTAuthenticationManager)
        {
            this.jWTAuthenticationManager = jWTAuthenticationManager;
        }



        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthModel model)
        {
            var token = jWTAuthenticationManager.Authenticate(model.Username, model.Password);

            if (token == null)
                return Unauthorized();

            AuthResponseModel authresmodel = new AuthResponseModel()
            {
                Token = token
            };

            return Ok(authresmodel);
        }
    }
}
