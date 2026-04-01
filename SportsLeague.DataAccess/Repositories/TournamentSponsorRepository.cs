using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.Infrastructure.Repositories
{
    public class TournamentSponsorRepository : ITournamentSponsorRepository


    {
        private readonly LeagueDbContext _context;

        public TournamentSponsorRepository(LeagueDbContext context)

        {

            _context = context;

        }

        public async Task<TournamentSponsor> CreateAsync(TournamentSponsor entity)

        {

            await _context.TournamentSponsors.AddAsync(entity);

            await _context.SaveChangesAsync();

            return entity;

        }

        public async Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId)

        {

            return await _context.TournamentSponsors

                .Where(ts => ts.SponsorId == sponsorId)

                .Include(ts => ts.Sponsor)

                .Include(ts => ts.Tournament)

                .ToListAsync();

        }

        public async Task<TournamentSponsor?> GetByIdsAsync(int sponsorId, int tournamentId)

        {

            return await _context.TournamentSponsors

                .FirstOrDefaultAsync(ts =>

                    ts.SponsorId == sponsorId &&

                    ts.TournamentId == tournamentId);

        }

        public async Task DeleteAsync(TournamentSponsor entity)

        {

            _context.TournamentSponsors.Remove(entity);

            await _context.SaveChangesAsync();

        }
    }
}