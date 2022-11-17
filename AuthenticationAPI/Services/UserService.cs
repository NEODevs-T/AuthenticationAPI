using AuthenticationAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthenticationAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DbNeoContext _context;
        public UserService(IHttpContextAccessor httpContextAccessor, DbNeoContext _DbNeoContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = _DbNeoContext;
        }

        public  Usuario usrdata { get; set; } = new Usuario ();
        List<Usuario> IUserService.usrdata { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string GetMyName()
        {
            var result=string.Empty;
            if(_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }
           return result;
        }
        public async Task<Usuario> DataUsr(string user)
        {
         
            usrdata = await _context.Usuarios.FirstOrDefaultAsync(a => a.UsFicha == user);
            return usrdata;
        }
       
    }
}
