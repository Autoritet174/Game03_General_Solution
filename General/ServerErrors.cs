//using System;
//using System.Collections.Generic;

//namespace General;
//public static class ServerErrors
//{
//    /// <summary>
//    /// Ошибки сервера в ответах клиенту
//    /// </summary>
//    //public enum Error : long
//    //{
//    //    //Unknown = 1,
//    //    //AuthInvalidCredentials = 2,
//    //    //RestApiBodyEmpty = 3,
//    //    //UnsupportedMediaType = 4,
//    //    //AuthAlreadyAuthenticated = 7,
//    //    //TooManyRequests = 6,
//    //    //AccountBannedUntil = 101,
//    //    //AccountBannedPermanently = 102,
//    //}

//    //public static Error GetResponse(long code) { 
//    //    return (Error)code;
//    //}


//    //public static bool CheckEnumServerResponse()
//    //{
//    //    Array values1 = Enum.GetValues(typeof(Error));

//    //    int i;
//    //    int iEnd = values1.Length;

//    //    List<long> values = [];
//    //    for (i = 0; i < iEnd; i++)
//    //    {
//    //        values.Add((long)(Error)values1.GetValue(i));
//    //    }

//    //    int j;
//    //    int jEnd = values.Count;
//    //    iEnd = jEnd - 1;
//    //    for (i = 0; i < iEnd; i++)
//    //    {
//    //        for (j = 1; j < jEnd; j++)
//    //        {
//    //            if (i != j && values[i] == values[j])
//    //            {
//    //                return false;
//    //            }
//    //        }
//    //    }
//    //    return true;
//    //}
//}
