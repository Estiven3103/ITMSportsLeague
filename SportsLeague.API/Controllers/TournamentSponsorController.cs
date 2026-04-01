using Microsoft.AspNetCore.Mvc;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers

{

    [ApiController]

    [Route("api/[controller]")]
    public class TournamentSponsorController : ControllerBase

    {

        private readonly ITournamentSponsorService _service;

        public TournamentSponsorController(ITournamentSponsorService service)

        {

            _service = service;

        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] TournamentSponsor entity)

        {

            if (entity.ContractAmount <= 0)

                return BadRequest("El monto debe ser mayor a 0.");

            var result = await _service.CreateAsync(entity);

            return Ok(result);

        }


        [HttpGet("by-sponsor/{sponsorId}")]

        public async Task<IActionResult> GetBySponsorId(int sponsorId)

        {

            var result = await _service.GetBySponsorIdAsync(sponsorId);

            return Ok(result);

        }

        [HttpDelete]

        public async Task<IActionResult> Delete([FromQuery] int sponsorId, [FromQuery] int tournamentId)

        {

            await _service.DeleteAsync(sponsorId, tournamentId);

            return NoContent();

        }
    }
}