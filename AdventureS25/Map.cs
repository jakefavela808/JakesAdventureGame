namespace AdventureS25;

public static class Map
{
    private static Dictionary<string, Location> nameToLocation = 
        new Dictionary<string, Location>();
    public static Location StartLocation;
    
    public static void Initialize()
    {
        Location bedroom = new Location("bedroom", 
            "You wake up in your bed... You see your rejected Computer Science job offerings. ");
        nameToLocation.Add("bedroom", bedroom);
        
        Location outside = new Location("outside", 
            "You are outside. You see an dark and ominous alley-way just up north.");
        nameToLocation.Add("outside", outside);
        
        Location alleyway = new Location("alleyway",
            "You are in a dark, narrow alley. The walls are covered in graffiti and there's a strange smell in the air. You see Jon waiting for you at the end of the alley.");
        nameToLocation.Add("alleyway", alleyway);
        
        bedroom.AddConnection("north", outside);
        outside.AddConnection("south", bedroom);
        outside.AddConnection("north", alleyway);
        alleyway.AddConnection("south", outside);

        StartLocation = bedroom;
    }
    

    public static void AddItem(string itemName, string locationName)
    {
        // find out which Location is named locationName
        Location location = GetLocationByName(locationName);
        Item item = Items.GetItemByName(itemName);
        
        // add the item to the location
        if (item != null && location != null)
        {
            location.AddItem(item);
        }
    }

    private static Location GetLocationByName(string locationName)
    {
        if (nameToLocation.ContainsKey(locationName))
        {
            return nameToLocation[locationName];
        }
        else
        {
            return null;
        }
    }
}