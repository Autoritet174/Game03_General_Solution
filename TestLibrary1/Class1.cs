using System;
using static System.Net.Mime.MediaTypeNames;

namespace TestLibrary1
{
    public class Class1
    {
        int a = 1;
        int? b = 1;
        int? c = null;
        public string s = "qwe";
        public Class1 Copy()
        {
            return (Class1)MemberwiseClone();
        }
    }
}
