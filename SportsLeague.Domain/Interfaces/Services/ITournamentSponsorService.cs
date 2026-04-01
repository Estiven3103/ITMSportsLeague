using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface ITournamentSponsorService
    {
        Task<TournamentSponsor> CreateAsync(TournamentSponsor entity);

        Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId);

        Task DeleteAsync(int sponsorId, int tournamentId);
    }
}