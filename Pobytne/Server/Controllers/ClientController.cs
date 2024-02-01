using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Procedural;

namespace Pobytne.Server.Controllers
{
    [Route("Client")]
    [ApiController]
    public class ClientController : ControllerBase
	{
        private readonly ClientService _clientService;
        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }
        [Authorize]
        [HttpGet]
        [Route("ClientsList")]
        public Task<IEnumerable<Shared.Procedural.Client>> GetClients(int moduleNumber)
        {
            return _clientService.GetClientsByModule(moduleNumber);
        }
    }
}
