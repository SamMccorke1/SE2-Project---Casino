using Microsoft.AspNetCore.Identity;

namespace Chuds2Chads.Data;

public class ApplicationUser : IdentityUser<Guid>
{
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    //optional: for future add of avatar/cosmetics fields later
}