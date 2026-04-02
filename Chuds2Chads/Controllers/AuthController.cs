using Chuds2Chads.Data;
using Chuds2Chads.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chuds2Chads.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AvatarService _avatarService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            AvatarService avatarService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _avatarService = avatarService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and password are required.");

            var existingUser = await _userManager.FindByNameAsync(request.Username);
            if (existingUser != null)
                return BadRequest(new { error = "Username already taken." });

            var email = request.Username + "@chuds2chads.local";
            var existingEmail = await _userManager.FindByEmailAsync(email);
            if (existingEmail != null)
                return BadRequest(new { error = "Email already in use." });

            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new { error = errors });
            }

            await _avatarService.EnsureUserAvatarInitializedAsync(user.Id);

            return Ok(new { message = "Account created successfully!" });
        }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Username and password are required.");
        }

        // Find user
        var user = await _userManager.FindByNameAsync(request.Username) ?? 
                  await _userManager.FindByEmailAsync(request.Username);
        
        if (user == null)
        {
            return Unauthorized(new { error = "Invalid username or password." });
        }

        // Check password using SignInManager
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return Unauthorized(new { error = "Invalid username or password." });
        }

        // Sign in the user (this sets the authentication cookie)
        await _signInManager.SignInAsync(user, isPersistent: false);

        return Ok(new { 
            message = "Login successful!", 
            userId = user.Id, 
            userName = user.UserName,
            email = user.Email
        });
    }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out successfully." });
        }

        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }
            return Ok(new { id = user.Id, userName = user.UserName, email = user.Email, createdDate = user.CreatedDate });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
