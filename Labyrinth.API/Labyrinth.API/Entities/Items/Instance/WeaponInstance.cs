using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Labyrinth.API.Entities.Items.Instance
{
    public class WeaponInstance : ItemInstance
    {
        [BsonElement("currentAmmo")]
        public int? CurrentAmmo { get; set; }  // Nullable if the weapon doesn't use ammo

        public WeaponInstance(ObjectId prototypeId, string ownerId)
            : base(prototypeId, ownerId)
        {
        }

        // Method to use ammo if applicable
        public void UseAmmo(int amount)
        {
            if (CurrentAmmo.HasValue && CurrentAmmo >= amount)
            {
                CurrentAmmo -= amount;
                if (CurrentAmmo < 0)
                    CurrentAmmo = 0;  // Ensure ammo doesn't go negative
            }
        }
    }
}
