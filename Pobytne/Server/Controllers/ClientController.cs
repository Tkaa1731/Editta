using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Struct;

namespace Pobytne.Server.Controllers
{
    [Route("Client")]
    [Authorize]
    [ApiController]
    public class ClientController : ControllerBase
	{
        private readonly ClientService _clientService;
        public const EPermition permition = EPermition.Client; 
        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }
        [HttpGet]
		[PermissionAuthorize(permition, EAccess.ReadOnly)]
		[Route("ClientsList")]
        public Task<IEnumerable<Shared.Procedural.Client>> GetClients(int moduleNumber)
        {
            return _clientService.GetClientsByModule(moduleNumber);
        }
    }
}
