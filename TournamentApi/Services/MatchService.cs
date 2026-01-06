using Microsoft.EntityFrameworkCore;
using TournamentApi.Data;
using TournamentApi.Domain;

namespace TournamentApi.Services;

public class MatchService
{
    private readonly AppDbContext _db;

    public MatchService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Match> PlayMatch(int matchId, int winnerUserId)
    {
        var match = await _db.Matches
            .Include(m => m.Bracket)
                .ThenInclude(b => b.Tournament)
            .FirstOrDefaultAsync(m => m.Id == matchId);

        if (match is null)
            throw new GraphQLException(ErrorBuilder.New().SetMessage("Match not found.").SetCode("NOT_FOUND").Build());

        if (match.Bracket.Tournament.Status != TournamentStatus.Started)
            throw new GraphQLException(ErrorBuilder.New().SetMessage("Tournament is not started.").SetCode("INVALID_STATUS").Build());

        if (match.WinnerId is not null)
            throw new GraphQLException(ErrorBuilder.New().SetMessage("Match already played.").SetCode("ALREADY_PLAYED").Build());

        if (winnerUserId != match.Player1Id && winnerUserId != match.Player2Id)
            throw new GraphQLException(ErrorBuilder.New().SetMessage("Winner must be one of the players.").SetCode("INVALID_WINNER").Build());

        match.WinnerId = winnerUserId;
        await _db.SaveChangesAsync();

        
        await EnsureNextRoundProgression(match);

        return match;
    }

    private async Task EnsureNextRoundProgression(Match finishedMatch)
    {
        
        var bracketId = finishedMatch.BracketId;
        var round = finishedMatch.Round;

        
        var roundMatches = await _db.Matches
            .Where(m => m.BracketId == bracketId && m.Round == round)
            .OrderBy(m => m.Id)
            .ToListAsync();

        if (roundMatches.Any(m => m.WinnerId is null))
            return;

        var winners = roundMatches.Select(m => m.WinnerId!.Value).ToList();
        if (winners.Count <= 1)
            return; 

        var nextRound = round + 1;

        
        var exists = await _db.Matches.AnyAsync(m => m.BracketId == bracketId && m.Round == nextRound);
        if (exists) return;

        for (int i = 0; i < winners.Count; i += 2)
        {
            if (i == winners.Count - 1)
            {
                var pid = winners[i];
                _db.Matches.Add(new Match
                {
                    BracketId = bracketId,
                    Round = nextRound,
                    Player1Id = pid,
                    Player2Id = pid,
                    WinnerId = pid
                });
            }
            else
            {
                _db.Matches.Add(new Match
                {
                    BracketId = bracketId,
                    Round = nextRound,
                    Player1Id = winners[i],
                    Player2Id = winners[i + 1],
                    WinnerId = null
                });
            }
        }

        await _db.SaveChangesAsync();
    }
}
