using IngresosCountry.Models;

namespace IngresosCountry.Services
{
    public interface IAuthService
    {
        Task<Usuario?> ValidateUserAsync(string username, string password);
    }
}