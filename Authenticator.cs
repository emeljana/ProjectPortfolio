using System.ComponentModel;
using System;
using System.Net;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Cryptography;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;

public class Authenticator
{
    public static List<User> usersList = new List<User>();
    NotificationService notificationService = new NotificationService();

    public Authenticator()
    {
        usersList.Add(new User("Sara", "Sara123!", "emeliecaroline99@gmail.com"));
        usersList.Add(new User("John", "John123!", "john@example.com"));
        usersList.Add(new User("Alice", "Alice123!", "alice@example.com"));
    }
    User user = new User("", "", "");
    MenuHelper menuHelper = new MenuHelper();
    QuestManagment questManagment = new QuestManagment();
    Quest quest = new Quest("", "", DateTime.Now, 1);

    public User? CreateUser() // Return the created user
    {
        Console.Write("Enter your username: ");
        user.Username = Console.ReadLine() ?? "";

        Console.Write("Enter your password: ");
        user.Password = Console.ReadLine() ?? "";

        Console.Write("Enter your email: ");
        user.Email = Console.ReadLine() ?? "";

        // If createUser is true, procced to ShowQuestMenu
        if (ValidatePassword(user))
        {
            usersList.Add(user);
            Console.WriteLine("Hi " + user.Username + "! Your account has been created.");
            return user; // Return the created user
        }
        return null; // Return null if validation failed
        
    }


    public bool ValidatePassword(User user)
    {
        while (true)
        {
            bool correctPassword = true;
            if (user.Password.Length < 6)
            {
                Console.WriteLine("Password must be at least 6 characters long.");
                correctPassword = false;
            }
            if (!user.Password.Any(char.IsUpper))
            {
                Console.WriteLine("Password must contain at least one uppercase letter.");
                correctPassword = false;
            }
            if (!user.Password.Any(char.IsLower))
            {
                Console.WriteLine("Password must contain at least one lowercase letter.");
                correctPassword = false;
            }
            if (!user.Password.Any(char.IsDigit))
            {
                Console.WriteLine("Password must contain at least one digit.");
                correctPassword = false;
            }
            if (!user.Password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                Console.WriteLine("Password must contain at least one special character.");
                correctPassword = false;
            }
            if (correctPassword)
            {
                return correctPassword; // Return true if password is valid
            }
            else
            {
                Console.Write("Please enter a valid password: ");
                user.Password = Console.ReadLine() ?? "";
                Console.WriteLine("Valid password. Press any key to continue...");
                Console.Read();
            }
        }
    }

    public User? LogIn() // Return the logged-in user, ? means it can be null
    {
        Console.Write("Enter your username: ");
        string inputUsername = Console.ReadLine() ?? "";

        Console.Write("Enter your password: ");
        string inputPassword = Console.ReadLine() ?? "";

        // FirstOrDefault method in LINQ to find a user matching the input credentials
        // If a match is found, it returns the user object; otherwise, it returns null
        var searchUser = usersList.FirstOrDefault(u => u.Username == inputUsername && u.Password == inputPassword);

        if (searchUser != null)
        {
            Console.WriteLine("Login successful!");
            TwoFactorAuthentication();
            /*questManagment.AssignQuestToUser(searchUser);*/
            notificationService.GetQuestsNearDeadline(searchUser, questManagment); // Call the method with the logged-in user
            return searchUser; // Return the actual logged-in user
        }

        else
        {
            Console.WriteLine("Invalid username or password. Press any key to continue...");
            Console.Read();
            return null; // Return null if login fails
        }

        
    }

    public void TwoFactorAuthentication()
    {
        // Generate a random code
        var random = new Random();
        string code = random.Next(100000, 999999).ToString();
        Console.Write("Enter your email for 2FA: ");
        string inputEmail = Console.ReadLine() ?? "";

        SendEmail(inputEmail, code);

        Console.Write("Enter the code sent to you: ");
        string inputCode = Console.ReadLine() ?? "";

        if (inputCode == code)
        {
            Console.WriteLine("Login successful! Press any key to assign quests...");
        }
        
        else
        {
            Console.WriteLine("Invalid code.");
        }
        Console.Read();
    }

    private void SendEmail(string toEmail, string newCode)
    {
        // send email logic here
        var message = new MailMessage();
        // who the email is from
        message.From = new MailAddress(AppConfig.FromEmail);
        // who the email is to
        message.To.Add(toEmail);
        // subject of the email, the heading of the email
        message.Subject = "Your 2FA Code";
        // body of the email, the main content of the email
        message.Body = "Your 2FA code is: " + newCode;

        using (var smtp = new SmtpClient(AppConfig.SmtpServer, AppConfig.SmtpPort))
        {
            smtp.UseDefaultCredentials = false; // Viktigt för Gmail!
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(AppConfig.FromEmail, AppConfig.EmailPassword);
            smtp.Send(message);
        }
        Console.WriteLine("Email sent.");
    }
}