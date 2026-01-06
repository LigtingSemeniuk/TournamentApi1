namespace TournamentApi.GraphQL.Inputs;

public record CreateTournamentInput(string Name, DateTime StartDate);
public record AddParticipantInput(int TournamentId);
public record StartTournamentInput(int TournamentId);
public record FinishTournamentInput(int TournamentId);
public record GetMatchesForRoundInput(int TournamentId, int Round);
