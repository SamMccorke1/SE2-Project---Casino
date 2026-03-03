namespace Chuds2Chads.Services;

/// <summary>
/// Simulates horse races with true per-run randomness.
/// </summary>
public class HorseRaceService
{
    // Domain types 
    public record Horse(string Name, string Emoji, double BaseSpeed, double Odds);

    public record RaceResult(
        Horse Winner,
        int   WinnerIndex,
        long  Payout,
        IReadOnlyList<RaceFrame> Frames);

    /// <summary>
    /// One animation frame: position of each horse (0–100).
    /// </summary>
    public record RaceFrame(IReadOnlyList<double> Positions);

    // Roster

    public static readonly IReadOnlyList<Horse> Horses = new Horse[]
    {
        new("Thunder Bolt",  "🐎", 1.10, 2.0),
        new("Shadow Runner", "🏇", 1.00, 3.5),
        new("Golden Hoof",   "🐴", 0.95, 4.0),
        new("Wild Wind",     "🐎", 0.90, 5.0),
        new("Iron Stallion", "🏇", 0.85, 6.5),
    };

    // Simulation

    /// <summary>
    /// Simulates a full race.  Every call draws fresh values from Random.Shared,
    /// so no two races are identical.
    /// </summary>
    public RaceResult SimulateRace(int horseCount, long stake)
    {
        horseCount = Math.Clamp(horseCount, 2, Horses.Count);
        var rng = Random.Shared;   // ← the fix

        // Shuffle roster so starting positions vary each race
        var contestants = Horses
            .Take(horseCount)
            .OrderBy(_ => rng.NextDouble())
            .ToArray();

        // Per-race speed: base * random multiplier in [0.75, 1.25]
        double[] speeds = contestants
            .Select(h => h.BaseSpeed * (0.75 + rng.NextDouble() * 0.50))
            .ToArray();

        double[] positions = new double[horseCount];
        var frames = new List<RaceFrame>(120);

        for (int tick = 0; tick < 200; tick++)
        {
            for (int i = 0; i < horseCount; i++)
            {
                double surge = rng.NextDouble() * 0.8;
                positions[i] = Math.Min(100.0, positions[i] + speeds[i] + surge);
            }

            frames.Add(new RaceFrame((double[])positions.Clone()));
            if (positions.Any(p => p >= 100.0)) break;
        }

        int winnerIdx = Array.IndexOf(positions, positions.Max());
        var winner    = contestants[winnerIdx];
        long payout   = (long)(stake * winner.Odds);

        return new RaceResult(winner, winnerIdx, payout, frames);
    }

    public static string FormatOdds(double odds) => $"{odds:F1}x";
}