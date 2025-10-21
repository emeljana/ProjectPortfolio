using System.Threading.Tasks;

public class MenuHelper
{
    QuestManagment quest = new QuestManagment();
    // Implement menu-related helper methods here
    public async Task ShowMenu(Authenticator authenticator)
    {
        bool programRunning = true;
        while (programRunning)
        {
            Console.Clear();
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Log In");
            Console.WriteLine("Press 'Q' to exit.");
            
            string inputChoice = Console.ReadLine() ?? "";
            
            switch (inputChoice)
            {
                case "1":
                    User? newUser = authenticator.CreateUser();
                    if (newUser != null)
                    {
                        Console.WriteLine("Press any key to get assigned quests...");
                        Console.Read();
                        quest.AssignQuestToUser(newUser);
                        await ShowQuestMenu(newUser);
                    }
                    break;
                case "2":
                    User? loggedInUser = authenticator.LogIn();
                    if (loggedInUser != null)
                    {
                        quest.AssignQuestToUser(loggedInUser);
                        await ShowQuestMenu(loggedInUser);
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

    public async Task ShowQuestMenu(User loggedInUser) // Accept the logged-in user
    {
        QuestManagment questManagment = new QuestManagment();
        Quest quest = new Quest("", "", DateOnly.FromDateTime(DateTime.Now), 1);

        bool runningQuestMenu = true;
        while (runningQuestMenu)
        {
            Console.Clear();
            Console.WriteLine("Quest Menu - Welcome " + loggedInUser.Username + "!"); // Show username
            Console.WriteLine("1. Show my Quests");
            Console.WriteLine("2. Show All Quests");
            Console.WriteLine("3. Update Quest");
            Console.WriteLine("4. Complete Quest");
            Console.WriteLine("5. Talk to GuildAdvisorAI");
            Console.WriteLine("6. Back to Main Menu");
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
                    GuildAdvisorAI guildAdvisorAI = new GuildAdvisorAI();
                    await guildAdvisorAI.GuildAdvisorAItest();
                    break;
                case 6:
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

