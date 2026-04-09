using Chuds2Chads.Data;
using Chuds2Chads.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Chuds2Chads.Services;

public class AvatarService
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _environment;

    public AvatarService(AppDbContext db, IWebHostEnvironment environment)
    {
        _db = db;
        _environment = environment;
    }

    public async Task EnsureCatalogSeededAsync()
    {
        if (await _db.CosmeticDefinitions.AnyAsync())
        {
            return;
        }

        _db.CosmeticDefinitions.AddRange(GetCatalogDefaults());
        await _db.SaveChangesAsync();
    }

    public async Task EnsureUserAvatarInitializedAsync(Guid userId)
    {
        await EnsureCatalogSeededAsync();

        var loadout = await _db.UserAvatarLoadouts.FirstOrDefaultAsync(a => a.UserId == userId);
        if (loadout is null)
        {
            loadout = new UserAvatarLoadout { UserId = userId };
            _db.UserAvatarLoadouts.Add(loadout);
        }

        var ownedDefinitionIds = await _db.UserCosmeticItems
            .AsNoTracking()
            .Where(i => i.UserId == userId)
            .Select(i => i.CosmeticDefinitionId)
            .ToHashSetAsync();

        var missingDefinitions = await _db.CosmeticDefinitions
            .AsNoTracking()
            .Where(c => !ownedDefinitionIds.Contains(c.Id))
            .ToListAsync();

        if (missingDefinitions.Count > 0)
        {
            var now = DateTime.UtcNow;
            foreach (var definition in missingDefinitions)
            {
                _db.UserCosmeticItems.Add(new UserCosmeticItem
                {
                    UserId = userId,
                    CosmeticDefinitionId = definition.Id,
                    EarnedUtc = now
                });
            }
        }

        var slotItems = await _db.UserCosmeticItems
            .AsNoTracking()
            .Include(i => i.CosmeticDefinition)
            .Where(i => i.UserId == userId)
            .OrderBy(i => i.EarnedUtc)
            .ToListAsync();

        loadout.HeadObjectId ??= slotItems.FirstOrDefault(i => i.CosmeticDefinition?.Slot == AvatarSlot.Head)?.ObjectId;
        loadout.FaceObjectId ??= slotItems.FirstOrDefault(i => i.CosmeticDefinition?.Slot == AvatarSlot.Face)?.ObjectId;
        loadout.BodyObjectId ??= slotItems.FirstOrDefault(i => i.CosmeticDefinition?.Slot == AvatarSlot.Body)?.ObjectId;
        loadout.TorsoObjectId ??= slotItems.FirstOrDefault(i => i.CosmeticDefinition?.Slot == AvatarSlot.Torso)?.ObjectId;
        loadout.LegsObjectId ??= slotItems.FirstOrDefault(i => i.CosmeticDefinition?.Slot == AvatarSlot.Legs)?.ObjectId;
        loadout.ShoeObjectId ??= slotItems.FirstOrDefault(i => i.CosmeticDefinition?.Slot == AvatarSlot.Shoe)?.ObjectId;
        loadout.PetObjectId ??= slotItems.FirstOrDefault(i => i.CosmeticDefinition?.Slot == AvatarSlot.Pet)?.ObjectId;

        await _db.SaveChangesAsync();
    }

    public async Task<AvatarCustomizationData?> GetCustomizationDataAsync(Guid userId)
    {
        await EnsureUserAvatarInitializedAsync(userId);

        var loadout = await _db.UserAvatarLoadouts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.UserId == userId);

        if (loadout is null)
        {
            return null;
        }

        var inventoryItems = await _db.UserCosmeticItems
            .AsNoTracking()
            .Include(i => i.CosmeticDefinition)
            .Where(i => i.UserId == userId)
            .OrderBy(i => i.CosmeticDefinition!.Slot)
            .ThenBy(i => i.CosmeticDefinition!.Name)
            .ThenBy(i => i.EarnedUtc)
            .ToListAsync();

        var inventory = inventoryItems.Select(i => new OwnedCosmeticDto
        {
            ObjectId = i.ObjectId,
            Slot = i.CosmeticDefinition!.Slot,
            Name = i.CosmeticDefinition.Name,
            AssetKey = i.CosmeticDefinition.AssetKey,
            ImagePath = ResolveCosmeticImagePath(i.CosmeticDefinition.AssetKey),
            Rarity = i.CosmeticDefinition.Rarity,
            EarnedUtc = i.EarnedUtc,
            IsEquipped = i.ObjectId == loadout.HeadObjectId
                || i.ObjectId == loadout.FaceObjectId
                || i.ObjectId == loadout.BodyObjectId
                || i.ObjectId == loadout.TorsoObjectId
                || i.ObjectId == loadout.LegsObjectId
                || i.ObjectId == loadout.ShoeObjectId
                || i.ObjectId == loadout.PetObjectId
        }).ToList();

        return new AvatarCustomizationData
        {
            EquippedObjectIds = new Dictionary<AvatarSlot, Guid?>
            {
                [AvatarSlot.Head] = loadout.HeadObjectId,
                [AvatarSlot.Face] = loadout.FaceObjectId,
                [AvatarSlot.Body] = loadout.BodyObjectId,
                [AvatarSlot.Torso] = loadout.TorsoObjectId,
                [AvatarSlot.Legs] = loadout.LegsObjectId,
                [AvatarSlot.Shoe] = loadout.ShoeObjectId,
                [AvatarSlot.Pet] = loadout.PetObjectId
            },
            OwnedCosmetics = inventory
        };
    }

    public async Task<bool> EquipAsync(Guid userId, Guid objectId)
    {
        var item = await _db.UserCosmeticItems
            .Include(i => i.CosmeticDefinition)
            .FirstOrDefaultAsync(i => i.UserId == userId && i.ObjectId == objectId);

        if (item is null || item.CosmeticDefinition is null)
        {
            return false;
        }

        var loadout = await _db.UserAvatarLoadouts.FirstOrDefaultAsync(a => a.UserId == userId);
        if (loadout is null)
        {
            return false;
        }

        switch (item.CosmeticDefinition.Slot)
        {
            case AvatarSlot.Head:
                loadout.HeadObjectId = item.ObjectId;
                break;
            case AvatarSlot.Face:
                loadout.FaceObjectId = item.ObjectId;
                break;
            case AvatarSlot.Body:
                loadout.BodyObjectId = item.ObjectId;
                break;
            case AvatarSlot.Torso:
                loadout.TorsoObjectId = item.ObjectId;
                break;
            case AvatarSlot.Legs:
                loadout.LegsObjectId = item.ObjectId;
                break;
            case AvatarSlot.Shoe:
                loadout.ShoeObjectId = item.ObjectId;
                break;
            case AvatarSlot.Pet:
                loadout.PetObjectId = item.ObjectId;
                break;
            default:
                return false;
        }

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> GrantCosmeticAsync(Guid userId, string assetKey)
    {
        var definition = await _db.CosmeticDefinitions
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.AssetKey == assetKey);

        if (definition is null)
        {
            return false;
        }

        _db.UserCosmeticItems.Add(new UserCosmeticItem
        {
            UserId = userId,
            CosmeticDefinitionId = definition.Id,
            EarnedUtc = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        return true;
    }

    private static IReadOnlyList<CosmeticDefinition> GetCatalogDefaults() =>
    [
        new() { Slot = AvatarSlot.Head, Name = "Lucky Cap", AssetKey = "head.base.cap", Rarity = "Common" },
        new() { Slot = AvatarSlot.Head, Name = "Dealer Top Hat", AssetKey = "head.dealer.top-hat", Rarity = "Rare" },
        new() { Slot = AvatarSlot.Face, Name = "Lucky Smile", AssetKey = "face.base.smile", Rarity = "Common" },
        new() { Slot = AvatarSlot.Face, Name = "Poker Shades", AssetKey = "face.poker.shades", Rarity = "Rare" },
        new() { Slot = AvatarSlot.Body, Name = "Skin Tone 2", AssetKey = "body.skin.tone-2", Rarity = "Common" },
        new() { Slot = AvatarSlot.Body, Name = "Skin Tone 3", AssetKey = "body.skin.tone-3", Rarity = "Common" },
        new() { Slot = AvatarSlot.Body, Name = "Skin Tone 4", AssetKey = "body.skin.tone-4", Rarity = "Common" },
        new() { Slot = AvatarSlot.Body, Name = "Skin Tone 5", AssetKey = "body.skin.tone-5", Rarity = "Common" },
        new() { Slot = AvatarSlot.Body, Name = "Skin Tone 6", AssetKey = "body.skin.tone-6", Rarity = "Common" },
        new() { Slot = AvatarSlot.Torso, Name = "Starter Jacket", AssetKey = "torso.base.jacket", Rarity = "Common" },
        new() { Slot = AvatarSlot.Torso, Name = "Royal Blazer", AssetKey = "torso.royal.blazer", Rarity = "Epic" },
        new() { Slot = AvatarSlot.Legs, Name = "Denim Pants", AssetKey = "legs.base.denim", Rarity = "Common" },
        new() { Slot = AvatarSlot.Legs, Name = "Velvet Trousers", AssetKey = "legs.velvet.trousers", Rarity = "Rare" },
        new() { Slot = AvatarSlot.Shoe, Name = "Casino Sneakers", AssetKey = "shoe.base.sneaker", Rarity = "Common" },
        new() { Slot = AvatarSlot.Shoe, Name = "Golden Loafers", AssetKey = "shoe.gold.loafer", Rarity = "Epic" },
        new() { Slot = AvatarSlot.Pet, Name = "Chipmunk", AssetKey = "pet.base.chipmunk", Rarity = "Common" },
        new() { Slot = AvatarSlot.Pet, Name = "Mini Lion", AssetKey = "pet.mini.lion", Rarity = "Legendary" }
    ];

    private string ResolveCosmeticImagePath(string assetKey)
    {
        // Prefer explicit mappings for the current uploaded PNG set.
        var normalized = assetKey.Trim().ToLowerInvariant();

        var explicitMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["head.base.cap"] = "avatarhairs/hair1.png",
            ["head.dealer.top-hat"] = "avatarhairs/hair2.png",
            ["face.base.smile"] = "avatarbodies/body2.png",
            ["face.poker.shades"] = "avatarbodies/body3.png",
            ["body.skin.tone-2"] = "avatarbodies/body2.png",
            ["body.skin.tone-3"] = "avatarbodies/body3.png",
            ["body.skin.tone-4"] = "avatarbodies/body4.png",
            ["body.skin.tone-5"] = "avatarbodies/body5.png",
            ["body.skin.tone-6"] = "avatarbodies/body6.png",
            ["torso.base.jacket"] = "avatartops/shirt1.png",
            ["torso.royal.blazer"] = "avatartops/shirt3.png",
            ["legs.base.denim"] = "avatarbottoms/shorts1.png",
            ["legs.velvet.trousers"] = "avatarbottoms/skirt2.png",
            ["shoe.base.sneaker"] = "avatarshoes/shoes1.png",
            ["shoe.gold.loafer"] = "avatarshoes/shoes2.png",
            ["pet.base.chipmunk"] = "avatarbodies/body5.png",
            ["pet.mini.lion"] = "avatarbodies/body6.png"
        };

        if (explicitMap.TryGetValue(normalized, out var mappedPath))
        {
            var mappedAbsolute = Path.Combine(
                _environment.WebRootPath,
                mappedPath.Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(mappedAbsolute))
            {
                return "/" + mappedPath;
            }
        }

        // Fallback: support both dot and dash filename styles and common image extensions.
        var dashed = normalized.Replace('.', '-');

        var candidateRelativePaths = new[]
        {
            $"images/cosmetics/basic/{normalized}.png",
            $"images/cosmetics/basic/{dashed}.png",
            $"images/cosmetics/{normalized}.png",
            $"images/cosmetics/{dashed}.png",
            $"images/cosmetics/basic/{normalized}.webp",
            $"images/cosmetics/basic/{dashed}.webp",
            $"images/cosmetics/{normalized}.webp",
            $"images/cosmetics/{dashed}.webp",
            $"images/cosmetics/basic/{normalized}.jpg",
            $"images/cosmetics/basic/{dashed}.jpg",
            $"images/cosmetics/{normalized}.jpg",
            $"images/cosmetics/{dashed}.jpg"
        };

        foreach (var relativePath in candidateRelativePaths)
        {
            var absolutePath = Path.Combine(
                _environment.WebRootPath,
                relativePath.Replace('/', Path.DirectorySeparatorChar));

            if (File.Exists(absolutePath))
            {
                return "/" + relativePath.Replace('\\', '/');
            }
        }

        return "/images/C2C-Logo-Transparent.png";
    }
}

public class AvatarCustomizationData
{
    public Dictionary<AvatarSlot, Guid?> EquippedObjectIds { get; set; } = new();
    public List<OwnedCosmeticDto> OwnedCosmetics { get; set; } = new();
}

public class OwnedCosmeticDto
{
    public Guid ObjectId { get; set; }
    public AvatarSlot Slot { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AssetKey { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public string Rarity { get; set; } = string.Empty;
    public DateTime EarnedUtc { get; set; }
    public bool IsEquipped { get; set; }
}