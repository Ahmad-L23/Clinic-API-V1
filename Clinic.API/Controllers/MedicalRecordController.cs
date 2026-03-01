using ClinicSystem.BLL;
using ClinicSystem.DTOs.MedicalRecordDTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ClinicSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MedicalRecordController : ControllerBase
    {
        /// <summary>
        /// Retrieves all medical records with pagination.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<MedicalRecordDTO>), 200)]
        public ActionResult<List<MedicalRecordDTO>> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var records = clsMedicalRecord.GetAllMedicalRecords(pageNumber, pageSize);
            return Ok(records);
        }

        /// <summary>
        /// Retrieves a specific medical record by ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MedicalRecordAddUpdateDTO), 200)]
        [ProducesResponseType(404)]
        public ActionResult<MedicalRecordAddUpdateDTO> GetById(int id)
        {
            var recordBLL = clsMedicalRecord.Find(id);

            if (recordBLL == null)
                return NotFound($"Medical record with ID {id} not found.");

            var dto = new MedicalRecordAddUpdateDTO(
                recordBLL.MedicalRecordID.Value,
                recordBLL.VisitDescription,
                recordBLL.Diagnosis,
                recordBLL.AdditionalNotes
            );

            return Ok(dto);
        }

        /// <summary>
        /// Adds a new medical record.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(MedicalRecordAddUpdateDTO), 201)]
        [ProducesResponseType(400)]
        public ActionResult<MedicalRecordAddUpdateDTO> Add([FromBody] MedicalRecordAddUpdateDTO newRecordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var recordBLL = new clsMedicalRecord(newRecordDto, clsMedicalRecord.enMode.Add);

            if (!recordBLL.Save())
                return BadRequest("Failed to add medical record.");

            var resultDto = new MedicalRecordAddUpdateDTO(
                recordBLL.MedicalRecordID.Value,
                recordBLL.VisitDescription,
                recordBLL.Diagnosis,
                recordBLL.AdditionalNotes
            );

            return CreatedAtAction(nameof(GetById),
                                   new { id = resultDto.MedicalRecordID },
                                   resultDto);
        }

        /// <summary>
        /// Updates an existing medical record.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MedicalRecordAddUpdateDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<MedicalRecordAddUpdateDTO> Update(int id, [FromBody] MedicalRecordAddUpdateDTO updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var recordBLL = clsMedicalRecord.Find(id);

            if (recordBLL == null)
                return NotFound($"Medical record with ID {id} not found.");

            recordBLL.VisitDescription = updateDto.VisitDescription;
            recordBLL.Diagnosis = updateDto.Diagnosis;
            recordBLL.AdditionalNotes = updateDto.AdditionalNotes;

            if (!recordBLL.Save())
                return BadRequest("Update failed.");

            var resultDto = new MedicalRecordAddUpdateDTO(
                recordBLL.MedicalRecordID.Value,
                recordBLL.VisitDescription,
                recordBLL.Diagnosis,
                recordBLL.AdditionalNotes
            );

            return Ok(resultDto);
        }

        /// <summary>
        /// Deletes a medical record by ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Delete(int id)
        {
            if (id < 1)
                return BadRequest($"Not accepted ID {id}");

            bool deleted = clsMedicalRecord.DeleteMedicalRecord(id);

            if (!deleted)
                return NotFound($"Medical record with ID {id} not found.");

            return NoContent();
        }
    }
}