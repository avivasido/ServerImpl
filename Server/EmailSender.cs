using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class EmailSender
    {
        private const string SYSTEM_EMAIL = "MedTrain2017@gmail.com";
        private const string SYSTEM_EMAIL_PASSWORD = "medtrainagr";
        private const string HOST = "smtp.gmail.com";
        private static string SIGNATURE = Environment.NewLine + "Sincerely," + Environment.NewLine + "Medical Training System staff";
        private const int PORT = 587;
        private const int TIMEOUT = 10000;

        private static SmtpClient client = new SmtpClient
        {
            Host = HOST,
            Port = PORT,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new System.Net.NetworkCredential(SYSTEM_EMAIL, SYSTEM_EMAIL_PASSWORD),
            Timeout = TIMEOUT,
        };

        internal static void sendMail(string eMail, string subject, string content)
        {
            try
            {
                client.Send(new MailMessage(SYSTEM_EMAIL, eMail, subject, content + SIGNATURE));
            }
            catch (Exception) { }
        }
    }
}
