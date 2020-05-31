using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using gendey.Models;
using gendey.Repositories.contract;

namespace gendey.Repositories.implementation
{
    public class AuthRepository : IAuthRepository<User>
    {
        private readonly IConfiguration _config;

        private readonly gendeyContext _context;

        private readonly string Salt = "T39MQAMUHb";

        private readonly string Issuer = "Gabriel Fernandes";
        private readonly string JwtKey = "oqMfrA7XUoKmD3Tg9Tqw1xPnkT39MQAMUHb6ISBXBX1OsbBaJhLheVEZ6Vbjho3";

        public TripleDESCryptoServiceProvider TripleDes = new TripleDESCryptoServiceProvider();

        public AuthRepository(gendeyContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<object> Authenticate(string user, string password)
        {
            var dbUser = await GetUser(user);

            if (dbUser == null)
                return false;

            if (!IsAuthenticated(dbUser.Password, password)) return null;
           
            var token = GenerateJSONWebToken(user);
            var refreshTokenCode = Guid.NewGuid().ToString();
            await CreateSession(token, dbUser.Id, refreshTokenCode);

            return new { token, refreshTokenCode };

        }

        public string GenerateJSONWebToken(string user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                Issuer,
                Issuer,
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);

            return encodeToken;
        }

        public bool ValidateCurrentToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey)),
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public string GetClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
            return stringClaimValue;
        }

        public bool IsAuthenticated(string dbPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password + Salt, dbPassword);
        }

        public async Task<User> GetUser(string user)
        {
            User dbUser = await _context.User.Where(u => u.Email == user).FirstOrDefaultAsync();

            return dbUser;
        }

        public string GetEncryptedPassword(string password)
        {
            var pwdToHash = password + Salt;
            var hashToStoreInDatabase =  BCrypt.Net.BCrypt.HashPassword(pwdToHash, BCrypt.Net.BCrypt.GenerateSalt());

            return hashToStoreInDatabase;
        }

        public async Task<object> RefreshToken(string token, string refreshTokenCode)
        {
            if (!ValidateCurrentToken(token)) return null;

            var session = await GetSession(token, refreshTokenCode);

            if (session == null) return null;

            var newToken = GenerateJSONWebToken(session.User.Email);
            var newRefreshTokenCode = Guid.NewGuid().ToString();

            await UpdateSession(session, newToken, newRefreshTokenCode);

            return new { token = newToken, refreshTokenCode = newRefreshTokenCode };
        }


        public async Task<Session> GetSession(string token, string refreshTokenCode)
        {
            var session = await _context.Session
                .Where(x => x.RefreshTokenCode == refreshTokenCode && x.LastToken == token)
                .Include(x => x.User)
                .FirstOrDefaultAsync();

            return session;
        } 

        public async Task CreateSession(string token, int userId, string refreshTokenCode)
        {
            var session = new Session()
            {
                AuthDate = DateTime.Now,
                LastToken = token,
                RefreshTokenCode = refreshTokenCode,
                UserId = userId,
                TokenRefreshDate = DateTime.Now
            };

            _context.Add(session);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateSession(Session session, string token, string refreshTokenCode)
        {
            session.LastToken = token;
            session.RefreshTokenCode = refreshTokenCode;
            session.TokenRefreshDate = DateTime.Now;

            _context.Update(session);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateRefreshTokenCode(string refreshTokenCode, string token)
        {
            var session = await _context.Session.Where(x => x.RefreshTokenCode == refreshTokenCode && x.LastToken == token).FirstOrDefaultAsync();

            return session != null ? true : false;
        }
    }
}
