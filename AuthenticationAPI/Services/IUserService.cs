using AuthenticationAPI.Models;

namespace AuthenticationAPI.Services
{
    public interface IUserService
    {
        List<Usuario> usrdata { get; set; }  
        string GetMyName();
        Task<Usuario> DataUsr(string usr);


    }
}
