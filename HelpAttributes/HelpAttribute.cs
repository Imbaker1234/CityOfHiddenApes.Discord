namespace Discordia.HelpAttributes
{
    using System;

    public abstract class HelpAttribute : Attribute
    {
        public abstract string Content { get; }
    }
}