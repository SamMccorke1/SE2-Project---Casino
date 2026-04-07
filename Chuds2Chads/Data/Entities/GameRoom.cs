using System;
using System.Collections.Generic;

namespace Chuds2Chads.Data.Entities;

public enum GameType
{
    Blackjack = 1,
    Roulette = 2,
    Slots = 3,
    Poker = 4,
    HorseRace = 5
}

public enum RoomStatus
{
    Lobby = 1,
    InGame = 2,
    Closed = 3
}

public class GameRoom
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Code { get; set; } = "";

    public GameType Game { get; set; }

    public RoomStatus Status { get; set; } = RoomStatus.Lobby;

    public Guid HostUserId { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public ICollection<RoomPlayer> Players { get; set; } = new List<RoomPlayer>();
}
