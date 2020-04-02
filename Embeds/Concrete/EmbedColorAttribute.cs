namespace CityOfHiddenApes.Discord.Core
{
    using System;
    using System.Reflection;
    using Arguments;
    using global::Discord;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class EmbedColorAttribute : EmbedAttribute
    {
        public EmbedColorAttribute(ReColor.Colors value)
        {
            Value = value;
        }

        public EmbedColorAttribute()
        {
        }

        public ReColor.Colors? Value { get; set; }

        public override EmbedBuilder ResolveProperty(object owner, PropertyInfo property, EmbedBuilder builder)
        {
            ReColor.Colors setterEnum = (ReColor.Colors) (property.GetValue(owner) ?? Value ?? ReColor.Random());

            builder.Color = setterEnum.ToDiscordColor();
            
            return builder;
        }

        public override EmbedBuilder ResolveClass(EmbedBuilder builder)
        {
            builder.Color = Value?.ToDiscordColor();
            return builder;
        }
    }
}