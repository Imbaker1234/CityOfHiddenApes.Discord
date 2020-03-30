namespace Discordia.Embeds
{
    using System;
    using System.Reflection;
    using Discord;

    public class EmbedFooterIconAttribute : EmbedUrlAttribute
    {
        public EmbedFooterIconAttribute(string value) : base(value)
        {
        }

        public EmbedFooterIconAttribute()
        {
        }

        public override EmbedBuilder ResolveProperty(object owner, PropertyInfo property, EmbedBuilder builder)
        {
            if (builder.Footer is null) builder.Footer = new EmbedFooterBuilder();
            //Calculate the value only once.
            builder.Footer.IconUrl =
                property.GetValue(owner)?.ToString() ??
                Value ??
                builder.Footer.IconUrl ??
                throw new NullReferenceException(
                    "When setting Description both the property and attribute values were null.");

            return builder;
        }

        public override EmbedBuilder ResolveClass(EmbedBuilder builder)
        {
            if (builder.Footer is null) builder.Footer = new EmbedFooterBuilder();

            try
            {
                builder.Footer.IconUrl = Value;
            }
            catch (NullReferenceException e)
            {
                throw new Exception("You must declare a value for attributes declared at the class level.");
            }

            return builder;
        }
    }
}