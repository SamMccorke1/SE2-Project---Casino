using Chuds2Chads.Data;
using Chuds2Chads.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chuds2Chads.Services;

/// <summary>
/// Handles all wallet reads, bet deductions, and payout credits.
/// Injected as a scoped service so each request gets its own DbContext lifetime.
/// </summary>
public class WalletService
{
    private readonly AppDbContext _db;

    public WalletService(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Returns the current coin balance for the given user.
    /// Returns 0 if no wallet exists yet.
    /// Wallet is set to be created on first bet.
    /// </summary>
    public async Task<long> GetBalanceAsync(Guid userId)
    {
        var wallet = await _db.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        return wallet?.Balance ?? 0;
    }

    /// <summary>
    /// Attempts to deduct <paramref name="amount"/> coins as a bet.
    /// Returns false if the balance is insufficient or wallet does not exist.
    /// Creates a BetPlaced Transaction record on success.
    /// </summary>
    public async Task<bool> TryPlaceBetAsync(Guid userId, long amount, string reference)
    {
        if (amount <= 0) return false;

        var wallet = await _db.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        if (wallet == null || wallet.Balance < amount) return false;

        wallet.Balance -= amount;
        wallet.UpdatedUtc = DateTime.UtcNow;

        _db.Transactions.Add(new Transaction
        {
            UserId = userId,
            Type = TransactionType.BetPlaced,
            Amount = -amount,
            BalanceAfter = wallet.Balance,
            Reference = reference,
            CreatedUtc = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Credits <paramref name="amount"/> coins to the user as a payout.
    /// Creates a Payout Transaction record.
    /// </summary>
    public async Task CreditPayoutAsync(Guid userId, long amount, string reference)
    {
        if (amount <= 0) return;

        var wallet = await _db.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        if (wallet == null)
        {
            // Auto-create wallet if somehow missing
            wallet = new Wallet { UserId = userId, Balance = 0 };
            _db.Wallets.Add(wallet);
        }

        wallet.Balance += amount;
        wallet.UpdatedUtc = DateTime.UtcNow;

        _db.Transactions.Add(new Transaction
        {
            UserId = userId,
            Type = TransactionType.Payout,
            Amount = amount,
            BalanceAfter = wallet.Balance,
            Reference = reference,
            CreatedUtc = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
    }
}