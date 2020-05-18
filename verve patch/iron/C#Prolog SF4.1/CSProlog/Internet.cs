using System;
using System.Windows.Forms;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net;

namespace Prolog
{
  public partial class PrologEngine
  {
    #region engine-mail
    static bool SendMail (string smtpHost, int port, string toAddr, string subject, string body)
    {
      try
      {
        SmtpClient client = new SmtpClient (smtpHost, port);
        MailAddress from  = new MailAddress ("xxxxxx@xxxxxx.xx");
        MailAddress to    = new MailAddress (toAddr);
        MailMessage msg   = new MailMessage ();
        msg.From = from;
        msg.To.Add (to);
        msg.Subject = subject;
        msg.Body = body;
        //client.EnableSsl = true;
        //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy ();
        client.Send (msg);
        //client.SendAsync (msg, <string> userState);

        //Attachment attachment = new Attachment ("you attachment file");
        //msg.Attachments.Add (attachment);

        return true;
      }
      catch (Exception x)
      {
        IO.Warning ("Unable to send message. Reason was:\r\n{0}", x.Message);

        return false;
      }
    }


    class AcceptAllCertificatePolicy : ICertificatePolicy
    {
      public AcceptAllCertificatePolicy ()
      {
      }

      public bool CheckValidationResult (ServicePoint sPoint,
         X509Certificate cert, WebRequest wRequest, int certProb)
      {
        // Always accept
        return true;
      }
    }
    #endregion engine-mail
  }
}
