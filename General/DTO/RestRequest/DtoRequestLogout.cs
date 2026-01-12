using System;
using System.Collections.Generic;
using System.Text;

namespace General.DTO.RestRequest;

public class DtoRequestLogout(string refreshToken)
{
    public string RefreshToken { get; set; } = refreshToken;
}
