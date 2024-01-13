﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Procedural;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pobytne.Server.Controllers
{

    public class ItemsController : ControllerBase
    {
        private readonly ClientService _clientService;
        public ItemsController(ClientService clientService)
        {
            _clientService = clientService;
        }
        // GET: api/<ItemsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ItemsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ItemsController>
        [HttpPost]
        public void Post([FromBody] Permition updatePermition)
        {
        }

        // PUT api/<ItemsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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
