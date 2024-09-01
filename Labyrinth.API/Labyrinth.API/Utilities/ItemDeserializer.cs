using Labyrinth.API.Entities.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Labyrinth.API.Utilities;
public static class ItemDeserializer
{
    public static List<Item> DeserializeItems(string jsonString)
    {
        var items = new List<Item>();
        var jsonArray = JArray.Parse(jsonString);

        foreach (var jsonObject in jsonArray)
        {
            var itemType = Enum.Parse<ItemType>(jsonObject["type"]!.ToString(), true);
            Item item;

            switch (itemType)
            {
                case ItemType.Weapon:
                    item = JsonConvert.DeserializeObject<Weapon>(jsonObject.ToString());
                    break;
                case ItemType.Armor:
                    item = JsonConvert.DeserializeObject<Armor>(jsonObject.ToString());
                    break;
                case ItemType.Consumable:
                    item = JsonConvert.DeserializeObject<Consumable>(jsonObject.ToString());
                    break;
                case ItemType.MagicItem:
                    item = JsonConvert.DeserializeObject<MagicItem>(jsonObject.ToString());
                    break;
                case ItemType.Tool:
                    item = JsonConvert.DeserializeObject<Tool>(jsonObject.ToString());
                    break;
                case ItemType.Treasure:
                    item = JsonConvert.DeserializeObject<Treasure>(jsonObject.ToString());
                    break;
                case ItemType.Container:
                    item = JsonConvert.DeserializeObject<Container>(jsonObject.ToString());
                    break;
                case ItemType.QuestItem:
                    item = JsonConvert.DeserializeObject<QuestItem>(jsonObject.ToString());
                    break;
                default:
                    throw new InvalidOperationException($"Unknown item type: {itemType}");
            }

            items.Add(item);
        }

        return items;
    }
}
