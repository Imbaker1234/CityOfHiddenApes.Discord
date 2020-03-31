namespace CityOfHiddenApes.Discord.Core
{
    using System;
    using System.Reflection;
    using global::Discord;

    public class EmbedThumbAttribute : EmbedUrlAttribute
    {
        public EmbedThumbAttribute(string value) : base(value)
        {
        }

        public EmbedThumbAttribute()
        {
        }

        public override EmbedBuilder ResolveProperty(object owner, PropertyInfo property, EmbedBuilder builder)
        {
            builder.ThumbnailUrl =
                property.GetValue(owner)?.ToString() ??
                Value ??
                builder.ThumbnailUrl ??
                throw new NullReferenceException(
                    "When setting Thumbnail the property, attribute, and initial values were null.");

            return builder;
        }

        public override EmbedBuilder ResolveClass(EmbedBuilder builder)
        {
            try
            {
                builder.ThumbnailUrl = Value;
            }
            catch (NullReferenceException e)
            {
                throw new Exception("You must declare a value for attributes declared at the class level.");
            }

            return builder;
        }
    }
}