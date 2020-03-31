namespace CityOfHiddenApes.Discord.Core.HelpAttributes
{
    using System;

    public abstract class HelpAttribute : Attribute
    {
        public abstract string Content { get; }
    }
}