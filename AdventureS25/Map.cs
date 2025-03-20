namespace AdventureS25;

public static class Map
{
    private static Dictionary<string, Location> nameToLocation = 
        new Dictionary<string, Location>();
    public static Location StartLocation;
    
    public static void Initialize()
    {
        Location bedroom = new Location("bedroom", 
            "You wake up in your bed... You see your rejected Game Dev job offerings. ");
        nameToLocation.Add("bedroom", bedroom);
        
        Location outside = new Location("outside", 
            "You are outside. You see a dark and ominous alley-way just up north, a park to the east, and a neighborhood to the south.");
        nameToLocation.Add("outside", outside);
        
        Location alleyway = new Location("alleyway",
            "You are in a dark, narrow alley. The walls are covered in graffiti and there's a strange smell in the air. You see Jon waiting for you at the end of the alley.\nTip: You can talk to him by typing 'story jon'.");
        nameToLocation.Add("alleyway", alleyway);
        
        Location park = new Location("park",
            "You are at the park. It's quiet and peaceful here. You feel your phone vibrate in your pocket.");
        nameToLocation.Add("park", park);
        
        Location parkBench = new Location("park bench",
            "You approach the bench where you see Creepy Uncle Lester sitting. He looks at you with a grin.\nTip: You can talk to him by typing 'story lester'.");
        nameToLocation.Add("park bench", parkBench);
        
        Location neighborhood = new Location("neighborhood",
            "You walk through a quiet suburban neighborhood. The houses are neatly arranged with well-maintained lawns. You can see a house to the south that looks like it might belong to programmers.");
        nameToLocation.Add("neighborhood", neighborhood);
        
        Location firstHouse = new Location("first house",
            "You arrive at a modest house with a 'Programmers Definitely Live Here' sign on the door. The curtains are drawn, and you can smell a faint odor coming from inside.\nTip: You can talk to Joe by typing 'story joe'.");
        nameToLocation.Add("first house", firstHouse);
        
        bedroom.AddConnection("north", outside);
        outside.AddConnection("south", bedroom);
        outside.AddConnection("north", alleyway);
        outside.AddConnection("west", neighborhood);
        alleyway.AddConnection("south", outside);
        outside.AddConnection("east", park);
        park.AddConnection("west", outside);
        park.AddConnection("north", parkBench);
        parkBench.AddConnection("south", park);
        neighborhood.AddConnection("east", outside);
        neighborhood.AddConnection("south", firstHouse);
        firstHouse.AddConnection("north", neighborhood);

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

    public static bool CanGoToParkBench()
    {
        // Check if the player has used the phone at the park
        return Player.HasUsedPhoneAtPark && Player.CurrentLocation.Name == "park";
    }
}