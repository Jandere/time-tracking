namespace Application.Common.Models;

public class AuthenticateResponse
{
    public string UserName { get; set; }
    public string UserId { get; set; }
    public string Token { get; set; }

    public AuthenticateResponse(string userName, string userId, string token)
    {
        UserName = userName;
        UserId = userId;
        Token = token;
    }
}