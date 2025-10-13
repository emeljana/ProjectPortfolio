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

public class Authenticator : User
{
    private List<User> usersList = new List<User>();
    public void CreateUser()
    {
        Console.Write("Enter your username: ");
        Username = Console.ReadLine() ?? "";

        Console.Write("Enter your password: ");
        Password = Console.ReadLine() ?? "";

        Console.Write("Enter your email: ");
        Email = Console.ReadLine() ?? "";

        usersList.Add(this);
        Console.WriteLine("Hi " + Username + "! Your account has been created.");
        Console.ReadKey();
    }

    public void ValidatePassword()
    {
        if (Password.Length < 6)
        {
            Console.WriteLine("Password must be at least 6 characters long.");
        }
        if (!Password.Any(char.IsUpper))
        {
            Console.WriteLine("Password must contain at least one uppercase letter.");
        }
        if (!Password.Any(char.IsLower))
        {
            Console.WriteLine("Password must contain at least one lowercase letter.");
        }
        if (!Password.Any(char.IsDigit))
        {
            Console.WriteLine("Password must contain at least one digit.");
        }
        if (!Password.Any(ch => !char.IsLetterOrDigit(ch)))
        {
            Console.WriteLine("Password must contain at least one special character.");
        }
    }

    public void LogIn()
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
        }

        else
        {
            Console.WriteLine("Invalid username or password. Press any key to continue...");
        }
        Console.ReadKey();
        Console.Clear();
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
            Console.WriteLine("Login successful!");
        }
        else
        {
            Console.WriteLine("Invalid code.");
        }
        Console.ReadKey();
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
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(AppConfig.FromEmail, AppConfig.EmailPassword);
            smtp.Send(message);

        }
        Console.WriteLine("Email sent.");
    }
}