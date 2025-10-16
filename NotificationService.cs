using System.Net.Mail;
using System.Net;

public class NotificationService
{
    // send email when quests is under 24 hours left
    QuestManagment questManagment = new QuestManagment();

    public bool GetQuestsNearDeadline(User loggedInUser)
    {
        DateTime rightNow = DateTime.Now;
        foreach (var quest in questManagment.ToDoList)
        {
            // loop through quests and check if any are due in less than 24 hours
            if (quest.DueDate - rightNow < TimeSpan.FromHours(24))
            {
                SendEmailNotification(quest, loggedInUser);
                return true;
            }
        }
        return false;
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
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(AppConfig.FromEmail, AppConfig.EmailPassword);
            smtp.Send(message);
        }
    }
}
