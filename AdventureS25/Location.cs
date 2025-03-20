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
        _name = nameInput;
        Name = nameInput;
        Description = descriptionInput;
        // Capitalize the first letter of the location name and add a period
        string capitalizedName = char.ToUpper(nameInput[0]) + nameInput.Substring(1);
        ShortDescription = "Location: " + capitalizedName + ".";
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
            foreach (string direction in Connections.Keys)
            {
                // Capitalize the first letter
                string formattedDirection = char.ToUpper(direction[0]) + direction.Substring(1);
                formattedDirections.Add(formattedDirection);
            }
            
            // Join with commas and period at the end
            fullDescription += " " + string.Join(", ", formattedDirections) + ".";
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