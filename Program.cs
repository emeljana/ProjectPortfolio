using System;

class Program
{
    
    static async Task Main(string[] args)
    {
        Console.Clear();
        MenuHelper menuHelper = new MenuHelper();
        Authenticator authenticator = new Authenticator();
        
        Console.WriteLine("Hello, welcome to the Adventure Game!");
        
        await menuHelper.ShowMenu(authenticator);
    }
}
