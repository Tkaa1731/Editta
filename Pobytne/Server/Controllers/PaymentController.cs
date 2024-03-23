using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;
using System.ComponentModel;
using System.Net;

namespace Pobytne.Server.Controllers
{
    [Route("Payment")]
    [ApiController]
    [Authorize]
    public class PaymentController(PaymentService paymentService) : ControllerBase
    {
        private PaymentService _paymentService = paymentService;
		public const EPermition permition = EPermition.PaymentType;

        [HttpGet]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        public async Task<IActionResult> Get([FromQuery] int moduleId, [FromQuery] string filterJSON = "")
        {
            try
            {
                var payments = await _paymentService.GetByModule(moduleId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpPut]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Update([FromBody] Payment updatePayment)
        {
            if (updatePayment is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var updatedPayment = await _paymentService.Update(updatePayment);
                if(updatedPayment is not null)
                    return Ok(updatedPayment);

                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Nepovedlo se uložit záznam."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
			}
        }
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Insert([FromBody] Payment insertPayment)
        {
            if (insertPayment is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var insertedPayment = await _paymentService.Insert(insertPayment);
                if(insertedPayment is not null)
                    return Ok(insertedPayment);

                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Nepovedlo se vložit záznam."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
			}
        }
        [HttpDelete("{id}")]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _paymentService.Delete(id);
                if (result > 0)
                    return NoContent();

                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Nepovedlo se smazat záznam."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
