
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static AutenUsrs user = new AutenUsrs();

        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AutenUsrs>> Registrar(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passHash, out byte[] passSalt);
            
            user.UserName = request.UserName;
            user.PasswordHash = passHash;
            user.PasswordSalt = passSalt;

            return Ok(user);


        }
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            if(user.UserName != request.UserName)
            {
                return BadRequest("No se encontró el usuario.");
            }

            if(!VerifyPasswordHash( request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Contraseña Incorrecta");
            }
            string token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(AutenUsrs user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
               _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims:claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);
                
            var jtw = new JwtSecurityTokenHandler().WriteToken(token);


            return jtw;
        }

        private void CreatePasswordHash(string pass,out byte[] passHash, out byte[] passSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passSalt = hmac.Key;
                passHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
            
            }
            
        }

        private bool VerifyPasswordHash(string pass, byte[] passHash, byte[] passSalt)
        {
            using(var hmac = new HMACSHA512(passSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
                return computedHash.SequenceEqual(passHash);
            }
        }

    }
}

/* https://www.youtube.com/watch?v=v7q3pEK1EA0 */