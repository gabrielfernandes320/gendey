using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gendey.Models;
using gendey.Repositories.contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gendey.Controllers
{
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository<User> _authRepository;

        public AuthController(IAuthRepository<User> authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("login")]
        public async Task<object> Login(User user)
        {
            var token = await _authRepository.Authenticate(user.Email, user.Password);

            if (token == null)
                return Unauthorized(new { message = "Usuário e/ou senha informada estão incorretos." });

            return Ok(token);
        }

        [HttpGet("ValidateToken")]
        public object ValidateToken(string token)
        {
            var isValid = _authRepository.ValidateCurrentToken(token);

            if (!isValid)
                return Ok(new { message = "Invalido", isValid = false, token });

            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(_authRepository.GetClaim(token, "exp"))).DateTime;

            return Ok(new { message = "Autorizado", experationDate = expirationTime, isValid = true });
        }

        [HttpPost("RefreshToken")]
        public async Task<object> RefreshTokenAsync(Session session)
        {
            var newToken = await _authRepository.RefreshToken(session.LastToken, session.RefreshTokenCode);

            if (newToken == null)
                return Unauthorized(new { message = "Token ou codigo incorreto" });

            return Ok(newToken);
        }
    }
}
