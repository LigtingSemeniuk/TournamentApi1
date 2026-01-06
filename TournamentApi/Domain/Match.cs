namespace TournamentApi.Domain;

public class Match
{
    public int Id { get; set; }
    public int Round { get; set; }

    public int BracketId { get; set; }
    public Bracket Bracket { get; set; } = default!;

    public int Player1Id { get; set; }
    public User Player1 { get; set; } = default!;

    public int Player2Id { get; set; }
    public User Player2 { get; set; } = default!;

    public int? WinnerId { get; set; }
    public User? Winner { get; set; }
}
