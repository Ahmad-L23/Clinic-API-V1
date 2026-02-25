using ClinicSystem.BLL;
using ClinicSystem.DTOs.DoctorDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;

namespace ClinicSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {


        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(200)]
        public ActionResult<List<DoctorDTO>> GetAll()
        {
            var AllDoctors = clsDoctor.GetAll();

            return Ok(AllDoctors);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public ActionResult<DoctorDTO> GetById(int id)
        {
            if (id < 1)
                return BadRequest("Pleas enter valid Id");

            var Doctor = clsDoctor.Find(id);

            if (Doctor == null)
                return NotFound($"doctor with id {id} not found");

            return Ok(Doctor);
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<DoctorAddUpdateDTO> Add(DoctorAddUpdateDTO DoctorDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            clsDoctor doctor = new clsDoctor(DoctorDto);

            if (!doctor.Add())
                return BadRequest("Failed to add Doctor");

            var DoctorData = new DoctorAddUpdateDTO(doctor.DoctorId, doctor.PersonId, doctor.Specialization);
        
            return Ok(DoctorData);
        }


        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public ActionResult<DoctorAddUpdateDTO> Update(DoctorAddUpdateDTO Dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            clsDoctor UpdatedDoctor = new clsDoctor(Dto, clsDoctor.enMode.Update);

            if (!UpdatedDoctor.Update())
                return NotFound("Field to update Doctor");

            return Ok(UpdatedDoctor);
        }


        [HttpDelete]
        [ProducesResponseType(201)]

        public ActionResult Delete(int id)
        {
            if (id < 1)
                return BadRequest("pleas enter valid id");

            if (!clsDoctor.DeleteDoctor(id))
                return NotFound($"Person with id {id} not found");

            return NoContent();
        }


    }
}
