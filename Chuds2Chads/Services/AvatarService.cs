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
        var defaults = GetCatalogDefaults().ToList();
        var defaultKeys = defaults.Select(d => d.AssetKey).ToHashSet(StringComparer.OrdinalIgnoreCase);

        var existing = await _db.CosmeticDefinitions
            .ToDictionaryAsync(c => c.AssetKey);

        foreach (var def in defaults)
        {
            if (existing.TryGetValue(def.AssetKey, out var row))
            {
                // Update mutable fields so path/name/rarity/IsDefault changes take effect.
                row.Name      = def.Name;
                row.Rarity    = def.Rarity;
                row.IsDefault = def.IsDefault;
                row.Slot      = def.Slot;
            }
            else
            {
                _db.CosmeticDefinitions.Add(def);
            }
        }

        // Remove deprecated definitions and any user items/loadout references that point to them.
        var deprecatedDefinitions = await _db.CosmeticDefinitions
            .Where(c => !defaultKeys.Contains(c.AssetKey))
            .ToListAsync();

        if (deprecatedDefinitions.Count > 0)
        {
            var deprecatedDefinitionIds = deprecatedDefinitions.Select(d => d.Id).ToHashSet();

            var deprecatedItems = await _db.UserCosmeticItems
                .Where(i => deprecatedDefinitionIds.Contains(i.CosmeticDefinitionId))
                .ToListAsync();

            var deprecatedObjectIds = deprecatedItems.Select(i => i.ObjectId).ToHashSet();

            if (deprecatedObjectIds.Count > 0)
            {
                var allLoadouts = await _db.UserAvatarLoadouts.ToListAsync();
                foreach (var loadout in allLoadouts)
                {
                    if (loadout.HeadObjectId is Guid head && deprecatedObjectIds.Contains(head)) loadout.HeadObjectId = null;
                    if (loadout.FaceObjectId is Guid face && deprecatedObjectIds.Contains(face)) loadout.FaceObjectId = null;
                    if (loadout.BodyObjectId is Guid body && deprecatedObjectIds.Contains(body)) loadout.BodyObjectId = null;
                    if (loadout.TorsoObjectId is Guid torso && deprecatedObjectIds.Contains(torso)) loadout.TorsoObjectId = null;
                    if (loadout.LegsObjectId is Guid legs && deprecatedObjectIds.Contains(legs)) loadout.LegsObjectId = null;
                    if (loadout.ShoeObjectId is Guid shoe && deprecatedObjectIds.Contains(shoe)) loadout.ShoeObjectId = null;
                    if (loadout.PetObjectId is Guid pet && deprecatedObjectIds.Contains(pet)) loadout.PetObjectId = null;
                }
            }

            if (deprecatedItems.Count > 0)
            {
                _db.UserCosmeticItems.RemoveRange(deprecatedItems);
            }

            _db.CosmeticDefinitions.RemoveRange(deprecatedDefinitions);
        }

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
            .Where(c => c.IsDefault && !ownedDefinitionIds.Contains(c.Id))
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
        loadout.PetObjectId = null;

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
                [AvatarSlot.Shoe] = loadout.ShoeObjectId
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

    public async Task<bool> UnequipAsync(Guid userId, AvatarSlot slot)
    {
        var loadout = await _db.UserAvatarLoadouts.FirstOrDefaultAsync(a => a.UserId == userId);
        if (loadout is null) return false;

        switch (slot)
        {
            case AvatarSlot.Head:  loadout.HeadObjectId  = null; break;
            case AvatarSlot.Face:  loadout.FaceObjectId  = null; break;
            case AvatarSlot.Body:  loadout.BodyObjectId  = null; break;
            case AvatarSlot.Torso: loadout.TorsoObjectId = null; break;
            case AvatarSlot.Legs:  loadout.LegsObjectId  = null; break;
            case AvatarSlot.Shoe:  loadout.ShoeObjectId  = null; break;
            case AvatarSlot.Pet:   loadout.PetObjectId   = null; break;
            default: return false;
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
        new() { Slot = AvatarSlot.Head,  Name = "Lucky Cap",        AssetKey = "head.base.cap",       Rarity = "Common",    IsDefault = true  },
        new() { Slot = AvatarSlot.Head,  Name = "Dealer Top Hat",    AssetKey = "head.dealer.top-hat", Rarity = "Rare",      IsDefault = true  },
        new() { Slot = AvatarSlot.Face,  Name = "Face 1",            AssetKey = "face.base.smile",     Rarity = "Common",    IsDefault = true  },
        new() { Slot = AvatarSlot.Face,  Name = "Face 2",            AssetKey = "face.f2",             Rarity = "Uncommon",  IsDefault = false },
        new() { Slot = AvatarSlot.Face,  Name = "Face 3",            AssetKey = "face.f3",             Rarity = "Uncommon",  IsDefault = false },
        new() { Slot = AvatarSlot.Face,  Name = "Face 4",            AssetKey = "face.f4",             Rarity = "Rare",      IsDefault = false },
        new() { Slot = AvatarSlot.Face,  Name = "Face 5",            AssetKey = "face.f5",             Rarity = "Rare",      IsDefault = false },
        new() { Slot = AvatarSlot.Body,  Name = "Skin Tone 2",       AssetKey = "body.skin.tone-2",    Rarity = "Common",    IsDefault = true  },
        new() { Slot = AvatarSlot.Body,  Name = "Skin Tone 3",       AssetKey = "body.skin.tone-3",    Rarity = "Common",    IsDefault = true  },
        new() { Slot = AvatarSlot.Body,  Name = "Skin Tone 4",       AssetKey = "body.skin.tone-4",    Rarity = "Common",    IsDefault = true  },
        new() { Slot = AvatarSlot.Body,  Name = "Skin Tone 5",       AssetKey = "body.skin.tone-5",    Rarity = "Common",    IsDefault = true  },
        new() { Slot = AvatarSlot.Body,  Name = "Skin Tone 6",       AssetKey = "body.skin.tone-6",    Rarity = "Common",    IsDefault = true  },
        new() { Slot = AvatarSlot.Torso, Name = "Starter Jacket",    AssetKey = "torso.base.jacket",   Rarity = "Common",    IsDefault = true  },
        new() { Slot = AvatarSlot.Torso, Name = "Royal Blazer",      AssetKey = "torso.royal.blazer",  Rarity = "Epic",      IsDefault = true  },
        new() { Slot = AvatarSlot.Legs,  Name = "Denim Pants",       AssetKey = "legs.base.denim",     Rarity = "Common",    IsDefault = true  },
        new() { Slot = AvatarSlot.Legs,  Name = "Velvet Trousers",   AssetKey = "legs.velvet.trousers",Rarity = "Rare",      IsDefault = true  },
        new() { Slot = AvatarSlot.Shoe,  Name = "Casino Sneakers",   AssetKey = "shoe.base.sneaker",   Rarity = "Common",    IsDefault = true  },
        new() { Slot = AvatarSlot.Shoe,  Name = "Golden Loafers",    AssetKey = "shoe.gold.loafer",    Rarity = "Epic",      IsDefault = true  },
        // Pet slot — entries added when real pet assets are available.
    ];

    private string ResolveCosmeticImagePath(string assetKey)
    {
        // Prefer explicit mappings for the current uploaded PNG set.
        var normalized = assetKey.Trim().ToLowerInvariant();

        var explicitMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["head.base.cap"]       = "Avatar/avatarhairs/hair1.png",
            ["head.dealer.top-hat"] = "Avatar/avatarhairs/hair2.png",
            ["face.base.smile"]     = "Avatar/avatarfaces/f1.png",
            ["face.f2"]             = "Avatar/avatarfaces/f2.png",
            ["face.f3"]             = "Avatar/avatarfaces/f3.png",
            ["face.f4"]             = "Avatar/avatarfaces/f4.png",
            ["face.f5"]             = "Avatar/avatarfaces/f5.png",
            ["body.skin.tone-2"]    = "Avatar/avatarbodies/body2.png",
            ["body.skin.tone-3"]    = "Avatar/avatarbodies/body3.png",
            ["body.skin.tone-4"]    = "Avatar/avatarbodies/body4.png",
            ["body.skin.tone-5"]    = "Avatar/avatarbodies/body5.png",
            ["body.skin.tone-6"]    = "Avatar/avatarbodies/body6.png",
            ["torso.base.jacket"]   = "Avatar/avatartops/shirt1.png",
            ["torso.royal.blazer"]  = "Avatar/avatartops/shirt3.png",
            ["legs.base.denim"]     = "Avatar/avatarbottoms/shorts1.png",
            ["legs.velvet.trousers"]= "Avatar/avatarbottoms/skirt2.png",
            ["shoe.base.sneaker"]   = "Avatar/avatarshoes/shoes1.png",
            ["shoe.gold.loafer"]    = "Avatar/avatarshoes/shoes2.png"
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