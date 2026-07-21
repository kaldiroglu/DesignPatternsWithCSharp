using dev.kaldiroglu.Facade.Notification.Subsystems;
using dev.kaldiroglu.Facade.Notification.Solution;

using ProblemOrderService = dev.kaldiroglu.Facade.Notification.Problem.OrderService;
using SolutionOrderService = dev.kaldiroglu.Facade.Notification.Solution.OrderService;

// ─────────────────────────────────────────────────────────────────────────────
// BEFORE — the client wires up all five subsystems and dispatches by hand.
// ─────────────────────────────────────────────────────────────────────────────
Console.WriteLine("== BEFORE Facade: OrderService depends on 5 subsystems ==");

var email = new EmailService("smtp.company.com", 587, "noreply", "***");
var sms = new SmsService("AC123", "token", "+1555000");
var slack = new SlackService("https://hooks.slack.com/...");
var push = new PushNotificationService("/secrets/firebase.json");
var log = new NotificationLogger("/var/log/notifications.log");

var before = new ProblemOrderService(email, sms, slack, push, log);
before.ConfirmOrder(
    userId: "user_42",
    userEmail: "alice@example.com",
    userPhone: "+15551234567",
    userSlack: "#orders",
    userDeviceToken: "dev_token_abc123",
    doNotDisturb: false,
    orderId: "ORD-9876");

// ─────────────────────────────────────────────────────────────────────────────
// AFTER — the client depends on ONE class and makes ONE call.
// ─────────────────────────────────────────────────────────────────────────────
Console.WriteLine("\n== AFTER Facade: OrderService depends only on NotificationFacade ==");

var config = new NotificationConfig(
    SmtpHost: "smtp.company.com", SmtpPort: 587, SmtpUsername: "noreply", SmtpPassword: "***",
    TwilioSid: "AC123", TwilioToken: "token", TwilioFromNumber: "+1555000",
    SlackWebhookUrl: "https://hooks.slack.com/...",
    FirebaseCredentialsPath: "/secrets/firebase.json",
    LogFilePath: "/var/log/notifications.log");

var notifier = new NotificationFacade(config);
var after = new SolutionOrderService(notifier);

var user = new User("user_42")
{
    Email = "alice@example.com",
    Phone = "+15551234567",
    SlackChannel = "#orders",
    DeviceToken = "dev_token_abc123",
};

after.ConfirmOrder(user, "ORD-9876");
