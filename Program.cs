using System;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Clear();
        Console.WriteLine("Hello, welcome to the Adventure Game!");
        MenuHelper menuHelper = new MenuHelper();
        Authenticator authenticator = new Authenticator();
        
        await menuHelper.ShowMenu(authenticator);
    }
}
