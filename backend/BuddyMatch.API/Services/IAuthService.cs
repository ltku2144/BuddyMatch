using System.Threading.Tasks;

namespace BuddyMatch.API.Services
{
    public interface IAuthService
    {
        Task<bool> ValidateCredentialsAsync(string username, string password);
    }
}