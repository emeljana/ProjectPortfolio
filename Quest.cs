using Twilio.Rest.Api.V2010.Account.Recording;

public class Quest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly DueDate { get; set; }
    public int Priority { get; set; }
    public bool IsCompleted { get; set; }

    public Quest(string title, string description, DateOnly dueDate, int priority)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
        Priority = priority;
        IsCompleted = false;
    }
    public void ShowAllQuest(QuestManagment questManagment)
    {
        Console.Clear();
        System.Console.WriteLine("--- Quest Details ---");
        foreach (var quest in questManagment.ToDoList)
        {
            System.Console.WriteLine("Title: " + quest.Title);
            System.Console.WriteLine("Description: " + quest.Description);
            System.Console.WriteLine("Due Date: " + quest.DueDate);
            System.Console.WriteLine("Priority: " + quest.Priority);
            System.Console.WriteLine("Is Completed: " + quest.IsCompleted);
            System.Console.WriteLine("-----------------------------------");
        }
    }
}


