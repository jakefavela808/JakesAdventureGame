namespace AdventureS25;

public class Item
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsTakeable { get; private set; }

    public Item(string name, string description, bool isTakeable = true)
    {
        Name = name;
        Description = description;
        IsTakeable = isTakeable;
    }

    public string ToString()
    {
        return Name + " - " + Description + " - " + IsTakeable;
    }

    public string GetLocationDescription()
    {
        string article = SemanticTools.CreateArticle(Name);
        
        if (SemanticTools.IsPlural(Name))
        {
            return "There are " + Name + " here";
        }
        else
        {
            return "There is " + article + " " + Name + " here";
        }
    }
}
