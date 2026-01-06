using System.Security.Claims;
using HotChocolate.Authorization;
using TournamentApi.Data;
using TournamentApi.Domain;
using TournamentApi.GraphQL.Inputs;
using TournamentApi.Security;
using TournamentApi.Services;

namespace TournamentApi.GraphQL;

public class Mutation
{
    public async Task<AuthPayload> Register(RegisterInput input, [Service] AuthService auth)
    {
        var (user, token) = await auth.Register(input.FirstName, input.LastName, input.Email, input.Password);
        return new AuthPayload(user.Id, token);
    }

    public async Task<AuthPayload> Login(LoginInput input, [Service] AuthService auth)
    {
        var (user, token) = await auth.Login(input.Email, input.Password);
        return new AuthPayload(user.Id, token);
    }

    public Task<Tournament> CreateTournament(CreateTournamentInput input, [Service] TournamentService svc)
        => svc.CreateTournament(input.Name, input.StartDate);

    
    [Authorize]
    public Task<Tournament> AddParticipant(AddParticipantInput input, ClaimsPrincipal claims, [Service] TournamentService svc)
    {
        var userId = JwtTokenService.GetUserId(claims);
        return svc.AddParticipant(input.TournamentId, userId);
    }

    [Authorize]
    public Task<Tournament> Start(StartTournamentInput input, [Service] TournamentService svc)
        => svc.StartTournament(input.TournamentId);

    [Authorize]
    public Task<Tournament> Finish(FinishTournamentInput input, [Service] TournamentService svc)
        => svc.FinishTournament(input.TournamentId);

    
    [Authorize]
    public Task<Tournament> GenerateBracket(StartTournamentInput input, [Service] TournamentService svc)
        => svc.StartTournament(input.TournamentId);

    
    public Task<List<Match>> GetMatchesForRound(GetMatchesForRoundInput input, [Service] TournamentService svc)
        => svc.GetMatchesForRound(input.TournamentId, input.Round);

    
    [Authorize]
    public Task<Match> Play(PlayMatchInput input, [Service] MatchService svc)
        => svc.PlayMatch(input.MatchId, input.WinnerUserId);
}
