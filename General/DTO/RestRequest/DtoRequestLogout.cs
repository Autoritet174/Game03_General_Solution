namespace General.DTO.RestRequest;

public class DtoRequestLogout(string refreshToken)
{
    public string refreshToken { get; set; } = refreshToken;
}
