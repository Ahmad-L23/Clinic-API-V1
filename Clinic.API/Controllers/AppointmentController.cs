using ClinicSystem.BLL;
using ClinicSystem.DTOs.AppointmentDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace ClinicSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AppointmentController : ControllerBase
    {
        /// <summary>
        /// Retrieves all appointments with pagination.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(object), 200)]
        public IActionResult GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var result = clsAppointment.GetAllAppointments(pageNumber, pageSize);

            return Ok(new
            {
                Data = result.Data,
                result.TotalRecords,
                result.TotalPages,
                PageNumber = pageNumber,
                PageSize = pageSize
            });
        }

        /// <summary>
        /// Retrieves a specific appointment by ID.
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AppointmentDTO), 200)]
        [ProducesResponseType(404)]
        public ActionResult<AppointmentDTO> GetById(int id)
        {
            var appointmentBLL = clsAppointment.Find(id);

            if (appointmentBLL == null)
                return NotFound($"Appointment with ID {id} not found.");

            return Ok(new AppointmentAddUpdateDTO(
                appointmentBLL.AppointmentID,
                appointmentBLL.PatientID,
                appointmentBLL.DoctorID,
                appointmentBLL.AppointmentDateTime,
                appointmentBLL.AppointmentStatus,
                appointmentBLL.MedicalRecordID,
                appointmentBLL.PaymentID
            ));
        }

        /// <summary>
        /// Retrieves appointments by Patient ID.
        /// </summary>
        [HttpGet("ByPatient/{patientId}")]
        [ProducesResponseType(typeof(List<AppointmentDTO>), 200)]
        public ActionResult<List<AppointmentDTO>> GetByPatient(int patientId)
        {
            var appointments = clsAppointment.GetAppointmentsByPatientID(patientId);
            return Ok(appointments);
        }

        /// <summary>
        /// Retrieves appointments by Doctor ID.
        /// </summary>
        [HttpGet("ByDoctor/{doctorId}")]
        [ProducesResponseType(typeof(List<AppointmentDTO>), 200)]
        public ActionResult<List<AppointmentDTO>> GetByDoctor(int doctorId)
        {
            var appointments = clsAppointment.GetAppointmentsByDoctorID(doctorId);
            return Ok(appointments);
        }

        /// <summary>
        /// Adds a new appointment.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AppointmentAddUpdateDTO), 201)]
        [ProducesResponseType(400)]
        public ActionResult Add([FromBody] AppointmentAddUpdateDTO newAppointmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Prevent double booking
            if (clsAppointment.AppointmentExists(
                newAppointmentDto.DoctorID,
                newAppointmentDto.AppointmentDateTime))
            {
                return BadRequest("This doctor already has an appointment at this time.");
            }

            var appointmentBLL = new clsAppointment(newAppointmentDto, clsAppointment.enMode.Add);

            if (!appointmentBLL.Save())
                return BadRequest("Failed to create appointment.");

            var resultDto = new AppointmentAddUpdateDTO(
                appointmentBLL.AppointmentID,
                appointmentBLL.PatientID,
                appointmentBLL.DoctorID,
                appointmentBLL.AppointmentDateTime,
                appointmentBLL.AppointmentStatus,
                appointmentBLL.MedicalRecordID,
                appointmentBLL.PaymentID
            );

            return CreatedAtAction(nameof(GetById),
                                   new { id = resultDto.AppointmentID },
                                   resultDto);
        }

        /// <summary>
        /// Updates an existing appointment.
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(AppointmentAddUpdateDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult Update(int id, [FromBody] AppointmentAddUpdateDTO updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appointmentBLL = clsAppointment.Find(id);

            if (appointmentBLL == null)
                return NotFound($"Appointment with ID {id} not found.");

            appointmentBLL.PatientID = updateDto.PatientID;
            appointmentBLL.DoctorID = updateDto.DoctorID;
            appointmentBLL.AppointmentDateTime = updateDto.AppointmentDateTime;
            appointmentBLL.AppointmentStatus = updateDto.AppointmentStatus;
            appointmentBLL.MedicalRecordID = updateDto.MedicalRecordID;
            appointmentBLL.PaymentID = updateDto.PaymentID;

            if (!appointmentBLL.Save())
                return BadRequest("Update failed.");

            return Ok(updateDto);
        }

        /// <summary>
        /// Deletes an appointment by ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult Delete(int id)
        {
            bool deleted = clsAppointment.DeleteAppointment(id);

            if (!deleted)
                return NotFound($"Appointment with ID {id} not found.");

            return NoContent();
        }
    }
}