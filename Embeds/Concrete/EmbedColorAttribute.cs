namespace CityOfHiddenApes.Discord.Core
{
    using System;
    using System.Reflection;
    using Arguments;
    using global::Discord;
    using ReColor;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class EmbedColorAttribute : EmbedAttribute
    {
        public EmbedColorAttribute(ReColor.ReColor.Colors value)
        {
            Value = value;
        }

        public EmbedColorAttribute()
        {
        }

        public ReColor.ReColor.Colors? Value { get; set; }

        public override EmbedBuilder ResolveProperty(object owner, PropertyInfo property, EmbedBuilder builder)
        {
            ReColor.ReColor.Colors setterEnum = (ReColor.ReColor.Colors) (property.GetValue(owner) ?? Value ?? ReColor.ReColor.Random());

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