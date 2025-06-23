using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace General;

/// <summary>
/// Global Functions
/// </summary>
public static class GF
{

    /// <summary>
    /// Пытается создать объект MailAddress из строки. Возвращает обратно входную строку без изменений при успехи, иначе null.
    /// </summary>
    /// <param name="email">Строка с предполагаемым адресом электронной почты.</param>
    /// <returns>email, если создание прошло успешно; иначе null.</returns>
    public static string? GetEmailOrNull(string email)
    {

        // Проверка на null или пустую строку
        if (string.IsNullOrWhiteSpace(email))
        {
            return null;
        }

        try
        {
            // Попытка создания MailAddress
            _ = new MailAddress(email);
            return email;
        }
        catch (FormatException)
        {
            // Невалидный формат email
            return null;
        }
        catch (Exception)
        {
            // другое исключение
            return null;
        }
    }
    #pragma warning disable IDE0055 
#pragma warning disable
     // 0x00FF0000 - Actions
     // 0x0000FF00 - Type
     // 0x000000FF - Success:0 OR Errors:>0

    //Action
    private const int sr_Auth        = 0x00010000;
    private const int sr_Reg         = 0x00020000;

    //Type
    private const int sr_Email       = 0x00000100;
    private const int sr_Password    = 0x00000200;

    //Success or Error
    private const int sr_Success     = 0x00000000;
    private const int sr_ErrorUnknown = 0x00000001;
    private const int sr_ErrorEmpty  = 0x00000002;
    private const int sr_ErrorBad    = 0x00000003;
    private const int sr_ErrorExists = 0x00000004;
    public enum ServerResponseError
    {
        Reg_Unknown        = sr_Reg + sr_ErrorUnknown,

        Reg_Email_Empty    = sr_Reg + sr_Email    + sr_ErrorEmpty,
        Reg_Email_Bad      = sr_Reg + sr_Email    + sr_ErrorBad,
        Reg_Email_Exists   = sr_Reg + sr_Email    + sr_ErrorExists,

        Reg_Password_Empty = sr_Reg + sr_Password + sr_ErrorEmpty,
    }
    //public enum ServerResponseOk {
    //    Reg_Success = sr_Reg  + sr_Success,
    //    Auth_Success = sr_Auth + sr_Success,
    //}
#pragma warning restore
#pragma warning restore IDE0055

    static GF()
    {
        CheckEnumServerResponse();
    }

    private static void CheckEnumServerResponse()
    {
        Array values1 = Enum.GetValues(typeof(ServerResponseError));
        //Array values2 = Enum.GetValues(typeof(ServerResponseOk));

        int i;
        int iEnd = values1.Length;

        List<int> values = [];
        for (i = 0; i < iEnd; i++)
        {
            values.Add((int)(ServerResponseError)values1.GetValue(i));
        }

        //iEnd = values2.Length;
        //for (i = 0; i < iEnd; i++)
        //{
        //    values.Add((int)(ServerResponseOk)values2.GetValue(i));
        //}


        int j;
        int jEnd = values.Count;
        iEnd = jEnd - 1;
        for (i = 0; i < iEnd; i++)
        {
            for (j = 1; j < jEnd; j++)
            {
                if (i != j && values[i] == values[j])
                {
                    throw new Exception("Bad enum ServerResponse");
                }
            }
        }
    }
}
