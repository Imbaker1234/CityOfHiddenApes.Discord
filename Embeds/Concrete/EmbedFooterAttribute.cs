namespace CityOfHiddenApes.Discord.Core
{
    using System;
    using System.Reflection;
    using global::Discord;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class EmbedFooterAttribute : EmbedStringValueAttribute
    {
        public EmbedFooterAttribute(string value) : base(value)
        {
        }

        public EmbedFooterAttribute()
        {
        }

        public override EmbedBuilder ResolveProperty(object owner, PropertyInfo property, EmbedBuilder builder)
        {
            if (builder.Footer is null) builder.Footer = new EmbedFooterBuilder();
            //Calculate the value only once.
            builder.Footer.Text =
                property.GetValue(owner)?.ToString() ??
                Value ??
                builder.Footer.Text ??
                throw new NullReferenceException(
                    "When setting Description both the property and attribute values were null.");

            return builder;
        }

        public override EmbedBuilder ResolveClass(EmbedBuilder builder)
        {
            if (builder.Footer is null) builder.Footer = new EmbedFooterBuilder();
            try
            {
                builder.Footer.Text = Value;
            }
            catch (NullReferenceException e)
            {
                throw new Exception("You must declare a value for attributes declared at the class level.");
            }

            return builder;
        }
    }
}