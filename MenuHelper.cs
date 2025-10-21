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
                        Console.Clear();
                        Console.WriteLine("Please log in to continue.");
                        authenticator.LogIn();
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
                case "q":
                    Console.WriteLine("Exiting the program. Goodbye!");
                    programRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
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
            Console.WriteLine("3. Add Quest");
            Console.WriteLine("4. Update Quest");
            Console.WriteLine("5. Complete Quest");
            Console.WriteLine("6. Talk to GuildAdvisorAI");
            Console.WriteLine("7. Back to Main Menu");
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
                    questManagment.AddQuest();
                    break;
                case 4:
                    questManagment.UpdateQuest(loggedInUser);
                    break;
                case 5:
                    questManagment.CompleteQuest(loggedInUser); // Pass the logged-in user
                    break;
                case 6:
                    GuildAdvisorAI guildAdvisorAI = new GuildAdvisorAI();
                    await guildAdvisorAI.AskAI(loggedInUser);
                    break;
                case 7:
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

