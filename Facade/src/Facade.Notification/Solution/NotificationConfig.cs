namespace dev.kaldiroglu.Facade.Notification.Solution;

/// <summary>
/// Value object holding all external configuration the Facade needs. One place to
/// change credentials — no scattering across client code.
/// </summary>
public sealed record NotificationConfig(
    string SmtpHost, int SmtpPort, string SmtpUsername, string SmtpPassword,
    string TwilioSid, string TwilioToken, string TwilioFromNumber,
    string SlackWebhookUrl,
    string FirebaseCredentialsPath,
    string LogFilePath);
