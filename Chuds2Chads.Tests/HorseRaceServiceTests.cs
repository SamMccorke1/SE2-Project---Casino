using Chuds2Chads.Services;

namespace Chuds2Chads.Tests;

public class HorseRaceServiceTests
{
    private readonly HorseRaceService _service = new();

    [Fact]
    public void FormatOdds_ReturnsSingleDecimalMultiplier()
    {
        Assert.Equal("3.5x", HorseRaceService.FormatOdds(3.5));
    }

    [Fact]
    public void SimulateRace_ClampsHorseCountToMinimumOfTwo()
    {
        var result = _service.SimulateRace(1, 25);

        Assert.InRange(result.WinnerIndex, 0, 1);
        Assert.NotEmpty(result.Frames);
        Assert.All(result.Frames, frame => Assert.Equal(2, frame.Positions.Count));
    }

    [Fact]
    public void SimulateRace_ClampsHorseCountToAvailableRoster()
    {
        var result = _service.SimulateRace(HorseRaceService.Horses.Count + 10, 25);

        Assert.InRange(result.WinnerIndex, 0, HorseRaceService.Horses.Count - 1);
        Assert.All(result.Frames, frame => Assert.Equal(HorseRaceService.Horses.Count, frame.Positions.Count));
    }

    [Fact]
    public void SimulateRace_PayoutMatchesWinningHorseOdds()
    {
        const long stake = 40;

        var result = _service.SimulateRace(5, stake);

        Assert.Equal((long)(stake * result.Winner.Odds), result.Payout);
    }

    [Fact]
    public void SimulateRace_ProducesBoundedMonotonicFramesUntilFinish()
    {
        var result = _service.SimulateRace(5, 10);

        Assert.InRange(result.Frames.Count, 1, 200);

        IReadOnlyList<double>? previous = null;
        foreach (var frame in result.Frames)
        {
            Assert.Equal(5, frame.Positions.Count);
            Assert.All(frame.Positions, position => Assert.InRange(position, 0.0, 100.0));

            if (previous is not null)
            {
                for (int i = 0; i < frame.Positions.Count; i++)
                {
                    Assert.True(frame.Positions[i] >= previous[i], $"Horse {i} moved backwards.");
                }
            }

            previous = frame.Positions;
        }

        Assert.Contains(result.Frames.Last().Positions, position => position >= 100.0);
    }
}
