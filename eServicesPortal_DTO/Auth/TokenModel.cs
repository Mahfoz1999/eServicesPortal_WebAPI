namespace eServicesPortal_DTO.Auth;

public class TokenModel
{
    public string token { get; set; } = string.Empty;
    public DateTime expiration { get; set; }
}
