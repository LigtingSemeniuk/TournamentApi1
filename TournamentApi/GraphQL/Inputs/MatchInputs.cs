namespace TournamentApi.GraphQL.Inputs;

public record PlayMatchInput(int MatchId, int WinnerUserId);

public enum MyMatchFilter
{
    ALL = 0,
    PLAYED = 1,
    UNPLAYED = 2
}
