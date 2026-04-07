namespace Chuds2Chads.Data.Entities;

public class RoomPlayer
{
    public Guid Id {get; set; } = Guid.NewGuid();
    public Guid RoomId {get; set; }
    public Guid UserId {get; set; }
    public int Seat {get; set; }
    public bool IsReady {get; set; }
    public DateTime JoinedUtc {get; set; } = DateTime.UtcNow;
    public GameRoom? Room {get; set; }
}