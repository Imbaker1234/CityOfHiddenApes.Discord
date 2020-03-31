namespace CityOfHiddenApes.Discord.Core.HelpAttributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ThumbAttribute : HelpAttribute
    {
        public ThumbAttribute(string thumbImageUrl)
        {
            Content = thumbImageUrl;
        }

        public override string Content { get; }
    }
}