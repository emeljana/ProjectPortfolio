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

    public void CreateUser()
    {
        Console.Write("Enter your username: ");
        Username = Console.ReadLine() ?? "";

        Console.Write("Enter your password: ");
        Password = Console.ReadLine() ?? "";

        Console.Write("Enter your email: ");
        Email = Console.ReadLine() ?? "";

        Console.Write("Enter your phone number: ");
        if (int.TryParse(Console.ReadLine(), out int phoneNumber))
        {
            PhoneNumber = phoneNumber;
        }
        else
        {
            PhoneNumber = 0;
            Console.WriteLine("Invalid phone number entered, set to 0.");
        }

        Console.WriteLine("Hi " + Username + "! Your account has been created.");
    }

    public void ValidatePassword()
    {
        if (Password.Length < 6)
        {
            Console.WriteLine("Password must be at least 6 characters long.");
        }
        if (Password.Any(char.IsUpper))
        {
            Console.WriteLine("Password must contain at least one uppercase letter.");
        }
        if (Password.Any(char.IsLower))
        {
            Console.WriteLine("Password must contain at least one lowercase letter.");
        }
        if (Password.Any(char.IsDigit))
        {
            Console.WriteLine("Password must contain at least one digit.");
        }
        if (Password.Any(ch => !char.IsLetterOrDigit(ch)))
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

        if (Username == inputUsername && Password == inputPassword)
        {
            Console.WriteLine("Login successful!");
        }
        else
        {
            Console.WriteLine("Invalid username or password.");
        }
    }

    public void TwoFactorAuthentication()
    {
        // Generate a random code
        var random = new Random();
        string code = random.Next(100000, 10000000).ToString();

        Console.Write("Which method do you want to use for 2FA?\n1. Email\n2. SMS\n");
        int choice = Convert.ToInt32(Console.ReadLine());
        if (choice != 1 && choice != 2)
        {
            Console.WriteLine("Invalid choice. Please try again.");
            return;
        }

        switch (choice)
        {
            case 1:
            Console.Write("Enter your email: ");
            string email = Console.ReadLine();
            SendEmail(email, code);
                break;
            case 2:
                Console.Write("Enter your phone number: ");
                if (int.TryParse(Console.ReadLine(), out int phoneNumber))
                {
                    SendSMS(phoneNumber, code);
                }
                break;
        }

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

    private void SendSMS(int phoneNumber, string newCode)
    {
        // Twilio account details from configuration
        string accountSid = AppConfig.TwilioAccountSid;
        string authToken = AppConfig.TwilioAuthToken;

        // Initialize Twilio client
        TwilioClient.Init(accountSid, authToken);

        // send SMS
        var message = MessageResource.Create(
            from: new PhoneNumber(AppConfig.TwilioFromPhoneNumber),
            to: new PhoneNumber("+46" + phoneNumber),
            body: "Your 2FA code is: " + newCode
        );
        Console.WriteLine("SMS sent.");
    }

    public void ShowMenu()
    {
        Console.WriteLine("Menu:");
        Console.WriteLine("1. Create User");
        Console.WriteLine("2. Log In");
        Console.WriteLine("3. Exit");
    }
}