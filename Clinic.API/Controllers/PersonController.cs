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
        public ActionResult<PersonDTO> Add([FromBody] PersonDTO newPersonDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            clsPerson personBLL = new clsPerson(
                new PersonDTO(
                    newPersonDto.PersonID,
                    newPersonDto.Name,
                    newPersonDto.DateOfBirth,
                    newPersonDto.Gender,
                    newPersonDto.PhoneNumber,
                    newPersonDto.Email,
                    newPersonDto.Address
                ));

            if (!personBLL.Save())
                return BadRequest("Failed to add person.");

            // Create new DTO with generated ID
            var resultDto = newPersonDto with { PersonID = personBLL.PersonID };

            return CreatedAtAction(nameof(GetById),
                                   new { id = resultDto.PersonID },
                                   resultDto);
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
        public ActionResult<PersonDTO> Update(int id, [FromBody] PersonDTO UpdatepersonDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            clsPerson? personBLL = clsPerson.Find(id);

            if (personBLL == null)
                return NotFound($"Person with ID {id} not found.");

            personBLL.Name = UpdatepersonDto.Name;
            personBLL.Gender = UpdatepersonDto.Gender;
            personBLL.DateOfBirth = UpdatepersonDto.DateOfBirth;
            personBLL.PhoneNumber = UpdatepersonDto.PhoneNumber;
            personBLL.Email = UpdatepersonDto.Email;
            personBLL.Address = UpdatepersonDto.Address;

            if (!personBLL.Save())
                return BadRequest("Update failed.");

            return Ok(UpdatepersonDto);
        }

        /// <summary>
        /// Deletes a person by ID.
        /// </summary>
        /// <param name="id">Person ID to delete</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Delete(int id)
        {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");
            
            bool deleted = clsPerson.DeletePerson(id);

            if (!deleted)
                return NotFound($"Person with ID {id} not found.");

            return NoContent();
        }
    }
}
