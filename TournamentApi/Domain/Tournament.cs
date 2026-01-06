namespace TournamentApi.Domain;

public class Tournament
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public DateTime StartDate { get; set; }
    public TournamentStatus Status { get; set; } = TournamentStatus.Draft;

    public List<User> Participants { get; set; } = new();

    public Bracket? Bracket { get; set; }
}
