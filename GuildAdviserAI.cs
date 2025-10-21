using System.Text.Json;
using System.Threading.Tasks;
using OpenAI.Chat;
using OpenAI.Models;
using Twilio.Types;
using System.Text;
using System.Net.Http;

// AI kan hjälpa till med:
//Generera quest descriptions → t.ex. användaren skriver bara titel "Rädda byn" → AI skapar en episk quest-text.
//Föreslå prioritet → baserat på deadline och innehåll.
//Sammanfatta quests → systemet ger en kort heroisk briefing över alla pågående uppdrag.

public class GuildAdvisorAI
{
    private static readonly HttpClient client = new HttpClient();

    public async Task AskAI(User loggedInUser)
    {
        Console.Clear();
        bool running = true;
        while (running)
        {
            Console.WriteLine("What do you want to ask the Guild Adviser AI about: ");
            Console.WriteLine("1. Create an epic quest text");
            Console.WriteLine("2. DueDates priorities");
            Console.WriteLine("3. Adventurer's Briefing (Summarize a quest description)");
            Console.WriteLine("4. Back to Quest Menu");
            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    // Call method to create epic quest text
                    Console.Clear();
                    Console.WriteLine("Create epic quest text...");
                    await CreateEpicQuestText(loggedInUser);
                    break;
                case "2":
                    // Call method to get due date priorities
                    Console.Clear();
                    Console.WriteLine("DueDates priorities...");
                    await GetDueDatePriorities(loggedInUser);
                    break;
                case "3":
                    // Call method to summarize a quest description
                    Console.Clear();
                    Console.WriteLine("Adventurer's Briefing...");
                    await SummarizeQuestDescription(loggedInUser);
                    break;
                case "4":
                    // Go back to the quest menu
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("Press enter to get back to Guild Adviser menu...");
            Console.ReadLine();
            Console.Clear();
        }
    }

    private async Task CreateEpicQuestText(User loggedInUser)
    {
        // Implementation for creating epic quest text
        if (loggedInUser.ActiveQuests.Count == 0)
        {
            System.Console.WriteLine("You have no active quests.");
            return; // Stop the method if no active quests
        }
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine("Here are the available quests. Choose which one by number:");
        for (int i = 0; i < loggedInUser.ActiveQuests.Count; i++)
        {
            Console.WriteLine(i + 1 + ". " + loggedInUser.ActiveQuests[i].Title);
        }
        string inputChoice = Console.ReadLine() ?? "";
        if (int.TryParse(inputChoice, out int choice))
        {
            choice = choice - 1;
            if (choice >= 0 && choice < loggedInUser.ActiveQuests.Count)
            {
                Quest selectedQuest = loggedInUser.ActiveQuests[choice];
                string prompt = "You are a guild advisor in a fantasy RPG world. Create an epic quest text for the title: " + selectedQuest.Title + " Description: " + selectedQuest.Description;
                await GuildAdvisorAIPackage(prompt);
            }
            else
            {
                Console.WriteLine("Invalid quest selection.");
            }
        }
    }

    private async Task GetDueDatePriorities(User loggedInUser)
    {
        // Implementation for getting due date priorities
        if (loggedInUser.ActiveQuests.Count == 0)
        {
            System.Console.WriteLine("You have no active quests.");
            return; // Stop the method if no active quests
        }
        Console.WriteLine("Prioritizing your quests based on due dates...");
        string prompt = "You are a guild advisor in a fantasy RPG world. Prioritize the following quests based on their due dates: ";
        foreach (var quest in loggedInUser.ActiveQuests)
        {
            prompt += "\n" + quest.Title + " - Due Date: " + quest.DueDate;
        }
        Console.WriteLine(prompt);
    }

    private async Task SummarizeQuestDescription(User loggedInUser)
    {
        // Implementation for summarizing a quest description
        if (loggedInUser.ActiveQuests.Count == 0)
        {
            System.Console.WriteLine("You have no active quests.");
            return; // Stop the method if no active quests
        }
        Console.WriteLine("Here is a brief summary of your active quests:");
        string prompt = "You are a guild advisor in a fantasy RPG world. Make a short summary of all the quests: ";
        foreach (var quest in loggedInUser.ActiveQuests)
        {
            prompt += "\n" + quest.Title + ": " + quest.Description;
        }
        Console.WriteLine(prompt);
    }


    public async Task GuildAdvisorAIPackage(string prompt)
    {
        string apiKey = AppConfig.OpenAIApiKey;
        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Error: OpenAI API key is not set in AppConfig!");
            return;
        }

        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

        var requestBody = new
        {
            model = "gpt-4.1-mini", // offentlig modell
            messages = new[]
            {
                new { role = "system", content = "You are a helpful guild advisor in a fantasy RPG world." },
                new { role = "user", content = prompt }
            }
        };

        string json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // --- Skicka förfrågan ---
        var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
        string responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"⚠️ API Error: {response.StatusCode}");
            Console.WriteLine(responseString);
            return;
        }

        // --- Läs och tolka svaret ---
        using JsonDocument doc = JsonDocument.Parse(responseString);

        if (!doc.RootElement.TryGetProperty("choices", out JsonElement choices))
        {
            Console.WriteLine("⚠️ Unexpected response format:");
            Console.WriteLine(responseString);
            return;
        }

        string reply = choices[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? "No response";

        Console.WriteLine("\n🤖 Guild Advisor says:\n" + reply);

    }
}








