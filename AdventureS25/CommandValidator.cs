namespace AdventureS25;

public static class CommandValidator
{
    private static float MaxSpeed;
    
    public static List<string> Verbs = new List<string>
        {"go", "eat", "take", "drop", "use", "story"};
    
    public static List<string> StandaloneVerbs = new List<string>
        {"exit", "inventory", "look", "tron", "troff", "accept", "deny", "help"};
    
    public static List<string> Nouns = new List<string>
    {
        "north", "south", "east", "west", "iphone", "clothes", "jon", "lester", "og-kush", "joe", "wallet"
    };
    
    public static bool IsValid(Command command)
    {
        bool isValid = false;
        
        if (IsVerb(command.Verb))
        {
            Debugger.Write("Valid verb: " + command.Verb);
            
            if (IsStandaloneVerb(command.Verb))
            {
                Debugger.Write("Valid standalone verb: " + command.Verb);

                if (HasNoNoun(command))
                {
                    isValid = true;
                }
                else
                {
                    TextAnimator.AnimateText("I don't know how to do that.");
                }
            }
            else if (IsNoun(command.Noun))
            {
                Debugger.Write("Valid Noun: " + command.Noun);
                isValid = true;
            }
            else
            {
                TextAnimator.AnimateText("I don't know how to do that.");
            }
        }
        else
        {
            TextAnimator.AnimateText("I don't know the word " + command.Verb + ".");
        }
            
        return isValid;
    }

    private static bool HasNoNoun(Command command)
    {
        if (command.Noun == String.Empty)
        {
            return true;
        }
        return false;
    }

    private static bool IsNoun(string commandNoun)
    {
        if (Nouns.Contains(commandNoun))
        {
            return true;
        }
        return false;
    }

    private static bool IsStandaloneVerb(string commandVerb)
    {
        if (StandaloneVerbs.Contains(commandVerb))
        {
            return true;
        }
        return false;
    }

    private static bool IsVerb(string commandVerb)
    {
        if (Verbs.Contains(commandVerb) || StandaloneVerbs.Contains(commandVerb))
        {
            return true;
        }
        
        return false;
    }   
}