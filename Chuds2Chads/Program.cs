using Chuds2Chads.Components;
using Chuds2Chads.Data;
using Chuds2Chads.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// UI
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Needed for Identity endpoints (Razor Pages)
builder.Services.AddRazorPages();

// SQLite + EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity + Roles (email + password)
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    // Email-based accounts
    options.User.RequireUniqueEmail = true;

    // Password rules
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Game services
builder.Services.AddScoped<WalletService>();
builder.Services.AddScoped<RouletteService>();
builder.Services.AddScoped<SlotsService>();
builder.Services.AddScoped<HorseRaceService>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// Static assets + Blazor endpoints
app.MapStaticAssets();
app.MapRazorPages();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

