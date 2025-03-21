using System.Collections.Generic;
using System.Linq;

namespace AdventureS25;

public static class Player
{
    public static Location CurrentLocation;
    public static List<Item> Inventory;
    public static bool HasUsedClothes = false;
    public static bool HasUsedPhone = false;
    public static bool HasUsedPhoneAtPark = false;
    public static bool HasUsedPhoneAfterLester = false;
    public static bool HasUsedPhoneAfterRicky = false;
    private static bool HasTalkedToJon = false;
    private static bool HasAcceptedOffer = false;
    private static bool HasDeniedOffer = false;
    public static bool HasReceivedOGKush = false;
    public static bool HasSoldToJoe = false;
    public static bool HasReceivedRickyCall = false;
    public static bool HasSoldToRicky = false;
    public static int Money = 0;
    public static bool HasWallet = false;

    public static void Initialize()
    {
        Inventory = new List<Item>();
        CurrentLocation = Map.StartLocation;
    }

    private static string GetFormattedDirections()
    {
        List<string> formattedDirections = new List<string>();
        foreach (var connection in CurrentLocation.Connections)
        {
            string direction = char.ToUpper(connection.Key[0]) + connection.Key.Substring(1);
            string destination = string.Join(" ", connection.Value.Name.Split(' ').Select(word => char.ToUpper(word[0]) + word.Substring(1)));
            formattedDirections.Add($"{direction} ({destination})");
        }
        return string.Join(", ", formattedDirections);
    }

    public static void Move(Command command)
    {
        if (command.Noun == null)
        {
            TextAnimator.AnimateText("Where do you want to go?\nPossible directions: " + GetFormattedDirections());
            return;
        }
        
        // Check if trying to go outside without using clothes
        if (command.Noun == "north" && CurrentLocation.Description.Contains("bed") && !HasUsedClothes)
        {
            TextAnimator.AnimateText("You walk outside without your clothes on... Your neighbors look at you in disgust as you go back inside.\nPossible directions: " + GetFormattedDirections());
            return;
        }
        
        // Check if trying to go outside without using phone
        if (command.Noun == "north" && CurrentLocation.Description.Contains("bed") && !HasUsedPhone)
        {
            TextAnimator.AnimateText("You should check your phone first.\nPossible directions: " + GetFormattedDirections());
            return;
        }
        
        if (CurrentLocation.CanMoveInDirection(command))
        {
            // Check if the player is trying to go to the park before talking to Jon
            if (command.Noun == "east" && CurrentLocation.Name == "outside" && !HasTalkedToJon)
            {
                TextAnimator.AnimateText("You should talk to Jon first. He's waiting for you in the alley up north.\nPossible directions: " + GetFormattedDirections());
                return;
            }
            
            // Check if the player can go to the park bench
            if (command.Noun == "north" && CurrentLocation.Name == "park" && !Map.CanGoToParkBench())
            {
                TextAnimator.AnimateText("You need to use your phone first to receive the call from Creepy Uncle Lester.\nPossible directions: " + GetFormattedDirections());
                return;
            }
            
            // Check if the player can go to the neighborhood path
            if (command.Noun == "west" && CurrentLocation.Name == "neighborhood" && (!HasSoldToJoe || !HasUsedPhoneAfterRicky))
            {
                if (!HasSoldToJoe)
                {
                    TextAnimator.AnimateText("You need to complete your delivery to Joe first.\nPossible directions: " + GetFormattedDirections());
                }
                else if (!HasUsedPhoneAfterRicky)
                {
                    TextAnimator.AnimateText("You need to check your phone first to get directions to Ricky's trailer.\nPossible directions: " + GetFormattedDirections());
                }
                return;
            }
            
            // Check if the player can go to the neighborhood
            if (command.Noun == "west" && CurrentLocation.Name == "outside" && (!HasReceivedOGKush || !HasUsedPhoneAfterLester))
            {
                if (!HasReceivedOGKush)
                {
                    TextAnimator.AnimateText("You don't know this neighborhood yet.\nPossible directions: " + GetFormattedDirections());
                }
                else if (!HasUsedPhoneAfterLester)
                {
                    TextAnimator.AnimateText("You should check your phone first to get directions.\nPossible directions: " + GetFormattedDirections());
                }
                return;
            }
            
            // Check if the player has clothes on before going outside
            if (command.Noun == "north" && CurrentLocation.Name == "bedroom" && !HasUsedClothes)
            {
                TextAnimator.AnimateText("You walk outside without your clothes on... Your neighbors look at you in disgust as you go back inside.\nPossible directions: " + GetFormattedDirections());
                return;
            }
            
            // Check if the player has their wallet before leaving the bedroom
            if (command.Noun == "north" && CurrentLocation.Name == "bedroom" && !HasWallet)
            {
                TextAnimator.AnimateText("You should take your wallet before leaving. You might need it later.\nPossible directions: " + GetFormattedDirections());
                return;
            }
            
            CurrentLocation = CurrentLocation.GetLocationInDirection(command);
            TextAnimator.AnimateText(CurrentLocation.GetDescription());
        }
        else
        {
            TextAnimator.AnimateText("You can't go that way.\nPossible directions: " + GetFormattedDirections());
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
        else if (command.Noun == "wallet")
        {
            HasWallet = true;
            CurrentLocation.RemoveItem(item);
            Inventory.Add(item);
            TextAnimator.AnimateText($"You've recieved 1x {command.Noun}. Your broke ${Money}.");
        }
        else if (!item.IsTakeable)
        {
            TextAnimator.AnimateText("The " + command.Noun + " can't be taken.");
        }
        else
        {
            Inventory.Add(item);
            CurrentLocation.RemoveItem(item);
            TextAnimator.AnimateText("You've recieved 1x " + command.Noun + ".");
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
                if (SemanticTools.IsPlural(item.Name))
                {
                    TextAnimator.AnimateText("- " + item.Name);
                }
                else
                {
                    TextAnimator.AnimateText("- " + article + " " + item.Name);
                }
            }
        }
    }

