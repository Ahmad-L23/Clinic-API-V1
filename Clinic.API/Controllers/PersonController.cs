using ClinicSystem.BLL;
using ClinicSystem.DTOs.PersonDTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ClinicSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PersonController : ControllerBase
    {
        /// <summary>
        /// Retrieves all persons from the system.
        /// </summary>
        /// <returns>List of PersonDTO objects</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<PersonDTO>), 200)]
        public ActionResult<List<PersonDTO>> GetAll()
        {
            var persons = clsPerson.GetAllPersons();
            return Ok(persons);
        }

        /// <summary>
        /// Retrieves a specific person by ID.
        /// </summary>
        /// <param name="id">Person ID</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PersonDTO), 200)]
        [ProducesResponseType(404)]
        public ActionResult<PersonDTO> GetById(int id)
        {
           
            var person = clsPerson.Find(id);

            if (person == null)
                return NotFound($"Person with ID {id} not found.");

            return Ok(person);
        }

        /// <summary>
        /// Adds a new person.
        /// </summary>
        /// <param name="personDto">Person data to add</param>
        [HttpPost]
        [ProducesResponseType(typeof(PersonDTO), 201)]
        [ProducesResponseType(400)]
        public ActionResult<PersonDTO> Add([FromBody] PersonDTO personDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var personBLL = new clsPerson(personDto, clsPerson.enMode.Add);

            if (!personBLL.Save())
                return BadRequest("Failed to add person.");

            return CreatedAtAction(nameof(GetById), new { id = personBLL.PersonID }, personDto);
        }

        /// <summary>
        /// Updates an existing person.
        /// </summary>
        /// <param name="id">Person ID to update</param>
        /// <param name="personDto">Updated person data</param>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Update(int id, [FromBody] PersonDTO personDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != personDto.PersonID)
                return BadRequest("ID mismatch.");

            var personBLL = new clsPerson(personDto, clsPerson.enMode.Update);

            if (!personBLL.Save())
                return NotFound($"Person with ID {id} not found or update failed.");

            return NoContent();
        }

        /// <summary>
        /// Deletes a person by ID.
        /// </summary>
        /// <param name="id">Person ID to delete</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(int id)
        {
            bool deleted = clsPerson.DeletePerson(id);

            if (!deleted)
                return NotFound($"Person with ID {id} not found.");

            return NoContent();
        }
    }
}
