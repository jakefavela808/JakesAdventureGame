namespace AdventureS25;

public static class Game
{
    public static void PlayGame()
    {
        Initialize();

        TextAnimator.AnimateText(Player.GetLocationDescription());
        
        bool isPlaying = true;
        
        while (isPlaying == true)
        {
            Command command = CommandProcessor.Process();
            
            if (command.IsValid)
            {
                if (command.Verb == "exit")
                {
                    TextAnimator.AnimateText("Game Over!");
                    isPlaying = false;
                }
                else
                {
                    CommandHandler.Handle(command);
                }
            }
        }
    }

    private static void Initialize()
    {
        Map.Initialize();
        Items.Initialize();
        Player.Initialize();
        TextAnimator.Initialize();
        
        // Welcome message
        TextAnimator.AnimateText("");
        
        // Display available commands
        DisplayAvailableCommands();
    }
    
    private static void DisplayAvailableCommands()
    {
        TextAnimator.AnimateText("Available commands:");
        
        // Display verbs that need objects
        TextAnimator.AnimateText("- go [direction]: Move in a direction (north, south, east, west)");
        TextAnimator.AnimateText("- take [item]: Pick up an item");
        TextAnimator.AnimateText("- drop [item]: Drop an item from your inventory");
        TextAnimator.AnimateText("- use [item]: Use an item in your inventory");
        TextAnimator.AnimateText("- story [person]: Talk to someone");
        
        // Display standalone verbs
        TextAnimator.AnimateText("- look: Look around your current location");
        TextAnimator.AnimateText("- inventory: Check what you're carrying");
        TextAnimator.AnimateText("- exit: Quit the game");
        
        TextAnimator.AnimateText("");
    }

    public static void DisplayHelp()
    {
        TextAnimator.AnimateText("Available commands:");
        
        // Display verbs that need a noun
        TextAnimator.AnimateText("- go [direction]: Move in a direction (north, south, east, west)");
        TextAnimator.AnimateText("- take [item]: Pick up an item");
        TextAnimator.AnimateText("- drop [item]: Drop an item from your inventory");
        TextAnimator.AnimateText("- use [item]: Use an item in your inventory");
        TextAnimator.AnimateText("- story [person]: Talk to someone");
        
        // Display standalone verbs
        TextAnimator.AnimateText("- look: Look around your current location");
        TextAnimator.AnimateText("- inventory: Check what you're carrying");
        TextAnimator.AnimateText("- accept: Accept an offer when prompted");
        TextAnimator.AnimateText("- deny: Deny an offer when prompted");
        TextAnimator.AnimateText("- help: Display this help message");
        TextAnimator.AnimateText("- exit: Quit the game");
    }
}