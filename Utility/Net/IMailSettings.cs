namespace Utilities.Net
{
    public interface IMailSettings
    {
        string BccAddresses { get; set; }

        string SenderDisplayName { get; set; }
        string SenderEmailAddress { get; set; }
        string SenderEmailPassword { get; set; }

        string SupportDisplayName { get; set; }
        string SupportEmailAddress { get; set; }

        string EmailSubjectTemplate { get; set; }

        int SmtpClientTimeout { get; set; }
        int SmtpClientPortNumber { get; set; }
        bool SmtpClientEnableSsl { get; set; }

        string SmtpClientHostAddress { get; set; }
    }
}
