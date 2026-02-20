using ClinicSystem.BLL;
using ClinicSystem.DTOs.PatientDTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ClinicSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PatientController : ControllerBase
    {
        /// <summary>
        /// Retrieves all patients from the system with pagination.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<PatientDTO>), 200)]
        public ActionResult<List<PatientDTO>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var patients = clsPatient.GetAllPatients(pageNumber, pageSize);
            return Ok(patients);
        }

        /// <summary>
        /// Retrieves a specific patient by ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PatientDTO), 200)]
        [ProducesResponseType(404)]
        public ActionResult<PatientDTO> GetById(int id)
        {
            var patientBLL = clsPatient.Find(id);

            if (patientBLL == null)
                return NotFound($"Patient with ID {id} not found.");

            var dto = new PatientAddUpdateDTO(patientBLL.PatientID.Value, patientBLL.PersonID);
            return Ok(dto);
        }

        /// <summary>
        /// Adds a new patient.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(PatientDTO), 201)]
        [ProducesResponseType(400)]
        public ActionResult<PatientDTO> Add([FromBody] PatientAddUpdateDTO newPatientDto)
        {
            if (newPatientDto.PersonID < 1)
                return BadRequest();

            if (!clsPerson.isExist(newPatientDto.PersonID))
                return NotFound($"Person with ID {newPatientDto.PersonID} not found");

            var patientBLL = new clsPatient(newPatientDto, clsPatient.enMode.Add);

            if (!patientBLL.Save())
                return BadRequest("Failed to add patient.");

            var resultDto = new PatientAddUpdateDTO(patientBLL.PatientID.Value, patientBLL.PersonID);
            return CreatedAtAction(nameof(GetById),
                                   new { id = resultDto.PatientID },
                                   resultDto);
        }

        /// <summary>
        /// Updates an existing patient.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PatientDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<PatientDTO> Update(int id, [FromBody] PatientAddUpdateDTO updatePatientDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var patientBLL = clsPatient.Find(id);

            if (patientBLL == null)
                return NotFound($"Patient with ID {id} not found.");

            // Only PersonID can be updated
            patientBLL.PersonID = updatePatientDto.PersonID;

            if (!patientBLL.Save())
                return BadRequest("Update failed.");

            var resultDto = new PatientAddUpdateDTO(patientBLL.PatientID.Value, patientBLL.PersonID);
            return Ok(resultDto);
        }

        /// <summary>
        /// Deletes a patient by ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Delete(int id)
        {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            bool deleted = clsPatient.DeletePatient(id);

            if (!deleted)
                return NotFound($"Patient with ID {id} not found.");

            return NoContent();
        }
    }
}