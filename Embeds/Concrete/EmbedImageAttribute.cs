namespace Discordia.Embeds
{
    using System;
    using System.Reflection;
    using Discord;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class EmbedImageAttribute : EmbedUrlAttribute
    {
        public EmbedImageAttribute(string value) : base(value)
        {
        }

        public EmbedImageAttribute()
        {
        }

        public override EmbedBuilder ResolveProperty(object owner, PropertyInfo property, EmbedBuilder builder)
        {
            builder.ImageUrl =
                property.GetValue(owner)?.ToString() ??
                Value ??
                builder.ImageUrl ??
                throw new NullReferenceException(
                    "When setting Description both the property and attribute values were null.");

            return builder;
        }

        public override EmbedBuilder ResolveClass(EmbedBuilder builder)
        {
            try
            {
                builder.ImageUrl = Value;
            }
            catch (NullReferenceException e)
            {
                throw new Exception("You must declare a value for attributes declared at the class level.");
            }

            return builder;
        }
    }
}