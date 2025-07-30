using System;
using System.Collections.Generic;
using System.Text;

namespace General;
public static class URLs
{
    /// <summary>
    /// URL API старт
    /// </summary>
    private const string URL_HEADER = "https://localhost:7227/api/";

    public static Uri Uri_login { get; private set; } = new(URL_HEADER + "Authentication");
    public static Uri Uri_reg { get; private set; } = new(URL_HEADER + "Registration");

    public static Uri Uri_test { get; private set; } = new(URL_HEADER + "Test");
    public static Uri Uri_GetListAllHeroes { get; private set; } = new(URL_HEADER + "GetListAllHeroes");
}
