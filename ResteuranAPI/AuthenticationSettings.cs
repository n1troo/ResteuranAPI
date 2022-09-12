namespace ResteuranAPI;

public class AuthenticationSettings
{
    public string Secret { get; set; }
    public string ValidAudience { get; set; }
    public string ValidIssuer { get; set; }
}