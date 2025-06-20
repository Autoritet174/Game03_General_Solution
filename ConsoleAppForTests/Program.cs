internal class Program {
    private static void Main(string[] args) {
        Console.WriteLine(IsStringsEqual("qweй", "QWe"));
    }
    public static bool IsStringsEqual(string s1, string s2) {
        return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
    }

}