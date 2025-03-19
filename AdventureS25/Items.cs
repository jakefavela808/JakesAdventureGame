namespace AdventureS25;

public static class Items
{
    private static Dictionary<string, Item> nameToItem = 
        new Dictionary<string, Item>();
    
    public static void Initialize()
    {
        Item iPhone = new Item("iphone",
            "a shiny new iPhone");
        nameToItem.Add("iphone", iPhone);
        
        Item clothes = new Item("clothes",
            "your favorite outfit");
        nameToItem.Add("clothes", clothes);
        
        Item marijuana = new Item("marijuana",
            "some high-quality marijuana");
        nameToItem.Add("marijuana", marijuana);
        
        // tell the map to add the item at a specific location
        Map.AddItem("iphone", "bedroom");
        Map.AddItem("clothes", "bedroom");
        Map.AddItem("marijuana", "alleyway");
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