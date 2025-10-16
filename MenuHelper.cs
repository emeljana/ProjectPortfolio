public class MenuHelper
{
    // Implement menu-related helper methods here
    public void ShowMenu()
    {
        Console.WriteLine("Menu:");
        Console.WriteLine("1. Create User");
        Console.WriteLine("2. Log In");
        Console.WriteLine("Press 'Q' to exit.");
    }

    public void ShowQuestMenu(User loggedInUser) // Accept the logged-in user
    {
        QuestManagment questManagment = new QuestManagment();
        Quest quest = new Quest("", "", DateTime.Now, 1);
        
        bool runningQuestMenu = true;
        while (runningQuestMenu)
        {
            Console.Clear();
            Console.WriteLine("Quest Menu - Welcome " + loggedInUser.Username + "!"); // Show username
            Console.WriteLine("1. Show my Quests");
            Console.WriteLine("2. Show All Quests");
            Console.WriteLine("3. Update Quest");
            Console.WriteLine("4. Complete Quest");
            Console.WriteLine("5. Back to Main Menu");
            int input = Convert.ToInt32(Console.ReadLine());

            switch (input)
            {
                case 1:
                    questManagment.ShowMyQuest(loggedInUser); // The logged-in user
                    break;
                case 2:
                    quest.ShowAllQuest(questManagment);
                    break;
                case 3:
                    questManagment.UpdateQuest();
                    break;
                case 4:
                    questManagment.CompleteQuest(); // Pass the logged-in user
                    break;
                case 5:
                    return; // Return to main menu 
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    runningQuestMenu = false;
                    break;
            }
            Console.Read();
        }
    }
}

