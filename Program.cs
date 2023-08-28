using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading;

class Program
{
    static void Main()
    {
        // Sender's email address and password
        string senderEmail = "hasnainkhanzada089@gmail.com";
        string senderPassword = "oampfnwwxlilmirz";

        // Attachment path
        string attachmentPath = @"C:/Users/DELL/Downloads/Haris.pdf";

        string attachmentFileName = Path.GetFileName(attachmentPath);

        // SMTP client configuration
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential(senderEmail, senderPassword),
            Timeout = 300000 // Set the timeout to 5 minutes (300,000 milliseconds)
        };

        // Read recipient email addresses from the file
        string[] recipientEmails = File.ReadAllLines(@"C:\Users\Haris Zahid\Downloads\email.txt");

        // Create the email message
        var message = new MailMessage();

        // Add the common properties for all emails
        message.From = new MailAddress(senderEmail);
        message.Subject = "DevOps/Cloud Intern";
        message.Body = "Hi Career Team,\n\n... Your email content here ...";

        try
        {
            foreach (string recipientEmail in recipientEmails)
            {
                // Set the recipient for the current email
                message.To.Clear();
                message.To.Add(recipientEmail);

                // Attach the file
                if (File.Exists(attachmentPath))
                {
                    var attachment = new Attachment(attachmentPath);
                    attachment.Name = attachmentFileName;
                    message.Attachments.Add(attachment);
                }
                else
                {
                    Console.WriteLine("Attachment file not found.");
                }

                // Send the email
                smtpClient.Send(message);
                Console.WriteLine($"Email sent to {recipientEmail} successfully.\n");

                // Remove the sent email from the file
                List<string> remainingEmails = new List<string>(recipientEmails);
                remainingEmails.Remove(recipientEmail);
                File.WriteAllLines(@"C:\Users\Haris Zahid\Downloads\email.txt", remainingEmails);

                // Dispose the attachments
                foreach (var attachment in message.Attachments)
                {
                    attachment.Dispose();
                }
                message.Attachments.Clear();

                // Wait for 15 seconds before sending the next email
                Thread.Sleep(15000);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error sending email: " + ex.Message);
        }
        finally
        {
            // Dispose the email message
            message.Dispose();
        }

        Console.ReadLine();
    }
}
