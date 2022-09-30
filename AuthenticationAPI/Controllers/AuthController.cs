using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static AutenUsrs user = new AutenUsrs();

        [HttpPost("register")]
        public async Task<ActionResult<AutenUsrs>> Registrar(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passHash, out byte[] passSalt);
            
            user.UserName = request.UserName;
            user.PasswordHash = passHash;
            user.PasswordSalt = passSalt;

            return Ok(user);


        }

        private void CreatePasswordHash(string pass,out byte[] passHash, out byte[] passSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passSalt = hmac.Key;
                passHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
            
            }
            
        }
    }
}

/* https://www.youtube.com/watch?v=v7q3pEK1EA0 */