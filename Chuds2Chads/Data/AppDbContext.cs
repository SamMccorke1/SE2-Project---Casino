using Chuds2Chads.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chuds2Chads.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Wallet> Wallets => Set<Wallet>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<GameRoom> GameRooms => Set<GameRoom>();
    public DbSet<RoomPlayer> RoomPlayers => Set<RoomPlayer>();
    public DbSet<GameSession> GameSessions => Set<GameSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GameRoom>()
            .HasIndex(r => r.Code)
            .IsUnique();

        modelBuilder.Entity<Wallet>()
            .HasIndex(w => w.UserId)
            .IsUnique();

        modelBuilder.Entity<GameRoom>()
            .HasMany(r => r.Players)
            .WithOne(p => p.Room!)
            .HasForeignKey(p => p.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RoomPlayer>()
            .HasIndex(p => new { p.RoomId, p.UserId })
            .IsUnique();
    }
}
