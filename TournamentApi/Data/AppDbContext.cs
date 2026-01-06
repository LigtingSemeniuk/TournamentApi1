using Microsoft.EntityFrameworkCore;
using TournamentApi.Domain;

namespace TournamentApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Tournament> Tournaments => Set<Tournament>();
    public DbSet<Bracket> Brackets => Set<Bracket>();
    public DbSet<Match> Matches => Set<Match>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        
        modelBuilder.Entity<Tournament>()
            .HasMany(t => t.Participants)
            .WithMany(u => u.Tournaments)
            .UsingEntity<Dictionary<string, object>>(
                "TournamentParticipants",
                j => j.HasOne<User>()
                      .WithMany()
                      .HasForeignKey("UserId")
                      .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Tournament>()
                      .WithMany()
                      .HasForeignKey("TournamentId")
                      .OnDelete(DeleteBehavior.Cascade),
                j => j.HasKey("TournamentId", "UserId")
            );

        
        modelBuilder.Entity<Tournament>()
            .HasOne(t => t.Bracket)
            .WithOne(b => b.Tournament)
            .HasForeignKey<Bracket>(b => b.TournamentId)
            .OnDelete(DeleteBehavior.Cascade);

        
        modelBuilder.Entity<Bracket>()
            .HasMany(b => b.Matches)
            .WithOne(m => m.Bracket)
            .HasForeignKey(m => m.BracketId)
            .OnDelete(DeleteBehavior.Cascade);

        
        modelBuilder.Entity<Match>()
            .HasOne(m => m.Player1)
            .WithMany(u => u.MatchesAsPlayer1)
            .HasForeignKey(m => m.Player1Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.Player2)
            .WithMany(u => u.MatchesAsPlayer2)
            .HasForeignKey(m => m.Player2Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.Winner)
            .WithMany(u => u.MatchesAsWinner)
            .HasForeignKey(m => m.WinnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
