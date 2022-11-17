
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AuthenticationAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static AutenUsrs user = new AutenUsrs();
        // public  List<Usuario> usuariodata = new List<Usuario>();
        public Usuario usuariodata = new Usuario();
        public Usuario usuario = new Usuario();

        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly DbNeoContext _context;

        public IUserService UserService { get; }

        public AuthController(IConfiguration configuration, IUserService userService, DbNeoContext _DbNeoContext)
        {
            _configuration = configuration;
            _userService = userService;
            _context = _DbNeoContext;
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
            //var user =  _userService.GetUsr(request.UserName);
            //  var usuaridata = _userService.DataUsr(request.UserName) ;

            var usuaridata = await _context.Nivels
                .Include(a=>a.IdUsuarioNavigation)
                .Include(a=>a.IdProyectoNavigation)
                .Include(a=>a.IdRolNavigation)
                .FirstOrDefaultAsync(a => a.IdUsuarioNavigation.UsFicha == request.UserName & a.IdProyectoNavigation.Pnombre==request.Proyecto);

            if (usuaridata == null)
            {
                return BadRequest("null");
            }

            if (usuaridata.IdUsuarioNavigation.UsFicha != request.UserName)
            {
                return BadRequest("NotFoundUser");
            }

            //Verifica contraseña sin ecriptar
            if (request.Password != usuaridata.IdUsuarioNavigation.UsPass)
            {
                return BadRequest("WrongPassword");
            }

            //Verifica la contraseña ebcriptada
            //if(!VerifyPasswordHash( request.Password, user.PasswordHash, user.PasswordSalt))
            //{
            //    return BadRequest("Contraseña Incorrecta");
            //}

            string token = CreateToken(usuaridata);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return Ok(token);
        }


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
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }
        private string CreateToken(Nivel user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.IdUsuarioNavigation.UsFicha), //Usuario (ficha)
                new Claim(ClaimTypes.Role,  user.IdRolNavigation.Rnombre), //Rol a recibir del consulta para permisos 
                new Claim(ClaimTypes.GivenName,user.IdUsuarioNavigation.UsNombre), // Nombre
                new Claim(ClaimTypes.Surname,user.IdUsuarioNavigation.UsApellido), // Apellido
                //new Claim(ClaimTypes.Country,user.IdNivel.ToString(); // Nombre
                //new Claim(ClaimTypes.)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
               _configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: cred);

            var jtw = new JwtSecurityTokenHandler().WriteToken(token);


            return jtw;
        }


        ////Crear token con clase usuario del api
        //private string CreateToken(AutenUsrs user)
        //{
        //    List<Claim> claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.UserName), //Usuario
        //        new Claim(ClaimTypes.Role, "Admin"), //Rol a recibir del consulta para permisos 
        //        //new Claim(ClaimTypes.)
        //    };

        //    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
        //       _configuration.GetSection("AppSettings:Token").Value));

        //    var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        //    var token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(7),
        //        signingCredentials: cred);

        //    var jtw = new JwtSecurityTokenHandler().WriteToken(token);


        //    return jtw;
        //}

        private void CreatePasswordHash(string pass, out byte[] passHash, out byte[] passSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passSalt = hmac.Key;
                passHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));

            }

        }

        private bool VerifyPasswordHash(string pass, byte[] passHash, byte[] passSalt)
        {
            using (var hmac = new HMACSHA512(passSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
                return computedHash.SequenceEqual(passHash);
            }
        }

    }
}

/* https://www.youtube.com/watch?v=v7q3pEK1EA0 */