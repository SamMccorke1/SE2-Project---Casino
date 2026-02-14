using Microsoft.AspNetCore.Identity;

namespace Chuds2Chads.Data;

public class ApplicationUser : IdentityUser<Guid>
{
    //optional: for future add of avatar/cosmetics fields later
}