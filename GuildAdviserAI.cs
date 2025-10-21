using System.Text.Json;
using System.Threading.Tasks;
using OpenAI.Chat;
using OpenAI.Models;
using Twilio.Types;

// AI kan hjälpa till med:
//Generera quest descriptions → t.ex. användaren skriver bara titel "Rädda byn" → AI skapar en episk quest-text.
//Föreslå prioritet → baserat på deadline och innehåll.
//Sammanfatta quests → systemet ger en kort heroisk briefing över alla pågående uppdrag.

public class GuildAdvisorAI
{
private static readonly HttpClient client = new HttpClient();
public async Task GuildAdvisorAItest()
{
    string apiKey = AppConfig.OpenAIApiKey;
    if (string.IsNullOrEmpty(apiKey))
    {
        Console.WriteLine("Error: OPENAI_API_KEY environment variable is not set!");
        return;
    }

    client.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

    Console.Write("Enter your message for the Guild Advisor AI: ");
    string userMessage = Console.ReadLine();

    var question = new
    {
        model = "gpt-4.1-nano-2025-04-14",
        messages = new[]
        {
            new { role = "system", content = "You are a helpful guild advisor in a fantasy RPG world." },
            new { role = "user", content = userMessage }
        }
    };

    string json = JsonSerializer.Serialize(question);
    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

    // --- Skicka till OpenAI API ---
    var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
        string responseString = await response.Content.ReadAsStringAsync();

    // --- ✅ Lägg in den här koden här ---
    using JsonDocument doc = JsonDocument.Parse(responseString);

    // 2️⃣ Kontrollera om choices finns
    if (!doc.RootElement.TryGetProperty("choices", out JsonElement choices))
    {
        Console.WriteLine("⚠️ Unexpected response format. Full response:");
        Console.WriteLine(responseString);
        return;
    }

    // 3️⃣ Läs ut AI-svaret säkert
    string reply = choices[0]
        .GetProperty("message")
        .GetProperty("content")
        .GetString() ?? "No response";

    Console.WriteLine("\nGuild Advisor AI says:\n" + reply);
}

}
