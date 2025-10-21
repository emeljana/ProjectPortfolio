using System.Net.Mail;
using System.Net;

public class NotificationService
{
    // send email when quests is under 24 hours left


    public bool GetQuestsNearDeadline(User loggedInUser, QuestManagment quest)
    {
        DateOnly rightNow = DateOnly.FromDateTime(DateTime.Now);
        bool foundAnyQuests = false;
        
        // Gå igenom användarens AKTIVA quests istället för alla quests
        foreach (var item in loggedInUser.ActiveQuests)
        {
            int DaysLeft = (item.DueDate.DayNumber - rightNow.DayNumber);
            // loop through quests and check if any are due in less than 24 hours and more than 0 hours
            if (DaysLeft < 1 && DaysLeft >= 0)
            {
                SendEmailNotification(item, loggedInUser);
                foundAnyQuests = true; // Found at least one quest
                // returna inte här eftersom om det finns flera quests nära deadline så ska den skicka email för alla
                // och return avslutar metoden 
            }
        }
        return foundAnyQuests;
    }

    private void SendEmailNotification(Quest quest, User loggedInUser)
    {
        // create the email object
        var message = new MailMessage();
        // who the email is from, gets the informaytion from AppConfig
        message.From = new MailAddress(AppConfig.FromEmail);
        message.To.Add(loggedInUser.Email); // Use the logged-in user's email
        message.Subject = "Quest Deadline Approaching";
        message.Body = "Your quest " + quest.Title + " is due on " + quest.DueDate.ToString("g");

        // configure the SMTP client, to the email server
        using (var smtp = new SmtpClient(AppConfig.SmtpServer, AppConfig.SmtpPort))
        {
            smtp.UseDefaultCredentials = false; // Viktigt för Gmail!
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(AppConfig.FromEmail, AppConfig.EmailPassword);
            smtp.Send(message);
        }
    }
}
