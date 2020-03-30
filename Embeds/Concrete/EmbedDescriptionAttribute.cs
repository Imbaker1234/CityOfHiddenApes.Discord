namespace Discordia.Embeds
{
    using System;
    using System.Reflection;
    using Discord;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class EmbedDescriptionAttribute : EmbedStringValueAttribute
    {
        public EmbedDescriptionAttribute(string value) : base(value)
        {
        }

        public EmbedDescriptionAttribute()
        {
        }

        public override EmbedBuilder ResolveClass(EmbedBuilder builder)
        {
            try
            {
                builder.Description = Value;
            }
            catch (NullReferenceException e)
            {
                throw new Exception("You must declare a value for attributes declared at the class level.");
            }

            return builder;
        }

        public override EmbedBuilder ResolveProperty(object owner, PropertyInfo property, EmbedBuilder builder)
        {
            builder.Description =
                property.GetValue(owner)?.ToString() ??
                Value ??
                builder.Description ??
                throw new Exception(
                    "When setting Description for embed the original attribute, and property values were null");

            return builder;
        }
    }
}