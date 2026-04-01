using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Services

{
    public class TournamentSponsorService : ITournamentSponsorService

    {
        private readonly ITournamentSponsorRepository _repository;

        public TournamentSponsorService(ITournamentSponsorRepository repository)

        {

            _repository = repository;

        }

        public async Task<TournamentSponsor> CreateAsync(TournamentSponsor entity)

        {

            var existing = await _repository.GetByIdsAsync(entity.SponsorId, entity.TournamentId);

            if (existing != null)

                throw new InvalidOperationException("Este sponsor ya está asociado a este torneo.");

            if (entity.ContractAmount <= 0)

                throw new InvalidOperationException("El monto debe ser mayor a 0.");

            entity.JoinedAt = DateTime.UtcNow;

            return await _repository.CreateAsync(entity);

        }

        public async Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId)

        {

            return await _repository.GetBySponsorIdAsync(sponsorId);

        }

        public async Task DeleteAsync(int sponsorId, int tournamentId)

        {

            var existing = await _repository.GetByIdsAsync(sponsorId, tournamentId);

            if (existing == null)

                throw new KeyNotFoundException("Relación no encontrada.");

            await _repository.DeleteAsync(existing);

        }
    }
}