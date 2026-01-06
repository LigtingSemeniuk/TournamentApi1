namespace TournamentApi.Domain;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;

    public List<Tournament> Tournaments { get; set; } = new();

    public List<Match> MatchesAsPlayer1 { get; set; } = new();
    public List<Match> MatchesAsPlayer2 { get; set; } = new();
    public List<Match> MatchesAsWinner { get; set; } = new();
}
