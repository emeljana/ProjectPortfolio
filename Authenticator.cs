using System.ComponentModel;
using System;
using System.Net;
using System.Net.Mail;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Cryptography;


public class Authenticator
{
    public static List<User> usersList = new List<User>();


    public Authenticator()
    {
        usersList.Add(new User("Sara", "Sara123!", "emeliecaroline99@gmail.com"));
        usersList.Add(new User("John", "John123!", "john@example.com"));
        usersList.Add(new User("Alice", "Alice123!", "alice@example.com"));
    }

    public User CreateUser() // Return the created user
    {
        User user = new User("", "", "");
        Console.Clear();
        Console.WriteLine("--- Create New User ---");
        Console.Write("Enter your username: ");
        user.Username = Console.ReadLine() ?? "";

        Console.Write("Enter your password: ");
        string password = ReadPassword() ?? "";

        // If createUser is true, procced to Login
        if (ValidatePassword(password))
        {
            user.Password = password;
        }

        Console.Write("Enter your email: ");
        user.Email = Console.ReadLine() ?? "";

        usersList.Add(user);
        Console.WriteLine("Hi " + user.Username + "! Your account has been created. Press enter to continue...");
        Console.ReadLine();
        return user; // Return the created user
    }
    

    public bool ValidatePassword(string password)
    {

        bool correctPassword = false;
        while (!correctPassword)
        {
            correctPassword = true;
            if (password.Length < 6)
            {
                Console.WriteLine("Password must be at least 6 characters long.");
                correctPassword = false;
            }
            if (!password.Any(char.IsUpper))
            {
                Console.WriteLine("Password must contain at least one uppercase letter.");
                correctPassword = false;
            }
            if (!password.Any(char.IsLower))
            {
                Console.WriteLine("Password must contain at least one lowercase letter.");
                correctPassword = false;
            }
            if (!password.Any(char.IsDigit))
            {
                Console.WriteLine("Password must contain at least one digit.");
                correctPassword = false;
            }
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                Console.WriteLine("Password must contain at least one special character.");
                correctPassword = false;
            }
            if (!correctPassword)
            {
                Console.Write("Please enter a valid password: ");
                password = ReadPassword() ?? "";
            }
            else
            {
                Console.WriteLine("Valid password.");
            }
        }
        return correctPassword; // Return true if password is valid
    }

    public User? LogIn() // Return the logged-in user to User class, ? means it can be null
    {
        Console.Clear();
        Console.WriteLine("--- Log In ---");
        Console.Write("Enter your username: ");
        string inputUsername = Console.ReadLine() ?? "";

        User? searchUser = null;

        Console.Write("Enter your password: ");
        string inputPassword = ReadPassword() ?? "";

        bool rightLogin = false;
        while (!rightLogin)
        {
            // FirstOrDefault method in LINQ to find a user matching the input credentials
            // If a match is found, it returns the user object; otherwise, it returns null
            searchUser = usersList.FirstOrDefault(u => u.Username == inputUsername && u.Password == inputPassword);
            
            if (searchUser != null)
            {
                Console.WriteLine("Login successful! Press enter to continue to 2FA...");
                Console.ReadLine(); // Wait for user to press enter
                TwoFactorAuthentication(searchUser); // värdet
                rightLogin = true;
            }
            else
            {
                Console.WriteLine("Invalid username or password. Please try again.");
                inputPassword = ReadPassword() ?? "";
            }
        }
        return searchUser; // Return the actual logged-in user
    }

    public void TwoFactorAuthentication(User user) // objekttypen user
    {
        Console.Clear();
        Console.WriteLine("--- Two-Factor Authentication ---");
        // Generate a random code
        var random = new Random();
        string code = random.Next(100000, 999999).ToString();
        Console.Write("An authentication code has been sent to your email.");

        SendEmail(user.Email, code);

        Console.Write("Enter the code sent to you: ");
        string inputCode = Console.ReadLine() ?? "";

        if (inputCode == code)
        {
            Console.WriteLine("Login successful! Press any key to be assigned quests...");
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

    private static string? ReadPassword()
    {
        // skapar en tom sträng för att lagra användarens lösenord
        // varje gång användaren trycker på en tangent läggs tecknet till i denna sträng
        string password = "";

        // skapar en variabel för att lagra information om varje tangenttryckning
        ConsoleKeyInfo key;

        while (true)
        {
            // läser varje tangenttryckning som görs utan att visa den i konsolen
            // "intercept: true" betyder att tecknet inte visas i konsolen
            key = Console.ReadKey(intercept: true);

            // kontrollerar om användaren tryckte på Enter-tangenten
            if (key.Key == ConsoleKey.Enter)
            {
                // om ja, görs en radbrytning och loopen avslutas
                Console.WriteLine();
                break;
            }

            // kontrollerar om användaren tryckte på Backspace-tangenten
            else if (key.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    // tar bort det sista tecknet från lösenordet
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b"); // Ta bort den sista stjärnan
                }
            }
            else
            {
                password += key.KeyChar;
                Console.Write("*"); // visar en stjärna för varje tecken som skrivs
            }

        }
        return password;
    }
}