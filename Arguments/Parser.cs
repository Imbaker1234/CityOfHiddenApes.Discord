namespace CityOfHiddenApes.Discord.Core.Arguments
{
    using System;
    using System.Text.RegularExpressions;

    public static class Parser
    {
        public static bool IsValidUrl(this string input)
        {
            return Uri.TryCreate(input, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        ///     Defines a named trait and a modifier.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Tuple<string, int> TraitModify(string input)
        {
            var matched = "(\\w+)( (\\+|-)\\w+|)";

            //Trim down to "Stealth +10"
            var initMatch = Regex.Match(input, matched, RegexOptions.IgnoreCase).Value;

            //Match out "Stealth"
            var trait = Regex.Match(initMatch, "(\\w+)", RegexOptions.IgnoreCase).Value;

            var value = "0";
            try
            {
                //Match out "+10"
                value = Regex.Match(initMatch, @"(\+|-)\w+", RegexOptions.IgnoreCase).Value;
            }
            catch (Exception e)
            {
            }

            return new Tuple<string, int>(trait, int.Parse(value));
        }

        // public static string RemovePrefixAndCommand(this string input, char? commandPrefix = null)
        // {
        //     var cPrfx = commandPrefix ?? RuntimeConstants.CommandPrefix ?? '!';
        //     return Regex.Replace(input, $"{cPrfx}((\\w+)-?(\\w+)?) ","");
        // }

        /// <summary>
        ///     Parses commands such as "Willpower = 21" and "Description=There once was a man from nantucket"
        ///     and returns (Willpower,21) and (Description,There once was a man from nantucket) respectively.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="traits"></param>
        /// <returns></returns>
        public static Tuple<string, string> TraitEquals(string input, params string[] traits)
        {
            var wholePattern = "(\\w+)\\s?=\\s?([0-9][1-9]?[1-9]?[1-9]?|\"(.*?)\")";
            var firstWordPattern = "(\\w+)";
            var quotePattern = "\"(.*?)\"";

            //If we have "/setTrait MyDescription = "King Krab of the Krab People!"
            //Match-- MyDescription = "King Krab of the Krab People!"
            var match = Regex.Match(input, wholePattern, RegexOptions.IgnoreCase).NextMatch().Value;
            //Match MyDescription
            var trait = Regex.Match(match, firstWordPattern, RegexOptions.IgnoreCase).NextMatch().Value;
            //Return ("Match", "King of the Krab People!")
            var value = Regex.Match(match, quotePattern, RegexOptions.IgnoreCase).NextMatch().Value;
            return new Tuple<string, string>(trait, value);
        }

        public static string DisplayPascalProperty(string name)
        {
            return Regex.Replace(name, "(\\B[A-Z])", " $1");
        }
    }
}