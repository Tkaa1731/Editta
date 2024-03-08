using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;

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
        public async Task<IEnumerable<Payment>> Get([FromQuery] int moduleNumber)
        {
            return await _paymentService.GetByModule(moduleNumber);
        }
    }
}
