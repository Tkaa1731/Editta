using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;
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
        public async Task<IEnumerable<Payment>> Get([FromQuery] int moduleId)
        {
            return await _paymentService.GetByModule(moduleId);
        }
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        [Route("Insert")]
        public async Task<IActionResult> Insert([FromBody] Payment insertPayment)
        {
            if (insertPayment is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "No data to insert"));
			}
            try
            {
                await _paymentService.Insert(insertPayment);
                return Ok();
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.Conflict, ex.Message));
			}
        }
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Payment updatePayment)
        {
            if (updatePayment is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "No data to insert"));
			}
            try
            {
                await _paymentService.Update(updatePayment);
                return Ok();
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.Conflict, ex.Message));
			}
        }
    }
}
