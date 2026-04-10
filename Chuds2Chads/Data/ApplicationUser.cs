<<<<<<< HEAD
using Microsoft.AspNetCore.Identity;

namespace Chuds2Chads.Data;

public enum UserPresenceStatus
{
    Offline = 0,
    Online = 1,
    InLobby = 2,
    AtTable = 3
}

public class ApplicationUser : IdentityUser<Guid>
{
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string FriendCode { get; set; } = string.Empty;
    public DateTime LastSeenUtc { get; set; } = DateTime.UtcNow;
    public UserPresenceStatus PresenceStatus { get; set; } = UserPresenceStatus.Offline;
    public Guid? ActiveRoomId { get; set; }

    //optional: for future add of avatar/cosmetics fields later
}
=======
using Microsoft.AspNetCore.Identity;
using Chuds2Chads.Data.Entities;

namespace Chuds2Chads.Data;

public class ApplicationUser : IdentityUser<Guid>
{
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public UserAvatarLoadout? AvatarLoadout { get; set; }
    public ICollection<UserCosmeticItem> OwnedCosmeticItems { get; set; } = new List<UserCosmeticItem>();
}
>>>>>>> origin/main
