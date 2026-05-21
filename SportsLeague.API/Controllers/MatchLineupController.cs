using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using SportsLeague.API.DTOs.Request;

using SportsLeague.API.DTOs.Response;

using SportsLeague.Domain.Entities;

using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers;

[ApiController]

[Route("api/match/{matchId}/lineup")]

public class MatchLineupController : ControllerBase

{

    private readonly IMatchLineupService _matchLineupService;

    private readonly IMapper _mapper;

    public MatchLineupController(

        IMatchLineupService matchLineupService,

        IMapper mapper)

    {

        _matchLineupService = matchLineupService;

        _mapper = mapper;

    }

    // Registrar alineacion

    [HttpPost]

    public async Task<ActionResult<MatchLineupResponseDTO>> RegisterLineup(

        int matchId,

        MatchLineupRequestDTO dto)

    {

        try

        {

            var lineup = _mapper.Map<MatchLineup>(dto);

            var created = await _matchLineupService

                .RegisterLineupAsync(matchId, lineup);

            var lineupList = await _matchLineupService

                .GetLineupByMatchAsync(matchId);

            var createdLineup = lineupList

                .FirstOrDefault(l => l.Id == created.Id);

            return Ok(_mapper.Map<MatchLineupResponseDTO>(

                createdLineup));

        }

        catch (KeyNotFoundException ex)

        {

            return NotFound(new { message = ex.Message });

        }

        catch (InvalidOperationException ex)

        {

            return Conflict(new { message = ex.Message });

        }

    }

    // Obtener alineacion completa

    [HttpGet]

    public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>>

        GetLineup(int matchId)

    {

        try

        {

            var lineup = await _matchLineupService

                .GetLineupByMatchAsync(matchId);

            return Ok(_mapper.Map

                <IEnumerable<MatchLineupResponseDTO>>(lineup));

        }

        catch (KeyNotFoundException ex)

        {

            return NotFound(new { message = ex.Message });

        }

    }

    // Obtener alineacion por equipo 

    [HttpGet("team/{teamId}")]

    public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>>

        GetLineupByTeam(int matchId, int teamId)

    {

        try

        {

            var lineup = await _matchLineupService

                .GetLineupByMatchAndTeamAsync(matchId, teamId);

            return Ok(_mapper.Map

                <IEnumerable<MatchLineupResponseDTO>>(lineup));

        }

        catch (KeyNotFoundException ex)

        {

            return NotFound(new { message = ex.Message });

        }

    }

    // Eliminar alineacion

    [HttpDelete("{lineupId}")]

    public async Task<ActionResult> DeleteLineup(

        int matchId,

        int lineupId)

    {

        try

        {

            await _matchLineupService

                .DeleteLineupAsync(lineupId);

            return NoContent();

        }

        catch (KeyNotFoundException ex)

        {

            return NotFound(new { message = ex.Message });

        }

    }
}