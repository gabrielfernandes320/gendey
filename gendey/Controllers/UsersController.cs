using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using gendey.Models;
using gendey.Repositories.contract;
using gendey.Repositories.implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace gendey.Controllers
{
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IGendeyRepository<User> _userRepository;

        public UsersController(IGendeyRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/users
        [HttpGet]
        public async Task<object> GetAllUsers()
        {
            var user = await _userRepository.GetAll();
            return Ok(user);
        }
    }
}
