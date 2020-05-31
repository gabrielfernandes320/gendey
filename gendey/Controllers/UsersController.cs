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

        
        [HttpGet]
        public async Task<object> GetAllUsers()
        {
            var users = await _userRepository.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userRepository.Get(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

      
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUserAsync(int id, User user)
        {

            if (id != user.Id)
            {
                return BadRequest();
            }

            var updateReturn = await _userRepository.Update(id, user);

            if (updateReturn != null)
            {
                return Ok(user);
            }

            return BadRequest();
        }

       
        [HttpPost]
        public async Task<ActionResult<User>> AddUserAsync(User user)
        {
            var addReturn = await _userRepository.Add(user);

            if (addReturn != null)
            {
                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }

            return BadRequest();
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUserAsync(int id)
        {
            var user = await _userRepository.Get(id);
            if (user == null)
            {
                return NotFound();
            }

            var deleteReturn = _userRepository.Delete(user);

            if (deleteReturn != null)
            {
                return CreatedAtAction("GetUser", new { id = user.Id }, user);
            }

            return BadRequest();

        }
    }
}
