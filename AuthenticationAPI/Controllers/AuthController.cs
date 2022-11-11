
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
        private readonly IUserService _userService;

        public IUserService UserService { get; }

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet, Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = _userService.GetMyName();
            return Ok(userName);

            //var userName=User.Identity?.Name;
            //var userName2 = User.FindFirstValue(ClaimTypes.Name);
            //var role = User.FindFirstValue(ClaimTypes.Role);
            //return Ok(new { userName, userName2, role });
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

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return Ok(token);
        }

        //[HttpPost("refresh-token")]
        //public async Task<IActionResult<string>> RefreshToken()
        //{
        //    var refreshToken = Request.Cookies["refreshToken"];

        //}
        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(1),
                Created = DateTime.Now
            };
            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token,cookieOptions);
            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }
        private string CreateToken(AutenUsrs user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName), //Usuario
                new Claim(ClaimTypes.Role, "Admin"), //Rol a recibir del consulta para permisos 
                //new Claim(ClaimTypes.)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
               _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims:claims,
                expires: DateTime.Now.AddDays(7),
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