    public static void Look()
    {
        TextAnimator.AnimateText(CurrentLocation.GetDescription());
    }

    public static void Drop(Command command)
    {
        if (command.Noun != null && command.Noun != "")
        {
            // Check if the player has the iPhone in their inventory before showing the message
            bool hasIPhone = false;
            foreach (Item item in Inventory)
            {
                if (item.Name == "iphone")
                {
                    hasIPhone = true;
                    break;
                }
            }
            
            // Never allow dropping the iPhone
            if (command.Noun == "iphone" && hasIPhone)
            {
                TextAnimator.AnimateText("You can't drop your iPhone. You need it to stay in contact with people.");
                return;
            }
            
            Item? itemToDrop = null;
            foreach (Item item in Inventory)
            {
                if (item.Name == command.Noun)
                {
                    itemToDrop = item;
                    break;
                }
            }

            if (itemToDrop != null)
            {
                // Check if the item has been used before allowing to drop it
                if (command.Noun == "clothes" && !HasUsedClothes)
                {
                    TextAnimator.AnimateText("You can't drop that item until you've used it.");
                    return;
                }
                
                Inventory.Remove(itemToDrop);
                CurrentLocation.AddItem(itemToDrop);
                TextAnimator.AnimateText("You drop the " + itemToDrop.Name + ".");
            }
            else
            {
                TextAnimator.AnimateText("You don't have that item.");
            }
        }
        else
        {
            TextAnimator.AnimateText("Drop what?");
        }
    }

