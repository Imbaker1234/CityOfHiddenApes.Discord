namespace Discordia.HelpAttributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Arguments;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AcceptedParameterAttribute : HelpAttribute
    {
        public override string Content { get; }

        public AcceptedParameterAttribute(Type type)
        {
            var dictionary = new Dictionary<string, List<string>>();
            type.GetProperties().ForEach(prop =>
            { 
                prop.GetCustomAttributes<AliasArgumentAttribute>()
                    .ForEach(att =>
                    {
                        if(dictionary.ContainsKey(att.HowItsUsed)) dictionary[att.HowItsUsed].Add(prop.Name);
                        else dictionary.Add(att.HowItsUsed, new List<string>() { prop.Name });
                    });
            });

            var sb = new StringBuilder();
            foreach (var kvp in dictionary)
            {
                sb.AppendLine($"({string.Join(" | ", kvp.Value)}) - {kvp.Key}\n");
            }
            
            Content = sb.ToString();
        }

        public AcceptedParameterAttribute(string nameOfArg, string howItsUsed)
        {
            Content = $"{nameOfArg} - {howItsUsed}";;
        }

    }
}