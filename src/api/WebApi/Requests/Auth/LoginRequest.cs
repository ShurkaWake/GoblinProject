namespace WebApi.Requests.Auth
{
    public sealed record LoginRequest
        (string Email,
        string Password);
}
