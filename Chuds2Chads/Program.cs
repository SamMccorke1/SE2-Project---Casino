using Chuds2Chads.Components;
using Chuds2Chads.Data;
using Chuds2Chads.Games.Blackjack;
using Chuds2Chads.Games.Poker;
using Chuds2Chads.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();

builder.Services.AddScoped<HttpClient>();
builder.Services.AddHttpClient("api");

builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<WalletService>();
builder.Services.AddScoped<AvatarService>();
builder.Services.AddScoped<RouletteService>();
builder.Services.AddScoped<SlotsService>();
builder.Services.AddScoped<HorseRaceService>();
builder.Services.AddSingleton<MultiplayerService>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<BlackjackLobbyService>();
builder.Services.AddSingleton<PokerLobbyService>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var avatarService = scope.ServiceProvider.GetRequiredService<AvatarService>();
    var multiplayerService = scope.ServiceProvider.GetRequiredService<MultiplayerService>();

    await db.Database.MigrateAsync();
    await SeedData.InitializeAsync(scope.ServiceProvider);
    await avatarService.EnsureCatalogSeededAsync();
    await multiplayerService.BackfillMissingFriendCodesAsync();
}

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

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapRazorPages();
app.MapControllers();

app.Run();
