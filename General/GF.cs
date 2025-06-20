using System;
using System.Net.Mail;

namespace General;

/// <summary>
/// Global Functions
/// </summary>
public static class GF {

    /// <summary>
    /// Пытается создать объект MailAddress из строки. Возвращает обратно входную строку без изменений при успехи, иначе null.
    /// </summary>
    /// <param name="email">Строка с предполагаемым адресом электронной почты.</param>
    /// <returns>email, если создание прошло успешно; иначе null.</returns>
    public static string? GetEmailOrNull(string email) {

        // Проверка на null или пустую строку
        if (string.IsNullOrWhiteSpace(email)) {
            return null;
        }

        try {
            // Попытка создания MailAddress
            _ = new MailAddress(email);
            return email;
        }
        catch (FormatException) {
            // Невалидный формат email
            return null;
        }
        catch (Exception) {
            // другое исключение
            return null;
        }
    }

}
