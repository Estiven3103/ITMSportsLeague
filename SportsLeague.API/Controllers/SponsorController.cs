using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Sponsor;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;
using SportsLeague.API.DTOs.TournamentSponsor;

namespace SportsLeague.API.Controllers

{

    [ApiController]

    [Route("api/[controller]")]

    public class SponsorsController : ControllerBase

    {

        private readonly ISponsorService _service;

        private readonly IMapper _mapper;

        public SponsorsController(ISponsorService service, IMapper mapper)

        {

            _service = service;

            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()

        {

            var sponsors = await _service.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<SponsorResponseDTO>>(sponsors));

        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id)

        {

            var sponsor = await _service.GetByIdAsync(id);


            if (sponsor == null)

                return NotFound();

            return Ok(_mapper.Map<SponsorResponseDTO>(sponsor));

        }

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] SponsorRequestDTO dto)

        {

            var entity = _mapper.Map<Sponsor>(dto);

            var created = await _service.CreateAsync(entity);

            return Ok(_mapper.Map<SponsorResponseDTO>(created));

        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, [FromBody] SponsorRequestDTO dto)

        {

            var entity = _mapper.Map<Sponsor>(dto);

            await _service.UpdateAsync(id, entity);

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)

        {

            await _service.DeleteAsync(id);

            return NoContent();

        }

        [HttpPost("{id}/tournaments")]
        public async Task<IActionResult> AddToTournament(int id, [FromBody] TournamentSponsorRequestDTO dto)

        {

            await _service.AddSponsorToTournamentAsync(id, dto.TournamentId, dto.ContractAmount);

            return Ok("Sponsor vinculado al torneo correctamente.");

        }

        [HttpGet("{id}/tournaments")]
        public async Task<IActionResult> GetTournaments(int id)

        {

            var relations = await _service.GetTournamentsBySponsorAsync(id);

            return Ok(_mapper.Map<IEnumerable<TournamentSponsorResponseDTO>>(relations));

        }

        [HttpDelete("{id}/tournaments/{tournamentId}")]
        public async Task<IActionResult> RemoveFromTournament(int id, int tournamentId)

        {

            await _service.RemoveSponsorFromTournamentAsync(id, tournamentId);

            return NoContent();

        }
    }
}