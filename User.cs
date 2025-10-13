public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public int PhoneNumber { get; set; }

    // Constructor for creating users with known data
    public User(string username, string password, string email, int phoneNumber)
    {
        Username = username;
        Password = password;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    // Parameterless constructor for interactive user creation
    public User()
    {
        Username = "";
        Password = "";
        Email = "";
        PhoneNumber = 0;
    }
    


}