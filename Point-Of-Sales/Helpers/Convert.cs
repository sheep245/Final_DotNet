namespace Point_Of_Sales.Helpers
{
    public class Convert
    {
        public static string Capitalize(string word)
        {
            return word.Substring(0, 1).ToUpper() + word.Substring(1).ToLower();
        }
    }
}
