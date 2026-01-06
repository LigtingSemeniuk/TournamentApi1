using Microsoft.EntityFrameworkCore;
using TournamentApi.Data;
using TournamentApi.Domain;
using TournamentApi.Security;

namespace TournamentApi.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher _hasher;
    private readonly JwtTokenService _jwt;

    public AuthService(AppDbContext db, PasswordHasher hasher, JwtTokenService jwt)
    {
        _db = db;
        _hasher = hasher;
        _jwt = jwt;
    }

    public async Task<(User user, string token)> Register(string firstName, string lastName, string email, string password)
    {
        email = email.Trim().ToLowerInvariant();

        var exists = await _db.Users.AnyAsync(u => u.Email == email);
        if (exists)
            throw new GraphQLException(ErrorBuilder.New()
                .SetMessage("Email already registered.")
                .SetCode("EMAIL_TAKEN")
                .Build());

        var user = new User
        {
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Email = email,
            PasswordHash = _hasher.Hash(password)
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        var token = _jwt.CreateToken(user);
        return (user, token);
    }

    public async Task<(User user, string token)> Login(string email, string password)
    {
        email = email.Trim().ToLowerInvariant();

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user is null || !_hasher.Verify(password, user.PasswordHash))
            throw new GraphQLException(ErrorBuilder.New()
                .SetMessage("Invalid credentials.")
                .SetCode("INVALID_CREDENTIALS")
                .Build());

        var token = _jwt.CreateToken(user);
        return (user, token);
    }
}
