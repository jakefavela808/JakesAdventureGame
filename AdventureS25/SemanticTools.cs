namespace AdventureS25;

public static class SemanticTools
{
    public static string CreateArticle(string word)
    {
        word = word.ToLower();
        
        // Check if the word is plural
        if (IsPlural(word))
        {
            return ""; // No article for plural words
        }
        
        if (word.StartsWith("a") || 
            word.StartsWith("e") || 
            word.StartsWith("i") || 
            word.StartsWith("o") || 
            word.StartsWith("u"))
        {
            return "an";
        }

        return "a";
    }
    
    public static bool IsPlural(string word)
    {
        word = word.ToLower();
        
        // Common plural words in the game
        string[] pluralWords = { "clothes", "pants", "glasses", "scissors", "jeans" };
        
        // Check if the word is in the plural words list
        foreach (string pluralWord in pluralWords)
        {
            if (word == pluralWord)
            {
                return true;
            }
        }
        
        // Check if the word ends with common plural suffixes
        if (word.EndsWith("s") && !word.EndsWith("ss"))
        {
            return true;
        }
        
        return false;
    }
}