    public static void Use(Command command)
    {
        if (command.Noun == null || string.IsNullOrEmpty(command.Noun))
        {
            TextAnimator.AnimateText("What do you want to use?");
            return;
        }
        
        Item item = Inventory.FirstOrDefault(i => i.Name == command.Noun);
        
        if (item == null)
        {
            TextAnimator.AnimateText("You don't have that item.");
            return;
        }
        
        if (command.Noun == "iphone")
        {
            if (!HasUsedPhone)
            {
                TextAnimator.AnimateText("Incoming call... Jon");
                TextAnimator.AnimateText("You answer the call.");
                TextAnimator.AnimateText("Jon: Hey, how's it going?");
                TextAnimator.AnimateText("Jon: I heard you been sad lately. You can't get any job with your Game Dev degree... LOL! Put your clothes on and meet me outside, I need to talk to you!");
                TextAnimator.AnimateText("You hang up the phone.");
                TextAnimator.AnimateText("★★Tip★★: You can skip dialogue by clicking 'Enter' while in dialogue.");
                HasUsedPhone = true;
            }
            else if (CurrentLocation.Name == "park" && !HasUsedPhoneAtPark)
            {
                TextAnimator.AnimateText("Incoming call... Creepy Uncle Lester");
                TextAnimator.AnimateText("You answer the call.");
                TextAnimator.AnimateText("Creepy Uncle Lester: Hey there! I heard you're looking for work. Meet me at the park bench. I'm already here waiting for you.");
                TextAnimator.AnimateText("You look around and see a suspicious-looking man sitting on a bench nearby up north.");
                TextAnimator.AnimateText("You hang up the phone.");

                HasUsedPhoneAtPark = true;
            }
            else if (HasReceivedOGKush && !HasUsedPhoneAfterLester && !HasSoldToJoe)
            {
                TextAnimator.AnimateText("2x unread messages from Creepy Uncle Lester.");
                TextAnimator.AnimateText("Message: 'The first programmer's name is Joe. He's a pretty average dude, so don't be nervous. Go west from outside, then south to reach his house.'");
                TextAnimator.AnimateText("Message: 'Don't mess this up fuck-head. The future of the industry depends on it.'");
                HasUsedPhoneAfterLester = true;
            }
            else if (HasSoldToJoe && HasReceivedRickyCall && !HasUsedPhoneAfterRicky)
            {
                TextAnimator.AnimateText("3x unread messages from Creepy Uncle Lester.");
                TextAnimator.AnimateText("Message: 'Good job with Joe. Maybe you're not such a nerd after all.'");
                TextAnimator.AnimateText("Message: 'You have another programmer to help. It's my good friend Ricky, he brought his trailer over from Canada and parked it somewhere in the neighborhood'");
                TextAnimator.AnimateText("Message: 'He's waiting for his delivery. Don't keep him waiting, he gets cranky when he can't smoke.'");
                HasUsedPhoneAfterRicky = true;
            }
            else if (HasReceivedRickyCall)
            {
                TextAnimator.AnimateText("You check your phone but there are no new messages or calls.");
            }
            else
            {
                TextAnimator.AnimateText("You check your phone but there are no new messages or calls.");
            }
        }
        else if (command.Noun == "clothes")
        {
            if (!HasUsedClothes)
            {
                TextAnimator.AnimateText("You put on your clothes. You're now ready to go outside without being arrested for indecent exposure.");
                TextAnimator.AnimateText($"Removed 1x {command.Noun} from inventory.");
                HasUsedClothes = true;
                Inventory.Remove(item); // Remove the item from inventory after use
            }
            else
            {
                TextAnimator.AnimateText("You are already wearing clothes dumbass.");
            }
        }
        else if (command.Noun == "wallet")
        {
            if (HasWallet)
            {
                TextAnimator.AnimateText($"You have ${Money}.");
            }
            else
            {
                TextAnimator.AnimateText("You don't have a wallet, you're broke.");
            }
        }
        else if (command.Noun == "og-kush")
        {
            TextAnimator.AnimateText("I can't smoke this right now... maybe I'll try again later.");
        }
        else
        {
            TextAnimator.AnimateText("You can't use that item.");
        }
    }

