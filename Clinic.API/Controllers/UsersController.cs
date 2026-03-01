using ClinicSystem.BLL;
using ClinicSystem.DTOs.UserDTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ClinicSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        // =========================================
        // Get All Users
        // =========================================
        [HttpGet]
        [ProducesResponseType(typeof(List<UserDTO>), 200)]
        public ActionResult<List<UserDTO>> GetAll()
        {
            var users = clsUser.GetAllUsers();
            return Ok(users);
        }

        // =========================================
        // Get By ID
        // =========================================
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserAddUpdateDTO), 200)]
        [ProducesResponseType(404)]
        public ActionResult<UserAddUpdateDTO> GetById(int id)
        {
            var userBLL = clsUser.Find(id);

            if (userBLL == null)
                return NotFound($"User with ID {id} not found.");

            var dto = new UserAddUpdateDTO(
                userBLL.UserID,
                userBLL.PersonID,
                userBLL.UserName,
                userBLL.Password,
                userBLL.Role);

            return Ok(dto);
        }

        // =========================================
        // Add User
        // =========================================
        [HttpPost]
        [ProducesResponseType(typeof(UserAddUpdateDTO), 201)]
        [ProducesResponseType(400)]
        public ActionResult<UserAddUpdateDTO> Add([FromBody] UserAddUpdateDTO newUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (clsUser.IsUserNameExists(newUserDto.UserName))
                return BadRequest("Username already exists.");

            var userBLL = new clsUser(newUserDto, clsUser.enMode.Add);

            if (!userBLL.Save())
                return BadRequest("Failed to add user.");

            var resultDto = new UserAddUpdateDTO(
                userBLL.UserID,
                userBLL.PersonID,
                userBLL.UserName,
                userBLL.Password,
                userBLL.Role);

            return CreatedAtAction(nameof(GetById),
                new { id = resultDto.UserID },
                resultDto);
        }

        // =========================================
        // Update User
        // =========================================
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UserAddUpdateDTO), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public ActionResult<UserAddUpdateDTO> Update(int id, [FromBody] UserAddUpdateDTO updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userBLL = clsUser.Find(id);

            if (userBLL == null)
                return NotFound($"User with ID {id} not found.");

            userBLL.PersonID = updateDto.PersonID;
            userBLL.UserName = updateDto.UserName;
            userBLL.Password = updateDto.Password;
            userBLL.Role = updateDto.Role;

            if (!userBLL.Save())
                return BadRequest("Update failed.");

            var resultDto = new UserAddUpdateDTO(
                userBLL.UserID,
                userBLL.PersonID,
                userBLL.UserName,
                userBLL.Password,
                userBLL.Role);

            return Ok(resultDto);
        }

        // =========================================
        // Delete User
        // =========================================
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(int id)
        {
            bool deleted = clsUser.DeleteUser(id);

            if (!deleted)
                return NotFound($"User with ID {id} not found.");

            return NoContent();
        }
    }
}