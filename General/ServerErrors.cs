using System;
using System.Collections.Generic;

namespace General;
public static class ServerErrors
{
    #pragma warning disable
    #pragma warning disable IDE0055
     // 0x0000_FFFF_0000_0000 - Actions
     // 0x0000_0000_FFFF_0000 - Type
     // 0x0000_0000_0000_FFFF - Errors

    //Action
    private const long a_Unknown              = 0x0000_0000_0000_0000;
    private const long a_Auth                 = 0x0000_0001_0000_0000;
    private const long a_Reg                  = 0x0000_0002_0000_0000;

    //Type
    private const long t_Unknown              = 0x0000_0000_0000_0000;
    private const long t_Email                = 0x0000_0000_0001_0000;
    private const long t_Password             = 0x0000_0000_0002_0000;
    private const long t_EmailOrPassword      = 0x0000_0000_0003_0000;
    private const long t_EmailAndPassword     = 0x0000_0000_0004_0000;

    //Error
    private const long e_Unknown              = 0x0000_0000_0000_0001;
    private const long e_Empty                = 0x0000_0000_0000_0002;
    private const long e_Bad                  = 0x0000_0000_0000_0003;
    private const long e_Exists               = 0x0000_0000_0000_0004;
    private const long e_NotFound             = 0x0000_0000_0000_0005;

    /// <summary>
    /// Ошибки сервера в ответах клиенту
    /// </summary>
    public enum Response : long
    {
        Unknown                             = a_Unknown           + t_Unknown           + e_Unknown,

        Reg_Unknown                         = a_Reg               + t_Unknown           + e_Unknown,

        Reg_Email_Empty                     = a_Reg               + t_Email             + e_Empty,
        Reg_Email_Bad                       = a_Reg               + t_Email             + e_Bad,
        Reg_Email_Exists                    = a_Reg               + t_Email             + e_Exists,

        Reg_Password_Empty                  = a_Reg               + t_Password          + e_Empty,

        Auth_EmailOrPassword_Empty          = a_Auth              + t_EmailOrPassword   + e_Empty,
        Auth_EmailAndPassword_NotFound      = a_Auth              + t_EmailAndPassword  + e_NotFound,
    }
    #pragma warning restore IDE0055
    #pragma warning restore


    public static bool CheckEnumServerResponse()
    {
        Array values1 = Enum.GetValues(typeof(Response));

        int i;
        int iEnd = values1.Length;

        List<long> values = [];
        for (i = 0; i < iEnd; i++)
        {
            values.Add((long)(Response)values1.GetValue(i));
        }

        int j;
        int jEnd = values.Count;
        iEnd = jEnd - 1;
        for (i = 0; i < iEnd; i++)
        {
            for (j = 1; j < jEnd; j++)
            {
                if (i != j && values[i] == values[j])
                {
                    return false;
                }
            }
        }
        return true;
    }
}
