using System;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine("Hello, welcome to the Adventure Game!");
        MenuHelper menuHelper = new MenuHelper();
        Authenticator authenticator = new Authenticator();
        QuestManagment quest = new QuestManagment();

        bool programRunning = true;
        while (programRunning)
        {
            menuHelper.ShowMenu();
            string inputChoice = Console.ReadLine() ?? ""; // Move this INSIDE the loop
            
            switch (inputChoice)
            {
                case "1":
                    User? newUser = authenticator.CreateUser(); // Get the created user
                    if (newUser != null)
                    {
                        // If user creation is successful, show the quest menu
                        Console.WriteLine("Press any key to get assigned quests...");
                        Console.Read();
                        quest.AssignQuestToUser(newUser);
                        await menuHelper.ShowQuestMenu(newUser);
                    }
                    break;
                case "2":
                    User? loggedInUser = authenticator.LogIn(); // Get the logged-in user
                    if (loggedInUser != null)
                    {
                        quest.AssignQuestToUser(loggedInUser);
                        // If login is successful, show the quest menu with the logged-in user
                        await menuHelper.ShowQuestMenu(loggedInUser);
                    }
                    break;
                case "Q":
                    Console.WriteLine("Exiting the program. Goodbye!");
                    programRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
