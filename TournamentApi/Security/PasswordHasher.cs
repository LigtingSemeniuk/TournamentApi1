using System.Security.Cryptography;
using System.Text;

namespace TournamentApi.Security;

public class PasswordHasher
{
    public string Hash(string password)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    public bool Verify(string password, string hash) => Hash(password) == hash;
}
