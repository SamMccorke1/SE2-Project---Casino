using Microsoft.AspNetCore.Identity;
using Chuds2Chads.Data.Entities;

namespace Chuds2Chads.Data;

public class ApplicationUser : IdentityUser<Guid>
{
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public UserAvatarLoadout? AvatarLoadout { get; set; }
    public ICollection<UserCosmeticItem> OwnedCosmeticItems { get; set; } = new List<UserCosmeticItem>();
}