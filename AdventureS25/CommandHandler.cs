namespace AdventureS25;

public static class CommandHandler
{
    private static Dictionary<string, Action<Command>> commandMap =
        new Dictionary<string, Action<Command>>()
        {
            {"go", Move},
            {"tron", Tron},
            {"troff", Troff},
            {"take", Take},
            {"inventory", ShowInventory},
            {"look", Look},
            {"drop", Drop},
            {"use", Use},
            {"story", Story},
            {"accept", Accept},
            {"deny", Deny},
            {"help", Help},
            {"debug", Debug}
        };

    public static void Handle(Command command)
    {
        if (commandMap.ContainsKey(command.Verb))
        {
            Action<Command> method = commandMap[command.Verb];
            method.Invoke(command);
        }
        else
        {
            TextAnimator.AnimateText("I don't know how to do that.");
        }
    }
    
    private static void Drop(Command command)
    {
        Player.Drop(command);
    }
    
    private static void Look(Command command)
    {
        Player.Look();
    }

    private static void ShowInventory(Command command)
    {
        Player.ShowInventory();
    }
    
    private static void Take(Command command)
    {
        Player.Take(command);
    }

    private static void Troff(Command command)
    {
        Debugger.Troff();
    }

    private static void Tron(Command command)
    {
        Debugger.Tron();
    }

    public static void Move(Command command)
    {
        Player.Move(command);
    }

    private static void Use(Command command)
    {
        Player.Use(command);
    }

    private static void Story(Command command)
    {
        Player.Story(command);
    }
    
    private static void Accept(Command command)
    {
        Player.Accept();
    }
    
    private static void Deny(Command command)
    {
        Player.Deny();
    }
    
    private static void Help(Command command)
    {
        Game.DisplayHelp();
    }
    
    private static void Debug(Command command)
    {
        if (command.Noun == "ricky")
        {
            TextAnimator.AnimateText("Debug: Testing Ricky command");
            Command storyCommand = new Command();
            storyCommand.Verb = "story";
            storyCommand.Noun = "ricky";
            Story(storyCommand);
        }
        else
        {
            TextAnimator.AnimateText("Debug command: " + command.Noun);
        }
    }
}