    public static void Story(Command command)
    {
        if (command.Noun == "jon")
        {
            if (CurrentLocation.Name != "alleyway")
            {
                TextAnimator.AnimateText("Jon isn't here.");
                return;
            }

            if (!HasTalkedToJon)
            {
                TextAnimator.AnimateText("Jon: Hey there! I'm glad you made it. I have a job opportunity for you.");
                TextAnimator.AnimateText("Jon: I know a tech startup. It's actually a small game studio, they're looking for someone with your skills.");
                TextAnimator.AnimateText("You feel a glimmer of hope for the first time in weeks. Finally, someone is giving you the opportunity to use your Game Dev degree.");
                TextAnimator.AnimateText("You think of all the amazing games you could make with your skills.");
                TextAnimator.AnimateText("Jon whispers: Actually, it's not what you think. The 'tech startup' is just a front. They need someone to deliver weed to their programmers. The pay is good though.");
                TextAnimator.AnimateText("Jon whispers: I don't do that type of work, but I can help you get started with a buddy of mine.");
                TextAnimator.AnimateText("Jon grins: So, what do you say? Your Game Dev degree might not be useful, but at least you'll be in the tech industry... sort of. hahahhaahah");
                TextAnimator.AnimateText("Type 'accept' to accept Jon's offer or 'deny' to refuse.");
                HasTalkedToJon = true;
            }
            else
            {
                TextAnimator.AnimateText("You find Jon again in the alley-way.");
                TextAnimator.AnimateText("Hey fuck-head, can't you see I'm busy with Sandie now? Fuck off and go do delivries or some shit.");
                return;
            }
        }
        else if (command.Noun == "lester")
        {
            if (CurrentLocation.Name == "park bench" && !HasReceivedOGKush)
            {
                TextAnimator.AnimateText("Creepy Uncle Lester: So, you're the one Jon sent. Welcome to the business, kid.");
                TextAnimator.AnimateText("Creepy Uncle Lester reaches into his jacket and pulls out a small package.");
                TextAnimator.AnimateText("Creepy Uncle Lester: Here's 5 grams of my finest OG Kush. Deliver it to the programmers at their house.");
                TextAnimator.AnimateText("You've recieved 5x OG Kush.");
                TextAnimator.AnimateText("Creepy Uncle Lester: Don't mess this up, those programmers depend on it for their sanity.");
                TextAnimator.AnimateText("Creepy Uncle Lester: They can't finish their game without their weed, and it all falls on you.");
                TextAnimator.AnimateText("You feel your phone vibrate in your pocket.");

                
                // Add OG Kush to inventory
                for (int i = 0; i < 5; i++)
                {
                    Item ogKush = Items.OGKush;
                    Inventory.Add(ogKush);
                }
                
                HasReceivedOGKush = true;
            }
            else if (CurrentLocation.Name == "park bench" && HasReceivedOGKush)
            {
                TextAnimator.AnimateText("Creepy Uncle Lester: What are you still doing here? Fuck off and go deliver that stuff!");
            }
            else
            {
                TextAnimator.AnimateText("That person is not here.");
            }
        }
        else if (command.Noun == "joe")
        {
            if (CurrentLocation.Name == "joes house" && !HasSoldToJoe && HasReceivedOGKush)
            {
                TextAnimator.AnimateText("You knock on the door... a young and average-looking guy in a dark hoodie answers.");
                TextAnimator.AnimateText("Joe: Hey, you must be the new delivery kid. Sick!");
                TextAnimator.AnimateText("Joe: So, you got the stuff? I've been waiting all day. Can't code without my OG Kush, you know?");
                TextAnimator.AnimateText("You hand over one bag of OG Kush to Joe.");
                TextAnimator.AnimateText($"Removed 1x {command.Noun} from inventory.");
                
                // Remove one OG Kush from inventory
                bool removed = false;
                foreach (Item item in Inventory.ToList())
                {
                    if (item.Name == "og-kush" && !removed)
                    {
                        Inventory.Remove(item);
                        removed = true;
                    }
                }
                
                TextAnimator.AnimateText("Joe: Thanks, man. Here's your payment.");
                TextAnimator.AnimateText("You've recieved $50.");
                Money += 50;
                TextAnimator.AnimateText("Joe: You're doing important work, you know. Without this stuff, we'd never ship our game on time.");
                TextAnimator.AnimateText("Joe: The crunch is real man, and this helps us deal with the pressure.");
                TextAnimator.AnimateText("Joe: Anyway, thanks for the delivery. Now fuck off, I gotta get back to coding.");
                TextAnimator.AnimateText("*SLAMS DOOR*");
                TextAnimator.AnimateText("★★Tip★★: You can check your wallet by typing 'use wallet'.");
                HasSoldToJoe = true;
                TextAnimator.AnimateText("You feel your phone vibrate in your pocket.");
                HasReceivedRickyCall = true;
            }
            else if (CurrentLocation.Name == "joes house" && HasSoldToJoe)
            {
                TextAnimator.AnimateText("You knock on Joe's door again.");
                TextAnimator.AnimateText("Joe: Thanks for the delivery, but you're being creepy now. Fuck off and come back another time when I need a fix.");
            }
            else
            {
                TextAnimator.AnimateText("That person is not here.");
            }
        }
        else if (command.Noun == "ricky")
        {
            if (CurrentLocation.Name == "rickys house" && HasSoldToJoe && !HasSoldToRicky)
            {
                TextAnimator.AnimateText("You knock on the trailer door... a scruffy, jittery man with greasy hair, a patchy mustache, and bloodshot eyes answers.");
                TextAnimator.AnimateText("Ricky: FUCK! What took you so goddamn long? I been waitin here like a dick in a hand, all fuckin day. You better have the good shit, not that Swayze train shit.");
                TextAnimator.AnimateText("You hand over one bag of OG Kush to Ricky.");
                TextAnimator.AnimateText($"Removed 1x {command.Noun} from inventory.");
                TextAnimator.AnimateText("*Ricky sniffs bag intensely*");
                TextAnimator.AnimateText("Ricky: ... Yeah, that's the stuff. Smells like freedom and uh kitties, I guess. Here's your money, don't spend it all on like books or some shit.");
                TextAnimator.AnimateText("You've received $50.");
                Money += 50;
                TextAnimator.AnimateText("Ricky: Sorry bout that, bud. I get a little, what's the word...fuckin PISSED when I'm low on dope. Just moved here from Sunnyvale, a little trailer park in Canada");
                TextAnimator.AnimateText("Ricky: I haven't been able to find any decent fucking dope anywheres. but I think, maybe...this is will do.");
                TextAnimator.AnimateText("Ricky: Anyways, I gotta get back to work and shit. I'll definitely be texting you again for more of this.");
                
                // Remove one OG Kush from inventory
                bool removed = false;
                foreach (Item item in Inventory.ToList())
                {
                    if (item.Name == "og-kush" && !removed)
                    {
                        Inventory.Remove(item);
                        removed = true;
                    }
                }
                
                HasSoldToRicky = true;
            }
            else if (CurrentLocation.Name == "rickys house" && HasSoldToRicky)
            {
                TextAnimator.AnimateText("You knock on Ricky's trailer door again.");
                TextAnimator.AnimateText("Ricky: What the fuck do you want now? I already got my shit. Fuck off!");
                TextAnimator.AnimateText("The door slams in your face.");
            }
            else if (CurrentLocation.Name == "rickys house" && !HasSoldToJoe)
            {
                TextAnimator.AnimateText("You need to complete your delivery to Joe first.");
            }
            else
            {
                TextAnimator.AnimateText("That person is not here.");
            }
        }
        else
        {
            TextAnimator.AnimateText("You can't talk to that.");
        }
    }
    
    public static void Accept()
    {
        if (HasTalkedToJon)
        {
            TextAnimator.AnimateText("You accept Jon's offer.");
            TextAnimator.AnimateText("Jon: Great! My buddy's name is 'Creepy Uncle Lester'. He'll meet you at the park east of your house.");
            TextAnimator.AnimateText("Jon tells you to use your phone to contact him when you get there.");
            HasAcceptedOffer = true;
        }
        else
        {
            TextAnimator.AnimateText("There's nothing to accept right now.");
        }
    }
    
    public static void Deny()
    {
        if (HasTalkedToJon)
        {
            TextAnimator.AnimateText("You try to refuse Jon's offer.");
            TextAnimator.AnimateText("Jon: Sorry, but you don't have a choice. My buddy's name is 'Creepy Uncle Lester'. He'll meet you at the park east of your house.");
            TextAnimator.AnimateText("Jon tells you to use your phone to contact him when you get there.");
            HasDeniedOffer = true;
            HasAcceptedOffer = true; // Force acceptance even after denial
        }
        else
        {
            TextAnimator.AnimateText("There's nothing to deny right now.");
        }
    }
}