namespace SportsLeague.API.DTOs.TournamentSponsor
{
    public class TournamentSponsorResponseDTO
    {
        public int TournamentId { get; set; }

        public decimal ContractAmount { get; set; }

        public DateTime JoinedAt { get; set; }
    }
}