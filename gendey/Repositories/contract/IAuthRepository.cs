using System.Threading.Tasks;

namespace gendey.Repositories.contract
{
    public interface IAuthRepository<T>
    {
        Task<object> Authenticate(string user, string password);

        bool ValidateCurrentToken(string token);

        Task<object> RefreshToken(string token, string refreshTokenCode);

        string GetClaim(string token, string claimType);

        public string GetEncryptedPassword(string password);
    }
}