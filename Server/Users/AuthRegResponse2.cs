//using General.DTO.RestResponse;
//using Newtonsoft.Json;
//using System.ComponentModel;
//using L = General.LocalizationKeys;

//namespace Server.Users;

///// <summary>
///// Класс для представления результатов пользователя.
///// </summary>
//public sealed record AuthRegResponse() : ResponseBase<AuthRegResponse>
//{
//    public DtoResponseAuthReg DtoAuthentication { get; private set; } = null!;

//    public static AuthRegResponse SuccessResponse(string accessToken, string refreshToken)
//    {
//        //GameDataJson = JsonConvert.SerializeObject(container, General.GlobalHelper.JsonSerializerSettings);
//        return new AuthRegResponse() { Success = true, DtoAuthentication = new DtoResponseAuthReg(accessToken, refreshToken, null) };
//    }

//    /// <summary>
//    /// Результат, если пользователь уже существует.
//    /// </summary>
//    /// <returns>Результат ошибки.</returns>
//    public static AuthRegResponse UserAlreadyExists()
//    {
//        return new() { ErrorKey = L.Error.Server.UserAlreadyExists };
//    }

//    /// <summary>
//    /// Результат для забаненного аккаунта.
//    /// </summary>
//    /// <param name="until">Дата окончания бана (null для перманентного).</param>
//    /// <returns>Результат ошибки с дополнительными данными.</returns>
//    public static AuthRegResponse Banned(DateTimeOffset? until)
//    {
//        return new()
//        {
//            ErrorKey = until == null
//                    ? L.Error.Server.AccountBannedPermanently
//                    : L.Error.Server.AccountBannedUntil,
//            Extra = until
//        };
//    }

//    /// <summary>
//    /// Результат, требующий двухфакторной аутентификации.
//    /// </summary>
//    /// <param name="userId">ID пользователя.</param>
//    /// <returns>Результат ошибки с дополнительными данными.</returns>
//    public static AuthRegResponse RequiresTwoFactor(Guid userId)
//    {
//        return new() { ErrorKey = L.Error.Server.Required2FA, Extra = userId };
//    }

//    public static AuthRegResponse RefreshTokenErrorCreating()
//    {
//        return new() { ErrorKey = L.Error.Server.RefreshTokenErrorCreating };
//    }


//}
