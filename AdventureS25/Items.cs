namespace AdventureS25;

public static class Items
{
    public static Item Clothes = new Item("clothes", "There are clothes here.");
    public static Item iPhone = new Item("iphone", "There is an iphone here.");
    public static Item OGKush = new Item("og-kush", "There is OG Kush here.");
    public static Item Wallet = new Item("wallet", "There is a wallet here.");

    private static Dictionary<string, Item> nameToItem = 
        new Dictionary<string, Item>();
    
    public static void Initialize()
    {
        nameToItem.Add("clothes", Clothes);
        nameToItem.Add("iphone", iPhone);
        nameToItem.Add("og-kush", OGKush);
        nameToItem.Add("wallet", Wallet);
        
        // tell the map to add the item at a specific location
        Map.AddItem("iphone", "bedroom");
        Map.AddItem("clothes", "bedroom");
        Map.AddItem("wallet", "bedroom");
    }

    public static Item GetItemByName(string itemName)
    {
        if (nameToItem.ContainsKey(itemName))
        {
            return nameToItem[itemName];
        }
        return null;
    }
}