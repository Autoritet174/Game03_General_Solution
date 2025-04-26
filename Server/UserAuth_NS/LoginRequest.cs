namespace Server.UserAuth_NS;

/// <summary>
/// Модель данных для авторизации.
/// </summary>
public class LoginRequest {
    public required string Username { get; set; }
    public required string Password { get; set; }
}