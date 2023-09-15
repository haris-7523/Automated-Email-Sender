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
        string senderEmail = "mowahidabdulrahman@gmail.com";
        string senderPassword = "qdsgdocmzldxqclf";

        // Attachment path
        string attachmentPath = @"F:/user/Abdul_Rehman_Resume.pdf";

        string attachmentFileName = Path.GetFileName(attachmentPath);

        // SMTP client configuration
        var smtpClient = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            EnableSsl = true,
            Credentials = new NetworkCredential(senderEmail, senderPassword),
            Timeout = 300000 // Set the timeout to 5 minutes (300,000 milliseconds)
        };

        try
        {
            while (true)
            {
                // Read recipient email addresses from the file
                string[] recipientEmails = File.ReadAllText(@"F:\user\email.txt")
                    .Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (recipientEmails.Length == 0)
                {
                    Console.WriteLine("No more emails to send.");
                    break;
                }

                // Create the email message
                var message = new MailMessage();

                // Add the common properties for all emails
                message.From = new MailAddress(senderEmail);
                message.Subject = "Associate SQA Engineer - Karachi";
                message.Body = @"Dear HR,

I trust this email finds you in good health. I am writing to express my enthusiastic interest in the SQA Associate position at your organization. My qualifications and dedication to software quality assurance make me a strong candidate for this role.

I have attached my detailed resume for your review. I am excited about the opportunity to discuss my potential contributions to your team in more detail.

Thank you for considering my application.

Warm regards,
Abdul Rehman";

                // Set the recipient for the current email
                message.To.Add(recipientEmails[0]);

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
                Console.WriteLine("sent");

                Console.WriteLine($"Email sent to {recipientEmails[0]} successfully.\n");

                // Remove the sent email from the file
                List<string> remainingEmails = new List<string>(recipientEmails);
                remainingEmails.RemoveAt(0);
                File.WriteAllText(@"F:\user\email.txt", string.Join(", ", remainingEmails));

                // Dispose the attachments
                foreach (var attachment in message.Attachments)
                {
                    attachment.Dispose();
                }
                message.Attachments.Clear();

                // Wait for 15 seconds before sending the next email
                Thread.Sleep(15000); // Sleep for 15 seconds
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error sending email: " + ex.Message);
        }

        Console.ReadLine();
    }
}
