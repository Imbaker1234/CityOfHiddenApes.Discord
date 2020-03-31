namespace CityOfHiddenApes.Discord.Core.HelpAttributes
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ImageAttribute : HelpAttribute
    {
        public ImageAttribute(string url)
        {
            Content = url;
        }

        public override string Content { get; }
    }
}