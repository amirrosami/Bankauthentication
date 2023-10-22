using BankAuthentication.Domain;
using BankAuthentication.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankAuthentication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationServices _authenticationService;

        public AuthenticationController(AuthenticationServices authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromForm]SendRequestDto sendRequestDto)
        {
            string ip=HttpContext.Connection.RemoteIpAddress.ToString();
           var response=await _authenticationService.InsertUser(sendRequestDto,ip);
            if (response !=1)
            {
                throw new Exception("error in insert user"); 
            }
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetStatus(string userName)
        {
            var response = await _authenticationService.GetStatus(userName);
            return Ok(response);
        }
    }
}
