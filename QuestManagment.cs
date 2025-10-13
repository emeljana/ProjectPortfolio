public class QuestManagment : Quest
{
    // Quest management logic goes here
    List<Quest> ToDoList = new List<Quest>();
    public QuestManagment(string title, string description, int dueDate, int priority) : base(title, description, dueDate, priority)
    {
        
    }
    public void AddQuest()
    {
        Console.Write("Enter quest title:");
        Title = Console.ReadLine() ?? "";
        Console.Write("Enter quest description:");
        Description = Console.ReadLine() ?? "";
        Console.Write("Enter quest due date:");
        DueDate = Convert.ToInt32(Console.ReadLine());
        Console.Write("Enter quest priority (1-5):");
        Priority = Convert.ToInt32(Console.ReadLine());

        // Create a new Quest object and add it to the ToDoList
        // Detta kallas att instantiera ett objekt
        ToDoList.Add(new Quest(Title, Description, DueDate, Priority));
        Console.WriteLine("Quest created successfully!");
    }

    public void CompleteQuest()
    {
        System.Console.WriteLine("Which quest would you like to mark as completed?");
        Title = Console.ReadLine() ?? "";
        foreach (var item in collection)
        {
            if (item.Title == Title)
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
        Title = Console.ReadLine() ?? "";
        foreach (var item in collection)
        {
            if (item.Title == Title)
            {
                System.Console.WriteLine("Enter a new description for the quest:");
                item.Description = Console.ReadLine() ?? "";
                System.Console.WriteLine("Enter a new due date for the quest:");
                item.DueDate = Convert.ToInt32(Console.ReadLine());
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