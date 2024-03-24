using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pobytne.Server.Service;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Struct;
using System.Net;
using ClientDTO = Pobytne.Shared.Procedural.DTO.Client;

namespace Pobytne.Server.Controllers
{
    [Route("Client")]
    [Authorize]
    [ApiController]
    public class ClientController(ClientService clientService) : ControllerBase
	{
        private readonly ClientService _clientService = clientService;
        public const EPermition permition = EPermition.Client;

        [HttpGet]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        public async Task<IActionResult> Get([FromQuery] int moduleId, [FromQuery] string filterJSON)
        {
            try
            {
                var filter = JsonConvert.DeserializeObject<LazyList>(filterJSON);
                if (filter is null)
                    return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));

                var clients = await _clientService.Get(moduleId,filter);
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpGet]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        [Route("Count")]
        public async Task<IActionResult> GetCount([FromQuery] int moduleId, [FromQuery] string filterJSON)
        {
            try
            {
                var filter = JsonConvert.DeserializeObject<LazyList>(filterJSON);
                if (filter is null)
                    return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));

                var count = await _clientService.GetCount(moduleId,filter);
                return Ok(count);
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpPut]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Update([FromBody] ClientDTO updateClient)
        {
            if (updateClient is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var updatedClient = await _clientService.Update(updateClient);
                if (updatedClient is not null)
                    return Ok(updatedClient);

                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Nepovedlo se uložit záznam."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Insert([FromBody] ClientDTO insertClient)
        {
            if (insertClient is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var insertedClient = await _clientService.Insert(insertClient);
                if (insertedClient is not null)
                    return Ok(insertedClient);

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
                var result = await _clientService.Delete(id);
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
