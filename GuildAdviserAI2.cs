using System.Text.Json;
using System.Text;
using System.Security.Cryptography.X509Certificates;


public class GuildAdviserAI2
{

    private static readonly HttpClient client = new HttpClient();
    public async Task GuildAdviserAIMenu()
    {
        Console.Clear();
        bool running = true;
        while (running)
        {
            Console.WriteLine("What do you want to ask the Guild Adviser AI about: ");
            Console.WriteLine("1. Create an epic quest text");
            Console.WriteLine("2. DueDates priorities");
            Console.WriteLine("3. Summerize a quest description");
            Console.WriteLine("4. Back to Quest Menu");
            string choice = Console.ReadLine() ?? "";

            QuestManagment quest = new QuestManagment();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Enter the title of the epic quest: ");
                    string title = Console.ReadLine() ?? "";
                    bool found = false;
                    foreach (var item in quest.ToDoList)
                    {
                        if (title == item.Title)
                        {
                            found = true;
                            string listText = string.Join("\n", quest.ToDoList.Select(q => q.Title + ": " + q.Description + ": " + q.DueDate));
                            string prompt = "You are a guild advisor in a fantasy RPG world. Create an epic quest text for the title: " + title + item.Description;
                            await AskOllma(prompt);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Quest not found.");
                        }
                    }
                    break;
                case "2":
                    // Call the method to get due date priorities
                    Console.WriteLine("Prioritizing quests based on due dates...");
                    string prompt2 = "who are you?";
                    await AskOllma(prompt2);
                    break;
                case "3":
                    // Call the method to summarize a quest description

                    string prompt3 = "You are a guild advisor in a fantasy RPG world. Make a short summary of all the quests: " + quest.ToDoList;
                    await AskOllma(prompt3);
                    break;
                case "4":
                    running = false; // Exit the loop to go back to Quest Menu
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }

    public async Task AskOllma(string prompt)
    {
        client.Timeout = TimeSpan.FromSeconds(60);

        var request = new
        {
            model = "gemma:2b",
            prompt = prompt,
            stream = true
        };

        string json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await client.PostAsync("http://localhost:11434/api/generate", content, CancellationToken.None);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            Console.WriteLine("\n💬 Guild Advisor säger:\n");

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                using var doc = JsonDocument.Parse(line);
                if (doc.RootElement.TryGetProperty("response", out var resp))
                {
                    Console.Write(resp.GetString());
                }

                if (doc.RootElement.TryGetProperty("done", out var doneProp)
                    && doneProp.GetBoolean())
                {
                    break; // 👈 Avbryt när Ollama markerar att svaret är klart
                }
            }

            Console.WriteLine("\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Ett fel uppstod: {ex.Message}");
        }
    }
}