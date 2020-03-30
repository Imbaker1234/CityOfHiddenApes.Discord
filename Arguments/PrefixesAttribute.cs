namespace Discordia.Arguments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PrefixesAttribute : Attribute
    {
        public PrefixesAttribute(params string[] possiblePrefixes)
        {
            Values = SortByLength(possiblePrefixes);
        }

        public IEnumerable<string> Values { get; set; }

        /// <summary>
        ///     Since these are matched via regex in a (this|orThis|orThat) clause
        ///     the one with the longest name is presented first and allows the arguments
        ///     that are longer in name to be matched as well instead of being overshadowed by
        ///     the shorter arg. For instance if "desc" comes first you cannot match "description"
        ///     as an argument name.
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        private IEnumerable<string> SortByLength(string[] strings)
        {
            // Use LINQ to sort the array received and return a copy.
            var sorted = from entry in strings
                orderby entry.Length descending
                select entry;
            return sorted;
        }
    }
}