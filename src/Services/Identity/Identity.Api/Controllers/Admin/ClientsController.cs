using System.Threading.Tasks;
using Adams.Services.Identity.Api.Configuration;
using Adams.Services.Identity.Api.Data;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adams.Services.Identity.Api.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json", "application/problem+json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Policy = AuthorizationConstants.AdministrationPolicy)]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationConfigurationDbContext _db;
        private readonly IClientStore _store;

        public ClientsController(IClientStore store, ApplicationConfigurationDbContext db)
        {
            _store = store;
            _db = db;
        }

        [HttpPost(Name = nameof(CreateClient))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateClient([FromBody] Client client)
        {
            var existingClient = await _store.FindClientByIdAsync(client.ClientId);
            if (existingClient != null)
            {
                return CreatedAtAction(nameof(GetClient), existingClient);
            }

            var entity = client.ToEntity();
            _db.Clients.Attach(entity);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClient), await _store.FindClientByIdAsync(entity.ClientId));
        }

        [HttpGet("{clientId}", Name = nameof(GetClient))]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClient(string clientId)
        {
            var client = await _store.FindClientByIdAsync(clientId);

            return Ok(client);
        }
    }
}