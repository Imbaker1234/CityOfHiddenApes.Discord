namespace CityOfHiddenApes.Discord.Core.HelpAttributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DescriptionAttribute : HelpAttribute
    {
        public DescriptionAttribute(string content)
        {
            Content = content;
        }

        public override string Content { get; }
    }
}