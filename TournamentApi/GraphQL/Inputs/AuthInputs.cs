namespace TournamentApi.GraphQL.Inputs;

public record RegisterInput(string FirstName, string LastName, string Email, string Password);
public record LoginInput(string Email, string Password);

public record AuthPayload(int UserId, string Token);
