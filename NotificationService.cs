using System.Net;
using System.Net.Mail;
using Microsoft.VisualBasic;

public class NotificationService
{
    public void CheckQuestDueDates(User user)
    {
        var userQuests = user.ActiveQuests;
        if (user == null)
        {
            Console.WriteLine("No user is logged in.");
            return;
        }
        DateTime todaysDate = DateTime.Now;

        foreach (var quest in userQuests)
        {
            // TimeSpan is a class that calculates the difference between two dates
            TimeSpan timeUntilDueDate = quest.DueDate - todaysDate;
            if (timeUntilDueDate.TotalHours <= 24 && !quest.IsCompleted)
            {
                SendQuestDueDateNotifications(quest, user.Email);
            }
        }
    }

    public void SendQuestDueDateNotifications(Quest quest, string toEmail)
    {
        // send email logic here
        var message = new MailMessage();
        // who the email is from
        message.From = new MailAddress(AppConfig.FromEmail);
        // who the email is to
        message.To.Add(toEmail);
        // subject of the email, the heading of the email
        message.Subject = "Your quest is due soon!";
        // body of the email, the main content of the email
        // ToShortDateString() formaterar datumet till ett kort format, tar bort klockslaget
        message.Body = "Your quest '" + quest.Title + "' is due on " + quest.DueDate.ToShortDateString();

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
