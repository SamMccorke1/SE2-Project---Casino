# Casino Games Backend Testing & Automation

## Primary Components

- Dedicated xUnit test project: `Chuds2Chads.Tests`
- Unit tests for the horse racing, roulette, and slots backend services
- A GitHub Actions workflow at `.github/workflows/backend-tests.yml`
- Automatic test execution on every push and every pull request

## How the unit tests are organized

The backend game logic is tested at the service layer so that the game rules can be verified without rendering the Blazor UI.

- `HorseRaceServiceTests` checks core race invariants:
  - horse count clamping
  - payout calculation from winning odds
  - frame bounds and monotonic progress
  - race completion within the simulation limit
- `RouletteServiceTests` checks deterministic rule handling:
  - straight bets
  - colour bets
  - odd/even
  - high/low
  - dozens
  - zero handling
  - wheel metadata helpers
- `SlotsServiceTests` checks deterministic payout logic:
  - jackpot detection
  - three-of-a-kind payouts
  - two-cherry special case
  - losing spins

## GitHub Actions

The workflow is stored in `.github/workflows/backend-tests.yml`. GitHub automatically discovers workflow files in that folder.

On every `push` and `pull_request`, GitHub Actions will:

1. Check out the repository code.
2. Install the .NET 10 SDK.
3. Restore NuGet packages with `dotnet restore`.
4. Build the solution in Release mode.
5. Run all unit tests with `dotnet test`.
6. Collect code coverage using `XPlat Code Coverage`.
7. Upload the test result artifacts so failures can be inspected in GitHub.


