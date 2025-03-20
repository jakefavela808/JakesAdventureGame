namespace AdventureS25;

public class Location
{
    private string _name;
    public string Name { 
        get { return _name; } 
        private set { _name = value; } 
    }
    public string Description;
    public string ShortDescription;
    public bool HasBeenVisited = false;
    
    public Dictionary<string, Location> Connections;
    public List<Item> Items = new List<Item>();
    
    public Location(string nameInput, string descriptionInput)
    {
        // Store the original name input for internal use
        _name = nameInput.ToLower().Trim();
        Name = _name;
        Description = descriptionInput;
        // Format the name for display by capitalizing each word
        string displayName = string.Join(" ", _name.Split(' ').Select(word => char.ToUpper(word[0]) + word.Substring(1)));
        ShortDescription = "Location: " + displayName;
        Connections = new Dictionary<string, Location>();
    }

    public void AddConnection(string direction, Location location)
    {
        Connections.Add(direction, location);
    }

    public bool CanMoveInDirection(Command command)
    {
        if (Connections.ContainsKey(command.Noun))
        {
            return true;
        }
        return false;
    }

    public Location GetLocationInDirection(Command command)
    {
        return Connections[command.Noun];
    }

    public string GetDescription()
    {
        string fullDescription;
        
        if (HasBeenVisited)
        {
            fullDescription = ShortDescription;
        }
        else
        {
            fullDescription = Description;
            HasBeenVisited = true;
        }

        foreach (Item item in Items)
        {
            fullDescription += "\n" + item.GetLocationDescription();
        }
        
        // Add available directions with proper formatting
        if (Connections.Count > 0)
        {
            fullDescription += "\nPossible directions:";
            
            List<string> formattedDirections = new List<string>();
            foreach (var connection in Connections)
            {
                // Format both direction and destination names
                string direction = char.ToUpper(connection.Key[0]) + connection.Key.Substring(1);
                string destination = string.Join(" ", connection.Value.Name.Split(' ').Select(word => char.ToUpper(word[0]) + word.Substring(1)));
                formattedDirections.Add($"{direction} ({destination})");
            }
            
            // Join with commas but no period at the end
            fullDescription += " " + string.Join(", ", formattedDirections);
        }
        
        return fullDescription;
    }

    public void AddItem(Item item)
    {
        Debugger.Write("Adding item "+ item.Name + "to " + Name);
        Items.Add(item);
    }

    public bool HasItem(Item itemLookingFor)
    {
        foreach (Item item in Items)
        {
            if (item.Name == itemLookingFor.Name)
            {
                return true;
            }
        }
        
        return false;
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
    }
}