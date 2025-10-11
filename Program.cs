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
            User user = new User(); // Use parameterless constructor for interactive creation
            User user1 = new User("julia12345", "Julia12345!", "emeliecaroline.halgh99@gmail.com", 1234567890);

            user.ShowMenu();
            inputChoice = Convert.ToInt32(Console.ReadLine());
            switch (inputChoice)
            {
                case 1:
                    user.CreateUser();
                    user.ValidatePassword();
                    break;
                case 2:
                    user1.LogIn();
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
