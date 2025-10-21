public class QuestManagment
{
    // Quest management logic goes here
    public List<Quest> ToDoList = new List<Quest>();

    public QuestManagment()
    {
        ToDoList.Add(new Quest("Slay the Dragon", "Defeat the dragon terrorizing the village", new DateOnly(2025, 10, 21), 5));
        ToDoList.Add(new Quest("Rescue the Princess", "Save the princess from the evil warlock", new DateOnly(2025, 10, 21), 4));
        ToDoList.Add(new Quest("Find the Lost Artifact", "Locate and retrieve the ancient artifact", new DateOnly(2025, 10, 21), 3));
        ToDoList.Add(new Quest("Explore the Haunted Forest", "Investigate the mysterious occurrences in the forest", new DateOnly(2025, 10, 18), 2));
        ToDoList.Add(new Quest("Find the lost Crown", "Locate the royal crown stolen from the castle treasury.", new DateOnly(2025, 10, 21), 5));
    }

    public void AddQuest()
    {
        Console.Write("Enter a new quest title: ");
        string inputTitle = Console.ReadLine() ?? "";
        Console.Write("Enter quest description: ");
        string inputDescription = Console.ReadLine() ?? "";
        Console.Write("Enter quest due date (YYYY-MM-DD): ");
        DateOnly inputDueDate = DateOnly.Parse(Console.ReadLine() ?? "");
        if (inputDueDate < DateOnly.FromDateTime(DateTime.Now))
        {
            // vad gör denna kod
            Console.WriteLine("Invalid due date. Please enter a future date: ");
            inputDueDate = DateOnly.Parse(Console.ReadLine() ?? "");
        }
        Console.Write("Enter quest priority (1-5):");
        int inputPriority = Convert.ToInt32(Console.ReadLine());
        if (inputPriority < 1 || inputPriority > 5)
        {
            Console.WriteLine("Invalid priority. Please enter a priority between 1 and 5.");
            inputPriority = Convert.ToInt32(Console.ReadLine());
        }

        // Create a new Quest object and add it to the ToDoList
        // Detta kallas att instantiera ett objekt
        ToDoList.Add(new Quest(inputTitle, inputDescription, inputDueDate, inputPriority));
        Console.WriteLine("Quest created successfully!");
    }

    public void CompleteQuest(User loggedInUser) // Accept the logged-in user
    {
        Console.Clear();
        if (loggedInUser.ActiveQuests.Count == 0)
        {
            System.Console.WriteLine("You have no active quests.");
            return; // Stop the method if no active quests
        }
        System.Console.WriteLine("Which quest would you like to mark as completed? Enter the number:");
        // list the user's active quests with numbers
        for (int i = 0; i < loggedInUser.ActiveQuests.Count; i++)
        {
            // (i+1) to show numbers starting from 1 instead of 0
            Console.WriteLine(i + 1 + ". " + loggedInUser.ActiveQuests[i].Title);
        }
        string inputCompleteChoice = Console.ReadLine() ?? "";
        
        if (int.TryParse(inputCompleteChoice, out int choice))
        {
            choice = choice - 1; // Adjust for zero-based index

            if (choice >= 0 && choice < loggedInUser.ActiveQuests.Count)
            {
                Quest quest = loggedInUser.ActiveQuests[choice];
                quest.IsCompleted = true;
                System.Console.WriteLine("Quest " + quest.Title + " marked as completed.");
                loggedInUser.ActiveQuests.RemoveAt(choice); // Remove the completed quest from active quests
            }
        }
        else
        {
            System.Console.WriteLine("Quest not found in your active quests.");
        }
    }

    public void UpdateQuest(User loggedInUser) // Accept the logged-in user
    {
        Console.Clear();
        if (loggedInUser.ActiveQuests.Count == 0)
        {
            System.Console.WriteLine("You have no active quests to update.");
            return; // Stop the method if no active quests
        }
        System.Console.WriteLine("Which quest would you like to update?");
        for (int i = 0; i < loggedInUser.ActiveQuests.Count; i++)
        {
            Console.WriteLine(i + 1 + ". " + loggedInUser.ActiveQuests[i].Title);
        }
        string inputUpdateChoice = Console.ReadLine() ?? "";

        if (int.TryParse(inputUpdateChoice, out int choiceUpdate))
        {
            choiceUpdate = choiceUpdate - 1;
            if (choiceUpdate >= 0 && choiceUpdate < loggedInUser.ActiveQuests.Count)
            {
                Quest quest = loggedInUser.ActiveQuests[choiceUpdate];
                Console.WriteLine("Choose which field to update:");
                Console.WriteLine("1. Description");
                Console.WriteLine("2. Due Date");
                Console.WriteLine("3. Priority");
                string fieldChoice = Console.ReadLine() ?? "";

                if (fieldChoice == "1")
                {
                    Console.WriteLine("Enter new description:");
                    quest.Description = Console.ReadLine() ?? "";
                    Console.WriteLine("Quest " + quest.Title + " description updated successfully!");
                }
                
                else if (fieldChoice == "2")
                {
                    Console.WriteLine("Enter new due date (YYYY-MM-DD):");
                    DateOnly newDueDate = DateOnly.Parse(Console.ReadLine() ?? "");
                    if (newDueDate < DateOnly.FromDateTime(DateTime.Now))
                    {
                        Console.WriteLine("Invalid due date. Please enter a future date: ");
                        newDueDate = DateOnly.Parse(Console.ReadLine() ?? "");
                    }
                    quest.DueDate = newDueDate;
                    Console.WriteLine("Quest " + quest.Title + " updated successfully to due date " + quest.DueDate + "!");
                }
                else if (fieldChoice == "3")
                {
                    Console.WriteLine("Enter new priority (1-5):");
                    quest.Priority = Convert.ToInt32(Console.ReadLine());
                    if (quest.Priority < 1 || quest.Priority > 5)
                    {
                        Console.WriteLine("Invalid priority. Please enter a priority between 1 and 5.");
                        quest.Priority = Convert.ToInt32(Console.ReadLine());
                    }
                    Console.WriteLine("Quest " + quest.Title + " updated successfully to priority " + quest.Priority + "!");
                }
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
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

    public void ShowMyQuest(User loggedInUser) // Accept the logged-in user
    {
        Console.Clear();
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


    