public class QuestManagment
{
    // Quest management logic goes here
    List<Quest> ToDoList = new List<Quest>();

    public QuestManagment()
    {
        ToDoList.Add(new Quest("Slay the Dragon", "Defeat the dragon terrorizing the village", new DateTime(2025, 10, 23), 5));
        ToDoList.Add(new Quest("Rescue the Princess", "Save the princess from the evil warlock", new DateTime(2025, 11, 15), 4));
        ToDoList.Add(new Quest("Find the Lost Artifact", "Locate and retrieve the ancient artifact", new DateTime(2025, 12, 5), 3));
        ToDoList.Add(new Quest("Explore the Haunted Forest", "Investigate the mysterious occurrences in the forest", new DateTime(2025, 10, 30), 2));
        ToDoList.Add(new Quest("Find the lost Crown", "Locate the royal crown stolen from the castle treasury.", new DateTime(2025, 11, 20), 5));
    }
    public void AddQuest()
    {
        Console.Write("Enter quest title: ");
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

    public void CompleteQuest()
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
}