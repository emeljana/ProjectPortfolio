public class MenuHelper
{
    // Implement menu-related helper methods here
    public void ShowMenu()
    {
        Console.WriteLine("Menu:");
        Console.WriteLine("1. Create User");
        Console.WriteLine("2. Log In");
        Console.WriteLine("3. Exit");
    }

    public void ShowQuestMenu()
    {
        Console.WriteLine("Quest Menu:");
        Console.WriteLine("1. Add Quest");
        Console.WriteLine("2. Complete Quest");
        Console.WriteLine("3. Update Quest");
        Console.WriteLine("4. Show All Quests");
        Console.WriteLine("5. Back to Main Menu");
        Console.WriteLine("6. Exit");
        int input = Convert.ToInt32(Console.ReadLine());

        QuestManagment questManagment = new QuestManagment();
        Quest quest = new Quest("", "", DateTime.Now, 1);

        while (input != 6)
        {
            input = Convert.ToInt32(Console.ReadLine());
            switch (input)
            {
                case 1:
                    questManagment.AddQuest();
                    break;
                case 2:
                    questManagment.CompleteQuest();
                    break;
                case 3:
                    questManagment.UpdateQuest();
                    break;
                case 4:
                    quest.ShowAllQuest();
                    break;
                case 5:
                    ShowMenu();
                    break;
                case 6:
                    Console.WriteLine("Exiting the program. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

}
