namespace AdventureS25;

public static class Player
{
    public static Location CurrentLocation;
    public static List<Item> Inventory;
    public static bool HasUsedClothes = false;
    public static bool HasUsedPhone = false;

    public static void Initialize()
    {
        Inventory = new List<Item>();
        CurrentLocation = Map.StartLocation;
    }

    public static void Move(Command command)
    {
        // Check if trying to go outside without using clothes
        if (command.Noun == "north" && CurrentLocation.Description.Contains("bed") && !HasUsedClothes)
        {
            TextAnimator.AnimateText("You can't go outside without putting on your clothes first!");
            return;
        }
        
        // Check if trying to go outside without answering the phone call
        if (command.Noun == "north" && CurrentLocation.Description.Contains("bed") && !HasUsedPhone)
        {
            TextAnimator.AnimateText("You're receiving a call. Take the iphone and use it first!");
            return;
        }
        
        if (CurrentLocation.CanMoveInDirection(command))
        {
            CurrentLocation = CurrentLocation.GetLocationInDirection(command);
            TextAnimator.AnimateText(CurrentLocation.GetDescription());
        }
        else
        {
            TextAnimator.AnimateText("You can't move " + command.Noun + ".");
        }
    }

    public static string GetLocationDescription()
    {
        return CurrentLocation.GetDescription();
    }

    public static void Take(Command command)
    {
        // figure out which item to take: turn the noun into an item
        Item item = Items.GetItemByName(command.Noun);

        if (item == null)
        {
            TextAnimator.AnimateText("I don't know what " + command.Noun + " is.");
        }
        else if (!CurrentLocation.HasItem(item))
        {
            TextAnimator.AnimateText("There is no " + command.Noun + " here.");
        }
        else if (!item.IsTakeable)
        {
            TextAnimator.AnimateText("The " + command.Noun + " can't be taked.");
        }
        else
        {
            Inventory.Add(item);
            CurrentLocation.RemoveItem(item);
            TextAnimator.AnimateText("You take the " + command.Noun + ".");
        }
    }

    public static void ShowInventory()
    {
        if (Inventory.Count == 0)
        {
            TextAnimator.AnimateText("You are empty-handed.");
        }
        else
        {
            TextAnimator.AnimateText("You are carrying:");
            foreach (Item item in Inventory)
            {
                string article = SemanticTools.CreateArticle(item.Name);
                TextAnimator.AnimateText(" " + article + " " + item.Name);
            }
        }
    }

    public static void Look()
    {
        TextAnimator.AnimateText(CurrentLocation.GetDescription());
    }

    public static void Drop(Command command)
    {       
        Item item = Items.GetItemByName(command.Noun);

        if (item == null)
        {
            string article = SemanticTools.CreateArticle(command.Noun);
            TextAnimator.AnimateText("I don't know what " + article + " " + command.Noun + " is.");
        }
        else if (!Inventory.Contains(item))
        {
            TextAnimator.AnimateText("You're not carrying the " + command.Noun + ".");
        }
        else
        {
            Inventory.Remove(item);
            CurrentLocation.AddItem(item);
            TextAnimator.AnimateText("You drop the " + command.Noun + ".");
        }

    }

    public static void Use(Command command)
    {
        // Check if the item exists
        Item item = Items.GetItemByName(command.Noun);

        if (item == null)
        {
            TextAnimator.AnimateText("I don't know what " + command.Noun + " is.");
            return;
        }

        // Check if the player has the item in their inventory
        if (!Inventory.Contains(item))
        {
            TextAnimator.AnimateText("You don't have the " + command.Noun + " to use.");
            return;
        }

        // Handle specific items
        if (command.Noun == "iphone")
        {
            TextAnimator.AnimateText("Incoming call... Jon");
            TextAnimator.AnimateText("You answer the call.");
            TextAnimator.AnimateText("Jon: Hey, how's it going?");
            TextAnimator.AnimateText("Jon: I heard you been sad lately. You can't get any job with your Computer Science degree. PUT ON YOUR CLOTHES AND MEET ME OUTSIDE NOW!");
            TextAnimator.AnimateText("You hang up the phone.");
            HasUsedPhone = true;
        }
        else if (command.Noun == "clothes")
        {
            TextAnimator.AnimateText("You put on your clothes. Now you're ready to go outside.");
            HasUsedClothes = true;
        }
        else if (command.Noun == "marijuana")
        {
            TextAnimator.AnimateText("You use the marijuana and start to feel more relaxed.");
            TextAnimator.AnimateText("Your worries about finding a job with your Computer Science degree begin to fade away.");
            TextAnimator.AnimateText("Maybe things aren't so bad after all...");
        }
        else
        {
            TextAnimator.AnimateText("You use the " + command.Noun + " but don't find anything interesting.");
        }
    }

    public static void Speak(Command command)
    {
        if (command.Noun == "jon")
        {
            // Check if Jon is in the alleyway
            if (CurrentLocation.Description.Contains("alley"))
            {
                TextAnimator.AnimateText("Jon: Hey there! I'm glad you made it. I have a job opportunity for you. ");
                TextAnimator.AnimateText("Jon: Here's the marijuana. Take it and it might make you feel better about being unemployed.");
                
            }
            else
            {
                TextAnimator.AnimateText("Jon isn't here. You need to find him first.");
            }
        }
        else
        {
            TextAnimator.AnimateText("You can't speak to " + command.Noun + ".");
        }
    }
}