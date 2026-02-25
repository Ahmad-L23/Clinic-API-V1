using ClinicSystem.BLL;
using ClinicSystem.DTOs.PaymentDTOs;
using ClinicSystem.DTOs.PersonDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace ClinicSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(List<PaymentDTO>), 200)]
        public ActionResult<List<PaymentDTO>> GetAll() =>
            Ok(clsPayment.GetAll());


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PersonDTO), 200)]
        [ProducesResponseType(404)]
        public ActionResult<PaymentDTO> GetById(int id)
        {
            var Payment = clsPayment.Find(id);
            
            if(Payment == null)
                return NotFound($"Payment with id {id} not found");

            return Ok(Payment);
        }


        [HttpPost]
        [ProducesResponseType(typeof(PaymentAddUpdateDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PaymentAddUpdateDTO> Add(PaymentAddUpdateDTO paymentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            clsPayment payment = new clsPayment(paymentDto);

            if (!payment.Save())
            {
                return BadRequest("Failed to add payment.");
            }

            var resultDto = paymentDto with { PaymentID = payment.PaymentId };

            return CreatedAtAction(
                nameof(GetById),
                new { id = payment.PaymentId },
                resultDto);
        }

        public ActionResult<PaymentAddUpdateDTO> Update(PaymentAddUpdateDTO PaymentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            clsPayment payment = new clsPayment(PaymentDto, clsPayment.enMode.Update);

            if (!payment.Save())
                return NotFound("not Found");

            return Ok(PaymentDto);

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            if (id < 1)
                return BadRequest("Pleas enter valid Id");

            bool Deleted = clsPayment.DeletePayment(id);

            if(!Deleted)
                return NotFound($"Person with ID {id} not found.");

            return NoContent();
        }
    }
}
