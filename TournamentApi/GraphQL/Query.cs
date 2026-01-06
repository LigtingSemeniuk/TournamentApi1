using System.Security.Claims;
using HotChocolate.Authorization;
using Microsoft.EntityFrameworkCore;
using TournamentApi.Data;
using TournamentApi.Domain;
using TournamentApi.GraphQL.Inputs;
using TournamentApi.Security;

namespace TournamentApi.GraphQL;

public class Query
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Tournament> GetTournaments([Service] AppDbContext db)
        => db.Tournaments.AsNoTracking();

    public Task<Tournament?> GetTournamentById(int id, [Service] AppDbContext db)
        => db.Tournaments
            .Include(t => t.Participants)
            .Include(t => t.Bracket)
                .ThenInclude(b => b!.Matches)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);

    [Authorize]
    public async Task<User> Me([Service] AppDbContext db, ClaimsPrincipal claims)
    {
        var userId = JwtTokenService.GetUserId(claims);
        var user = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        return user ?? throw new GraphQLException(ErrorBuilder.New().SetMessage("Unauthorized").SetCode("UNAUTHORIZED").Build());
    }

    [Authorize]
    public async Task<List<Match>> MyMatches(
        MyMatchFilter filter,
        [Service] AppDbContext db,
        ClaimsPrincipal claims)
    {
        var userId = JwtTokenService.GetUserId(claims);

        var q = db.Matches
            .Include(m => m.Player1)
            .Include(m => m.Player2)
            .Include(m => m.Winner)
            .Where(m => m.Player1Id == userId || m.Player2Id == userId);

        q = filter switch
        {
            MyMatchFilter.PLAYED => q.Where(m => m.WinnerId != null),
            MyMatchFilter.UNPLAYED => q.Where(m => m.WinnerId == null),
            _ => q
        };

        return await q.AsNoTracking().ToListAsync();
    }

    
    public Task<List<Match>> MatchesForRound(int tournamentId, int round, [Service] AppDbContext db)
        => db.Matches
            .Include(m => m.Player1)
            .Include(m => m.Player2)
            .Include(m => m.Winner)
            .Where(m => m.Bracket.TournamentId == tournamentId && m.Round == round)
            .AsNoTracking()
            .ToListAsync();
}
