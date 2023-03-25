using Microsoft.AspNetCore.Identity.UI.Services;

namespace Printer.Utilities;

public class StripeSettings
{
    public string SecretKey { get; set; }
    public string PublishableKey { get; set; }
}
