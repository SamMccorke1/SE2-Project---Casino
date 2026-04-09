using Chuds2Chads.Components;
using Chuds2Chads.Data;
using Chuds2Chads.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
//using Chuds2Chads.Services;
using Chuds2Chads.Games;
var builder = WebApplication.CreateBuilder(args);

// UI
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Controllers for API endpoints
builder.Services.AddControllers();

// HttpClient for API calls
builder.Services.AddScoped<HttpClient>();
builder.Services.AddHttpClient("api");

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
builder.Services.AddScoped<AvatarService>();
builder.Services.AddScoped<RouletteService>();
builder.Services.AddScoped<SlotsService>();
builder.Services.AddScoped<HorseRaceService>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<Chuds2Chads.Games.Blackjack.BlackjackLobbyService>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var avatarService = scope.ServiceProvider.GetRequiredService<AvatarService>();

    await dbContext.Database.MigrateAsync();
    await SeedData.InitializeAsync(scope.ServiceProvider);
    await avatarService.EnsureCatalogSeededAsync();
}

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.Use(async (context, next) =>
    {
        await next();

        // Prevent stale Blazor runtime assets from being served from browser cache during development.
        if (context.Request.Path.StartsWithSegments("/_framework"))
        {
            context.Response.Headers.CacheControl = "no-store, no-cache, max-age=0";
            context.Response.Headers.Pragma = "no-cache";
            context.Response.Headers.Expires = "0";
        }
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

// Static assets + Blazor endpoints
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapRazorPages();
app.MapControllers();

app.Run();

