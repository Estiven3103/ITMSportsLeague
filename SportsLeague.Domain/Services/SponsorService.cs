using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System.Net.Mail;

namespace SportsLeague.API.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorRepository;

        private readonly ITournamentRepository _tournamentRepository;

        private readonly ITournamentSponsorRepository _tournamentSponsorRepository;

        public SponsorService(

            ISponsorRepository sponsorRepository,

            ITournamentRepository tournamentRepository,

            ITournamentSponsorRepository tournamentSponsorRepository)

        {
            _sponsorRepository = sponsorRepository;

            _tournamentRepository = tournamentRepository;

            _tournamentSponsorRepository = tournamentSponsorRepository;

        }

        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            return await _sponsorRepository.GetAllAsync();
        }

        public async Task<Sponsor?> GetByIdAsync(int id)
        {
            return await _sponsorRepository.GetByIdAsync(id);
        }

        public async Task<Sponsor> CreateAsync(Sponsor sponsor)
        {

            var exists = await _sponsorRepository.ExistsByNameAsync(sponsor.Name);

            if (exists)

                throw new InvalidOperationException("Ya existe un sponsor con ese nombre.");

            if (!IsValidEmail(sponsor.ContactEmail))

                throw new InvalidOperationException("El email no es válido.");

            sponsor.CreatedAt = DateTime.UtcNow;

            return await _sponsorRepository.CreateAsync(sponsor);
        }

        public async Task UpdateAsync(int id, Sponsor sponsor)

        {

            var existing = await _sponsorRepository.GetByIdAsync(id);

            if (existing == null)

                throw new KeyNotFoundException("Sponsor no encontrado.");


            var exists = await _sponsorRepository.ExistsByNameAsync(sponsor.Name);

            if (exists && existing.Name != sponsor.Name)

                throw new InvalidOperationException("Ya existe un sponsor con ese nombre.");

            if (!IsValidEmail(sponsor.ContactEmail))

                throw new InvalidOperationException("El email no es válido.");

            existing.Name = sponsor.Name;

            existing.ContactEmail = sponsor.ContactEmail;

            existing.Phone = sponsor.Phone;

            existing.WebsiteUrl = sponsor.WebsiteUrl;

            existing.Category = sponsor.Category;

            existing.UpdatedAt = DateTime.UtcNow;

            await _sponsorRepository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id)

        {

            var existing = await _sponsorRepository.GetByIdAsync(id);

            if (existing == null)

                throw new KeyNotFoundException("Sponsor no encontrado.");

            await _sponsorRepository.DeleteAsync(id);

        }

        public async Task AddSponsorToTournamentAsync(int sponsorId, int tournamentId, decimal contractAmount)

        {

            var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);

            if (sponsor == null)

                throw new KeyNotFoundException("Sponsor no encontrado.");

            var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);

            if (tournament == null)

                throw new KeyNotFoundException("Torneo no encontrado.");

            if (contractAmount <= 0)

                throw new InvalidOperationException("El monto debe ser mayor a 0.");

            var existingRelation = await _tournamentSponsorRepository

                .GetByIdsAsync(sponsorId, tournamentId);

            if (existingRelation != null)

                throw new InvalidOperationException("Este sponsor ya está vinculado a este torneo.");

            var relation = new TournamentSponsor

            {

                SponsorId = sponsorId,

                TournamentId = tournamentId,

                ContractAmount = contractAmount,

                JoinedAt = DateTime.UtcNow

            };

            await _tournamentSponsorRepository.CreateAsync(relation);

        }
        public async Task RemoveSponsorFromTournamentAsync(int sponsorId, int tournamentId)

        {

            var relation = await _tournamentSponsorRepository

                .GetByIdsAsync(sponsorId, tournamentId);

            if (relation == null)

                throw new KeyNotFoundException("Relación no encontrada.");

            await _tournamentSponsorRepository.DeleteAsync(relation);

        }

        public async Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId)

        {

            var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);


            if (sponsor == null)

                throw new KeyNotFoundException("Sponsor no encontrado.");

            return await _tournamentSponsorRepository.GetBySponsorIdAsync(sponsorId);

        }

        private bool IsValidEmail(string email)

        {

            try

            {

                var mail = new MailAddress(email);

                return true;

            }

            catch


            {
                return false;

            }
        }
    }
}