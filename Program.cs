using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Clear();
        int inputChoice = 0;
        while (inputChoice != 3)
        {
            Console.WriteLine("Hello, welcome to the Adventure Game!");
            Authenticator authenticator = new Authenticator();

            MenuHelper menuHelper = new MenuHelper();
            menuHelper.ShowMenu();
            inputChoice = Convert.ToInt32(Console.ReadLine());
            switch (inputChoice)
            {
                case 1:
                    authenticator.CreateUser();
                    authenticator.ValidatePassword();
                    break;
                case 2:
                    authenticator.LogIn();
                    break;
                case 3:
                    Console.WriteLine("Exiting the program. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
