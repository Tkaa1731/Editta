using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Client.Pages.ModulePages;
using Pobytne.Server.Service;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;

namespace Pobytne.Server.Controllers
{
    [Route("Payment")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private PaymentService _paymentService;
		public const EPermition permition = EPermition.PaymentType;

		public PaymentController(PaymentService paymentService) => _paymentService = paymentService;

        [HttpGet]
        public async Task<IEnumerable<Payment>> Get([FromQuery] int moduleNumber)
        {
            return await _paymentService.GetByModule(moduleNumber);
        }
    }
}
