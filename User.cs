public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public List<Quest> ActiveQuests { get; set; } = new List<Quest>(); // Fixed: Quest objects, not User objects

    // Constructor for creating users with known data
    public User(string username, string password, string email)
    {
        Username = username;
        Password = password;
        Email = email;
    }
}