namespace CityOfHiddenApes.Discord.Core.HelpAttributes
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using Arguments;
    using Extensions;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AcceptedParameter : HelpAttribute
    {
        public AcceptedParameter(Type type)
        {
            var dictionary = new Dictionary<string, List<string>>();
            type.GetProperties().ForEach(prop =>
            {
                prop.GetCustomAttributes<HelpArgumentAttribute>()
                    .ForEach(att =>
                    {
                        if (dictionary.ContainsKey(att.HowItsUsed)) dictionary[att.HowItsUsed].Add(prop.Name);
                        else dictionary.Add(att.HowItsUsed, new List<string> {prop.Name});
                    });
            });

            var sb = new StringBuilder();
            foreach (var kvp in dictionary) sb.AppendLine($"({string.Join(" | ", kvp.Value)}) - {kvp.Key}\n");

            Content = sb.ToString();
        }

        public AcceptedParameter(string nameOfArg, string howItsUsed)
        {
            Content = $"{nameOfArg} - {howItsUsed}";
        }

        public override string Content { get; }
    }
}