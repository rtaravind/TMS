namespace TMS.API.Services
{
    public interface IAuthService
    {
        string Authenticate(string username, string password);
    }
}
