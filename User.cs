public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }

    List<User> usersList = new List<User>();

    // Constructor for creating users with known data
    public User(string username, string password, string email)
    {
        Username = username;
        Password = password;
        Email = email;
    }

    // Parameterless constructor for interactive user creation
    public User()
    {
        usersList.Add(new User("Sara", "Sara123!", "sara@example.com"));
        usersList.Add(new User("John", "John123!", "john@example.com"));
        usersList.Add(new User("Alice", "Alice123!", "alice@example.com"));
    }
    


}