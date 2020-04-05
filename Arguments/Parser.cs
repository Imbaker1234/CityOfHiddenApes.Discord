namespace CityOfHiddenApes.Discord.Core
{
    using System.Text.RegularExpressions;

    public static class Parser
    {
        public static string DisplayPascalProperty(string name)
        {
            return Regex.Replace(name, "(\\B[A-Z])", " $1");
        }
    }
}