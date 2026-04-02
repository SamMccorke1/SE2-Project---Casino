using Chuds2Chads.Data;
using Chuds2Chads.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chuds2Chads.Services;

public class AvatarService
{
    private readonly AppDbContext _db;

    public AvatarService(AppDbContext db)
    {
        _db = db;
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

        foreach (var slot in Enum.GetValues<AvatarSlot>())
        {
            var hasItemForSlot = await _db.UserCosmeticItems
                .Include(i => i.CosmeticDefinition)
                .AnyAsync(i => i.UserId == userId && i.CosmeticDefinition!.Slot == slot);

            if (!hasItemForSlot)
            {
                var starterAsset = GetStarterAssetKey(slot);
                await GrantCosmeticAsync(userId, starterAsset);
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

        var inventory = await _db.UserCosmeticItems
            .AsNoTracking()
            .Include(i => i.CosmeticDefinition)
            .Where(i => i.UserId == userId)
            .OrderBy(i => i.CosmeticDefinition!.Slot)
            .ThenBy(i => i.CosmeticDefinition!.Name)
            .ThenBy(i => i.EarnedUtc)
            .Select(i => new OwnedCosmeticDto
            {
                ObjectId = i.ObjectId,
                Slot = i.CosmeticDefinition!.Slot,
                Name = i.CosmeticDefinition.Name,
                AssetKey = i.CosmeticDefinition.AssetKey,
                Rarity = i.CosmeticDefinition.Rarity,
                EarnedUtc = i.EarnedUtc,
                IsEquipped = i.ObjectId == loadout.HeadObjectId
                    || i.ObjectId == loadout.FaceObjectId
                    || i.ObjectId == loadout.TorsoObjectId
                    || i.ObjectId == loadout.LegsObjectId
                    || i.ObjectId == loadout.ShoeObjectId
                    || i.ObjectId == loadout.PetObjectId
            })
            .ToListAsync();

        return new AvatarCustomizationData
        {
            EquippedObjectIds = new Dictionary<AvatarSlot, Guid?>
            {
                [AvatarSlot.Head] = loadout.HeadObjectId,
                [AvatarSlot.Face] = loadout.FaceObjectId,
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

    private static string GetStarterAssetKey(AvatarSlot slot) => slot switch
    {
        AvatarSlot.Head => "head.base.cap",
        AvatarSlot.Face => "face.base.smile",
        AvatarSlot.Torso => "torso.base.jacket",
        AvatarSlot.Legs => "legs.base.denim",
        AvatarSlot.Shoe => "shoe.base.sneaker",
        AvatarSlot.Pet => "pet.base.chipmunk",
        _ => throw new ArgumentOutOfRangeException(nameof(slot), slot, "Unsupported slot.")
    };

    private static IReadOnlyList<CosmeticDefinition> GetCatalogDefaults() =>
    [
        new() { Slot = AvatarSlot.Head, Name = "Lucky Cap", AssetKey = "head.base.cap", Rarity = "Common" },
        new() { Slot = AvatarSlot.Head, Name = "Dealer Top Hat", AssetKey = "head.dealer.top-hat", Rarity = "Rare" },
        new() { Slot = AvatarSlot.Face, Name = "Lucky Smile", AssetKey = "face.base.smile", Rarity = "Common" },
        new() { Slot = AvatarSlot.Face, Name = "Poker Shades", AssetKey = "face.poker.shades", Rarity = "Rare" },
        new() { Slot = AvatarSlot.Torso, Name = "Starter Jacket", AssetKey = "torso.base.jacket", Rarity = "Common" },
        new() { Slot = AvatarSlot.Torso, Name = "Royal Blazer", AssetKey = "torso.royal.blazer", Rarity = "Epic" },
        new() { Slot = AvatarSlot.Legs, Name = "Denim Pants", AssetKey = "legs.base.denim", Rarity = "Common" },
        new() { Slot = AvatarSlot.Legs, Name = "Velvet Trousers", AssetKey = "legs.velvet.trousers", Rarity = "Rare" },
        new() { Slot = AvatarSlot.Shoe, Name = "Casino Sneakers", AssetKey = "shoe.base.sneaker", Rarity = "Common" },
        new() { Slot = AvatarSlot.Shoe, Name = "Golden Loafers", AssetKey = "shoe.gold.loafer", Rarity = "Epic" },
        new() { Slot = AvatarSlot.Pet, Name = "Chipmunk", AssetKey = "pet.base.chipmunk", Rarity = "Common" },
        new() { Slot = AvatarSlot.Pet, Name = "Mini Lion", AssetKey = "pet.mini.lion", Rarity = "Legendary" }
    ];
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
    public string Rarity { get; set; } = string.Empty;
    public DateTime EarnedUtc { get; set; }
    public bool IsEquipped { get; set; }
}