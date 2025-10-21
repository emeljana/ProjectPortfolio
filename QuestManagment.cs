public class QuestManagment
{
    // Quest management logic goes here
    public List<Quest> ToDoList = new List<Quest>();

    public QuestManagment()
    {
        ToDoList.Add(new Quest("Slay the Dragon", "Defeat the dragon terrorizing the village", new DateTime(2025, 10, 21), 5));
        ToDoList.Add(new Quest("Rescue the Princess", "Save the princess from the evil warlock", new DateTime(2025, 10, 21), 4));
        ToDoList.Add(new Quest("Find the Lost Artifact", "Locate and retrieve the ancient artifact", new DateTime(2025, 10, 21), 3));
        ToDoList.Add(new Quest("Explore the Haunted Forest", "Investigate the mysterious occurrences in the forest", new DateTime(2025, 10, 18), 2));
        ToDoList.Add(new Quest("Find the lost Crown", "Locate the royal crown stolen from the castle treasury.", new DateTime(2025, 10, 21), 5));
    }


    public void AddQuest()
    {
        Console.Write("Enter a new quest title: ");
        string title = Console.ReadLine() ?? "";
        Console.Write("Enter quest description: ");
        string description = Console.ReadLine() ?? "";
        Console.Write("Enter quest due date and time (YYYY-MM-DD HH:MM): ");
        DateTime dueDate = DateTime.Parse(Console.ReadLine() ?? "");
        if (dueDate < DateTime.Now)
        {
            Console.WriteLine("Invalid due date. Please enter a future date: ");
            dueDate = DateTime.Parse(Console.ReadLine() ?? "");
        }
        Console.Write("Enter quest priority (1-5):");
        int priority = Convert.ToInt32(Console.ReadLine());

        // Create a new Quest object and add it to the ToDoList
        // Detta kallas att instantiera ett objekt
        ToDoList.Add(new Quest(title, description, dueDate, priority));
        Console.WriteLine("Quest created successfully!");
    }

    public void CompleteQuest() // Accept the logged-in user
    {
        System.Console.WriteLine("Which quest would you like to mark as completed?");
        string title = Console.ReadLine() ?? "";
        foreach (var item in ToDoList)
        {
            if (item.Title == title)
            {
                item.IsCompleted = true;
                System.Console.WriteLine("Quest marked as completed.");
                return;
            }
            else
            {
                System.Console.WriteLine("Quest not found.");
            }
        }
    }
    public void UpdateQuest()
    {
        System.Console.WriteLine("Which quest would you like to update?");
        string title = Console.ReadLine() ?? "";
        foreach (var item in ToDoList)
        {
            if (item.Title == title)
            {
                System.Console.WriteLine("Enter a new description for the quest:");
                item.Description = Console.ReadLine() ?? "";
                System.Console.WriteLine("Enter a new due date for the quest:");
                item.DueDate = DateTime.Parse(Console.ReadLine() ?? "");
                if (item.DueDate < DateTime.Now)
                {
                    Console.WriteLine("Invalid due date. Please enter a future date: ");
                    item.DueDate = DateTime.Parse(Console.ReadLine() ?? "");
                }
                System.Console.WriteLine("Enter a new priority for the quest (1-5):");
                item.Priority = Convert.ToInt32(Console.ReadLine());
                System.Console.WriteLine("Quest updated.");
                return;
            }
            else
            {
                System.Console.WriteLine("Quest not found.");
            }
        }
    }

    public int GetRandomQuestAmount()
    {
        Random random = new Random();
        return random.Next(1, 4);
    }

    // Method to assign random quests to a user
    public void AssignQuestToUser(User loggedInUser) // Accept the logged-in user
    {
        Console.Clear();
        if (loggedInUser.ActiveQuests.Count > 0)
        {
            Console.WriteLine("You already have active quests assigned.");
        }
        else
        {
            int questAmount = GetRandomQuestAmount(); // Use the method we created
            Random randomQuest = new Random();

            Console.WriteLine(questAmount + " quests have been assigned to you!");

            for (int i = 0; i < questAmount; i++)
            {
                // Select a random quest from the ToDoList
                // user cant be assigned the same quest twice
                int questIndex = randomQuest.Next(ToDoList.Count);
                while (loggedInUser.ActiveQuests.Contains(ToDoList[questIndex]))
                {
                    questIndex = randomQuest.Next(ToDoList.Count);
                }
                // Assign the quest to the user
                Quest assignedQuest = ToDoList[questIndex];
                AddQuestToUser(loggedInUser, assignedQuest); // Pass both user and quest
            }
        }
        Console.WriteLine("Press any key to continue...");
        Console.Read();

    }

    // Helper method to assign a quest to a user
    private void AddQuestToUser(User user, Quest quest)
    {
        user.ActiveQuests.Add(quest); // Actually add the quest to the users list
        Console.WriteLine("Quest '" + quest.Title + "' has been assigned to " + user.Username);
    }

    Quest quest = new Quest("", "", DateTime.Now, 1);
    public void ShowMyQuest(User loggedInUser) // Accept the logged-in user
    {
        if (loggedInUser.ActiveQuests.Count == 0)
        {
            Console.WriteLine("You have no active quests.");
        }
        else
        {
            Console.WriteLine("Active quests for " + loggedInUser.Username + ":");
            foreach (var quest in loggedInUser.ActiveQuests)
            {
                Console.WriteLine("Title: " + quest.Title);
                Console.WriteLine("Description: " + quest.Description);
                Console.WriteLine("Due Date: " + quest.DueDate);
                Console.WriteLine("Priority: " + quest.Priority);
                Console.WriteLine("Is Completed: " + quest.IsCompleted);
                Console.WriteLine("-----------------------------------");
            }
        }
        Console.WriteLine("Press any key to continue...");
    }
}   


    