namespace Discordia.Embeds
{
    using System;
    using System.Reflection;
    using Discord;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class EmbedTitleAttribute : EmbedStringValueAttribute
    {
        public EmbedTitleAttribute()
        {
        }

        public EmbedTitleAttribute(string value) : base(value)
        {
        }

        public override EmbedBuilder ResolveProperty(object owner, PropertyInfo property, EmbedBuilder builder)
        {
            builder.Title =
                property.GetValue(owner)?.ToString() ??
                Value ??
                builder.Title ??
                throw new NullReferenceException(
                    "When setting Title both the property and attribute values were null.");

            return builder;
        }

        public override EmbedBuilder ResolveClass(EmbedBuilder builder)
        {
            try
            {
                builder.Title = Value;
            }
            catch (NullReferenceException e)
            {
                throw new Exception("You must declare a value for attributes declared at the class level.");
            }

            return builder;
        }
    }
}